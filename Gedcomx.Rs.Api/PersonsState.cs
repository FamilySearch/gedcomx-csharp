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
    /// <summary>
    /// The PersonsState exposes management functions for a persons collection.
    /// </summary>
    public class PersonsState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new PersonsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Gets the list of persons represented by this current instance from <see cref="P:Gedcomx.Persons"/>.
        /// </summary>
        /// <value>
        /// The list of persons represented by this current instance from <see cref="P:Gedcomx.Persons"/>.
        /// </value>
        public List<Person> Persons
        {
            get
            {
                Gedcomx entity = Entity;
                return entity == null ? null : entity.Persons;
            }
        }

        /// <summary>
        /// Adds a person to the current collection.
        /// </summary>
        /// <param name="person">The person to add to the current collection.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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
