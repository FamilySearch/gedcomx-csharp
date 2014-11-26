using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Common;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The PersonSpousesState exposes management functions for person spouses.
    /// </summary>
    public class PersonSpousesState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonSpousesState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal PersonSpousesState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new PersonSpousesState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the persons, which are spouses to the current person from <see cref="P:Gedcomx.Persons"/>.
        /// </summary>
        /// <value>
        /// The persons, which are spouses to the current person from <see cref="P:Gedcomx.Persons"/>.
        /// </value>
        public List<Person> Persons
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Persons;
            }
        }

        /// <summary>
        /// Gets the relationships for the current person's spouses from <see cref="P:Gedcomx.Relationships"/>.
        /// </summary>
        /// <value>
        /// The relationships for the current person's spouses from <see cref="P:Gedcomx.Relationships"/>.
        /// </value>
        public List<Relationship> Relationships
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Relationships;
            }
        }

        /// <summary>
        /// Finds the relationship to the specified spouse. See remarks for more information.
        /// </summary>
        /// <param name="spouse">The spouse for which the relationship is sought.</param>
        /// <returns>
        /// The <see cref="Relationship"/> the spouse is in, or <c>null</c> if a relationship was not found.
        /// </returns>
        /// <remarks>
        /// This method iterates over the current <see cref="P:Relationships"/>, and each item is examined
        /// to determine if the spouse ID in the relationship matches the spouse ID for the specified spouse. If one is found,
        /// that relationship object containing that spouse ID is returned, and no other relationships are examined further.
        /// </remarks>
        public Relationship FindRelationshipTo(Person spouse)
        {
            List<Relationship> relationships = Relationships;
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    ResourceReference personReference = relationship.Person1;
                    if (personReference != null)
                    {
                        String reference = personReference.Resource;
                        if (reference.Equals("#" + spouse.Id))
                        {
                            return relationship;
                        }
                    }
                    personReference = relationship.Person2;
                    if (personReference != null)
                    {
                        String reference = personReference.Resource;
                        if (reference.Equals("#" + spouse.Id))
                        {
                            return relationship;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Reads the current person represented by this state instance.
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
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the specified person, which is a spouse to the current person. This person could come <see cref="P:Persons"/>.
        /// </summary>
        /// <param name="person">The person, which is a spouse to the current person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadSpouse(Person person, params StateTransitionOption[] options)
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
        /// Reads ancestry of the specified person, which is a spouse of the current person.
        /// </summary>
        /// <param name="person">The spouse whose ancestry will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadAncestryWithSpouse(Person person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.ANCESTRY);
            if (link == null || link.Href== null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Read descendancy of the specified person, which is a spouse of the current person.
        /// </summary>
        /// <param name="person">The spouse whose ancestry will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadDescendancyWithSpouse(Person person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.DESCENDANCY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the specified relationship, which is a relationship between the current person and a spouse. This relationship could come from <see cref="P:Relationships"/>.
        /// </summary>
        /// <param name="relationship">The relationship to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState ReadRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Removes the specified relationship, which is a relationship between the current person and a spouse.
        /// </summary>
        /// <param name="relationship">The relationship to remove.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">
        /// Thrown if a link to the required resource cannot be found.
        /// </exception>
        public RelationshipState RemoveRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Removes the relationship from the current person to the specified person.
        /// </summary>
        /// <param name="spouse">The spouse to which the current person has a relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">
        /// Thrown if a relationship between the current person and the specified spouse could not be found
        /// or
        /// Thrown if a link to the required resource cannot be found.
        /// </exception>
        public RelationshipState RemoveRelationshipTo(Person spouse, params StateTransitionOption[] options)
        {
            Relationship relationship = FindRelationshipTo(spouse);
            if (relationship == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: not found.");
            }

            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
