using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilySearch.Api.Ft
{
    public class FamilyTreePersonState
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private RestSharp.IRestClient client;
        private string accessToken;
        private FamilyTreeStateFactory familyTreeStateFactory;

        public FamilyTreePersonState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, RestSharp.IRestClient client, string accessToken, FamilyTreeStateFactory familyTreeStateFactory)
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
