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

        protected override RelationshipsState NewRelationshipsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipsState(request, response, client, accessToken, this);
        }

        internal virtual RelationshipsState NewRelationshipsStateInt(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return this.NewRelationshipsState(request, response, client, accessToken);
        }

        protected override CollectionState NewCollectionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilySearchFamilyTree(request, response, client, accessToken, this);
        }

        protected override PersonState NewPersonState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonState(request, response, client, accessToken, this);
        }

        protected override RelationshipState NewRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreeRelationshipState(request, response, client, accessToken, this);
        }

        internal virtual RelationshipState NewRelationshipStateInt(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return this.NewRelationshipState(request, response, client, accessToken);
        }

        protected override PersonParentsState NewPersonParentsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonParentsState(request, response, client, accessToken, this);
        }

        protected override PersonChildrenState NewPersonChildrenState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new FamilyTreePersonChildrenState(request, response, client, accessToken, this);
        }

        protected internal ChangeHistoryState NewChangeHistoryState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return new ChangeHistoryState(request, response, client, accessToken, this);
        }

        internal virtual SourceDescriptionsState NewSourceDescriptionsStateInt(IRestRequest request, IRestResponse response, IRestClient client, String accessToken)
        {
            return this.NewSourceDescriptionsState(request, response, client, accessToken);
        }
    }
}
