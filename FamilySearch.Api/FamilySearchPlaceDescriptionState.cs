using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;

namespace FamilySearch.Api
{
    public class FamilySearchPlaceDescriptionState : PlaceDescriptionState
    {
        protected internal FamilySearchPlaceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchPlaceDescriptionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public FamilySearchPlaceState ReadPlace(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PLACE);
            link = link == null ? this.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPlaceState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
