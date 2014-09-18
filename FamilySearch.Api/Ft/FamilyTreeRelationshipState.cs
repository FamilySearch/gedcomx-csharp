using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Fs;
using System.Net;
using FamilySearch.Api.Util;
using Gx.Links;

namespace FamilySearch.Api.Ft
{
    public class FamilyTreeRelationshipState : RelationshipState, PreferredRelationshipState
    {
        protected internal FamilyTreeRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilyTreeRelationshipState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        protected override Gx.Gedcomx LoadEntityConditionally(IRestResponse response)
        {
            if (Request.Method == Method.GET && (response.StatusCode == HttpStatusCode.OK
                  || response.StatusCode == HttpStatusCode.Gone)
                    || response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return LoadEntity(response);
            }
            else
            {
                return null;
            }
        }

        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return response.ToIRestResponse<FamilySearchPlatform>().Data;
        }

        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            if (Rel.DISCUSSION_REFERENCES.Equals(rel))
            {
                return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
            }
            else
            {
                return base.CreateRequestForEmbeddedResource(rel);
            }
        }

        public FamilyTreeRelationshipState LoadDiscussionReferences(params StateTransitionOption[] options)
        {
            return (FamilyTreeRelationshipState)base.LoadEmbeddedResources(new String[] { Rel.DISCUSSION_REFERENCES }, options);
        }

        public ChangeHistoryState ReadChangeHistory(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CHANGE_HISTORY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChangeHistoryState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public FamilyTreeRelationshipState restore(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RESTORE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
