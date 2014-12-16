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
using Gx.Common;
using Gedcomx.Model;
using Gedcomx.Support;

namespace Gx.Rs.Api
{
    /// <summary>
    /// This is the base class for all state instances.
    /// </summary>
    public abstract class GedcomxApplicationState : HypermediaEnabledData
    {
        /// <summary>
        /// The factory responsible for creating new state instances from REST API response data.
        /// </summary>
        protected internal readonly StateFactory stateFactory;
        /// <summary>
        /// The default link loader for reading links from <see cref="Gx.Gedcomx"/> instances. Also see <seealso cref="Gx.Rs.Api.Util.EmbeddedLinkLoader.DEFAULT_EMBEDDED_LINK_RELS"/> for types of links that will be loaded.
        /// </summary>
        protected static readonly EmbeddedLinkLoader DEFAULT_EMBEDDED_LINK_LOADER = new EmbeddedLinkLoader();
        private readonly String gzipSuffix = "-gzip";
        /// <summary>
        /// Gets or sets the main REST API client to use with all API calls.
        /// </summary>
        /// <value>
        /// The REST API client to use with all API calls.
        /// </value>
        public IFilterableRestClient Client { get; protected set; }
        /// <summary>
        /// Gets or sets the current access token (the OAuth2 token), see https://familysearch.org/developers/docs/api/authentication/Access_Token_resource.
        /// </summary>
        /// <value>
        /// The current access token (the OAuth2 token), see https://familysearch.org/developers/docs/api/authentication/Access_Token_resource.
        /// </value>
        public String CurrentAccessToken { get; set; }
        /// <summary>
        /// The link factory for managing RFC 5988 compliant hypermedia links.
        /// </summary>
        protected Tavis.LinkFactory linkFactory;
        /// <summary>
        /// The parser for extracting RFC 5988 compliant hypermedia links from a web response header.
        /// </summary>
        protected Tavis.LinkHeaderParser linkHeaderParser;
        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get { return CurrentAccessToken != null; } }
        /// <summary>
        /// Gets or sets the REST API request.
        /// </summary>
        /// <value>
        /// The REST API request.
        /// </value>
        public IRestRequest Request { get; protected set; }
        /// <summary>
        /// Gets or sets the REST API response.
        /// </summary>
        /// <value>
        /// The REST API response.
        /// </value>
        public IRestResponse Response { get; protected set; }
        /// <summary>
        /// Gets or sets the last embedded request (from a previous call to GedcomxApplicationState{T}.Embed{T}()).
        /// </summary>
        /// <value>
        /// The last embedded request (from a previous call to GedcomxApplicationState{T}.Embed{T}()).
        /// </value>
        public IRestRequest LastEmbeddedRequest { get; set; }
        /// <summary>
        /// Gets or sets the last embedded response (from a previous call to GedcomxApplicationState{T}.Embed{T}()).
        /// </summary>
        /// <value>
        /// The last embedded response (from a previous call to GedcomxApplicationState{T}.Embed{T}()).
        /// </value>
        public IRestResponse LastEmbeddedResponse { get; set; }

        /// <summary>
        /// Gets the entity tag of the entity represented by this instance.
        /// </summary>
        /// <value>
        /// The entity tag of the entity represented by this instance.
        /// </value>
        public string ETag
        {
            get
            {
                var result = this.Response != null ? this.Response.Headers.Get("ETag").Select(x => x.Value.ToString()).FirstOrDefault() : null;
                if (result != null && result.IndexOf(gzipSuffix) != -1)
                {
                    result = result.Replace(gzipSuffix, String.Empty);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the last modified date of the entity represented by this instance.
        /// </summary>
        /// <value>
        /// The last modified date of the entity represented by this instance.
        /// </value>
        public DateTime? LastModified
        {
            get
            {
                return this.Response != null ? this.Response.Headers.Get("Last-Modified").Select(x => (DateTime?)DateTime.Parse(x.Value.ToString())).FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// Gets the link loader for reading links from <see cref="Gx.Gedcomx"/> instances. Also see <seealso cref="Gx.Rs.Api.Util.EmbeddedLinkLoader.DEFAULT_EMBEDDED_LINK_RELS"/> for types of links that will be loaded.
        /// </summary>
        /// <value>
        /// This always returns the default embedded link loader.
        /// </value>
        protected EmbeddedLinkLoader EmbeddedLinkLoader
        {
            get
            {
                return DEFAULT_EMBEDDED_LINK_LOADER;
            }
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected abstract SupportsLinks MainDataElement
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationState{T}"/> class.
        /// </summary>
        protected GedcomxApplicationState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
        {
            linkFactory = new Tavis.LinkFactory();
            linkHeaderParser = new Tavis.LinkHeaderParser(linkFactory);
            this.Request = request;
            this.Response = response;
            this.Client = client;
            this.CurrentAccessToken = accessToken;
            this.stateFactory = stateFactory;
        }

        /// <summary>
        /// Executes the specified link and embeds the response in the specified Gedcomx entity.
        /// </summary>
        /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
        /// <param name="link">The link to execute.</param>
        /// <param name="entity">The entity which will embed the reponse data.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <exception cref="GedcomxApplicationException">Thrown when the server responds with HTTP status code >= 500 and &lt; 600.</exception>
        protected void Embed<T>(Link link, Gedcomx entity, params StateTransitionOption[] options) where T : Gedcomx
        {
            if (link.Href != null)
            {
                LastEmbeddedRequest = CreateRequestForEmbeddedResource(link.Rel).Build(link.Href, Method.GET);
                LastEmbeddedResponse = Invoke(LastEmbeddedRequest, options);
                if (LastEmbeddedResponse.StatusCode == HttpStatusCode.OK)
                {
                    entity.Embed(LastEmbeddedResponse.ToIRestResponse<T>().Data);
                }
                else if (LastEmbeddedResponse.HasServerError())
                {
                    throw new GedcomxApplicationException(String.Format("Unable to load embedded resources: server says \"{0}\" at {1}.", LastEmbeddedResponse.StatusDescription, LastEmbeddedRequest.Resource), LastEmbeddedResponse);
                }
                else
                {
                    //todo: log a warning? throw an error?
                }
            }
        }

        /// <summary>
        /// Creates a REST API request (with appropriate authentication headers).
        /// </summary>
        /// <param name="rel">This parameter is currently unused.</param>
        /// <returns>A REST API requeset (with appropriate authentication headers).</returns>
        protected virtual IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            return CreateAuthenticatedGedcomxRequest();
        }

        /// <summary>
        /// Applies the specified options before calling IFilterableRestClient.Handle() which applies any filters before executing the request.
        /// </summary>
        /// <param name="request">The REST API request.</param>
        /// <param name="options">The options to applying before the request is handled.</param>
        /// <returns>The REST API response after being handled.</returns>
        protected internal IRestResponse Invoke(IRestRequest request, params StateTransitionOption[] options)
        {
            IRestResponse result;

            foreach (StateTransitionOption option in options)
            {
                option.Apply(request);
            }

            result = this.Client.Handle(request);

            return result;
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected abstract GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client);

        /// <summary>
        /// Creates a REST API request with authentication.
        /// </summary>
        /// <returns>The REST API request with authentication.</returns>
        /// <remarks>This also sets the accept and content type headers.</remarks>
        protected internal IRestRequest CreateAuthenticatedGedcomxRequest()
        {
            return CreateAuthenticatedRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).ContentType(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);
        }

        /// <summary>
        /// Creates a REST API request with authentication.
        /// </summary>
        /// <returns>The REST API request with authentication.</returns>
        protected IRestRequest CreateAuthenticatedRequest()
        {
            IRestRequest request = CreateRequest();
            if (this.CurrentAccessToken != null)
            {
                request = request.AddHeader("Authorization", "Bearer " + this.CurrentAccessToken);
            }
            return request;
        }

        /// <summary>
        /// Creates a basic REST API request.
        /// </summary>
        /// <returns>A basic REST API request</returns>
        protected IRestRequest CreateRequest()
        {
            return new RestRequest();
        }

        /// <summary>
        /// Extracts embedded links from the specified entity, calls each one, and embeds the response into the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
        /// <param name="entity">The entity with links and which shall have the data loaded into.</param>
        /// <param name="options">The options to apply before handling the REST API requests.</param>
        protected void IncludeEmbeddedResources<T>(Gedcomx entity, params StateTransitionOption[] options) where T : Gedcomx
        {
            Embed<T>(EmbeddedLinkLoader.LoadEmbeddedLinks(entity), entity, options);
        }

        /// <summary>
        /// Executes the specified links and embeds the response into the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
        /// <param name="links">The links to call.</param>
        /// <param name="entity">The entity which shall have the data loaded into.</param>
        /// <param name="options">The options to apply before handling the REST API requests.</param>
        protected void Embed<T>(IEnumerable<Link> links, Gedcomx entity, params StateTransitionOption[] options) where T : Gedcomx
        {
            foreach (Link link in links)
            {
                Embed<T>(link, entity, options);
            }
        }

        /// <summary>
        /// Gets the URI of the REST API request associated to this state instance.
        /// </summary>
        /// <returns>The URI of the REST API request associated to this state instance.</returns>
        public string GetUri()
        {
            return this.Client.BaseUrl + this.Request.Resource;
        }

        /// <summary>
        /// Determines whether this instance has error (server [code >= 500 and &lt; 600] or client [code >= 400 and &lt; 500]).
        /// </summary>
        /// <returns>True if a server or client error exists; otherwise, false.</returns>
        public bool HasError()
        {
            return this.Response.HasClientError() || this.Response.HasServerError();
        }

        /// <summary>
        /// Determines whether the current REST API response has the specified status.
        /// </summary>
        /// <param name="status">The status to evaluate.</param>
        /// <returns>True if the current REST API response has the specified status; otherwise, false.</returns>
        public bool HasStatus(HttpStatusCode status)
        {
            return this.Response.StatusCode == status;
        }

        /// <summary>
        /// Returns the current state instance if there are no errors in the current REST API response; otherwise, it throws an exception with the response details.
        /// </summary>
        /// <returns>
        /// A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response or throws an exception with the response details.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if <see cref="HasError()" /> returns true.</exception>
        public virtual GedcomxApplicationState IfSuccessful()
        {
            if (HasError())
            {
                throw new GedcomxApplicationException(String.Format("Unsuccessful {0} to {1}", this.Request.Method, GetUri()), this.Response);
            }
            return this;
        }

        /// <summary>
        /// Executes a HEAD verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Head(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }

            request.Build(GetSelfUri(), Method.HEAD);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Executes an OPTIONS verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Options(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            request.Build(GetSelfUri(), Method.OPTIONS);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Executes a GET verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Get(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }

            request.Build(GetSelfUri(), Method.GET);
            IRestResponse response = Invoke(request, options);
            return Clone(request, response, this.Client);
        }

        /// <summary>
        /// Executes an DELETE verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Delete(params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            request.Build(GetSelfUri(), Method.DELETE);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Executes a PUT verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="entity">The entity to be used as the body of the REST API request. This is the entity to be PUT.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Put(object entity, params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            Parameter contentType = this.Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.SetEntity(entity).Build(GetSelfUri(), Method.PUT);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Executes a POST verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="entity">The entity to be used as the body of the REST API request. This is the entity to be POST.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Post(object entity, params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            Parameter contentType = this.Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Gets the warning headers from the current REST API response.
        /// </summary>
        /// <value>
        /// The warning headers from the current REST API response.
        /// </value>
        public List<HttpWarning> Warnings
        {
            get
            {
                List<HttpWarning> warnings = new List<HttpWarning>();
                IEnumerable<Parameter> warningValues = this.Response.Headers.Get("Warning");
                if (warningValues != null)
                {
                    foreach (Parameter warningValue in warningValues)
                    {
                        warnings.AddRange(HttpWarning.Parse(warningValue));
                    }
                }
                return warnings;
            }
        }

        /// <summary>
        /// Authenticates this session via OAuth2 password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public virtual GedcomxApplicationState AuthenticateViaOAuth2Password(String username, String password, String clientId)
        {
            return AuthenticateViaOAuth2Password(username, password, clientId, null);
        }

        /// <summary>
        /// Authenticates this session via OAuth2 password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public virtual GedcomxApplicationState AuthenticateViaOAuth2Password(String username, String password, String clientId, String clientSecret)
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

        /// <summary>
        /// Authenticates this session via OAuth2 authentication code.
        /// </summary>
        /// <param name="authCode">The authentication code.</param>
        /// <param name="redirect">The redirect.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId)
        {
            return AuthenticateViaOAuth2Password(authCode, authCode, clientId, null);
        }

        /// <summary>
        /// Authenticates this session via OAuth2 authentication code.
        /// </summary>
        /// <param name="authCode">The authentication code.</param>
        /// <param name="redirect">The redirect.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2AuthCode(String authCode, String redirect, String clientId, String clientSecret)
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

        /// <summary>
        /// Authenticates this session via OAuth2 client credentials.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2ClientCredentials(String clientId, String clientSecret)
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

        /// <summary>
        /// Creates a state instance without authentication. It will produce an access token, but only good for requests that do not need authentication.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState UnauthenticatedAccess(string ipAddress, string clientId, string clientSecret = null)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);
            if (clientSecret != null)
            {
                formData.Add("client_secret", clientSecret);
            }
            return AuthenticateViaOAuth2(formData);
        }

        /// <summary>
        /// Sets the current access token to the one specified. The server is not contacted during this operation.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>Returns this instance.</returns>
        public GedcomxApplicationState AuthenticateWithAccessToken(String accessToken)
        {
            this.CurrentAccessToken = accessToken;
            return this;
        }

        /// <summary>
        /// Authenticates this session via OAuth2.
        /// </summary>
        /// <param name="formData">The form data.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <exception cref="GedcomxApplicationException">
        /// Illegal access token response: no access_token provided.
        /// or
        /// Unable to obtain an access token.
        /// </exception>
        public GedcomxApplicationState AuthenticateViaOAuth2(IDictionary<String, String> formData, params StateTransitionOption[] options)
        {
            Link tokenLink = this.GetLink(Rel.OAUTH2_TOKEN);
            if (tokenLink == null || tokenLink.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("No OAuth2 token URI supplied for resource at {0}.", GetUri()));
            }

            IRestRequest request = CreateRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            IRestResponse response = Invoke(request, options);

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
            {
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

        /// <summary>
        /// Reads a page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="rel">The rel name to use when looking for the link.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public GedcomxApplicationState ReadPage(String rel, params StateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedRequest();
            Parameter accept = this.Request.GetHeaders().Get("Accept").FirstOrDefault();
            Parameter contentType = this.Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.Build(link.Href, Method.GET);
            return Clone(request, Invoke(request, options), this.Client);
        }

        /// <summary>
        /// Reads the next page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.NEXT.</remarks>
        public GedcomxApplicationState ReadNextPage(params StateTransitionOption[] options)
        {
            return ReadPage(Rel.NEXT, options);
        }

        /// <summary>
        /// Reads the previous page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.PREVIOUS.</remarks>
        public GedcomxApplicationState ReadPreviousPage(params StateTransitionOption[] options)
        {
            return ReadPage(Rel.PREVIOUS, options);
        }

        /// <summary>
        /// Reads the first page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.FIRST.</remarks>
        public GedcomxApplicationState ReadFirstPage(params StateTransitionOption[] options)
        {
            return ReadPage(Rel.FIRST, options);
        }

        /// <summary>
        /// Reads the last page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.LAST.</remarks>
        public GedcomxApplicationState ReadLastPage(params StateTransitionOption[] options)
        {
            return ReadPage(Rel.LAST, options);
        }

        /// <summary>
        /// Creates an authenticated feed request by attaching the authentication token and specifying the accept type.
        /// </summary>
        /// <returns>A REST API requeset (with appropriate authentication headers).</returns>
        protected IRestRequest CreateAuthenticatedFeedRequest()
        {
            return CreateAuthenticatedRequest().Accept(MediaTypes.ATOM_GEDCOMX_JSON_MEDIA_TYPE);
        }

        /// <summary>
        /// Reads the contributor for the current state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>An <see cref="AgentState"/> instance containing the REST API response.</returns>
        public AgentState ReadContributor(params StateTransitionOption[] options)
        {
            var scope = MainDataElement;
            if (scope is Attributable)
            {
                return ReadContributor((Attributable)scope, options);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the contributor for the specified <see cref="Attributable"/>.
        /// </summary>
        /// <param name="attributable">The attributable.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>An <see cref="AgentState"/> instance containing the REST API response.</returns>
        public AgentState ReadContributor(Attributable attributable, params StateTransitionOption[] options)
        {
            Attribution attribution = attributable.Attribution;
            if (attribution == null)
            {
                return null;
            }

            return ReadContributor(attribution.Contributor, options);
        }

        /// <summary>
        /// Reads the contributor for the specified <see cref="ResourceReference"/>.
        /// </summary>
        /// <param name="contributor">The contributor.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>An <see cref="AgentState"/> instance containing the REST API response.</returns>
        public AgentState ReadContributor(ResourceReference contributor, params StateTransitionOption[] options)
        {
            if (contributor == null || contributor.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(contributor.Resource, Method.GET);
            return this.stateFactory.NewAgentState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Gets the headers of the current REST API response.
        /// </summary>
        /// <value>
        /// The headers of the current REST API response.
        /// </value>
        public IList<Parameter> Headers
        {
            get
            {
                return Response.Headers;
            }
        }

        /// <summary>
        /// Gets the URI representing this current state instance.
        /// </summary>
        /// <returns>The URI representing this current state instance</returns>
        public string GetSelfUri()
        {
            String selfRel = SelfRel;
            Link link = null;
            if (selfRel != null)
            {
                link = this.GetLink(selfRel);
            }
            link = link == null ? this.GetLink(Rel.SELF) : link;
            String self = link == null ? null : link.Href == null ? null : link.Href;
            return self == null ? GetUri() : self;
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public virtual String SelfRel
        {
            get
            {
                return null;
            }
        }
    }

    /// <summary>
    /// This is the base class for all state instances with generic specific functionality.
    /// </summary>
    /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
    public abstract class GedcomxApplicationState<T> : GedcomxApplicationState where T : class, new()
    {
        /// <summary>
        /// Gets the entity represented by this state (if applicable). Not all responses produce entities.
        /// </summary>
        /// <value>
        /// The entity represented by this state.
        /// </value>
        public T Entity { get; private set; }
        /// <summary>
        /// Returns the entity from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The entity from the REST API response.</returns>
        protected virtual T LoadEntity(IRestResponse response)
        {
            T result = null;

            if (response != null)
            {
                result = response.ToIRestResponse<T>().Data;
            }

            return result;
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override SupportsLinks MainDataElement
        {
            get
            {
                return (SupportsLinks)Entity;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationState{T}"/> class.
        /// </summary>
        /// <param name="request">The REST API request.</param>
        /// <param name="response">The REST API response.</param>
        /// <param name="client">The REST API client.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="stateFactory">The state factory.</param>
        protected GedcomxApplicationState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
            this.Entity = LoadEntityConditionally(this.Response);
            List<Link> links = LoadLinks(this.Response, this.Entity, this.Request.RequestFormat);
            this.Links = new List<Link>();
            this.Links.AddRange(links);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was not a HEAD or OPTIONS request and the response status is OK.</remarks>
        protected virtual T LoadEntityConditionally(IRestResponse response)
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

        /// <summary>
        /// Invokes the specified REST API request and returns a state instance of the REST API response.
        /// </summary>
        /// <param name="request">The REST API request to execute.</param>
        /// <returns>The state instance of the REST API response.</returns>
        public GedcomxApplicationState Inject(IRestRequest request)
        {
            return Clone(request, Invoke(request), this.Client);
        }

        /// <summary>
        /// Loads all links from a REST API response and entity object, whether from the header, response body, or any other properties available to extract useful links for this state instance.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <param name="entity">The entity to also consider for finding links.</param>
        /// <param name="contentFormat">The content format (JSON or XML) of the REST API response data.</param>
        /// <returns>A list of all links discovered from the REST API response and entity object.</returns>
        protected List<Link> LoadLinks(IRestResponse response, T entity, DataFormat contentFormat)
        {
            List<Link> links = new List<Link>();
            var location = response.Headers.FirstOrDefault(x => x.Name == "Location");

            //if there's a location, we'll consider it a "self" link.
            if (location != null && location.Value != null)
            {
                links.Add(new Link() { Rel = Rel.SELF, Href = location.Value.ToString() });
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
            SupportsLinks mainElement = MainDataElement;
            if (mainElement != null && mainElement.Links != null)
            {
                links.AddRange(mainElement.Links);
            }

            //load links at the document level
            var collection = entity as SupportsLinks;
            if (entity != mainElement && collection != null && collection.Links != null)
            {
                links.AddRange(collection.Links);
            }

            return links;
        }
    }
}

