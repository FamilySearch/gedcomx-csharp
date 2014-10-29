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

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
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
