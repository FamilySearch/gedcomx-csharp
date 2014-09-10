using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Gedcomx.Model;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
	public class AncestryResultsState : GedcomxApplicationState<Gedcomx>
    {
		internal AncestryResultsState(IRestRequest request, IRestClient client, StateFactory stateFactory)
			: this(request, client.Execute(request), client, null, stateFactory)
		{
		}

		internal AncestryResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
			: base(request, response, client, accessToken, stateFactory)
		{
		}

		public override String SelfRel
		{
			get
			{
				return Rel.ANCESTRY;
			}
		}

		protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
		{
			return new AncestryResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
		}

		public List<Person> Persons
		{
			get
			{
				return Entity == null ? null : Entity.Persons;
			}
		}
    }
}
