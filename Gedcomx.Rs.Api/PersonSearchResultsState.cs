using Gedcomx.Model;
using Gx.Atom;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using RestSharp.Extensions;

namespace Gx.Rs.Api
{
    public class PersonSearchResultsState : GedcomxApplicationState<Feed>
    {

        internal PersonSearchResultsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new PersonSearchResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Entity;
            }
        }

        public Feed Results
        {
            get
            {
                return Entity;
            }
        }

        public PersonState ReadPerson(Entry person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.PERSON);
            link = link == null ? person.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RecordState ReadRecord(Entry person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.RECORD);
            link = link == null ? person.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRecordState(request, Invoke(request, options), this.CurrentAccessToken);
        }

        public PersonState ReadPerson(Gx.Conclusion.Person person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.PERSON);
            link = link == null ? person.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
