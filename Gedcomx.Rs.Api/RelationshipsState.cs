using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Links;
using Gx.Common;
using Gx.Types;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The RelationshipsState exposes management functions for a relationships collection.
    /// </summary>
    public class RelationshipsState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal RelationshipsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new RelationshipsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the relationships represented by the current collection from <see cref="P:Gedcomx.Relationships"/>.
        /// </summary>
        /// <value>
        /// The relationships represented by the current collection from <see cref="P:Gedcomx.Relationships"/>.
        /// </value>
        public List<Relationship> Relationships
        {
            get
            {
                return Entity == null ? null : Entity.Relationships;
            }
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
        /// Creates a spouse relationship between the two specified persons.
        /// </summary>
        /// <param name="person1">A person to be the husband. See remarks.</param>
        /// <param name="person2">A person to be the wife. See remarks.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// The person1 parameter does not have to be the husband, it could be the wife; however, person2 must be the opposite. So if you specify a hasband for person1
        /// then person2 must be the wife. Conversely, if you specify a wife for person1 then person2 must be the husband.
        /// </remarks>
        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(person1.GetSelfUri());
            relationship.Person2 = new ResourceReference(person2.GetSelfUri());
            relationship.KnownType = RelationshipType.Couple;
            return AddRelationship(relationship, options);
        }

        /// <summary>
        /// Creates a parent child relationship for the specified persons.
        /// </summary>
        /// <param name="parent">The <see cref="PersonState"/> representing the parent for the new relationship.</param>
        /// <param name="child">The <see cref="PersonState"/> representing the child for the new relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(parent.GetSelfUri());
            relationship.Person2 = new ResourceReference(child.GetSelfUri());
            relationship.KnownType = RelationshipType.ParentChild;
            return AddRelationship(relationship, options);
        }

        /// <summary>
        /// Creates a relationship between the persons specified in the relationship parameter.
        /// </summary>
        /// <param name="relationship">This specifies the persons and relationship type to create.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public virtual RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddRelationship(relationship);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
