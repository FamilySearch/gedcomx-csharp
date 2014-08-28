using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public class PlaceDescriptionsState
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private string accessToken;
        private StateFactory stateFactory;

        public PlaceDescriptionsState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, string accessToken, StateFactory stateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.accessToken = accessToken;
            this.stateFactory = stateFactory;
        }
    }
}
