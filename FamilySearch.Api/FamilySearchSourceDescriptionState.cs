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
    public class FamilySearchSourceDescriptionState : SourceDescriptionState
    {
        protected internal FamilySearchSourceDescriptionState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilySearchSourceDescriptionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

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
            return ((FamilySearchStateFactory)this.stateFactory).NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
