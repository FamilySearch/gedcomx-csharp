using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public class StateFactory
    {
        protected static readonly String ENABLE_JERSEY_LOGGING_ENV_NAME = "enableJerseyLogging";        // env variable/property to set

        public CollectionState newCollectionState(Uri discoveryUri)
        {
            return newCollectionState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public CollectionState newCollectionState(Uri discoveryUri, IRestClient client)
        {
            return newCollectionState(discoveryUri, client, Method.GET);
        }

        public CollectionState newCollectionState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest(discoveryUri, method).AddHeader("Accept", MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);
            return newCollectionState(request, client.Execute<Gedcomx>(request), client, null);
        }

        public PersonState newPersonState(Uri discoveryUri)
        {
            return newPersonState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public PersonState newPersonState(Uri discoveryUri, IRestClient client)
        {
            return newPersonState(discoveryUri, client, Method.GET);
        }

        public PersonState newPersonState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest(discoveryUri, method).AddHeader("Accept", MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);
            return newPersonState(request, client.Execute(request), null);
        }

        public RecordState newRecordState(Uri discoveryUri)
        {
            return newRecordState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public RecordState newRecordState(Uri discoveryUri, IRestClient client)
        {
            return newRecordState(discoveryUri, client, Method.GET);
        }

        public RecordState newRecordState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest(discoveryUri, method).AddHeader("Accept", MediaTypes.GEDCOMX_JSON_MEDIA_TYPE);
            return newRecordState(request, client.Execute(request), null);
        }

        internal IRestClient LoadDefaultClient(Uri baseUri)
        {
            IRestClient client = new RestClient(baseUri.GetLeftPart(UriPartial.Authority));
            bool enableJerseyLogging;

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

        protected AgentState newAgentState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new AgentState(request, response, accessToken, this);
        }

        protected AncestryResultsState newAncestryResultsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new AncestryResultsState(request, response, accessToken, this);
        }

        protected CollectionsState newCollectionsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new CollectionsState(request, response, accessToken, this);
        }

        protected CollectionState newCollectionState(IRestRequest request, IRestResponse<Gedcomx> response, IRestClient client, String accessToken)
        {
            return new CollectionState(request, response, client, accessToken, this);
        }

        protected DescendancyResultsState newDescendancyResultsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new DescendancyResultsState(request, response, accessToken, this);
        }

        protected PersonChildrenState newPersonChildrenState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonChildrenState(request, response, accessToken, this);
        }

        protected PersonParentsState newPersonParentsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonParentsState(request, response, accessToken, this);
        }

        protected PersonSearchResultsState newPersonSearchResultsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonSearchResultsState(request, response, accessToken, this);
        }

        protected PlaceSearchResultsState newPlaceSearchResultsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceSearchResultsState(request, response, accessToken, this);
        }

        protected PlaceDescriptionState newPlaceDescriptionState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceDescriptionState(request, response, accessToken, this);
        }

        protected PlaceDescriptionsState newPlaceDescriptionsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceDescriptionsState(request, response, accessToken, this);
        }

        public PlaceGroupState newPlaceGroupState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PlaceGroupState(request, response, accessToken, this);
        }

        public VocabElementState newVocabElementState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new VocabElementState(request, response, accessToken, this);
        }

        public VocabElementListState newVocabElementListState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new VocabElementListState(request, response, accessToken, this);
        }

        protected PersonSpousesState newPersonSpousesState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonSpousesState(request, response, accessToken, this);
        }

        protected PersonsState newPersonsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonsState(request, response, accessToken, this);
        }

        protected PersonState newPersonState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new PersonState(request, response, accessToken, this);
        }

        protected RecordsState newRecordsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new RecordsState(request, response, accessToken, this);
        }

        protected RecordState newRecordState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new RecordState(request, response, accessToken, this);
        }

        protected RelationshipsState newRelationshipsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new RelationshipsState(request, response, accessToken, this);
        }

        protected RelationshipState newRelationshipState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new RelationshipState(request, response, accessToken, this);
        }

        protected SourceDescriptionsState newSourceDescriptionsState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new SourceDescriptionsState(request, response, accessToken, this);
        }

        protected SourceDescriptionState newSourceDescriptionState(IRestRequest request, IRestResponse response, String accessToken)
        {
            return new SourceDescriptionState(request, response, accessToken, this);
        }
    }
}
