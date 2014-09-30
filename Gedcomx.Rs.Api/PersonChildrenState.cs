using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gedcomx.Model;
using Gx.Conclusion;
using Gx.Common;
using Gx.Links;

namespace Gx.Rs.Api
{
    public class PersonChildrenState : GedcomxApplicationState<Gedcomx>
    {

        protected internal PersonChildrenState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonChildrenState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public List<Person> Persons
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Persons;
            }
        }

        public List<Relationship> Relationships
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Relationships;
            }
        }

        public Relationship FindRelationshipTo(Person child)
        {
            List<Relationship> relationships = Relationships;
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    ResourceReference childReference = relationship.Person2;
                    if (childReference != null)
                    {
                        String reference = childReference.Resource;
                        if (reference.Equals("#" + child.Id))
                        {
                            return relationship;
                        }
                    }
                }
            }
            return null;
        }

        public PersonState ReadPerson(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadChild(Person person, params StateTransitionOption[] options)
        {
            Link link = person.GetLink(Rel.PERSON);
            link = link == null ? person.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState ReadRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState RemoveRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState RemoveRelationshipTo(Person child, params StateTransitionOption[] options)
        {
            Relationship relationship = FindRelationshipTo(child);
            if (relationship == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: not found.");
            }

            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to remove relationship: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
