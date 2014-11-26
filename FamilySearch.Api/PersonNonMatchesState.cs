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
    /// <summary>
    /// The PersonNonMatchesState exposes management functions for person non matches.
    /// </summary>
    public class PersonNonMatchesState : PersonsState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonNonMatchesState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonNonMatchesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new PersonNonMatchesState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Adds a person as a non match to this collection.
        /// </summary>
        /// <param name="person">The person that is not a match (from potential duplicate search results).</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
        public PersonNonMatchesState AddNonMatch(Person person, params StateTransitionOption[] options)
        {
            return (PersonNonMatchesState)Post(new Gx.Gedcomx() { Persons = new List<Person>() { person } }, options);
        }

        /// <summary>
        /// Removes the declared non match person from this collection.
        /// </summary>
        /// <param name="nonMatch">The person that was previously declared as a non-match.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
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
