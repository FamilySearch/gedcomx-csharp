using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Source;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model.Rt
{
    public class GedcomxModelVisitorBase : IGedcomxModelVisitor
    {
        protected readonly Stack contextStack = new Stack();

        public virtual void VisitGedcomx(Gx.Gedcomx gx)
        {
            this.contextStack.Push(gx);

            List<Person> persons = gx.Persons;
            if (persons != null)
            {
                foreach (Person person in persons)
                {
                    if (person != null)
                    {
                        person.Accept(this);
                    }
                }
            }

            List<Relationship> relationships = gx.Relationships;
            if (relationships != null)
            {
                foreach (Relationship relationship in relationships)
                {
                    if (relationship != null)
                    {
                        relationship.Accept(this);
                    }
                }
            }

            List<SourceDescription> sourceDescriptions = gx.SourceDescriptions;
            if (sourceDescriptions != null)
            {
                foreach (SourceDescription sourceDescription in sourceDescriptions)
                {
                    if (sourceDescription != null)
                    {
                        sourceDescription.Accept(this);
                    }
                }
            }

            List<Agent> agents = gx.Agents;
            if (agents != null)
            {
                foreach (Agent agent in agents)
                {
                    if (agent != null)
                    {
                        agent.Accept(this);
                    }
                }
            }

            List<Event> events = gx.Events;
            if (events != null)
            {
                foreach (Event @event in events)
                {
                    if (@event != null)
                    {
                        @event.Accept(this);
                    }
                }
            }

            List<PlaceDescription> places = gx.Places;
            if (places != null)
            {
                foreach (PlaceDescription place in places)
                {
                    if (place != null)
                    {
                        place.Accept(this);
                    }
                }
            }

            List<Document> documents = gx.Documents;
            if (documents != null)
            {
                foreach (Document document in documents)
                {
                    if (document != null)
                    {
                        document.Accept(this);
                    }
                }
            }

            List<Field> fields = gx.Fields;
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    if (field != null)
                    {
                        field.Accept(this);
                    }
                }
            }

            List<RecordDescriptor> recordDescriptors = gx.RecordDescriptors;
            if (recordDescriptors != null)
            {
                foreach (RecordDescriptor rd in recordDescriptors)
                {
                    if (rd != null)
                    {
                        rd.Accept(this);
                    }
                }
            }

            List<Collection> collections = gx.Collections;
            if (collections != null)
            {
                foreach (Collection collection in collections)
                {
                    if (collection != null)
                    {
                        collection.Accept(this);
                    }
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

            List<EventRole> roles = @event.Roles;
            if (roles != null)
            {
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
            List<SourceReference> sources = sourceDescription.Sources;
            if (sources != null)
            {
                foreach (SourceReference source in sources)
                {
                    source.Accept(this);
                }
            }

            List<Note> notes = sourceDescription.Notes;
            if (notes != null)
            {
                foreach (Note note in notes)
                {
                    note.Accept(this);
                }
            }

            List<SourceCitation> citations = sourceDescription.Citations;
            if (citations != null)
            {
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

            List<SourceReference> media = subject.Media;
            if (media != null)
            {
                foreach (SourceReference reference in media)
                {
                    reference.Accept(this);
                }
            }

            List<EvidenceReference> evidence = subject.Evidence;
            if (evidence != null)
            {
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
            List<NamePart> parts = form.Parts;
            if (parts != null)
            {
                foreach (NamePart part in parts)
                {
                    part.Accept(this);
                }
            }

            List<Field> fields = form.Fields;
            if (fields != null)
            {
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
            List<Field> fields = part.Fields;
            if (fields != null)
            {
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
