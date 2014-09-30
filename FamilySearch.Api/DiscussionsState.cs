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
    public class DiscussionsState : GedcomxApplicationState<FamilySearchPlatform>
    {
        public DiscussionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<FamilySearchPlatform> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new DiscussionsState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public List<Discussion> Discussions
        {
            get
            {
                return Entity == null ? null : Entity.Discussions;
            }
        }

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

        public DiscussionState AddDiscussion(Discussion discussion, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedGedcomxRequest()).SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
