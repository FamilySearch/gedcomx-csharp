using Gx.Common;
using Gx.Conclusion;
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

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The FamilyTreePersonChildrenState exposes management functions for person children.
    /// </summary>
    public class FamilyTreePersonChildrenState : PersonChildrenState
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonChildrenState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilyTreePersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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
            return new FamilyTreePersonChildrenState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was a GET and the response status is OK or GONE.</remarks>
        protected override Gx.Gedcomx LoadEntityConditionally(IRestResponse response)
        {
            if (Request.Method == Method.GET && (response.StatusCode == HttpStatusCode.OK
                  || response.StatusCode == HttpStatusCode.Gone))
            {
                return LoadEntity(response);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the <see cref="Gx.Gedcomx"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="Gx.Gedcomx"/> from the REST API response.</returns>
        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return response.ToIRestResponse<FamilySearchPlatform>().Data;
        }

        /// <summary>
        /// Gets the child and parents relationships represented by this state instance.
        /// </summary>
        /// <value>
        /// The child and parents relationships represented by this state instance.
        /// </value>
        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

        /// <summary>
        /// Finds the child and parents relationship to the specified child. See remarks for more information.
        /// </summary>
        /// <param name="child">The child for which the relationship is sought.</param>
        /// <returns>
        /// The <see cref="ChildAndParentsRelationship"/> the child is in, or <c>null</c> if a relationship was not found.
        /// </returns>
        /// <remarks>
        /// This method iterates over the current <see cref="P:ChildAndParentsRelationship"/>, and each item is examined
        /// to determine if the child ID in the relationship matches the child ID for the specified child. If one is found,
        /// that relationship object containing that child ID is returned, and no other relationships are examined further.
        /// </remarks>
        public ChildAndParentsRelationship FindChildAndParentsRelationshipTo(Person child)
        {
            List<ChildAndParentsRelationship> relationships = ChildAndParentsRelationships;
            if (relationships != null)
            {
                foreach (ChildAndParentsRelationship relationship in relationships)
                {
                    ResourceReference personReference = relationship.Child;
                    if (personReference != null)
                    {
                        String reference = personReference.Resource;
                        if (reference.Equals("#" + child.Id))
                        {
                            return relationship;
                        }
                    }
                }
            }
            return null;
        }
    }
}
