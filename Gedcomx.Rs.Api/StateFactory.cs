using Gx.Atom;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;

namespace Gx.Rs.Api
{
    public class StateFactory
    {
        protected static readonly String ENABLE_JERSEY_LOGGING_ENV_NAME = "enableJerseyLogging";        // env variable/property to set

        public CollectionState NewCollectionState(Uri discoveryUri)
        {
            return NewCollectionState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public CollectionState NewCollectionState(Uri discoveryUri, IRestClient client)
        {
            return NewCollectionState(discoveryUri, client, Method.GET);
        }

        public CollectionState NewCollectionState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewCollectionState(request, client.Execute(request), client, null);
        }

        public PersonState NewPersonState(Uri discoveryUri)
        {
            return NewPersonState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public PersonState NewPersonState(Uri discoveryUri, IRestClient client)
        {
            return NewPersonState(discoveryUri, client, Method.GET);
        }

        public PersonState NewPersonState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPersonState(request, client.Execute(request), client, null);
        }

        public RecordState NewRecordState(Uri discoveryUri)
        {
            return NewRecordState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public RecordState NewRecordState(Uri discoveryUri, IRestClient client)
        {
            return NewRecordState(discoveryUri, client, Method.GET);
        }

        public RecordState NewRecordState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewRecordState(request, client.Execute(request), null);
        }

        internal IRestClient LoadDefaultClient(Uri uri)
        {
            IRestClient client;
            bool enableJerseyLogging;

            client = new RestClient(uri.GetBaseUrl())
            {
                FollowRedirects = false,
            };

            if (!bool.TryParse(Environment.GetEnvironmentVariable(ENABLE_JERSEY_LOGGING_ENV_NAME), out enableJerseyLogging))
            {
                // Default if environment variable is not found
                enableJerseyLogging = false;
            }

            if (enableJerseyLogging)
            {
                // handles null
                // TODO: client.addFilter(new com.sun.jersey.api.client.filter.LoggingFilter());
            }
            return client;
        }

        internal AgentState NewAgentState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new AgentState(request, response, accessToken, this);
        }

        internal AncestryResultsState NewAncestryResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
			return new AncestryResultsState(request, response, client, accessToken, this);
        }

        internal CollectionsState NewCollectionsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new CollectionsState(request, response, client, accessToken, this);
        }

        internal CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new CollectionState(request, response, client, accessToken, this);
        }

        internal DescendancyResultsState NewDescendancyResultsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new DescendancyResultsState(request, response, accessToken, this);
        }

        internal PersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonChildrenState(request, response, client, accessToken, this);
        }

        internal PersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonParentsState(request, response, client, accessToken, this);
        }

        internal PersonSearchResultsState NewPersonSearchResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonSearchResultsState(request, response, client, accessToken, this);
        }

        internal PlaceSearchResultsState NewPlaceSearchResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PlaceSearchResultsState(request, response, accessToken, this);
        }

        protected PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceDescriptionState(request, response, accessToken, this);
        }

        protected PlaceDescriptionsState NewPlaceDescriptionsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceDescriptionsState(request, response, accessToken, this);
        }

        protected PlaceGroupState NewPlaceGroupState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceGroupState(request, response, accessToken, this);
        }

        protected VocabElementState NewVocabElementState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new VocabElementState(request, response, accessToken, this);
        }

        protected VocabElementListState NewVocabElementListState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new VocabElementListState(request, response, accessToken, this);
        }

        internal PersonSpousesState NewPersonSpousesState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonSpousesState(request, response, client, accessToken, this);
        }

        internal PersonsState NewPersonsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonsState(request, response, client, accessToken, this);
        }

        internal PersonState NewPersonState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonState(request, response, client, accessToken, this);
        }

        internal RecordsState NewRecordsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new RecordsState(request, response, client, accessToken, this);
        }

        internal RecordState NewRecordState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new RecordState(request, response, accessToken, this);
        }

        internal RelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new RelationshipsState(request, response, client, accessToken, this);
        }

        internal RelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new RelationshipState(request, response, client, accessToken, this);
        }

        internal SourceDescriptionsState NewSourceDescriptionsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new SourceDescriptionsState(request, response, accessToken, this);
        }

        internal SourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new SourceDescriptionState(request, response, accessToken, this);
        }
    }
}
