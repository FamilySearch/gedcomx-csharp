using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using System.Net;
using Gx.Fs;
using Gx.Fs.Tree;
using Gx.Conclusion;
using Gx.Common;

namespace FamilySearch.Api.Ft
{
    public class FamilyTreePersonParentsState : PersonParentsState
    {
        protected internal FamilyTreePersonParentsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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
            return new FamilyTreePersonParentsState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was a GET and the response status is OK or GONE.</remarks>
        protected override Gx.Gedcomx LoadEntityConditionally(IRestResponse response)
        {
            if (Request.Method == Method.GET && (Response.StatusCode == HttpStatusCode.OK
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

        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

        public ChildAndParentsRelationship FindChildAndParentsRelationshipTo(Person spouse)
        {
            List<ChildAndParentsRelationship> relationships = ChildAndParentsRelationships;
            if (relationships != null)
            {
                foreach (ChildAndParentsRelationship relationship in relationships)
                {
                    ResourceReference personReference = relationship.Father;
                    if (personReference != null)
                    {
                        String reference = personReference.Resource;
                        if (reference.Equals("#" + spouse.Id))
                        {
                            return relationship;
                        }
                    }
                    personReference = relationship.Mother;
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
    }
}
