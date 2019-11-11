using Gx.Rs.Api;
using RestSharp;
using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using FamilySearch.Api.Util;
using Gedcomx.Support;

namespace FamilySearch.Api
{
    /// <summary>
    /// The state factory is responsible for instantiating state classes from REST API responses.
    /// </summary>
    public class FamilySearchStateFactory : StateFactory
    {
        /// <summary>
        /// Creates a new discussions state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="DiscussionsState"/> instance containing the REST API response.
        /// </returns>
        protected internal DiscussionsState NewDiscussionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DiscussionsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new discussion state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        protected internal DiscussionState NewDiscussionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DiscussionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new user state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="UserState"/> instance containing the REST API response.
        /// </returns>
        protected internal UserState NewUserState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new UserState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person merge state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        protected internal PersonMergeState NewPersonMergeState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonMergeState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person match results state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonMatchResultsState"/> instance containing the REST API response.
        /// </returns>
        protected internal PersonMatchResultsState NewPersonMatchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonMatchResultsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Returns a new FamilySearch places state by invoking the specified URI.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaces"/> instance containing the REST API response.
        /// </returns>
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri)
        {
            return NewPlacesState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        /// <summary>
        /// Returns a new FamilySearch places state by invoking the specified URI using the specified client.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaces"/> instance containing the REST API response.
        /// </returns>
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewPlacesState(discoveryUri, client, Method.GET);
        }

        /// <summary>
        /// Returns a new FamilySearch places state by invoking the specified URI using the specified client and forcing the specific HTTP verb.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="method">The HTTP verb to use for invoking the discovery URI.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaces"/> instance containing the REST API response.
        /// </returns>
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RedirectableRestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPlacesState(request, client.Handle(request), client, null);
        }

        /// <summary>
        /// Creates a new FamilySearch places state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaces"/> instance containing the REST API response.
        /// </returns>
        protected internal FamilySearchPlaces NewPlacesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaces(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new FamilySearch collection state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
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
            return new FamilySearchCollectionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new FamilySearch collection state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        internal virtual CollectionState NewCollectionStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewCollectionStateInt(request, response, client, accessToken);
        }

        /// <summary>
        /// Creates a new FamilySearch source description state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        protected override SourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchSourceDescriptionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new FamilySearch source description state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        internal virtual SourceDescriptionState NewSourceDescriptionStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewSourceDescriptionState(request, response, client, accessToken);
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
            return base.NewPersonState(request, response, client, accessToken);
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
        internal virtual PersonState NewPersonStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewPersonState(request, response, client, accessToken);
        }

        /// <summary>
        /// Creates a new person non-matches state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PersonNonMatchesState NewPersonNonMatchesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonNonMatchesState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new FamilySearch place state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaceState"/> instance containing the REST API response.
        /// </returns>
        protected internal FamilySearchPlaceState NewPlaceState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaceState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new place description state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PlaceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        protected override PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaceDescriptionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Loads the default client for executing REST API requests.
        /// </summary>
        /// <param name="uri">The base URI for all future REST API requests to use with this client.</param>
        /// <returns>
        /// A <see cref="IFilterableRestClient" /> with the default configuration and filters.
        /// </returns>
        /// <remarks>
        /// REST API request logging is disabled by default. To enable logging of REST API requests, set the environment variable
        /// "enableLog4NetLogging" to "True" within the scope of the execution context (or a greater scope). The environment variable will
        /// be evaluated only once and only during this method. After the client has been created using this method, the environment variable
        /// will not enable or disable client request logging.
        /// 
        /// This specific overload enables the "birth-date-not-considered-death-declaration" feature for every client created.
        /// </remarks>
        protected override IFilterableRestClient LoadDefaultClient(Uri uri)
        {
            var client = base.LoadDefaultClient(uri);

            //how to add an experiment:
            client.AddFilter(new ExperimentsFilter("birth-date-not-considered-death-declaration"));

            return client;
        }

        /// <summary>
        /// Loads the default client for executing REST API requests.
        /// </summary>
        /// <param name="uri">The base URI for all future REST API requests to use with this client.</param>
        /// <returns>
        /// A <see cref="IFilterableRestClient" /> with the default configuration and filters.
        /// </returns>
        /// <remarks>
        /// REST API request logging is disabled by default. To enable logging of REST API requests, set the environment variable
        /// "enableLog4NetLogging" to "True" within the scope of the execution context (or a greater scope). The environment variable will
        /// be evaluated only once and only during this method. After the client has been created using this method, the environment variable
        /// will not enable or disable client request logging.
        /// 
        /// This specific overload enables the "birth-date-not-considered-death-declaration" feature for every client created.
        /// </remarks>
        internal virtual IFilterableRestClient LoadDefaultClientInt(Uri uri)
        {
            return this.LoadDefaultClient(uri);
        }
    }
}
