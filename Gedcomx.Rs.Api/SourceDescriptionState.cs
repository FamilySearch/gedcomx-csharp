using Gedcomx.Model;
using Gx.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public class SourceDescriptionState : GedcomxApplicationState<Gedcomx>
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private string accessToken;

        public SourceDescriptionState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, string accessToken, StateFactory stateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.accessToken = accessToken;
        }

        protected override GedcomxApplicationState Clone(RestSharp.IRestRequest request, RestSharp.IRestResponse response, RestSharp.IRestClient client)
        {
            throw new NotImplementedException();
        }
    }
}
