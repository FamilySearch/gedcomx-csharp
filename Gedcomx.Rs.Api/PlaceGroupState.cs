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
    /// <summary>
    /// The PlaceGroupState exposes management functions for a place group.
    /// </summary>
    public class PlaceGroupState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceGroupState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal PlaceGroupState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Gets the rel name for the currrent state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the currrent state instance
        /// </value>
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
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PlaceGroupState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override SupportsLinks MainDataElement
        {
            get
            {
                List<PlaceDescription> placeGroup = PlaceGroup;
                return placeGroup == null ? null : placeGroup.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a list of place descriptions represented by the current place group from <see cref="P:Gedcomx.Places"/>.
        /// </summary>
        /// <value>
        /// A list of place descriptions represented by the current place group from <see cref="P:Gedcomx.Places"/>.
        /// </value>
        public List<PlaceDescription> PlaceGroup
        {
            get
            {
                return Entity == null ? null : Entity.Places;
            }
        }
    }
}
