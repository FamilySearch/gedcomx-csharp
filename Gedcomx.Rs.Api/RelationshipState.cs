using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gedcomx.Model;
using Gx.Common;
using Gx.Source;
using Gx.Links;

namespace Gx.Rs.Api
{
    public class RelationshipState : GedcomxApplicationState<Gedcomx>
    {

        protected internal RelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.RELATIONSHIP;
            }
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new RelationshipState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Relationship;
            }
        }

        public Relationship Relationship
        {
            get
            {
                return Entity == null ? null : Entity.Relationships == null ? null : Entity.Relationships.FirstOrDefault();
            }
        }

        public Conclusion.Conclusion Conclusion
        {
            get
            {
                return Fact;
            }
        }

        public Fact Fact
        {
            get
      {
          Relationship relationship = Relationship;
          return relationship == null ? null : relationship.Facts == null ? null : relationship.Facts.FirstOrDefault();
      }
        }

        public Note Note
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Notes == null ? null : relationship.Notes.FirstOrDefault();
            }
        }

        public SourceReference SourceReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Sources == null ? null : relationship.Sources.FirstOrDefault();
            }
        }

        public EvidenceReference EvidenceReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Evidence == null ? null : relationship.Evidence.FirstOrDefault();
            }
        }

        public SourceReference MediaReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Media == null ? null : relationship.Media.FirstOrDefault();
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

        public PersonState ReadPerson1(params StateTransitionOption[] options)
        {
            Relationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference person1 = relationship.Person1;
            if (person1 == null || person1.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(person1.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadPerson2(params StateTransitionOption[] options)
        {
            Relationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference person2 = relationship.Person2;
            if (person2 == null || person2.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(person2.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState LoadEmbeddedResources(params StateTransitionOption[] options)
        {
            IncludeEmbeddedResources<Gedcomx>(this.Entity);
            return this;
        }

        public RelationshipState LoadEmbeddedResources(String[] rels, params StateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed<Gedcomx>(link, this.Entity, options);
                }
            }
            return this;
        }

        public RelationshipState LoadConclusions(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        public RelationshipState LoadSourceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        public RelationshipState LoadMediaReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        public RelationshipState LoadEvidenceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        public RelationshipState LoadNotes(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        protected Relationship CreateEmptySelf()
        {
            Relationship relationship = new Relationship();
            relationship.Id = LocalSelfId;
            return relationship;
        }

        protected String LocalSelfId
        {
            get
            {
                Relationship me = Relationship;
                return me == null ? null : me.Id;
            }
        }

        public RelationshipState AddFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddFacts(new Fact[] { fact }, options);
        }

        public RelationshipState AddFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Facts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        public RelationshipState UpdateFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateFacts(new Fact[] { fact }, options);
        }

        public RelationshipState UpdateFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Facts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        protected RelationshipState UpdateConclusions(Relationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.CONCLUSIONS);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState DeleteFact(Fact fact, params StateTransitionOption[] options)
        {
            Link link = fact.GetLink(Rel.CONCLUSION);
            link = link == null ? fact.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddSourceReference(SourceDescriptionState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri();
            return AddSourceReference(reference, options);
        }

        public RelationshipState AddSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference }, options);
        }

        public RelationshipState AddSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        public RelationshipState UpdateSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        public RelationshipState UpdateSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        protected RelationshipState UpdateSourceReferences(Relationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.SOURCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState DeleteSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.SOURCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Source reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddMediaReference(SourceDescriptionState description, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = description.GetSelfUri();
            return AddMediaReference(reference, options);
        }

        public RelationshipState AddMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        public RelationshipState AddMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        public RelationshipState UpdateMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        public RelationshipState UpdateMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        protected RelationshipState UpdateMediaReferences(Relationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.MEDIA_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState DeleteMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.MEDIA_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Media reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddEvidenceReference(RelationshipState evidence, params StateTransitionOption[] options)
        {
            EvidenceReference reference = new EvidenceReference();
            reference.Resource = evidence.GetSelfUri();
            return AddEvidenceReference(reference, options);
        }

        public RelationshipState AddEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public RelationshipState AddEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        public RelationshipState UpdateEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public RelationshipState UpdateEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        protected RelationshipState UpdateEvidenceReferences(Relationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.EVIDENCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState DeleteEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.EVIDENCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Evidence reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState ReadNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be read: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddNote(Note note, params StateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        public RelationshipState AddNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        public RelationshipState UpdateNote(Note note, params StateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        public RelationshipState UpdateNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        protected RelationshipState UpdateNotes(Relationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.NOTES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState DeleteNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be deleted: missing link.");
            }
            
            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
