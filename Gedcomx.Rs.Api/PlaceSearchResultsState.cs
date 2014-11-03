using Gx.Atom;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The PlaceSearchResultsState exposes management functions for place search results.
    /// </summary>
    public class PlaceSearchResultsState : GedcomxApplicationState<Feed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceSearchResultsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal PlaceSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
        protected override GedcomxApplicationState<Feed> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PlaceSearchResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the results of the current search response.
        /// </summary>
        /// <value>
        /// The results of the current search response.
        /// </value>
        public Feed Results
        {
            get
            {
                return Entity;
            }
        }

        /// <summary>
        /// Reads the place description described by a single entry from the results.
        /// </summary>
        /// <param name="place">A place description described by an entry from <see cref="P:Feed.Results"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PlaceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public PlaceDescriptionState ReadPlaceDescription(Entry place, params StateTransitionOption[] options)
        {
            Link link = place.GetLink(Rel.DESCRIPTION);
            link = link == null ? place.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPlaceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
