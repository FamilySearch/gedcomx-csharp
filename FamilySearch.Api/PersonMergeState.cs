using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilySearch.Api
{
    public class PersonMergeState
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private RestSharp.IRestClient client;
        private string accessToken;
        private FamilySearchStateFactory familySearchStateFactory;

        public PersonMergeState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, RestSharp.IRestClient client, string accessToken, FamilySearchStateFactory familySearchStateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.client = client;
            this.accessToken = accessToken;
            this.familySearchStateFactory = familySearchStateFactory;
        }
    }
}
