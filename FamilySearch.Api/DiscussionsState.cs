using Gx.Fs;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Fs.Discussions;
using Gx.Links;
using FamilySearch.Api.Util;

namespace FamilySearch.Api
{
    /// <summary>
    /// The DiscussionsState exposes management functions for discussions.
    /// </summary>
    public class DiscussionsState : GedcomxApplicationState<FamilySearchPlatform>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        public DiscussionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
            return new DiscussionsState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Gets the current <see cref="P:FamilySearchPlatform.Discussions"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="P:FamilySearchPlatform.Discussions"/>.
        /// </value>
        public List<Discussion> Discussions
        {
            get
            {
                return Entity == null ? null : Entity.Discussions;
            }
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewCollectionStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a discussion to this discussions state instance.
        /// </summary>
        /// <param name="discussion">The discussion to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState AddDiscussion(Discussion discussion, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedGedcomxRequest()).SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
