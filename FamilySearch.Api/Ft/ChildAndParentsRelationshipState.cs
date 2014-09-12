using Gx.Rs.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilySearch.Api.Ft
{
    public class ChildAndParentsRelationshipState : PreferredRelationshipState
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private RestSharp.IRestClient client;
        private string accessToken;
        private FamilyTreeStateFactory familyTreeStateFactory;

        public ChildAndParentsRelationshipState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, RestSharp.IRestClient client, string accessToken, FamilyTreeStateFactory familyTreeStateFactory)
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
