using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Ft
{
    public class FamilyTreeStateFactory : FamilySearchStateFactory
    {
        public FamilySearchFamilyTree NewFamilyTreeState()
        {
            return NewFamilyTreeState(true);
        }

		public FamilySearchFamilyTree NewFamilyTreeState(bool production)
        {
			var uri = new Uri (production ? FamilySearchFamilyTree.URI : FamilySearchFamilyTree.SANDBOX_URI);
			return NewFamilyTreeState (uri);
        }

        public FamilySearchFamilyTree NewFamilyTreeState(Uri discoveryUri)
        {
			var collection = NewCollectionState(discoveryUri);
			return NewCollectionState (collection.Request, collection.Response, collection.Client, collection.CurrentAccessToken);
        }

        protected internal ChildAndParentsRelationshipState NewChildAndParentsRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new ChildAndParentsRelationshipState(request, response, client, accessToken, this);
        }

        protected internal FamilyTreeRelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipsState(request, response, client, accessToken, this);
        }

        new protected FamilySearchFamilyTree NewCollectionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchFamilyTree(request, response, client, accessToken, this);
        }

        new protected internal FamilyTreePersonState NewPersonState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonState(request, response, client, accessToken, this);
        }

        protected internal FamilyTreeRelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipState(request, response, client, accessToken, this);
        }

        protected FamilyTreePersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonParentsState(request, response, client, accessToken, this);
        }

        protected FamilyTreePersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonChildrenState(request, response, client, accessToken, this);
        }

        new protected internal PersonNonMatchesState NewPersonNonMatchesState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return base.NewPersonNonMatchesState(request, response, client, accessToken);
        }

        protected internal ChangeHistoryState NewChangeHistoryState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new ChangeHistoryState(request, response, client, accessToken, this);
        }

        new protected PersonMatchResultsState NewPersonMatchResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return base.NewPersonMatchResultsState(request, response, client, accessToken);
        }

        new protected internal PersonMergeState NewPersonMergeState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return base.NewPersonMergeState(request, response, client, accessToken);
        }

        new protected internal SourceDescriptionsState NewSourceDescriptionsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return base.NewSourceDescriptionsState(request, response, client, accessToken);
        }
    }
}
