using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;

namespace FamilySearch.Api
{
    public class FamilySearchStateFactory : StateFactory
    {
        protected internal DiscussionsState NewDiscussionsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new DiscussionsState(request, response, client, accessToken, this);
        }

        protected internal DiscussionState NewDiscussionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new DiscussionState(request, response, client, accessToken, this);
        }

        protected internal UserState NewUserState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new UserState(request, response, client, accessToken, this);
        }

        protected internal PersonMergeState NewPersonMergeState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonMergeState(request, response, client, accessToken, this);
        }

        protected internal PersonMatchResultsState NewPersonMatchResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
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
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IRestClient client)
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
        public FamilySearchPlaces NewPlacesState(Uri discoveryUri, IRestClient client, Method method)
        {
            IRestRequest request = new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(discoveryUri, method);
            return NewPlacesState(request, client.Execute(request), client, null);
        }

        protected FamilySearchPlaces NewPlacesState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchPlaces(request, response, client, accessToken, this);
        }

        protected internal FamilySearchCollectionState NewCollectionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchCollectionState(request, response, client, accessToken, this);
        }

        protected FamilySearchSourceDescriptionState NewSourceDescriptionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchSourceDescriptionState(request, response, client, accessToken, this);
        }

        new protected internal virtual PersonState NewPersonState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return base.NewPersonState(request, response, client, accessToken);
        }

        protected internal PersonNonMatchesState NewPersonNonMatchesState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new PersonNonMatchesState(request, response, client, accessToken, this);
        }

        protected internal FamilySearchPlaceState NewPlaceState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchPlaceState(request, response, client, accessToken, this);
        }

        protected PlaceDescriptionState NewPlaceDescriptionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchPlaceDescriptionState(request, response, client, accessToken, this);
        }

        // TODO: Determine if the core LoadDefaultClient() has missing functionality. If so, implement here; otherwise, remove this method.
        new protected internal IRestClient LoadDefaultClient(Uri uri)
        {
            return base.LoadDefaultClient(uri);
        }
    }
}
