using Gx.Fs;
using Gx.Fs.Tree;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Types;
using Gx.Common;
using FamilySearch.Api.Util;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The FamilyTreeRelationshipsState exposes management functions for family tree relationships.
    /// </summary>
    public class FamilyTreeRelationshipsState : RelationshipsState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreeRelationshipsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilyTreeRelationshipsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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
        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilyTreeRelationshipsState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Gets the child and parents relationships of the current <see cref="P:FamilySearchPlatform.ChildAndParentsRelationships"/> from <see cref="P:Entity"/>.
        /// </summary>
        /// <value>
        /// The child and parents relationships of the current <see cref="P:FamilySearchPlatform.ChildAndParentsRelationships"/> from <see cref="P:Entity"/>.
        /// </value>
        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

        /// <summary>
        /// Returns the <see cref="Gx.Gedcomx"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="Gx.Gedcomx"/> from the REST API response.</returns>
        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return Response.StatusCode == HttpStatusCode.OK ? Response.ToIRestResponse<Gx.Gedcomx>().Data : null;
        }

        /// <summary>
        /// Adds the specified relationship to this collection of relationships.
        /// </summary>
        /// <param name="relationship">The relationship to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">
        /// Thrown if the relationship type is a <see cref="RelationshipType.ParentChild"/> relationship, since this collection does not support adding those types
        /// of relationships. See remarks for more information.
        /// </exception>
        /// <remarks>
        /// To add a <see cref="RelationshipType.ParentChild"/> relationship, use <see cref="O:AddChildAndParentsRelationship"/> instead.
        /// </remarks>
        public override RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            if (relationship.KnownType == RelationshipType.ParentChild)
            {
                throw new GedcomxApplicationException("FamilySearch Family Tree doesn't support adding parent-child relationships. You must instead add a child-and-parents relationship.");
            }
            return base.AddRelationship(relationship, options);
        }

        /// <summary>
        /// Adds a child and parents relationship to the current relationships collection.
        /// </summary>
        /// <param name="child">The child in the relationship.</param>
        /// <param name="father">The father in the relationship.</param>
        /// <param name="mother">The mother in the relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(PersonState child, PersonState father, PersonState mother, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship chap = new ChildAndParentsRelationship();
            chap.Child = new ResourceReference(child.GetSelfUri());
            if (father != null)
            {
                chap.Father = new ResourceReference(father.GetSelfUri());
            }
            if (mother != null)
            {
                chap.Mother = new ResourceReference(mother.GetSelfUri());
            }
            return AddChildAndParentsRelationship(chap, options);
        }

        /// <summary>
        /// Adds a child and parents relationship to the current relationships collection.
        /// </summary>
        /// <param name="chap">The relationship to add to the current relationships collection, with the father, mother, and child set as desired.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(ChildAndParentsRelationship chap, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { chap };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
