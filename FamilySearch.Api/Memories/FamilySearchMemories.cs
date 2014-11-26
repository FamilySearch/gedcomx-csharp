using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using FamilySearch.Api.Ft;
using Gedcomx.Support;

namespace FamilySearch.Api.Memories
{
    /// <summary>
    /// The FamilySearchMemories is a collection of FamilySearch memories and exposes management of those memories.
    /// </summary>
    public class FamilySearchMemories : FamilySearchCollectionState
    {
        /// <summary>
        /// The default production environment URI for this collection.
        /// </summary>
        public static readonly String URI = "https://familysearch.org/platform/collections/memories";
        /// <summary>
        /// The default sandbox environment URI for this collection.
        /// </summary>
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/memories";

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class using the production environment URI.
        /// </summary>
        public FamilySearchMemories()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="sandbox">If set to <c>true</c> this will use the sandbox environment URI; otherwise, it will use production.</param>
        public FamilySearchMemories(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        public FamilySearchMemories(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchMemories(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchMemories(Uri uri, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchMemories(IRestRequest request, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchMemories"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected FamilySearchMemories(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchMemories(request, response, this.Client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Creates a state instance without authentication. It will produce an access token, but only good for requests that do not need authentication.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A <see cref="FamilySearchMemories"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public FamilySearchMemories AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchMemories)this.AuthenticateViaOAuth2(formData);
        }
    }
}
