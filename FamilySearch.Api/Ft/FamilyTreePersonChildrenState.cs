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
    public class FamilyTreePersonChildrenState : PersonChildrenState
    {

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
        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilyTreePersonChildrenState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

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

        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return response.ToIRestResponse<FamilySearchPlatform>().Data;
        }

        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

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
