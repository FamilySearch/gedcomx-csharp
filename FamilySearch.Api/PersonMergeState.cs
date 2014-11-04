using Gx.Fs;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Fs.Tree;
using Gx.Links;
using FamilySearch.Api.Util;

namespace FamilySearch.Api
{
    /// <summary>
    /// The PersonMergeState exposes management functions for a person merge.
    /// </summary>
    public class PersonMergeState : GedcomxApplicationState<FamilySearchPlatform>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonMergeState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonMergeState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
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
        protected override GedcomxApplicationState<FamilySearchPlatform> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonMergeState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Returns the <see cref="FamilySearchPlatform"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="FamilySearchPlatform"/> from the REST API response.</returns>
        protected override FamilySearchPlatform LoadEntity(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK ? response.ToIRestResponse<FamilySearchPlatform>().Data : null;
        }

        /// <summary>
        /// Gets the analysis of the current person merge.
        /// </summary>
        /// <value>
        /// The analysis of the current person merge.
        /// </value>
        public MergeAnalysis Analysis
        {
            get
            {
                return Entity == null ? null : Entity.MergeAnalyses == null ? null : Entity.MergeAnalyses.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the merge is allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the merge is allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowed
        {
            get
            {
                return Entity != null || this.Response.Headers.Get("Allow").Where(x => x.Value != null && x.Value.ToString().ToUpper().Contains(Method.POST.ToString())).Any();
            }
        }

        /// <summary>
        /// Reads the merge mirror for the current merge.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        public PersonMergeState ReadMergeMirror(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.MERGE_MIRROR);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMergeState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the survivor of the current merge.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadSurvivor(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Performs the specified merge operation.
        /// </summary>
        /// <param name="merge">The merge operation to perform.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        public PersonMergeState DoMerge(Merge merge, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddMerge(merge);
            return DoMerge(entity, options);
        }

        /// <summary>
        /// Performs the specified merges from <see cref="P:FamilySearchPlatform.Merges"/>.
        /// </summary>
        /// <param name="entity">The entity with the merge operations to perform.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        public PersonMergeState DoMerge(FamilySearchPlatform entity, params StateTransitionOption[] options)
        {
            return (PersonMergeState)Post(entity, options);
        }
    }
}
