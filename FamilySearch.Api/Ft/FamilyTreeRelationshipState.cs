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
    public class FamilyTreeRelationshipState : RelationshipState, PreferredRelationshipState
    {
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
        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
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

        public FamilyTreeRelationshipState LoadDiscussionReferences(params StateTransitionOption[] options)
        {
            return (FamilyTreeRelationshipState)base.LoadEmbeddedResources(new String[] { Rel.DISCUSSION_REFERENCES }, options);
        }

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
