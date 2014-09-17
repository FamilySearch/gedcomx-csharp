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
    public class ChangeHistoryState : GedcomxApplicationState<Feed>
    {
        protected internal ChangeHistoryState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Feed> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new ChangeHistoryState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        public ChangeHistoryPage Page
        {
            get
            {
                Feed feed = Entity;
                return feed == null ? null : new ChangeHistoryPage(feed);
            }
        }

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
