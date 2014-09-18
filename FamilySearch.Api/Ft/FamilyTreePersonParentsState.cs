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
        protected internal FamilyTreePersonParentsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilyTreePersonParentsState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

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
