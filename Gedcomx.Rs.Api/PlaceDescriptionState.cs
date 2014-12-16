using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;
using Gx.Conclusion;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The PlaceDescriptionState exposes management functions for a place description.
    /// </summary>
    public class PlaceDescriptionState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceDescriptionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public override String SelfRel
        {
            get
            {
                return Rel.DESCRIPTION;
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
            return new PlaceDescriptionState(request, response, client, this.CurrentAccessToken, this.stateFactory);
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
                return PlaceDescription;
            }
        }

        /// <summary>
        /// Gets the first place description represented by the current state instance from <see cref="P:Gedcomx.Places"/>.
        /// </summary>
        /// <value>
        /// The first place description represented by the current state instance from <see cref="P:Gedcomx.Places"/>.
        /// </value>
        public PlaceDescription PlaceDescription
        {
            get
            {
                return Entity == null ? null : Entity.Places == null ? null : Entity.Places.FirstOrDefault();
            }
        }

        /// <summary>
        /// Reads the children of the current place description.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PlaceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        public PlaceDescriptionsState ReadChildren(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CHILDREN);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPlaceDescriptionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
