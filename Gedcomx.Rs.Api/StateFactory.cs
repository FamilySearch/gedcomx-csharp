using Gx.Atom;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Support;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The state factory is responsible for instantiating state classes from REST API responses.
    /// </summary>
    public class StateFactory
    {
        /// <summary>
        /// This is the environment variable to use at runtime to determine if REST API request logging will occur.
        /// </summary>
        protected static readonly String ENABLE_LOG4NET_LOGGING_ENV_NAME = "enableLog4NetLogging";        // env variable/property to set

        /// <summary>
        /// Returns a new collection state by invoking the specified URI.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <returns>
        /// A <see cref="CollectionState" /> instance containing the REST API response.
        /// </returns>
        public CollectionState NewCollectionState(Uri discoveryUri)
        {
            return NewCollectionState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        /// <summary>
        /// Returns a new collection state by invoking the specified URI using the specified client.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState NewCollectionState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewCollectionState(discoveryUri, client, Method.GET);
        }

        /// <summary>
        /// Returns a new collection state by invoking the specified URI using the specified client and forcing the specific HTTP verb.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="method">The HTTP verb to use for invoking the discovery URI.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState NewCollectionState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewCollectionState(request, client.Handle(request), client, null);
        }

        /// <summary>
        /// Returns a new person state by invoking the specified URI.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState NewPersonState(Uri discoveryUri)
        {
            return NewPersonState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        /// <summary>
        /// Returns a new person state by invoking the specified URI using the specified client.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState NewPersonState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewPersonState(discoveryUri, client, Method.GET);
        }

        /// <summary>
        /// Returns a new person state by invoking the specified URI using the specified client and forcing the specific HTTP verb.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="method">The HTTP verb to use for invoking the discovery URI.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState NewPersonState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPersonState(request, client.Handle(request), client, null);
        }

        /// <summary>
        /// Returns a new record state by invoking the specified URI.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState NewRecordState(Uri discoveryUri)
        {
            return NewRecordState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        /// <summary>
        /// Returns a new record state by invoking the specified URI using the specified client.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState NewRecordState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewRecordState(discoveryUri, client, Method.GET);
        }

        /// <summary>
        /// Returns a new record state by invoking the specified URI using the specified client and forcing the specific HTTP verb.
        /// </summary>
        /// <param name="discoveryUri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="method">The HTTP verb to use for invoking the discovery URI.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState NewRecordState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewRecordState(request, client.Handle(request), client, null);
        }

        /// <summary>
        /// Loads the default client for executing REST API requests.
        /// </summary>
        /// <param name="uri">The base URI for all future REST API requests to use with this client.</param>
        /// <returns>
        /// A <see cref="IFilterableRestClient"/> with the default configuration and filters.</returns>
        /// <remarks>REST API request logging is disabled by default. To enable logging of REST API requests, set the environment variable
        /// "enableLog4NetLogging" to "True" within the scope of the execution context (or a greater scope). The environment variable will
        /// be evaluated only once and only during this method. After the client has been created using this method, the environment variable
        /// will not enable or disable client request logging.
        /// </remarks>
        protected internal virtual IFilterableRestClient LoadDefaultClient(Uri uri)
        {
            IFilterableRestClient client;
            bool enableJerseyLogging;

            client = new FilterableRestClient(uri.GetBaseUrl())
            {
                FollowRedirects = false,
            };

            if (!bool.TryParse(Environment.GetEnvironmentVariable(ENABLE_LOG4NET_LOGGING_ENV_NAME), out enableJerseyLogging))
            {
                // Default if environment variable is not found
                enableJerseyLogging = false;
            }

            if (enableJerseyLogging)
            {
                // handles null
                client.AddFilter(new Log4NetLoggingFilter());
            }
            return client;
        }

        /// <summary>
        /// Creates a new agent state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="AgentState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual AgentState NewAgentState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new AgentState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new ancestry results state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="AncestryResultsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual AncestryResultsState NewAncestryResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new AncestryResultsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new collections state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="CollectionsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual CollectionsState NewCollectionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new CollectionsState(request, response, client, accessToken, this);
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
        protected internal virtual CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new CollectionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new descendancy state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="DescendancyResultsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual DescendancyResultsState NewDescendancyResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DescendancyResultsState(request, response, client, accessToken, this);
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
        protected internal virtual PersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonChildrenState(request, response, client, accessToken, this);
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
        protected internal virtual PersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonParentsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person search results state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PersonSearchResultsState NewPersonSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonSearchResultsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new place search results state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PlaceSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PlaceSearchResultsState NewPlaceSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceSearchResultsState(request, response, client, accessToken, this);
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
        protected internal virtual PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceDescriptionState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new place descriptions state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PlaceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PlaceDescriptionsState NewPlaceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceDescriptionsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new place group state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PlaceGroupState"/> instance containing the REST API response.
        /// </returns>
        public PlaceGroupState NewPlaceGroupState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceGroupState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new vocab element state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="VocabElementState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementState NewVocabElementState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new VocabElementState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new vocab element list state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="VocabElementListState"/> instance containing the REST API response.
        /// </returns>
        public VocabElementListState NewVocabElementListState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new VocabElementListState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new person spouses state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonSpousesState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PersonSpousesState NewPersonSpousesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonSpousesState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new persons state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="PersonsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual PersonsState NewPersonsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonsState(request, response, client, accessToken, this);
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
        protected internal virtual PersonState NewPersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new records state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RecordsState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual RecordsState NewRecordsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RecordsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new record state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual RecordState NewRecordState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RecordState(request, response, client, accessToken, this);
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
        protected internal virtual RelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RelationshipsState(request, response, client, accessToken, this);
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
        protected internal virtual RelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RelationshipState(request, response, client, accessToken, this);
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
        protected internal virtual SourceDescriptionsState NewSourceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new SourceDescriptionsState(request, response, client, accessToken, this);
        }

        /// <summary>
        /// Creates a new source description state from the specified parameters. Since a response is provided as a parameter, a REST API request will not be invoked.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        protected internal virtual SourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new SourceDescriptionState(request, response, client, accessToken, this);
        }
    }
}
