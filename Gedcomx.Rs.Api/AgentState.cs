using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;

namespace Gx.Rs.Api
{
    public class AgentState : GedcomxApplicationState<Gedcomx>
    {
        internal AgentState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new AgentState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public override String SelfRel
        {
            get
            {
                return Rel.AGENT;
            }
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Agent;
            }
        }

        public Agent.Agent Agent
        {
            get
            {
                return Entity == null ? null : Entity.Agents == null ? null : Entity.Agents.FirstOrDefault();
            }
        }
    }
}
