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
    public class DiscussionState : GedcomxApplicationState<FamilySearchPlatform>
    {
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
        protected override GedcomxApplicationState<FamilySearchPlatform> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
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

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Discussion;
            }
        }

        public Discussion Discussion
        {
            get
            {
                return Entity == null ? null : Entity.Discussions == null ? null : Entity.Discussions.FirstOrDefault();
            }
        }

        protected Discussion CreateEmptySelf()
        {
            Discussion discussion = new Discussion();
            discussion.Id = LocalSelfId;
            return discussion;
        }

        protected String LocalSelfId
        {
            get
            {
                Discussion me = Discussion;
                return me == null ? null : me.Id;
            }
        }

        public DiscussionState LoadComments(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COMMENTS);
            if (this.Entity != null && link != null && link.Href != null)
            {
                Embed<FamilySearchPlatform>(link, this.Entity, options);
            }

            return this;
        }

        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
        }

        public DiscussionState Update(Discussion discussion, params StateTransitionOption[] options)
        {
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public DiscussionState AddComment(String comment, params StateTransitionOption[] options)
        {
            return AddComments(new Comment[] { new Comment() { Text = comment } }, options);
        }

        public DiscussionState AddComment(Comment comment, params StateTransitionOption[] options)
        {
            return AddComments(new Comment[] { comment }, options);
        }

        public DiscussionState AddComments(Comment[] comments, params StateTransitionOption[] options)
        {
            Discussion discussion = CreateEmptySelf();
            discussion.Comments = comments.ToList();
            return UpdateComments(discussion, options);
        }

        public DiscussionState UpdateComment(Comment comment, params StateTransitionOption[] options)
        {
            return UpdateComments(new Comment[] { comment }, options);
        }

        public DiscussionState UpdateComments(Comment[] comments, params StateTransitionOption[] options)
        {
            Discussion discussion = CreateEmptySelf();
            discussion.Comments = comments.ToList();
            return UpdateComments(discussion, options);
        }

        protected DiscussionState UpdateComments(Discussion discussion, params StateTransitionOption[] options)
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

        public DiscussionState DeleteComment(Comment comment, params StateTransitionOption[] options)
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
