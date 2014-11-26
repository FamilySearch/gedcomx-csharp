using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Fs;
using System.Net;
using FamilySearch.Api.Util;
using Gx.Links;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The FamilyTreeRelationshipState exposes management and other FamilySearch specific functions for a relationship.
    /// </summary>
    public class FamilyTreeRelationshipState : RelationshipState, PreferredRelationshipState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreeRelationshipState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilyTreeRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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
            return new FamilyTreeRelationshipState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was a GET and the response status is OK, GONE, or PRECONDITIONFAILED.</remarks>
        protected override Gx.Gedcomx LoadEntityConditionally(IRestResponse response)
        {
            if (Request.Method == Method.GET && (response.StatusCode == HttpStatusCode.OK
                  || response.StatusCode == HttpStatusCode.Gone)
                    || response.StatusCode == HttpStatusCode.PreconditionFailed)
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
        /// Creates a REST API request (with appropriate authentication headers).
        /// </summary>
        /// <param name="rel">If the value is equal to the discussion references link, the resulting request is built with accept and content-type headers of "application/x-fs-v1+json"; otherwise, "application/x-gedcomx-v1+json" is used.</param>
        /// <returns>
        /// A REST API requeset (with appropriate authentication headers).
        /// </returns>
        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            if (Rel.DISCUSSION_REFERENCES.Equals(rel))
            {
                return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
            }
            else
            {
                return base.CreateRequestForEmbeddedResource(rel);
            }
        }

        /// <summary>
        /// Loads all discussion references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreeRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreeRelationshipState LoadDiscussionReferences(params StateTransitionOption[] options)
        {
            return (FamilyTreeRelationshipState)base.LoadEmbeddedResources(new String[] { Rel.DISCUSSION_REFERENCES }, options);
        }

        /// <summary>
        /// Reads the change history of the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChangeHistoryState"/> instance containing the REST API response.
        /// </returns>
        public ChangeHistoryState ReadChangeHistory(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CHANGE_HISTORY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChangeHistoryState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Restore the current relationship (if it is currently deleted).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreeRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreeRelationshipState Restore(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RESTORE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            return (FamilyTreeRelationshipState)((FamilyTreeStateFactory)this.stateFactory).NewRelationshipStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }        
    }
}
