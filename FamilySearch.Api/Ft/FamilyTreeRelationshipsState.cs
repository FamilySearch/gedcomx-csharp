using Gx.Rs.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilySearch.Api.Ft
{
    public class FamilyTreeRelationshipsState : RelationshipsState
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private RestSharp.IRestClient client;
        private string accessToken;
        private FamilyTreeStateFactory familyTreeStateFactory;

        public FamilyTreeRelationshipsState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, RestSharp.IRestClient client, string accessToken, FamilyTreeStateFactory familyTreeStateFactory)
            : base(request, response, client, accessToken, familyTreeStateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.client = client;
            this.accessToken = accessToken;
            this.familyTreeStateFactory = familyTreeStateFactory;
        }
    }
}
