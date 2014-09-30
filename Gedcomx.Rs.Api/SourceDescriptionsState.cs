using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Links;

namespace Gx.Rs.Api
{
    public class SourceDescriptionsState : GedcomxApplicationState<Gedcomx>
    {
        internal SourceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new SourceDescriptionsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public List<SourceDescription> SourceDescriptions
        {
            get
            {
                return Entity == null ? null : Entity.SourceDescriptions;
            }
        }

        public SourceDescriptionState AddSourceDescription(SourceDescription source, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddSourceDescription(source);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
