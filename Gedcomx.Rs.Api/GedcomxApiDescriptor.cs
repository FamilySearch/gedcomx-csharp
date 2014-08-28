using System;
using RestSharp;
using Gx.Atom;
using Gx.Links;
using System.Collections.Generic;
using Tavis.UriTemplates;

namespace Gx.Rs.Api
{
    public class GedcomxApiDescriptor
    {
        private RestClient sourceClient;
        private string sourcePath;
        private Dictionary<string, RestClient> clients = new Dictionary<string, RestClient>();
        private Dictionary<string, Link> links;
        private DateTime expiration;

        public GedcomxApiDescriptor(String host, string discoveryPath)
            : this(new RestClient(host), discoveryPath)
        {
        }

        /// <summary>
        /// Initialize an API descriptor from the specified discovery URI.
        /// </summary>
        /// <param name='client'>
        /// The client to use to discover the API.
        /// </param>
        /// <param name='discoveryPath'>
        /// The path to the discovery resource on the host.
        /// </param>
        public GedcomxApiDescriptor(RestClient client, string discoveryPath)
        {
            Initialize(client, discoveryPath);
        }

        void Initialize(RestClient client, string discoveryPath)
        {
            var request = new RestRequest();
            request.Resource = discoveryPath;
            request.AddHeader("Accept", MediaTypes.ATOM_XML_MEDIA_TYPE);

            DateTime now = DateTime.Now;
            var response = client.Execute<Feed>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpException(response.StatusCode);
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            Feed feed = response.Data;
            DateTime expiration = DateTime.MaxValue;
            foreach (Parameter header in response.Headers)
            {
                if ("cache-control".Equals(header.Name.ToLowerInvariant()))
                {
                    CacheControl cacheControl = CacheControl.Parse(header.Value.ToString());
                    expiration = now.AddSeconds(cacheControl.MaxAge);
                }
            }

            this.expiration = expiration;
            this.links = BuildLinkLookup(feed != null ? feed.Links : null);
            this.sourceClient = client;
            this.sourcePath = discoveryPath;
            this.clients.Add(client.BaseUrl, client);
        }

        public bool RefreshWithAuthentication(string token)
        {
            var request = new RestRequest();
            request.Resource = this.sourcePath;
            request.AddHeader("Accept", MediaTypes.ATOM_XML_MEDIA_TYPE);
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));

            DateTime now = DateTime.Now;
            var response = this.sourceClient.Execute<Feed>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                //throw new HttpException (response.StatusCode);
                return false;
            }

            if (response.ErrorException != null)
            {
                //throw response.ErrorException;
                return false;
            }

            Feed feed = response.Data;
            DateTime expiration = DateTime.MaxValue;
            foreach (Parameter header in response.Headers)
            {
                if ("cache-control".Equals(header.Name.ToLowerInvariant()))
                {
                    CacheControl cacheControl = CacheControl.Parse(header.Value.ToString());
                    expiration = now.AddSeconds(cacheControl.MaxAge);
                }
            }

            this.expiration = expiration;
            this.links = BuildLinkLookup(feed != null ? feed.Links : null);
            return true;
        }

        /// <summary>
        /// Builds the link lookup table.
        /// </summary>
        /// <returns>
        /// The link lookup.
        /// </returns>
        /// <param name='links'>
        /// The links to initialize.
        /// </param>
        Dictionary<string, Link> BuildLinkLookup(List<Link> links)
        {
            Dictionary<string, Link> lookup = new Dictionary<string, Link>();
            if (links != null)
            {
                foreach (Link link in links)
                {
                    if (link != null && link.Rel != null)
                    {
                        lookup.Add(link.Rel, link);
                    }
                }
            }
            return lookup;
        }

        /// <summary>
        /// Tries to resolve a relative or absolute URI.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if URI resolution was successful, <c>false</c> otherwise.
        /// </returns>
        /// <param name='relativeOrAbsoluteUri'>
        /// Relative or absolute URI.
        /// </param>
        /// <param name='result'>
        /// Result.
        /// </param>
        bool TryUriResolution(string relativeOrAbsoluteUri, out Uri result)
        {
            if (!Uri.TryCreate(relativeOrAbsoluteUri, UriKind.Absolute, out result))
            {
                if (Uri.TryCreate(new Uri(this.sourceClient.BaseUrl), relativeOrAbsoluteUri, out result))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        public bool GetRequestByRel(string rel, out RestClient client, out string requestPath)
        {
            Link link;
            if (this.links.TryGetValue(rel, out link))
            {
                return GetRequest(link, out client, out requestPath);
            }
            else
            {
                client = null;
                requestPath = null;
                return false;
            }
        }

        public bool GetRequest(Link link, out RestClient client, out string requestPath)
        {
            if (link == null)
            {
                client = null;
                requestPath = null;
                return false;
            }
            else
            {
                return GetRequest(link.Href, out client, out requestPath);
            }
        }

        public bool GetRequest(string uri, out RestClient client, out string requestPath)
        {
            if (uri == null)
            {
                client = null;
                requestPath = null;
                return false;
            }

            Uri parsed;
            if (TryUriResolution(uri, out parsed))
            {
                return GetRequest(parsed, out client, out requestPath);
            }
            else
            {
                client = null;
                requestPath = null;
                return false;
            }
        }

        public bool GetRequest(Uri uri, out RestClient client, out string requestPath)
        {
            if (uri == null)
            {
                client = null;
                requestPath = null;
                return false;
            }
            else if (uri.IsAbsoluteUri)
            {
                string uriValue = uri.ToString();
                string host = uri.Host;
                var splitHostIndex = uriValue.IndexOf(host) + host.Length;
                client = GetClient(uriValue.Substring(0, splitHostIndex));
                requestPath = uriValue.Length > splitHostIndex + 1 ? uriValue.Substring(splitHostIndex + 1) : "/";
                return true;
            }
            else
            {
                client = this.sourceClient;
                requestPath = uri.ToString();
                return true;
            }
        }

        RestClient GetClient(string baseUri)
        {
            RestClient client;
            if (!this.clients.TryGetValue(baseUri, out client))
            {
                client = new RestClient(baseUri);
                this.clients.Add(baseUri, client);
            }
            return client;
        }

        public bool GetRequest(Uri uri, out RestClient client, out RestRequest request)
        {
            String requestPath;
            if (GetRequest(uri, out client, out requestPath))
            {
                request = new RestRequest(requestPath);
                return true;
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetRequest(string uri, out RestClient client, out RestRequest request)
        {
            String requestPath;
            if (GetRequest(uri, out client, out requestPath))
            {
                request = new RestRequest(requestPath);
                return true;
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetRequest(Link link, out RestClient client, out RestRequest request)
        {
            String requestPath;
            if (GetRequest(link, out client, out requestPath))
            {
                request = new RestRequest(requestPath);
                return true;
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetRequestByRel(string rel, out RestClient client, out RestRequest request)
        {
            Link link;
            if (this.links.TryGetValue(rel, out link))
            {
                return GetRequest(link, out client, out request);
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetTemplatedRequest(string uri, Dictionary<string, string> templateVariables, out RestClient client, out RestRequest request)
        {
            String requestPath;
            if (GetRequest(uri, out client, out requestPath))
            {
                UriTemplate template = new UriTemplate(requestPath);
                foreach (KeyValuePair<string, string> entry in templateVariables)
                {
                    template.SetParameter(entry.Key, entry.Value);
                }
                request = new RestRequest(template.Resolve());
                return true;
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetTemplatedRequest(Link link, Dictionary<string, string> templateVariables, out RestClient client, out RestRequest request)
        {
            if (link == null)
            {
                client = null;
                request = null;
                return false;
            }
            String requestPath;
            if (GetRequest(link.Template, out client, out requestPath))
            {
                UriTemplate template = new UriTemplate(requestPath);
                foreach (KeyValuePair<string, string> entry in templateVariables)
                {
                    template.SetParameter(entry.Key, entry.Value);
                }
                request = new RestRequest(template.Resolve());
                return true;
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        public bool GetTemplatedRequestByRel(string rel, Dictionary<string, string> templateVariables, out RestClient client, out RestRequest request)
        {
            Link link;
            if (this.links.TryGetValue(rel, out link))
            {
                return GetTemplatedRequest(link, templateVariables, out client, out request);
            }
            else
            {
                client = null;
                request = null;
                return false;
            }
        }

        /// <summary>
        /// The links that describe the API.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public Dictionary<string, Link> Links
        {
            get
            {
                return this.links;
            }
        }

        /// <summary>
        /// Gets or sets the expiration of this descriptor.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public DateTime Expiration
        {
            get
            {
                return this.expiration;
            }
            set
            {
                expiration = value;
            }
        }

        /// <summary>
        /// Whether this descriptor is expired.
        /// </summary>
        /// <value>
        /// <c>true</c> if expired; otherwise, <c>false</c>.
        /// </value>
        public bool Expired
        {
            get
            {
                return DateTime.Now > this.expiration;
            }
        }

        /// <summary>
        /// Refresh this descriptor using the specified client.
        /// </summary>
        /// <param name='client'>
        /// The client.
        /// </param>
        public bool Refresh()
        {
            try
            {
                Initialize(this.sourceClient, this.sourcePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Uri GetOAuth2AuthorizationUri()
        {
            Uri authorizationUri = null;
            Link authorizationLink;
            this.Links.TryGetValue(Rel.OAUTH2_AUTHORIZE, out authorizationLink);
            if (authorizationLink != null && authorizationLink.Href != null)
            {
                TryUriResolution(authorizationLink.Href, out authorizationUri);
            }
            return authorizationUri;
        }

        public bool GetOAuth2TokenRequest(out RestClient client, out RestRequest request)
        {
            return GetRequestByRel(Rel.OAUTH2_TOKEN, out client, out request);
        }

        public bool GetPersonRequest(string personId, out RestClient client, out RestRequest request)
        {
            Dictionary<string, string> variables = new Dictionary<string, string>();
            variables.Add(TemplateVariables.PersonId, personId);
            return GetTemplatedRequestByRel(Rel.PERSON, variables, out client, out request);
        }
    }
}

