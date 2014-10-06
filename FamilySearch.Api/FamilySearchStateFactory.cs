using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using FamilySearch.Api.Util;
using Gedcomx.Support;

namespace FamilySearch.Api
{
    public class FamilySearchStateFactory : StateFactory
    {
        protected internal DiscussionsState NewDiscussionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DiscussionsState(request, response, client, accessToken, this);
        }

        protected internal DiscussionState NewDiscussionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new DiscussionState(request, response, client, accessToken, this);
        }

        protected internal UserState NewUserState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new UserState(request, response, client, accessToken, this);
        }

        protected internal PersonMergeState NewPersonMergeState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonMergeState(request, response, client, accessToken, this);
        }

        protected internal PersonMatchResultsState NewPersonMatchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonMatchResultsState(request, response, client, accessToken, this);
        }

        /**
         * Create a new places state with the given URI
         *
         * @param discoveryUri the discovery URI for places
         * @return a new places state created with with the given URI
         */
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri)
        {
            return NewPlacesState(discoveryUri, LoadDefaultClient(discoveryUri));
        }

        /**
         * Create a new places state with the given URI
         *
         * @param discoveryUri the discovery URI for places
         * @param client the client that will use the new places state
         * @return a new places state created with with the given URI
         */
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IFilterableRestClient client)
        {
            return NewPlacesState(discoveryUri, client, Method.GET);
        }

        /**
         * Create a new places state with the given URI
         *
         * @param discoveryUri the discovery URI for places
         * @param client the client that will use the new places state
         * @param method the HTTP method to call
         * @return a new places state created with with the given URI
         */
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IFilterableRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPlacesState(request, client.Handle(request), client, null);
        }

        protected internal FamilySearchPlaces NewPlacesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaces(request, response, client, accessToken, this);
        }

		protected internal override CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchCollectionState(request, response, client, accessToken, this);
        }

        internal virtual CollectionState NewCollectionStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewCollectionStateInt(request, response, client, accessToken);
        }

		protected internal override SourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchSourceDescriptionState(request, response, client, accessToken, this);
        }

        internal virtual SourceDescriptionState NewSourceDescriptionStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewSourceDescriptionState(request, response, client, accessToken);
        }

		protected internal override PersonState NewPersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return base.NewPersonState(request, response, client, accessToken);
        }

        internal virtual PersonState NewPersonStateInt(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return this.NewPersonState(request, response, client, accessToken);
        }

        protected internal virtual PersonNonMatchesState NewPersonNonMatchesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new PersonNonMatchesState(request, response, client, accessToken, this);
        }

        protected internal FamilySearchPlaceState NewPlaceState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaceState(request, response, client, accessToken, this);
        }

		protected internal override PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken)
        {
            return new FamilySearchPlaceDescriptionState(request, response, client, accessToken, this);
        }

		protected internal override IFilterableRestClient LoadDefaultClient(Uri uri)
        {
            var client = base.LoadDefaultClient(uri);

            //how to add an experiment:
            client.AddFilter(new ExperimentsFilter("birth-date-not-considered-death-declaration"));

            return client;
        }

        internal virtual IFilterableRestClient LoadDefaultClientInt(Uri uri)
        {
            return this.LoadDefaultClient(uri);
        }
    }
}
