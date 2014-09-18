using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Links;
using Gx.Common;
using Gx.Types;

namespace Gx.Rs.Api
{
    public class RelationshipsState : GedcomxApplicationState<Gedcomx>
    {
        protected internal RelationshipsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new RelationshipsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public List<Relationship> Relationships
        {
            get
            {
                return Entity == null ? null : Entity.Relationships;
            }
        }

        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(person1.GetSelfUri());
            relationship.Person2 = new ResourceReference(person2.GetSelfUri());
            relationship.KnownType = RelationshipType.Couple;
            return AddRelationship(relationship, options);
        }

        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(parent.GetSelfUri());
            relationship.Person2 = new ResourceReference(child.GetSelfUri());
            relationship.KnownType = RelationshipType.ParentChild;
            return AddRelationship(relationship, options);
        }

        public virtual RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddRelationship(relationship);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
