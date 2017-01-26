using Gx.Rs.Api;
using Gx.Rs.Api.Util;
using RestSharp;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The state factory is responsible for instantiating state classes from REST API responses.
    /// </summary>
    public class FamilyTreeStateFactory : FamilySearchStateFactory
    {
        /// <summary>
        /// Instantiates a new FamilySearchFamilyTree using the production environment URI.
        /// </summary>
        /// <returns></returns>
        public FamilySearchFamilyTree NewFamilyTreeState()
        {
            return NewFamilyTreeState(true);
        }

        /// <summary>
        /// Instantiates a new FamilySearchFamilyTree using the environment specified.
        /// </summary>
        /// <param name="production">If set to <c>true</c>, the resulting FamilySearchFamilyTree will use the production environment URI; otherwise, it will use the sandbox environment URI.</param>
        /// <returns></returns>
        public FamilySearchFamilyTree NewFamilyTreeState(bool production)
        {
            return (FamilySearchFamilyTree)NewCollectionState(new Uri(production ? FamilySearchFamilyTree.URI : FamilySearchFamilyTree.SANDBOX_URI));
        }

        /// <summary>
        /// Instantiates a new FamilySearchFamilyTree using the environment URI specified.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <returns></returns>
        public FamilySearchFamilyTree NewFamilyTreeState(Uri discoveryUri)
        {
            return (FamilySearchFamilyTree)base.NewCollectionState(discoveryUri);
        }

        /// <summary>
        /// Creates a new child and parents relationship state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected internal ChildAndParentsRelationshipState NewChildAndParentsRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new ChildAndParentsRelationshipState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new relationships state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RelationshipsState"/> instance containing the REST API response.
        /// </returns>
        protected override RelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new relationships state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RelationshipsState"/> instance containing the REST API response.
        /// </returns>
        internal virtual RelationshipsState NewRelationshipsStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewRelationshipsState(request, response, client, accessToken);
        }

        /// <summary>
        /// Creates a new collection state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        protected override CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchFamilyTree(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        protected override PersonState NewPersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilyTreePersonState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new relationship state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected override RelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new relationship state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        internal virtual RelationshipState NewRelationshipStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewRelationshipState(request, response, client, accessToken);
        }

        /// <summary>
        /// Creates a new person parents state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonParentsState"/> instance containing the REST API response.
        /// </returns>
        protected override PersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilyTreePersonParentsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person children state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonChildrenState"/> instance containing the REST API response.
        /// </returns>
        protected override PersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilyTreePersonChildrenState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new change history state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="ChangeHistoryState"/> instance containing the REST API response.
        /// </returns>
        protected internal ChangeHistoryState NewChangeHistoryState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new ChangeHistoryState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new source descriptions state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        internal virtual SourceDescriptionsState NewSourceDescriptionsStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewSourceDescriptionsState(request, response, client, accessToken);
        }
    }
}
