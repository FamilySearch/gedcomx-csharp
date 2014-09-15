using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;
using Gx.Conclusion;

namespace FamilySearch.Api
{
    public class FamilySearchPlaceState : GedcomxApplicationState<Gx.Gedcomx>
    {

        protected internal FamilySearchPlaceState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.PLACE;
            }
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilySearchPlaceState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Place;
            }
        }

        public PlaceDescription Place
        {
            get
            {
                return Entity == null ? null : Entity.Places == null ? null : Entity.Places.FirstOrDefault();
            }
        }
    }
}
