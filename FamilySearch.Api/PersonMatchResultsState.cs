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
using Gedcomx.Support;

namespace FamilySearch.Api
{
    /// <summary>
    /// The PersonMatchResultsState exposes management functions for a person match results.
    /// </summary>
    public class PersonMatchResultsState : PersonSearchResultsState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMatchResultsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonMatchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonMatchResultsState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Reads the current person for these potential match results.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Reads merge options for the specified search result from <see cref="P:Results.Entries"/>.
        /// </summary>
        /// <param name="entry">A search result entry from <see cref="P:Results.Entries"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Creates a merge analysis for the current person and the potential duplicate person specified by the search result entry from <see cref="P:Results.Entries"/>.
        /// </summary>
        /// <param name="entry">The search result entry to perform the merge analysis upon.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Declares the specified search result entry (from <see cref="P:Results.Entries"/>) as not a match for the current person.
        /// </summary>
        /// <param name="entry">The search result entry from <see cref="P:Results.Entries"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Declares the match status for the current person the specified search result entry from <see cref="P:Results.Entries"/>.
        /// </summary>
        /// <param name="entry">The search result entry (from <see cref="P:Results.Entries"/>) to have the match status updated.</param>
        /// <param name="status">The new status to apply to the specified search result entry from <see cref="P:Results.Entries"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMatchResultsState"/> instance containing the REST API response.
        /// </returns>
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
