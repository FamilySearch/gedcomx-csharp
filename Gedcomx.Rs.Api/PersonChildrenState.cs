﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;
using Gx.Conclusion;
using Gx.Common;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The PersonChildrenState exposes management functions for person children.
    /// </summary>
    public class PersonChildrenState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonChildrenState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new PersonChildrenState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the <see cref="P:Gedcomx.Persons"/> represented by the current state instance.
        /// </summary>
        /// <value>
        /// The <see cref="P:Gedcomx.Persons"/> represented by the current state instance.
        /// </value>
        public List<Person> Persons
        {
            get
            {
                return this.Entity == null ? null : this.Entity.AnyPersons() ? this.Entity.Persons : null;
            }
        }

        /// <summary>
        /// Gets the <see cref="P:Gedcomx.Relationships"/> represented by the current state instance.
        /// </summary>
        /// <value>
        /// The <see cref="P:Gedcomx.Relationships"/> represented by the current state instance.
        /// </value>
        public List<Relationship> Relationships
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Relationships;
            }
        }

        /// <summary>
        /// Finds the relationship to the specified child. See remarks for more information.
        /// </summary>
        /// <param name="child">The child for which the relationship is sought.</param>
        /// <returns>
        /// The <see cref="Relationship"/> the child is in, or <c>null</c> if a relationship was not found.
        /// </returns>
        /// <remarks>
        /// This method iterates over the current <see cref="P:Relationships"/>, and each item is examined
        /// to determine if the child ID in the relationship matches the child ID for the specified child. If one is found,
        /// that relationship object containing that child ID is returned, and no other relationships are examined further.
        /// </remarks>
        public Relationship FindRelationshipTo(Person child)
        {
            List<Relationship> relationships = Relationships;
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    ResourceReference childReference = relationship.Person2;
                    if (childReference != null)
                    {
                        String reference = childReference.Resource;
                        if (reference.Equals("#" + child.Id))
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
        public PersonState ReadPerson(params IStateTransitionOption[] options)
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
        /// Reads the child from the person specified.
        /// </summary>
        /// <param name="person">The person from which the child will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadChild(Person person, params IStateTransitionOption[] options)
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
        /// Reads the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState ReadRelationship(Relationship relationship, params IStateTransitionOption[] options)
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
        /// Removes the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship to remove.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
        public RelationshipState RemoveRelationship(Relationship relationship, params IStateTransitionOption[] options)
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
        /// Removes the relationship to the specified child.
        /// </summary>
        /// <param name="child">The child to which the relationship will be removed.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">
        /// Thrown if a relationship to the specified child cannot be found.
        /// or
        /// Thrown if a link to the required resource cannot be found.
        /// </exception>
        public RelationshipState RemoveRelationshipTo(Person child, params IStateTransitionOption[] options)
        {
            Relationship relationship = FindRelationshipTo(child);
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
