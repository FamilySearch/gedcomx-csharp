using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;
using Gedcomx.Model;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
    public class PersonsState : GedcomxApplicationState<Gedcomx>
    {
        internal PersonsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new PersonsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Entity;
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

        public List<Person> Persons
        {
            get
            {
                Gedcomx entity = Entity;
                return entity == null ? null : entity.Persons;
            }
        }

        public PersonState AddPerson(Person person, params StateTransitionOption[] options)
        {
            Link link = GetLink("self");
            String href = link == null ? null : link.Href == null ? null : link.Href;
            href = href == null ? GetUri() : href;

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(href, Method.POST);
            return (PersonState)this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken).IfSuccessful();
        }
    }
}
