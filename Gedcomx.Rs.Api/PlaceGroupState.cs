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
        internal PlaceGroupState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
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
