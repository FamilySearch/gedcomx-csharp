using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;
using Gx.Source;
using FamilySearch.Api.Util;

namespace FamilySearch.Api
{
    /// <summary>
    /// The FamilySearchSourceDescriptionState exposes management functions for a FamilySearch source description.
    /// </summary>
    public class FamilySearchSourceDescriptionState : SourceDescriptionState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchSourceDescriptionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilySearchSourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
            return new FamilySearchSourceDescriptionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Reads the comments on the current source description.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState ReadComments(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COMMENTS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        //TODO: Create FamilysearchSourceReferencesQueryState class, add it to FamilySearchStateFactory when link is created
        /*
        public FamilySearchSourceReferencesQueryState ReadSourceReferencesQuery()
        {
            Link link = GetLink( //TODO: Put Rel here when added );
            if (link == null || link.Href = null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewFamilySearchSourceReferencesQueryState(request, Invoke(request), this.Client, this.CurrentAccessToken);
        }
        */

        /// <summary>
        /// Moves the current source description to the specified collection.
        /// </summary>
        /// <param name="collection">The target collection to contain this source description.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilySearchSourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public FamilySearchSourceDescriptionState MoveToCollection(CollectionState collection, params StateTransitionOption[] options)
        {
            Link link = collection.GetLink(Rel.SOURCE_DESCRIPTIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            SourceDescription me = SourceDescription;
            if (me == null || me.Id == null)
            {
                return null;
            }

            Gx.Gedcomx gx = new Gx.Gedcomx();
            gx.AddSourceDescription(new SourceDescription() { Id = me.Id });
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(link.Href, Method.POST);
            return (FamilySearchSourceDescriptionState)((FamilySearchStateFactory)this.stateFactory).NewSourceDescriptionStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
