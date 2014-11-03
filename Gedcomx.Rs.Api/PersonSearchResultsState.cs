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
    /// <summary>
    /// The PersonSearchResultsState exposes management functions for person search results.
    /// </summary>
    public class PersonSearchResultsState : GedcomxApplicationState<Feed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonSearchResultsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonSearchResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
        protected override GedcomxApplicationState<Feed> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonSearchResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the search results from the atom feed response.
        /// </summary>
        /// <value>
        /// The search results from the atom feed response.
        /// </value>
        public Feed Results
        {
            get
            {
                return Entity;
            }
        }

        /// <summary>
        /// Reads the person from the specified atom feed entry.
        /// </summary>
        /// <param name="person">The person from the atom feed entry. This could come from <see cref="P:Results.Entries"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Reads the person record from the specified atom feed entry.
        /// </summary>
        /// <param name="person">The person from the atom feed entry. This could come from <see cref="P:Results.Entries"/>.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState ReadRecord(Entry person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.RECORD);
            link = link == null ? person.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRecordState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person from the specified person model.
        /// </summary>
        /// <param name="person">The person to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// The specified person model will need a self link; otherwise, this method will return null.
        /// </remarks>
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
