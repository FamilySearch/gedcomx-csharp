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
    public class FamilyTreeRelationshipsState : RelationshipsState
    {
        protected internal FamilyTreeRelationshipsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilyTreeRelationshipsState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            return Response.StatusCode == HttpStatusCode.OK ? Response.ToIRestResponse<Gx.Gedcomx>().Data : null;
        }

        public override RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            if (relationship.KnownType == RelationshipType.ParentChild)
            {
                throw new GedcomxApplicationException("FamilySearch Family Tree doesn't support adding parent-child relationships. You must instead add a child-and-parents relationship.");
            }
            return base.AddRelationship(relationship, options);
        }

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

        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(ChildAndParentsRelationship chap, params StateTransitionOption[] options)
        {
            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { chap };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
