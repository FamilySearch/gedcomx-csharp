using System.Collections;
using System.Collections.Generic;

using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Source;

namespace Gedcomx.Model.Rt
{
    public class GedcomxModelVisitorBase : IGedcomxModelVisitor
    {
        protected readonly Stack contextStack = new Stack();

        public virtual void VisitGedcomx(Gx.Gedcomx gx)
        {
            this.contextStack.Push(gx);

            if (gx.AnyPersons())
            {
                List<Person> persons = gx.Persons;
                foreach (Person person in persons)
                {
                    person?.Accept(this);
                }
            }

            if (gx.AnyRelationships())
            {
                List<Relationship> relationships = gx.Relationships;
                foreach (Relationship relationship in relationships)
                {
                    relationship?.Accept(this);
                }
            }

            if (gx.AnySourceDescriptions())
            {
                List<SourceDescription> sourceDescriptions = gx.SourceDescriptions;
                foreach (SourceDescription sourceDescription in sourceDescriptions)
                {
                    sourceDescription?.Accept(this);
                }
            }

            if (gx.AnyAgents())
            {
                List<Agent> agents = gx.Agents;
                foreach (Agent agent in agents)
                {
                    agent?.Accept(this);
                }
            }

            if (gx.AnyEvents())
            {
                List<Event> events = gx.Events;
                foreach (Event @event in events)
                {
                    @event?.Accept(this);
                }
            }

            if (gx.AnyPlaces())
            {
                List<PlaceDescription> places = gx.Places;
                foreach (PlaceDescription place in places)
                {
                    place?.Accept(this);
                }
            }

            if (gx.AnyDocuments())
            {
                List<Document> documents = gx.Documents;
                foreach (Document document in documents)
                {
                    document?.Accept(this);
                }
            }

            if (gx.AnyFields())
            {
                List<Field> fields = gx.Fields;
                foreach (Field field in fields)
                {
                    field?.Accept(this);
                }
            }

            if (gx.AnyRecordDescriptors())
            {
                List<RecordDescriptor> recordDescriptors = gx.RecordDescriptors;
                foreach (RecordDescriptor rd in recordDescriptors)
                {
                    rd?.Accept(this);
                }
            }

            if (gx.AnyCollections())
            {
                List<Collection> collections = gx.Collections;
                foreach (Collection collection in collections)
                {
                    collection?.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitDocument(Document document)
        {
            this.contextStack.Push(document);
            VisitConclusion(document);
            this.contextStack.Pop();
        }

        public virtual void VisitPlaceDescription(PlaceDescription place)
        {
            this.contextStack.Push(place);
            VisitSubject(place);
            this.contextStack.Pop();
        }

        public virtual void VisitEvent(Event @event)
        {
            this.contextStack.Push(@event);
            VisitSubject(@event);
            DateInfo date = @event.Date;
            if (date != null)
            {
                date.Accept(this);
            }

            PlaceReference place = @event.Place;
            if (place != null)
            {
                place.Accept(this);
            }

            if (@event.AnyRoles())
            {
                List<EventRole> roles = @event.Roles;
                foreach (EventRole role in roles)
                {
                    role.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitEventRole(EventRole role)
        {
            this.contextStack.Push(role);
            VisitConclusion(role);
            this.contextStack.Pop();
        }

        public virtual void VisitAgent(Agent agent)
        {
            //no-op.
        }

        public virtual void VisitSourceDescription(SourceDescription sourceDescription)
        {
            this.contextStack.Push(sourceDescription);
            if (sourceDescription.AnySources())
            {
                List<SourceReference> sources = sourceDescription.Sources;
                foreach (SourceReference source in sources)
                {
                    source.Accept(this);
                }
            }

            if (sourceDescription.AnyNotes())
            {
                List<Note> notes = sourceDescription.Notes;
                foreach (Note note in notes)
                {
                    note.Accept(this);
                }
            }

            if (sourceDescription.AnyCitations())
            {
                List<SourceCitation> citations = sourceDescription.Citations;
                foreach (SourceCitation citation in citations)
                {
                    citation.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitSourceCitation(SourceCitation citation)
        {
            //no-op.
        }

        public virtual void VisitCollection(Collection collection)
        {
        }

        public virtual void VisitRecordDescriptor(RecordDescriptor recordDescriptor)
        {
            //no-op.
        }

        public virtual void VisitField(Field field)
        {
            this.contextStack.Push(field);

            List<FieldValue> values = field.Values;
            if (values != null)
            {
                foreach (FieldValue value in values)
                {
                    value.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitFieldValue(FieldValue fieldValue)
        {
            this.contextStack.Push(fieldValue);
            VisitConclusion(fieldValue);
            this.contextStack.Pop();
        }

        public virtual void VisitRelationship(Relationship relationship)
        {
            this.contextStack.Push(relationship);
            VisitSubject(relationship);

            List<Fact> facts = relationship.Facts;
            if (facts != null)
            {
                foreach (Fact fact in facts)
                {
                    fact.Accept(this);
                }
            }

            List<Field> fields = relationship.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        protected virtual void VisitConclusion(Conclusion conclusion)
        {
            List<SourceReference> sourceReferences = conclusion.Sources;
            if (sourceReferences != null)
            {
                foreach (SourceReference sourceReference in sourceReferences)
                {
                    sourceReference.Accept(this);
                }
            }

            List<Note> notes = conclusion.Notes;
            if (notes != null)
            {
                foreach (Note note in notes)
                {
                    note.Accept(this);
                }
            }
        }

        protected virtual void VisitSubject(Subject subject)
        {
            VisitConclusion(subject);

            if (subject.AnyMedia())
            {
                List<SourceReference> media = subject.Media;
                foreach (SourceReference reference in media)
                {
                    reference.Accept(this);
                }
            }

            if (subject.AnyEvidence())
            {
                List<EvidenceReference> evidence = subject.Evidence;
                foreach (EvidenceReference evidenceReference in evidence)
                {
                    evidenceReference.Accept(this);
                }
            }
        }

        public virtual void VisitPerson(Person person)
        {
            this.contextStack.Push(person);
            VisitSubject(person);

            if (person.Gender != null)
            {
                person.Gender.Accept(this);
            }

            List<Name> names = person.Names;
            if (names != null)
            {
                foreach (Name name in names)
                {
                    name.Accept(this);
                }
            }

            List<Fact> facts = person.Facts;
            if (facts != null)
            {
                foreach (Fact fact in facts)
                {
                    fact.Accept(this);
                }
            }

            List<Field> fields = person.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitFact(Fact fact)
        {
            this.contextStack.Push(fact);
            VisitConclusion(fact);
            DateInfo date = fact.Date;
            if (date != null)
            {
                date.Accept(this);
            }

            PlaceReference place = fact.Place;
            if (place != null)
            {
                place.Accept(this);
            }

            List<Field> fields = fact.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitPlaceReference(PlaceReference place)
        {
            this.contextStack.Push(place);
            List<Field> fields = place.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitDate(DateInfo date)
        {
            this.contextStack.Push(date);
            List<Field> fields = date.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitName(Name name)
        {
            this.contextStack.Push(name);
            VisitConclusion(name);

            List<NameForm> forms = name.NameForms;
            if (forms != null)
            {
                foreach (NameForm form in forms)
                {
                    form.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitNameForm(NameForm form)
        {
            this.contextStack.Push(form);
            if (form.AnyParts())
            {
                List<NamePart> parts = form.Parts;
                foreach (NamePart part in parts)
                {
                    part.Accept(this);
                }
            }

            if (form.AnyFields())
            {
                List<Field> fields = form.Fields;
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitNamePart(NamePart part)
        {
            this.contextStack.Push(part);
            if (part.AnyFields())
            {
                List<Field> fields = part.Fields;
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitGender(Gender gender)
        {
            this.contextStack.Push(gender);
            VisitConclusion(gender);

            List<Field> fields = gender.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    field.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitSourceReference(SourceReference sourceReference)
        {
            //no-op
        }

        public virtual void VisitNote(Note note)
        {
            //no-op.
        }

        public virtual void VisitEvidenceReference(EvidenceReference evidenceReference)
        {
            //no-op
        }

        public Stack ContextStack
        {
            get
            {
                return contextStack;
            }
        }
    }
}
