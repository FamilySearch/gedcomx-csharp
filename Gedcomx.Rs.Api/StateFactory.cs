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
    public class StateFactory
    {
        protected static readonly String ENABLE_LOG4NET_LOGGING_ENV_NAME = "enableLog4NetLogging";        // env variable/property to set

        public CollectionState NewCollectionState(Uri discoveryUri)
        {
            return NewCollectionState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public CollectionState NewCollectionState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewCollectionState(discoveryUri, client, Method.GET);
        }

        public CollectionState NewCollectionState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewCollectionState(request, client.Handle(request), client, null);
        }

        public PersonState NewPersonState(Uri discoveryUri)
        {
            return NewPersonState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public PersonState NewPersonState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewPersonState(discoveryUri, client, Method.GET);
        }

        public PersonState NewPersonState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPersonState(request, client.Handle(request), client, null);
        }

        public RecordState NewRecordState(Uri discoveryUri)
        {
            return NewRecordState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        public RecordState NewRecordState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewRecordState(discoveryUri, client, Method.GET);
        }

        public RecordState NewRecordState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewRecordState(request, client.Handle(request), client, null);
        }

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

        protected internal virtual AgentState NewAgentState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new AgentState(request, response, client, accessToken, this);
        }

        protected internal virtual AncestryResultsState NewAncestryResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new AncestryResultsState(request, response, client, accessToken, this);
        }

        protected internal virtual CollectionsState NewCollectionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new CollectionsState(request, response, client, accessToken, this);
        }

        protected internal virtual CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new CollectionState(request, response, client, accessToken, this);
        }

        protected internal virtual DescendancyResultsState NewDescendancyResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DescendancyResultsState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonChildrenState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonParentsState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonSearchResultsState NewPersonSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonSearchResultsState(request, response, client, accessToken, this);
        }

        protected internal virtual PlaceSearchResultsState NewPlaceSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceSearchResultsState(request, response, client, accessToken, this);
        }

        protected internal virtual PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceDescriptionState(request, response, client, accessToken, this);
        }

        protected internal virtual PlaceDescriptionsState NewPlaceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceDescriptionsState(request, response, client, accessToken, this);
        }

        public PlaceGroupState NewPlaceGroupState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PlaceGroupState(request, response, client, accessToken, this);
        }

        public VocabElementState NewVocabElementState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new VocabElementState(request, response, client, accessToken, this);
        }

        public VocabElementListState NewVocabElementListState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new VocabElementListState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonSpousesState NewPersonSpousesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonSpousesState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonsState NewPersonsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonsState(request, response, client, accessToken, this);
        }

        protected internal virtual PersonState NewPersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonState(request, response, client, accessToken, this);
        }

        protected internal virtual RecordsState NewRecordsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RecordsState(request, response, client, accessToken, this);
        }

        protected internal virtual RecordState NewRecordState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RecordState(request, response, client, accessToken, this);
        }

        protected internal virtual RelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RelationshipsState(request, response, client, accessToken, this);
        }

        protected internal virtual RelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new RelationshipState(request, response, client, accessToken, this);
        }

        protected internal virtual SourceDescriptionsState NewSourceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new SourceDescriptionsState(request, response, client, accessToken, this);
        }

        protected internal virtual SourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new SourceDescriptionState(request, response, client, accessToken, this);
        }
    }
}
