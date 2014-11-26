using Gx.Atom;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;
using FamilySearch.Api.Util;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The ChangeHistoryState exposes management functions for a change history.
    /// </summary>
    public class ChangeHistoryState : GedcomxApplicationState<Feed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeHistoryState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal ChangeHistoryState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new ChangeHistoryState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Gets the change history page represented by the current state instance.
        /// </summary>
        /// <value>
        /// The change history page represented by the current state instance.
        /// </value>
        public ChangeHistoryPage Page
        {
            get
            {
                Feed feed = Entity;
                return feed == null ? null : new ChangeHistoryPage(feed);
            }
        }

        /// <summary>
        /// Restores the specified change (if it had been reverted).
        /// </summary>
        /// <param name="change">The change to restore.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChangeHistoryState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
        public ChangeHistoryState RestoreChange(Entry change, params StateTransitionOption[] options)
        {
            Link link = change.GetLink(Rel.RESTORE);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unrestorable change: " + change.Id);
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChangeHistoryState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
