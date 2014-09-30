using Gedcomx.Model;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
    public class SourceDescriptionState : GedcomxApplicationState<Gedcomx>
    {
        protected internal SourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.DESCRIPTION;
            }
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new SourceDescriptionState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return SourceDescription;
            }
        }

        public SourceDescription SourceDescription
        {
            get
            {
                return Entity == null ? null : Entity.SourceDescriptions == null ? null : Entity.SourceDescriptions.FirstOrDefault();
            }
        }

        public SourceDescriptionState Update(SourceDescription description, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.SourceDescriptions = new List<SourceDescription>() { description };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonsState ReadPersonas(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                return this.stateFactory.NewPersonsState(this.Request, this.Response, this.Client, this.CurrentAccessToken);
            }
            else
            {
                IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
                return this.stateFactory.NewPersonsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
            }
        }

        public PersonState AddPersona(Person person, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddPerson(person);
            return AddPersona(entity, options);
        }

        public PersonState AddPersona(Gedcomx entity, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link link = GetLink(Rel.PERSONS);
            if (link != null && link.Href != null)
            {
                target = link.Href;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionState QueryAttachedReferences(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.SOURCE_REFERENCES_QUERY);
            if (link == null || link.Href == null)
            {
                return null;
            }
            else
            {
                IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
                return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
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
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
