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
    public class PersonMergeState : GedcomxApplicationState<FamilySearchPlatform>
    {
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

        protected override FamilySearchPlatform LoadEntity(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK ? response.ToIRestResponse<FamilySearchPlatform>().Data : null;
        }

        public MergeAnalysis Analysis
        {
            get
            {
                return Entity == null ? null : Entity.MergeAnalyses == null ? null : Entity.MergeAnalyses.FirstOrDefault();
            }
        }

        public bool IsAllowed
        {
            get
            {
                return Entity != null || this.Response.Headers.Get("Allow").Where(x => x.Value != null && x.Value.ToString().ToUpper().Contains(Method.POST.ToString())).Any();
            }
        }

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

        public PersonMergeState DoMerge(Merge merge, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddMerge(merge);
            return DoMerge(entity, options);
        }

        public PersonMergeState DoMerge(FamilySearchPlatform entity, params StateTransitionOption[] options)
        {
            return (PersonMergeState)Post(entity, options);
        }
    }
}
