using Flurl;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Links;
using Gx.Rs.Api.Util;
using FamilySearch.Api.Util;
using Gx.Conclusion;
using Gx.Fs.Tree;
using Gx.Types;

namespace FamilySearch.Api
{
    public class PersonMatchResultsState : PersonSearchResultsState
    {
        protected internal PersonMatchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Atom.Feed> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonMatchResultsState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public PersonState ReadPerson(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonMergeState ReadMergeOptions(Gx.Atom.Entry entry, params StateTransitionOption[] options)
        {
            Link link = entry.GetLink(Rel.MERGE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.OPTIONS);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMergeState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonMergeState ReadMergeAnalysis(Gx.Atom.Entry entry, params StateTransitionOption[] options)
        {
            Link link = entry.GetLink(Rel.MERGE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMergeState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonNonMatchesState AddNonMatch(Gx.Atom.Entry entry, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.NOT_A_MATCHES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            Gx.Gedcomx entity = new Gx.Gedcomx();
            entity.AddPerson(new Person() { Id = entry.Id });
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(entity).Build(link.Href, Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonNonMatchesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonMatchResultsState UpdateMatchStatus(Gx.Atom.Entry entry, MatchStatus status, params StateTransitionOption[] options)
        {
            String updateStatusUri = GetSelfUri().SetQueryParam(FamilySearchOptions.STATUS, status.ToString().ToLower()).ToString();
            IRestRequest request = CreateAuthenticatedRequest().ContentType(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE)
              .SetEntity(new Gx.Gedcomx() { Persons = new List<Person>() { new Person() { Identifiers = new List<Identifier>() { new Identifier() { KnownType = IdentifierType.Persistent, Value = entry.Id } } } } })
              .Build(updateStatusUri, Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMatchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
