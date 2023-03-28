using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

using Gedcomx.Model;
using Gedcomx.Support;

using Gx.Common;
using Gx.Links;
using Gx.Rs.Api.Util;

using Newtonsoft.Json;

using RestSharp;

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
        private readonly string gzipSuffix = "-gzip";
        /// <summary>
        /// Gets or sets the main REST API client to use with all API calls.
        /// </summary>
        /// <value>
        /// The REST API client to use with all API calls.
        /// </value>
        public IFilterableRestClient Client { get; protected set; }
        /// <summary>
        /// Gets or sets the current access token (the OAuth2 token), see https://www.familysearch.org/developers/docs/api/authentication/Access_Token_resource.
        /// </summary>
        /// <value>
        /// The current access token (the OAuth2 token), see https://www.familysearch.org/developers/docs/api/authentication/Access_Token_resource.
        /// </value>
        public string CurrentAccessToken { get; set; }
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
                var result = Response?.Headers.Get("ETag").Select(x => x.Value.ToString()).FirstOrDefault();
                if (result != null && result.IndexOf(gzipSuffix) != -1)
                {
                    result = result.Replace(gzipSuffix, string.Empty);
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
                return Response?.Headers.Get("Last-Modified").Select(x => (DateTime?)DateTime.Parse(x.Value.ToString())).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the link loader for reading links from <see cref="Gx.Gedcomx"/> instances. Also see <seealso cref="Gx.Rs.Api.Util.EmbeddedLinkLoader.DEFAULT_EMBEDDED_LINK_RELS"/> for types of links that will be loaded.
        /// </summary>
        /// <value>
        /// This always returns the default embedded link loader.
        /// </value>
        protected static EmbeddedLinkLoader EmbeddedLinkLoader
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
        protected abstract ISupportsLinks MainDataElement
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationState{T}"/> class.
        /// </summary>
        protected GedcomxApplicationState(IRestRequest request, IRestResponse response, IFilterableRestClient client, string accessToken, StateFactory stateFactory)
        {
            linkFactory = new Tavis.LinkFactory();
            linkHeaderParser = new Tavis.LinkHeaderParser(linkFactory);
            Request = request;
            Response = response;
            Client = client;
            CurrentAccessToken = accessToken;
            this.stateFactory = stateFactory;
        }

        /// <summary>
        /// Executes the specified link and embeds the response in the specified Gedcomx entity.
        /// </summary>
        /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
        /// <param name="link">The link to execute.</param>
        /// <param name="entity">The entity which will embed the reponse data.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <exception cref="GedcomxApplicationException">Thrown when the server responds with HTTP status code >= 500 and < 600.</exception>
        protected void Embed<T>(Link link, Gedcomx entity, params IStateTransitionOption[] options) where T : Gedcomx
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
                    throw new GedcomxApplicationException(string.Format("Unable to load embedded resources: server says \"{0}\" at {1}.", LastEmbeddedResponse.StatusDescription, LastEmbeddedRequest.Resource), LastEmbeddedResponse);
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
        protected virtual IRestRequest CreateRequestForEmbeddedResource(string rel)
        {
            return CreateAuthenticatedGedcomxRequest();
        }

        /// <summary>
        /// Applies the specified options before calling IFilterableRestClient.Handle() which applies any filters before executing the request.
        /// </summary>
        /// <param name="request">The REST API request.</param>
        /// <param name="options">The options to applying before the request is handled.</param>
        /// <returns>The REST API response after being handled.</returns>
        protected internal IRestResponse Invoke(IRestRequest request, params IStateTransitionOption[] options)
        {
            IRestResponse result;

            foreach (var option in options)
            {
                option.Apply(request);
            }

            result = Client.Handle(request);

            Debug.WriteLine(string.Format("\nRequest: {0}", request.Resource));
            foreach (var header in request.Parameters)
            {
                Debug.WriteLine(string.Format("{0} {1}", header.Name, header.Value));
            }
            Debug.WriteLine(string.Format("\nResponse Status: {0} {1}", result.StatusCode, result.StatusDescription));
            Debug.WriteLine(string.Format("\nResponse Content: {0}", result.Content));

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
            var request = CreateRequest();
            if (CurrentAccessToken != null)
            {
                request = request.AddHeader("Authorization", "Bearer " + CurrentAccessToken);
            }
            return request;
        }

        /// <summary>
        /// Creates a basic REST API request.
        /// </summary>
        /// <returns>A basic REST API request</returns>
        protected static IRestRequest CreateRequest()
        {
            return new RedirectableRestRequest();
        }

        /// <summary>
        /// Extracts embedded links from the specified entity, calls each one, and embeds the response into the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of the expected response. The raw response data will be parsed (from JSON or XML) and casted to this type.</typeparam>
        /// <param name="entity">The entity with links and which shall have the data loaded into.</param>
        /// <param name="options">The options to apply before handling the REST API requests.</param>
        protected void IncludeEmbeddedResources<T>(Gedcomx entity, params IStateTransitionOption[] options) where T : Gedcomx
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
        protected void Embed<T>(IEnumerable<Link> links, Gedcomx entity, params IStateTransitionOption[] options) where T : Gedcomx
        {
            foreach (var link in links)
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
            return Client.BaseUrl + Request.Resource;
        }

        /// <summary>
        /// Determines whether this instance has error (server [code >= 500 and < 600] or client [code >= 400 and < 500]).
        /// </summary>
        /// <returns>True if a server or client error exists; otherwise, false.</returns>
        public bool HasError()
        {
            return Response.HasClientError() || Response.HasServerError();
        }

        /// <summary>
        /// Determines whether the current REST API response has the specified status.
        /// </summary>
        /// <param name="status">The status to evaluate.</param>
        /// <returns>True if the current REST API response has the specified status; otherwise, false.</returns>
        public bool HasStatus(HttpStatusCode status)
        {
            return Response.StatusCode == status;
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
                throw new GedcomxApplicationException(string.Format("Unsuccessful {0} to {1}", Request.Method, GetUri()), Response);
            }
            return this;
        }

        /// <summary>
        /// Executes a HEAD verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Head(params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }

            request.Build(GetSelfUri(), Method.HEAD);
            return Clone(request, Invoke(request, options), Client);
        }

        /// <summary>
        /// Executes an OPTIONS verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Options(params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            request.Build(GetSelfUri(), Method.OPTIONS);
            return Clone(request, Invoke(request, options), Client);
        }

        /// <summary>
        /// Executes a GET verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Get(params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }

            request.Build(GetSelfUri(), Method.GET);
            var response = Invoke(request, options);
            return Clone(request, response, Client);
        }

        /// <summary>
        /// Executes an DELETE verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Delete(params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            request.Build(GetSelfUri(), Method.DELETE);
            return Clone(request, Invoke(request, options), Client);
        }

        /// <summary>
        /// Executes a PUT verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="entity">The entity to be used as the body of the REST API request. This is the entity to be PUT.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Put(object entity, params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            var contentType = Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.SetEntity(entity).Build(GetSelfUri(), Method.PUT);
            return Clone(request, Invoke(request, options), Client);
        }

        /// <summary>
        /// Executes a POST verb request against the current REST API request and returns a state instance with the response.
        /// </summary>
        /// <param name="entity">The entity to be used as the body of the REST API request. This is the entity to be POST.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        public virtual GedcomxApplicationState Post(object entity, params IStateTransitionOption[] options)
        {
            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            var contentType = Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return Clone(request, Invoke(request, options), Client);
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
                var warnings = new List<HttpWarning>();
                var warningValues = Response.Headers.Get("Warning");
                if (warningValues != null)
                {
                    foreach (var warningValue in warningValues)
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
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public virtual GedcomxApplicationState AuthenticateViaOAuth2Password(string username, string password, string clientId)
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
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public virtual GedcomxApplicationState AuthenticateViaOAuth2Password(string username, string password, string clientId, string clientSecret)
        {
            IDictionary<string, string> formData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password },
                { "client_id", clientId }
            };
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
        /// <param name="clientId">The client identifier.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2AuthCode(string authCode, string clientId)
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
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2AuthCode(string authCode, string redirect, string clientId, string clientSecret)
        {
            IDictionary<string, string> formData = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authCode },
                { "redirect_uri", redirect },
                { "client_id", clientId }
            };
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
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState AuthenticateViaOAuth2ClientCredentials(string clientId, string clientSecret)
        {
            IDictionary<string, string> formData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId }
            };
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
        /// <remarks>See https://www.familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public GedcomxApplicationState UnauthenticatedAccess(string ipAddress, string clientId, string clientSecret = null)
        {
            IDictionary<string, string> formData = new Dictionary<string, string>
            {
                { "grant_type", "unauthenticated_session" },
                { "client_id", clientId },
                { "ip_address", ipAddress }
            };
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
        public GedcomxApplicationState AuthenticateWithAccessToken(string accessToken)
        {
            CurrentAccessToken = accessToken;
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
        public GedcomxApplicationState AuthenticateViaOAuth2(IDictionary<string, string> formData, params IStateTransitionOption[] options)
        {
            var tokenLink = GetLink(Rel.OAUTH2_TOKEN);
            if (tokenLink == null || tokenLink.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("No OAuth2 token URI supplied for resource at {0}.", GetUri()));
            }

            var request = CreateRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .ContentType(MediaTypes.APPLICATION_FORM_URLENCODED_TYPE)
                .SetEntity(formData)
                .Build(tokenLink.Href, Method.POST);
            var response = Invoke(request, options);

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
            {
                var accessToken = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content);
                string access_token = null;

                if (accessToken.TryGetValue("access_token", out var value))
                {
                    access_token = value as string;
                }
                if (access_token == null && accessToken.TryGetValue("token", out var token))
                {
                    //workaround to accommodate providers that were built on an older version of the oauth2 specification.
                    access_token = token as string;
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
        public GedcomxApplicationState ReadPage(string rel, params IStateTransitionOption[] options)
        {
            var link = GetLink(rel);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedRequest();
            var accept = Request.GetHeaders().Get("Accept").FirstOrDefault();
            var contentType = Request.GetHeaders().Get("Content-Type").FirstOrDefault();
            if (accept != null)
            {
                request.Accept(accept.Value as string);
            }
            if (contentType != null)
            {
                request.ContentType(contentType.Value as string);
            }
            request.Build(link.Href, Method.GET);
            return Clone(request, Invoke(request, options), Client);
        }

        /// <summary>
        /// Reads the next page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.NEXT.</remarks>
        public GedcomxApplicationState ReadNextPage(params IStateTransitionOption[] options)
        {
            return ReadPage(Rel.NEXT, options);
        }

        /// <summary>
        /// Reads the previous page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.PREVIOUS.</remarks>
        public GedcomxApplicationState ReadPreviousPage(params IStateTransitionOption[] options)
        {
            return ReadPage(Rel.PREVIOUS, options);
        }

        /// <summary>
        /// Reads the first page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.FIRST.</remarks>
        public GedcomxApplicationState ReadFirstPage(params IStateTransitionOption[] options)
        {
            return ReadPage(Rel.FIRST, options);
        }

        /// <summary>
        /// Reads the last page of results (usually of type <see cref="Gx.Atom.Feed"/>).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="GedcomxApplicationState{T}"/> instance containing the REST API response.</returns>
        /// <remarks>This is a shorthand method for calling <see cref="ReadPage"/> and specifying Rel.LAST.</remarks>
        public GedcomxApplicationState ReadLastPage(params IStateTransitionOption[] options)
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
        public AgentState ReadContributor(params IStateTransitionOption[] options)
        {
            var scope = MainDataElement;
            if (scope is IAttributable attributable)
            {
                return ReadContributor(attributable, options);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the contributor for the specified <see cref="IAttributable"/>.
        /// </summary>
        /// <param name="attributable">The attributable.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>An <see cref="AgentState"/> instance containing the REST API response.</returns>
        public AgentState ReadContributor(IAttributable attributable, params IStateTransitionOption[] options)
        {
            var attribution = attributable.Attribution;
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
        public AgentState ReadContributor(ResourceReference contributor, params IStateTransitionOption[] options)
        {
            if (contributor == null || contributor.Resource == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(contributor.Resource, Method.GET);
            return stateFactory.NewAgentState(request, Invoke(request, options), Client, CurrentAccessToken);
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
            var selfRel = SelfRel;
            Link link = null;
            if (selfRel != null)
            {
                link = GetLink(selfRel);
            }
            link = link ?? GetLink(Rel.SELF);
            var self = link == null ? null : link.Href ?? null;
            return self ?? GetUri();
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public virtual string SelfRel
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
        protected override ISupportsLinks MainDataElement
        {
            get
            {
                return (ISupportsLinks)Entity;
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
        protected GedcomxApplicationState(IRestRequest request, IRestResponse response, IFilterableRestClient client, string accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
            Entity = LoadEntityConditionally(Response);
            var links = LoadLinks(Response, Entity);
            Links.AddRange(links);
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
            return Clone(request, Invoke(request), Client);
        }

        /// <summary>
        /// Loads all links from a REST API response and entity object, whether from the header, response body, or any other properties available to extract useful links for this state instance.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <param name="entity">The entity to also consider for finding links.</param>
        /// <returns>A list of all links discovered from the REST API response and entity object.</returns>
        protected List<Link> LoadLinks(IRestResponse response, T entity)
        {
            var links = new List<Link>();
            var location = response.Headers.FirstOrDefault(x => x.Name == "Location");

            //if there's a location, we'll consider it a "self" link.
            if (location != null && location.Value != null)
            {
                links.Add(new Link() { Rel = Rel.SELF, Href = location.Value.ToString() });
            }

            //initialize links with link headers
            foreach (var header in response.Headers.Where(x => x.Name == "Link" && x.Value != null).SelectMany(x => linkHeaderParser.Parse(response.ResponseUri, x.Value.ToString())))
            {
                var link = new Link
                {
                    Rel = header.Relation,
                    Href = header.Target.ToString(),
                    Template = header.LinkExtensions.Any(x => x.Key == "template") ? header.GetLinkExtension("template") : null,
                    Title = header.Title,
                    Accept = header.LinkExtensions.Any(x => x.Key == "accept") ? header.GetLinkExtension("accept") : null,
                    Allow = header.LinkExtensions.Any(x => x.Key == "allow") ? header.GetLinkExtension("allow") : null,
                    Hreflang = header.HrefLang.Select(x => x.Name).FirstOrDefault(),
                    Type = header.LinkExtensions.Any(x => x.Key == "type") ? header.GetLinkExtension("type") : null
                };
                links.Add(link);
            }

            //load the links from the main data element
            var mainElement = MainDataElement;
            if (mainElement != null && mainElement.Links != null)
            {
                links.AddRange(mainElement.Links);
            }

            //load links at the document level
            if (entity != mainElement && entity is ISupportsLinks collection && collection.Links != null)
            {
                links.AddRange(collection.Links);
            }

            return links;
        }
    }
}

