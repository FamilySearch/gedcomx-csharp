using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
    public class PlaceGroupState : GedcomxApplicationState<Gedcomx>
    {
        internal PlaceGroupState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.PLACE_GROUP;
            }
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new PlaceGroupState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                List<PlaceDescription> placeGroup = PlaceGroup;
                return placeGroup == null ? null : placeGroup.FirstOrDefault();
            }
        }

        /**
         * Get the place group
         *
         * @return the place group associated with this place group application state
         */
        public List<PlaceDescription> PlaceGroup
        {
            get
            {
                return Entity == null ? null : Entity.Places;
            }
        }
    }
}
