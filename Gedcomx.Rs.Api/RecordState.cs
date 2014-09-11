using Gedcomx.Model;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;

namespace Gx.Rs.Api
{
    public class RecordState : GedcomxApplicationState<Gedcomx>
    {
        public RecordState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        private RecordState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private RecordState(Uri uri, IRestClient client, StateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private RecordState(IRestRequest request, IRestClient client, StateFactory stateFactory)
            : this(request, client.Execute(request), client, null, stateFactory)
        {
        }

        public RecordState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.RECORD;
            }
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new RecordState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }
    }
}
