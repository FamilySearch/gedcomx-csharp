using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;

namespace FamilySearch.Api
{
    /// <summary>
    /// The FamilySearchPlaceDescriptionState exposes management functions for a FamilySearch place description.
    /// </summary>
    public class FamilySearchPlaceDescriptionState : PlaceDescriptionState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchPlaceDescriptionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilySearchPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchPlaceDescriptionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Reads the place described by the current place description.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilySearchPlaceState"/> instance containing the REST API response.
        /// </returns>
        public FamilySearchPlaceState ReadPlace(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PLACE);
            link = link == null ? this.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPlaceState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
