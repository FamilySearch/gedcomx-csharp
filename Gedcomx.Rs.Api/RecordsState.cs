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
        private StateFactory stateFactory;

        public RecordsState(RestSharp.IRestRequest request, RestSharp.IRestResponse response, IRestClient client, string accessToken, StateFactory stateFactory)
        {
            // TODO: Complete member initialization
            this.request = request;
            this.response = response;
            this.accessToken = accessToken;
            this.stateFactory = stateFactory;
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            throw new NotImplementedException();
        }

        protected override Gedcomx LoadEntity(IRestResponse response)
        {
            throw new NotImplementedException();
        }

        protected override global::Gedcomx.Model.SupportsLinks MainDataElement
        {
            get { throw new NotImplementedException(); }
        }
    }
}
