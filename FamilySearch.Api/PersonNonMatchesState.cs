using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Rs.Api;
using Gx.Conclusion;
using Gx.Links;
using FamilySearch.Api.Util;

namespace FamilySearch.Api
{
    public class PersonNonMatchesState : PersonsState
    {
        protected internal PersonNonMatchesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonNonMatchesState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public PersonNonMatchesState AddNonMatch(Person person, params StateTransitionOption[] options)
        {
            return (PersonNonMatchesState)Post(new Gx.Gedcomx() { Persons = new List<Person>() { person } }, options);
        }

        public PersonNonMatchesState RemoveNonMatch(Person nonMatch, params StateTransitionOption[] options)
        {
            Link link = nonMatch.GetLink(Rel.NOT_A_MATCH);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonNonMatchesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
