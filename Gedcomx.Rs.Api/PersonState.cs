using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Common;
using Gx.Source;
using Gx.Links;
using Gx.Records;
using Gedcomx.Model;
using RestSharp.Extensions;
using Gedcomx.Support;

namespace Gx.Rs.Api
{
    public class PersonState : GedcomxApplicationState<Gedcomx>
    {

        public PersonState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        private PersonState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private PersonState(Uri uri, IFilterableRestClient client, StateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        internal PersonState(IRestRequest request, IFilterableRestClient client, StateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        protected internal PersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.PERSON;
            }
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new PersonState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Entity == null ? null : Entity.Persons == null ? null : Entity.Persons.FirstOrDefault();
            }
        }

        public Person Person
        {
            get
            {
                return (Person)MainDataElement;
            }
        }

        public List<Relationship> GetRelationships()
        {
            return Entity == null ? null : Entity.Relationships;
        }

        public List<Relationship> GetSpouseRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it during enumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.Couple)
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        public List<Relationship> GetChildRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it during enumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.ParentChild || !RefersToMe(relationship.Person1))
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        public List<Relationship> GetParentRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it duringenumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.ParentChild || !RefersToMe(relationship.Person2))
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        protected bool RefersToMe(ResourceReference @ref)
        {
            return @ref != null && @ref.Resource != null && @ref.Resource.ToString().Equals("#" + GetLocalSelfId());
        }

        public DisplayProperties GetDisplayProperties()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.DisplayExtension;
        }

        public Gx.Conclusion.Conclusion GetConclusion()
        {
            return GetName() != null ? GetName() : GetGender() != null ? (Gx.Conclusion.Conclusion)GetGender() : GetFact() != null ? GetFact() : null;
        }

        public Name GetName()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Names == null ? null : person.Names.FirstOrDefault();
        }

        public Gender GetGender()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Gender;
        }

        public Fact GetFact()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Facts == null ? null : person.Facts.FirstOrDefault();
        }

        public Note GetNote()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Notes == null ? null : person.Notes.FirstOrDefault();
        }

        public SourceReference GetSourceReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Sources == null ? null : person.Sources.FirstOrDefault();
        }

        public EvidenceReference GetEvidenceReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Evidence == null ? null : person.Evidence.FirstOrDefault();
        }

        public EvidenceReference GetPersonaReference()
        {
            return GetEvidenceReference();
        }

        public SourceReference GetMediaReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Media == null ? null : person.Media.FirstOrDefault();
        }

        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public AncestryResultsState ReadAncestry(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.ANCESTRY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewAncestryResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public DescendancyResultsState ReadDescendancy(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.DESCENDANCY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewDescendancyResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState LoadEmbeddedResources(params StateTransitionOption[] options)
        {
            IncludeEmbeddedResources<Gedcomx>(this.Entity, options);
            return this;
        }

        public PersonState LoadEmbeddedResources(String[] rels, params StateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = this.GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed<Gedcomx>(link, this.Entity, options);
                }
            }
            return this;
        }

        public PersonState LoadConclusions(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        public PersonState LoadSourceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        public PersonState LoadMediaReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        public PersonState LoadEvidenceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        public PersonState LoadPersonaReferences(params StateTransitionOption[] options)
        {
            return LoadEvidenceReferences(options);
        }

        public PersonState LoadNotes(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        public PersonState LoadParentRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.PARENT_RELATIONSHIPS }, options);
        }

        public PersonState LoadSpouseRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SPOUSE_RELATIONSHIPS }, options);
        }

        public PersonState LoadChildRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CHILD_RELATIONSHIPS }, options);
        }

        protected Person CreateEmptySelf()
        {
            Person person = new Person();
            person.Id = GetLocalSelfId();
            return person;
        }

        protected String GetLocalSelfId()
        {
            Person me = (Person)MainDataElement;
            return me == null ? null : me.Id;
        }

        public PersonState Update(Person person, params StateTransitionOption[] options)
        {
            if (this.GetLink(Rel.CONCLUSIONS) != null && (person.Names != null || person.Facts != null || person.Gender != null))
            {
                UpdateConclusions(person);
            }

            if (this.GetLink(Rel.EVIDENCE_REFERENCES) != null && person.Evidence != null)
            {
                UpdateEvidenceReferences(person);
            }

            if (this.GetLink(Rel.MEDIA_REFERENCES) != null && person.Media != null)
            {
                UpdateMediaReferences(person);
            }

            if (this.GetLink(Rel.SOURCE_REFERENCES) != null && person.Sources != null)
            {
                UpdateSourceReferences(person);
            }

            if (this.GetLink(Rel.NOTES) != null && person.Notes != null)
            {
                UpdateNotes(person);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddGender(Gender gender, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Gender = gender;
            return UpdateConclusions(person, options);
        }

        public PersonState AddName(Name name, params StateTransitionOption[] options)
        {
            return AddNames(new Name[] { name }, options);
        }

        public PersonState AddNames(Name[] names, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Names = names.ToList();
            return UpdateConclusions(person, options);
        }

        public PersonState AddFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddFacts(new Fact[] { fact }, options);
        }

        public PersonState AddFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Facts = facts.ToList();
            return UpdateConclusions(person, options);
        }

        public PersonState UpdateGender(Gender gender, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Gender = gender;
            return UpdateConclusions(person, options);
        }

        public PersonState UpdateName(Name name, params StateTransitionOption[] options)
        {
            return UpdateNames(new Name[] { name }, options);
        }

        public PersonState UpdateNames(Name[] names, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Names = names.ToList();
            return UpdateConclusions(person);
        }

        public PersonState UpdateFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateFacts(new Fact[] { fact }, options);
        }

        public PersonState UpdateFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Facts = facts.ToList();
            return UpdateConclusions(person, options);
        }

        public PersonState UpdateConclusions(Person person, params StateTransitionOption[] options)
        {
            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };

            return UpdateConclusions(gx, options);
        }

        public PersonState UpdateConclusions(Gedcomx gx, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.CONCLUSIONS);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState DeleteName(Name name, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(name, options);
        }

        public PersonState DeleteGender(Gender gender, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(gender, options);
        }

        public PersonState DeleteFact(Fact fact, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(fact, options);
        }

        protected PersonState DoDeleteConclusion(Gx.Conclusion.Conclusion conclusion, params StateTransitionOption[] options)
        {
            Link link = conclusion.GetLink(Rel.CONCLUSION);
            link = link == null ? conclusion.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddSourceReference(SourceDescriptionState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri().ToString();
            return AddSourceReference(reference, options);
        }

        public PersonState AddSourceReference(RecordState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri().ToString();
            return AddSourceReference(reference, options);
        }

        public PersonState AddSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference }, options);
        }

        public PersonState AddSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Sources = refs.ToList();
            return UpdateSourceReferences(person, options);
        }

        public PersonState UpdateSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        public PersonState UpdateSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Sources = refs.ToList();
            return UpdateSourceReferences(person, options);
        }

        public PersonState UpdateSourceReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.SOURCE_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState DeleteSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.SOURCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Source reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionsState ReadArtifacts(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.ARTIFACTS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionState AddArtifact(DataSource artifact, params StateTransitionOption[] options)
        {
            return AddArtifact(null, artifact, options);
        }

        public SourceDescriptionState AddArtifact(SourceDescription description, DataSource artifact, params StateTransitionOption[] options)
        {
            return CollectionState.AddArtifact(this, description, artifact, options);
        }

        public PersonState AddMediaReference(SourceDescriptionState description, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = description.GetSelfUri();
            return AddMediaReference(reference, options);
        }

        public PersonState AddMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        public PersonState AddMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Media = refs.ToList();
            return UpdateMediaReferences(person, options);
        }

        public PersonState UpdateMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        public PersonState UpdateMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Media = refs.ToList();
            return UpdateMediaReferences(person, options);
        }

        public PersonState UpdateMediaReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.MEDIA_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState DeleteMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.MEDIA_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Media reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddEvidenceReference(PersonState evidence, params StateTransitionOption[] options)
        {
            EvidenceReference reference = new EvidenceReference();
            reference.Resource = evidence.GetSelfUri().ToString();
            return AddEvidenceReference(reference, options);
        }

        public PersonState AddEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public PersonState AddEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Evidence = refs.ToList();
            return UpdateEvidenceReferences(person, options);
        }

        public PersonState UpdateEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public PersonState UpdateEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Evidence = refs.ToList();
            return UpdateEvidenceReferences(person, options);
        }

        public PersonState UpdateEvidenceReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.EVIDENCE_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState DeleteEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.EVIDENCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Evidence reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddPersonaReference(PersonState persona, params StateTransitionOption[] options)
        {
            return AddEvidenceReference(persona, options);
        }

        public PersonState AddPersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReference(reference, options);
        }

        public PersonState AddPersonaReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(refs, options);
        }

        public PersonState UpdatePersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReference(reference, options);
        }

        public PersonState UpdatePersonaReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(refs, options);
        }

        public PersonState UpdatePersonaReferences(Person person, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(person, options);
        }

        public PersonState DeletePersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return DeleteEvidenceReference(reference, options);
        }

        public PersonState ReadNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be read: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddNote(Note note, params StateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        public PersonState AddNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Notes = notes.ToList();
            return UpdateNotes(person, options);
        }

        public PersonState UpdateNote(Note note, params StateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        public PersonState UpdateNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Notes = notes.ToList();
            return UpdateNotes(person, options);
        }

        public PersonState UpdateNotes(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.NOTES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState DeleteNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
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

        public PersonState ReadRelative(Relationship relationship, params StateTransitionOption[] options)
        {
            ResourceReference reference = null;
            if (RefersToMe(relationship.Person1))
            {
                reference = relationship.Person2;
            }
            else if (RefersToMe(relationship.Person2))
            {
                reference = relationship.Person1;
            }
            if (reference == null || reference.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(reference.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadFirstSpouse(params StateTransitionOption[] options)
        {
            return ReadSpouse(0, options);
        }

        public PersonState ReadSpouse(int index, params StateTransitionOption[] options)
        {
            List<Relationship> spouseRelationships = GetSpouseRelationships();
			if (spouseRelationships == null || spouseRelationships.Count <= index)
            {
                return null;
            }
            return ReadSpouse(spouseRelationships[index], options);
        }

        public PersonState ReadSpouse(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        public PersonSpousesState ReadSpouses(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SPOUSES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonSpousesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddSpouse(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddSpouseRelationship(this, person, options);
        }

        public PersonState ReadFirstChild(params StateTransitionOption[] options)
        {
            return ReadChild(0, options);
        }

        public PersonState ReadChild(int index, params StateTransitionOption[] options)
        {
            List<Relationship> childRelationships = GetChildRelationships();
            if (childRelationships.Count <= index)
            {
                return null;
            }
            return ReadChild(childRelationships[index], options);
        }

        public PersonState ReadChild(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        public PersonChildrenState ReadChildren(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.CHILDREN);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonChildrenState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddChild(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddParentChildRelationship(this, person, options);
        }

        public PersonState ReadFirstParent(params StateTransitionOption[] options)
        {
            return ReadParent(0, options);
        }

        public PersonState ReadParent(int index, params StateTransitionOption[] options)
        {
            List<Relationship> parentRelationships = GetParentRelationships();
            if (parentRelationships.Count <= index)
            {
                return null;
            }
            return ReadParent(parentRelationships[index], options);
        }

        public PersonState ReadParent(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        public PersonParentsState ReadParents(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PARENTS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonParentsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddParent(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddParentChildRelationship(person, this, options);
        }
    }
}
