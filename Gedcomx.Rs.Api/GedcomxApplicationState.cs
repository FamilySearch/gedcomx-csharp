using System;
using RestSharp;
using System.Text;
using System.Collections.Generic;
using Gx.Conclusion;
using Gx.Rs.Api.Util;
using Gx.Links;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using Gx.Records;

namespace Gx.Rs.Api
{
    public abstract class GedcomxApplicationState
    {
    }

    public abstract class GedcomxApplicationState<T> : GedcomxApplicationState where T : class, new()
    {
        protected static readonly EmbeddedLinkLoader DEFAULT_EMBEDDED_LINK_LOADER = new EmbeddedLinkLoader();

        protected readonly StateFactory stateFactory;
        protected readonly Dictionary<String, Link> links;
        public IRestClient Client { get; private set; }
        public String CurrentAccessToken { get; set; }
        private Tavis.LinkFactory linkFactory;
        private Tavis.LinkHeaderParser linkHeaderParser;
        public bool IsAuthenticated { get { return CurrentAccessToken != null; } }
        public IRestRequest Request { get; private set; }
        public IRestResponse<T> Response { get; private set; }
        public T Entity { get; private set; }
        protected abstract GedcomxApplicationState Clone(IRestRequest request, IRestResponse<T> response, IRestClient client);
        protected abstract T LoadEntity(IRestResponse<T> response);
        protected abstract Collection MainDataElement { get; }


        protected GedcomxApplicationState()
        {
            linkFactory = new Tavis.LinkFactory();
            linkHeaderParser = new Tavis.LinkHeaderParser(linkFactory);
        }

        protected GedcomxApplicationState(IRestRequest request, IRestResponse<T> response, IRestClient client, String accessToken, StateFactory stateFactory)
            : this()
        {
            this.Request = request;
            this.Response = response;
            this.Client = client;
            this.CurrentAccessToken = accessToken;
            this.stateFactory = stateFactory;
            this.Entity = LoadEntityConditionally(this.Response);
            List<Link> links = LoadLinks(this.Response, this.Entity, this.Request.RequestFormat);
            this.links = new Dictionary<String, Link>();
            foreach (Link link in links)
            {
                this.links[link.Rel] = link;
            }
        }

        protected T LoadEntityConditionally(IRestResponse<T> response)
        {
            if (Request.Method != Method.HEAD && Request.Method != Method.OPTIONS && response.StatusCode == HttpStatusCode.OK)
            {
                return LoadEntity(response);
            }
            else
            {
                return null;
            }
        }

        public GedcomxApplicationState Inject(IRestRequest request)
        {
            return Clone(request, Invoke(request), this.Client);
        }

        protected List<Link> LoadLinks(IRestResponse response, T entity, DataFormat contentFormat)
        {
            List<Link> links = new List<Link>();

            //if there's a location, we'll consider it a "self" link.
            if (response.ResponseUri != null)
            {
                links.Add(new Link() { Rel = Rel.SELF, Href = response.ResponseUri.ToString() });
            }

            //initialize links with link headers
            foreach (var header in response.Headers.Where(x => x.Name == "Link" && x.Value != null).SelectMany(x => linkHeaderParser.Parse(response.ResponseUri, x.Value.ToString())))
            {
                Link link = new Link() { Rel = header.Relation, Href = header.Target.ToString() };
                link.Template = header.GetLinkExtensionSafe("template");
                link.Title = header.Title;
                link.Accept = header.GetLinkExtensionSafe("accept");
                link.Allow = header.GetLinkExtensionSafe("allow");
                link.Hreflang = header.HrefLang.Select(x => x.Name).FirstOrDefault();
                link.Type = header.GetLinkExtensionSafe("type");
                links.Add(link);
            }

            //load the links from the main data element
            Collection mainElement = MainDataElement;
            if (mainElement != null && mainElement.Links != null)
            {
                links.AddRange(mainElement.Links);
            }

            //load links at the document level
            var collection = entity as Collection;
            if (entity != mainElement && collection != null && collection.Links != null)
            {
                links.AddRange(collection.Links);
            }


            return links;
        }

        public Uri GetUri()
        {
            return new Uri(this.Client.BaseUrl + this.Request.Resource);
        }

        public bool HasError()
        {
            return this.Response.ResponseStatus == ResponseStatus.Error;
        }

        public bool HasStatus(ResponseStatus status)
        {
            return this.Response.ResponseStatus == status;
        }

        protected IRestResponse<T> Invoke(IRestRequest request, params StateTransitionOption[] options)
        {
            string originalBaseUrl = this.Client.BaseUrl;
            string originalResource = request.Resource;
            bool restore = false;
            IRestResponse<T> result;

            foreach (StateTransitionOption option in options)
            {
                option.Apply(request);
            }

            Uri uri;
            if (Uri.TryCreate(request.Resource, UriKind.RelativeOrAbsolute, out uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    restore = true;
                    this.Client.BaseUrl = uri.GetLeftPart(UriPartial.Authority);
                    request.Resource = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                }
            }

            result = this.Client.Execute<T>(request);

            if (restore)
            {
                this.Client.BaseUrl = originalBaseUrl;
                request.Resource = originalResource;
            }

            return result;
        }

        public virtual GedcomxApplicationState IfSuccessful()
        {
            if (HasError())
            {
                throw new GedcomxApplicationException(String.Format("Unsuccessful {0} to {1}", this.Request.Method, GetUri()), this.Response);
            }
            return this;
        }

        public virtual GedcomxApplicationState Head(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();

            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            if (accept != null)
            {
                request.AddParameter(accept);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.HEAD;

            return Clone(request, Invoke(request, options), this.Client);
        }

        public virtual GedcomxApplicationState Options(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            if (accept != null)
            {
                request.AddParameter(accept);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.OPTIONS;

            return Clone(request, Invoke(request, options), this.Client);
        }

        public virtual GedcomxApplicationState Get(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            if (accept != null)
            {
                request.AddParameter(accept);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.GET;

            return Clone(request, Invoke(request, options), this.Client);
        }

        public virtual GedcomxApplicationState Delete(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            if (accept != null)
            {
                request.AddParameter(accept);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.DELETE;

            return Clone(request, Invoke(request, options), this.Client);
        }

        public virtual GedcomxApplicationState Put(T entity, params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            Parameter contentType = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Content-Type");

            if (accept != null)
            {
                request.AddParameter(accept);
            }

            if (contentType != null)
            {
                request.AddParameter(contentType);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.PUT;
#warning Need to resolve "builder.entity(entity)" pattern
            request.AddObject(entity);

            return Clone(request, Invoke(request, options), this.Client);
        }

        public virtual GedcomxApplicationState Post(T entity, params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept");
            Parameter contentType = this.Request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Content-Type");

            if (accept != null)
            {
                request.AddParameter(accept);
            }

            if (contentType != null)
            {
                request.AddParameter(contentType);
            }

            request.Resource = GetSelfUri().ToString();
            request.Method = Method.POST;
#warning Need to resolve "builder.entity(entity)" pattern
            request.AddObject(entity);

            return Clone(request, Invoke(request, options), this.Client);
        }

        protected virtual GedcomxApplicationState AuthenticateViaOAuth2Password(String username, String password, String clientId)
        {
            return AuthenticateViaOAuth2Password(username, password, clientId, null);
        }

        protected virtual GedcomxApplicationState AuthenticateViaOAuth2Password(String username, String password, String clientId, String clientSecret)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "password");
            formData.Add("username", username);
            formData.Add("password", password);
            formData.Add("client_id", clientId);
            if (clientSecret != null)
            {
                formData.Add("client_secret", clientSecret);
            }
            return AuthenticateViaOAuth2(formData);
        }

        protected GedcomxApplicationState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId)
        {
            return AuthenticateViaOAuth2Password(authCode, authCode, clientId, null);
        }

        protected GedcomxApplicationState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId, String clientSecret)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "authorization_code");
            formData.Add("code", authCode);
            formData.Add("redirect_uri", redirect);
            formData.Add("client_id", clientId);
            if (clientSecret != null)
            {
                formData.Add("client_secret", clientSecret);
            }
            return AuthenticateViaOAuth2(formData);
        }

        protected GedcomxApplicationState AuthenticateViaOAuth2ClientCredentials(String clientId, String clientSecret)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "client_credentials");
            formData.Add("client_id", clientId);
            if (clientSecret != null)
            {
                formData.Add("client_secret", clientSecret);
            }
            return AuthenticateViaOAuth2(formData);
        }

        protected GedcomxApplicationState AuthenticateWithAccessToken(String accessToken)
        {
            this.CurrentAccessToken = accessToken;
            return this;
        }

        protected GedcomxApplicationState AuthenticateViaOAuth2(IDictionary<String, String> formData, params StateTransitionOption[] options)
        {
            Link tokenLink = GetLink(Rel.OAUTH2_TOKEN);
            if (tokenLink == null || tokenLink.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("No OAuth2 token URI supplied for resource at {0}.", GetUri()));
            }

            IRestRequest request = CreateRequest();

            request.SetDataFormat(DataFormat.Json);
            request.AddHeader("Content-Type", MediaTypes.APPLICATION_FORM_URLENCODED_TYPE);
#warning Need to resolve ".entity(formData)" pattern
            foreach (var key in formData.Keys)
            {
                request.AddParameter(key, formData[key]);
            }

            request.Resource = tokenLink.Href;
            request.Method = Method.POST;

            IRestResponse response = Invoke(request, options);

            // TODO: Confirm response status SUCCESS = ResponseStatus.Completed
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
#warning Once entity is encoded correctly, need to confirm retrieval pattern
                var accessToken = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
                String access_token = null;

                if (accessToken.ContainsKey("access_token"))
                {
                    access_token = accessToken["access_token"] as string;
                }
                if (access_token == null && accessToken.ContainsKey("token"))
                {
                    //workaround to accommodate providers that were built on an older version of the oauth2 specification.
                    access_token = accessToken["token"] as string;
                }

                if (access_token == null)
                {
                    throw new GedcomxApplicationException("Illegal access token response: no access_token provided.", response);
                }

                return AuthenticateWithAccessToken(access_token);
            }
            else
            {
                throw new GedcomxApplicationException("Unable to obtain an access token.", response);
            }
        }

        protected IRestRequest CreateAuthenticatedRequest()
        {
            IRestRequest request = CreateRequest();
            if (this.CurrentAccessToken != null)
            {
                request = request.AddHeader("Authorization", "Bearer " + this.CurrentAccessToken);
            }
            return request;
        }

        protected IRestRequest CreateAuthenticatedGedcomxRequest()
        {
            IRestRequest result = CreateAuthenticatedRequest();

            // TODO: Confirm headers are replaced and not added twice
            result.AddHeader("Accept", MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);
            result.AddHeader("Content-Type", MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);

            return result;
        }

        protected IRestRequest CreateRequest()
        {
            return new RestRequest();
        }

        public Uri GetSelfUri()
        {
            String selfRel = GetSelfRel();
            Link link = null;
            if (selfRel != null)
            {
                link = GetLink(selfRel);
            }
            link = link == null ? GetLink(Rel.SELF) : link;
            Uri self = link == null ? null : link.Href == null ? null : new Uri(link.Href);
            return self == null ? GetUri() : self;
        }

        public String GetSelfRel()
        {
            return null;
        }

        public Link GetLink(String rel)
        {
            return this.links.Where(x => x.Key == rel).Select(x => x.Value).FirstOrDefault();
        }
    }
}

