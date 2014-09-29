using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;

namespace Gx.Rs.Api
{
    public class DescendancyResultsState : GedcomxApplicationState<Gedcomx>
    {
        internal DescendancyResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new DescendancyResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public DescendancyTree Tree
        {
            get
            {
                return Entity != null ? new DescendancyTree(Entity) : null;
            }
        }
    }
}
