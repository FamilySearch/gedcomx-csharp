using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Rs.Api;
using Gx.Fs;
using System.Net;
using Gx.Fs.Discussions;
using Gx.Links;
using FamilySearch.Api.Util;
using Gedcomx.Model;

namespace FamilySearch.Api
{
    /// <summary>
    /// The DiscussionState exposes management functions for a discussion.
    /// </summary>
    public class DiscussionState : GedcomxApplicationState<FamilySearchPlatform>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        public DiscussionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
            return new DiscussionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Returns the <see cref="FamilySearchPlatform"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="FamilySearchPlatform"/> from the REST API response.</returns>
        protected override FamilySearchPlatform LoadEntity(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK ? response.ToIRestResponse<FamilySearchPlatform>().Data : null;
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override ISupportsLinks MainDataElement
        {
            get
            {
                return Discussion;
            }
        }

        /// <summary>
        /// Gets the first <see cref="Discussion"/> from <see cref="P:FamilySearchPlatform.Discussions"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="Discussion"/> from <see cref="P:FamilySearchPlatform.Discussions"/>.
        /// </value>
        public Discussion Discussion
        {
            get
            {
                return Entity == null ? null : Entity.Discussions == null ? null : Entity.Discussions.FirstOrDefault();
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="Discussion"/> and only sets the <see cref="P:Discussion.Id"/> to the current discussion's ID.
        /// </summary>
        /// <returns>A new <see cref="Discussion"/> with a matching discussion ID for the current discussion ID.</returns>
        protected Discussion CreateEmptySelf()
        {
            Discussion discussion = new Discussion();
            discussion.Id = LocalSelfId;
            return discussion;
        }

        /// <summary>
        /// Gets the current <see cref="P:Discussion.Id" />.
        /// </summary>
        /// <value>
        /// The current <see cref="P:Discussion.Id"/>
        /// </value>
        protected String LocalSelfId
        {
            get
            {
                Discussion me = Discussion;
                return me == null ? null : me.Id;
            }
        }

        /// <summary>
        /// Loads the comments for the current discussion.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState LoadComments(params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COMMENTS);
            if (this.Entity != null && link != null && link.Href != null)
            {
                Embed<FamilySearchPlatform>(link, this.Entity, options);
            }

            return this;
        }

        /// <summary>
        /// Creates a REST API request (with appropriate authentication headers).
        /// </summary>
        /// <param name="rel">This parameter is currently unused.</param>
        /// <returns>
        /// A REST API requeset (with appropriate authentication headers).
        /// </returns>
        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
        }

        /// <summary>
        /// Updates the specified discussion.
        /// </summary>
        /// <param name="discussion">The discussion to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState Update(Discussion discussion, params IStateTransitionOption[] options)
        {
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a comment to the current discussion.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState AddComment(String comment, params IStateTransitionOption[] options)
        {
            return AddComments(new Comment[] { new Comment() { Text = comment } }, options);
        }

        /// <summary>
        /// Adds a comment to the current discussion.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState AddComment(Comment comment, params IStateTransitionOption[] options)
        {
            return AddComments(new Comment[] { comment }, options);
        }

        /// <summary>
        /// Adds the specified comments to the current discussion.
        /// </summary>
        /// <param name="comments">The comments to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState AddComments(Comment[] comments, params IStateTransitionOption[] options)
        {
            Discussion discussion = CreateEmptySelf();
            discussion.Comments = comments.ToList();
            return UpdateComments(discussion, options);
        }

        /// <summary>
        /// Updates the specified comment on the current discussion.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState UpdateComment(Comment comment, params IStateTransitionOption[] options)
        {
            return UpdateComments(new Comment[] { comment }, options);
        }

        /// <summary>
        /// Updates the specified comments on the current discussion.
        /// </summary>
        /// <param name="comments">The comments to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        public DiscussionState UpdateComments(Comment[] comments, params IStateTransitionOption[] options)
        {
            Discussion discussion = CreateEmptySelf();
            discussion.Comments = comments.ToList();
            return UpdateComments(discussion, options);
        }

        /// <summary>
        /// Updates the comments on the specified discussion.
        /// </summary>
        /// <param name="discussion">The discussion with comments to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        protected DiscussionState UpdateComments(Discussion discussion, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link link = GetLink(Rel.COMMENTS);
            if (link != null && link.Href != null)
            {
                target = link.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.Discussions = new List<Discussion>() { discussion };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified comment from the current discussion.
        /// </summary>
        /// <param name="comment">The comment to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DiscussionState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Comment cannot be deleted: missing link.</exception>
        public DiscussionState DeleteComment(Comment comment, params IStateTransitionOption[] options)
        {
            Link link = comment.GetLink(Rel.COMMENT);
            link = link == null ? comment.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Comment cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
