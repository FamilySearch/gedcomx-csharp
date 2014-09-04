using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public class RecordsState : GedcomxApplicationState<Gedcomx>
    {
        private RestSharp.IRestRequest request;
        private RestSharp.IRestResponse response;
        private string accessToken;

        public RecordsState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, IRestClient client, string accessToken, StateFactory stateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.accessToken = accessToken;
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            throw new NotImplementedException();
        }

        protected override global::Gedcomx.Model.SupportsLinks MainDataElement
        {
            get { throw new NotImplementedException(); }
        }
    }
}
