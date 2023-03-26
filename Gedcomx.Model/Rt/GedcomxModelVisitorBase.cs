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
                foreach (Person person in gx.Persons)
                {
                    person?.Accept(this);
                }
            }

            if (gx.AnyRelationships())
            {
                foreach (Relationship relationship in gx.Relationships)
                {
                    relationship?.Accept(this);
                }
            }

            if (gx.AnySourceDescriptions())
            {
                foreach (SourceDescription sourceDescription in gx.SourceDescriptions)
                {
                    sourceDescription?.Accept(this);
                }
            }

            if (gx.AnyAgents())
            {
                foreach (Agent agent in gx.Agents)
                {
                    agent?.Accept(this);
                }
            }

            if (gx.AnyEvents())
            {
                foreach (Event @event in gx.Events)
                {
                    @event?.Accept(this);
                }
            }

            if (gx.AnyPlaces())
            {
                foreach (PlaceDescription place in gx.Places)
                {
                    place?.Accept(this);
                }
            }

            if (gx.AnyDocuments())
            {
                foreach (Document document in gx.Documents)
                {
                    document?.Accept(this);
                }
            }

            if (gx.AnyFields())
            {
                foreach (Field field in gx.Fields)
                {
                    field?.Accept(this);
                }
            }

            if (gx.AnyRecordDescriptors())
            {
                foreach (RecordDescriptor rd in gx.RecordDescriptors)
                {
                    rd?.Accept(this);
                }
            }

            if (gx.AnyCollections())
            {
                foreach (Collection collection in gx.Collections)
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
            @event.Date?.Accept(this);

            @event.Place?.Accept(this);

            if (@event.AnyRoles())
            {
                foreach (EventRole role in @event.Roles)
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
                foreach (SourceReference source in sourceDescription.Sources)
                {
                    source.Accept(this);
                }
            }

            if (sourceDescription.AnyNotes())
            {
                foreach (Note note in sourceDescription.Notes)
                {
                    note.Accept(this);
                }
            }

            if (sourceDescription.AnyCitations())
            {
                foreach (SourceCitation citation in sourceDescription.Citations)
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

            if (field.AnyValues())
            {
                var values = field.Values;
                foreach (var value in values)
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

            if (relationship.AnyFacts())
            {
                foreach (Fact fact in relationship.Facts)
                {
                    fact.Accept(this);
                }
            }

            if (relationship.AnyFields())
            {
                foreach (Field field in relationship.Fields)
                {
                    field.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        protected virtual void VisitConclusion(Conclusion conclusion)
        {
            if (conclusion.AnySources())
            {
                foreach (SourceReference sourceReference in conclusion.Sources)
                {
                    sourceReference.Accept(this);
                }
            }

            if (conclusion.AnyNotes())
            {
                foreach (Note note in conclusion.Notes)
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
                foreach (SourceReference reference in subject.Media)
                {
                    reference.Accept(this);
                }
            }

            if (subject.AnyEvidence())
            {
                foreach (EvidenceReference evidenceReference in subject.Evidence)
                {
                    evidenceReference.Accept(this);
                }
            }
        }

        public virtual void VisitPerson(Person person)
        {
            this.contextStack.Push(person);
            VisitSubject(person);

            person.Gender?.Accept(this);

            if (person.AnyNames())
            {
                foreach (Name name in person.Names)
                {
                    name.Accept(this);
                }
            }

            if (person.AnyFacts())
            {
                foreach (Fact fact in person.Facts)
                {
                    fact.Accept(this);
                }
            }

            if (person.AnyFields())
            {
                foreach (Field field in person.Fields)
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
            fact.Date?.Accept(this);

            fact.Place?.Accept(this);

            if (fact.AnyFields())
            {
                foreach (Field field in fact.Fields)
                {
                    field.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitPlaceReference(PlaceReference place)
        {
            this.contextStack.Push(place);
            if (place.AnyFields())
            {
                foreach (Field field in place.Fields)
                {
                    field.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitDate(DateInfo date)
        {
            this.contextStack.Push(date);
            if (date.AnyFields())
            {
                foreach (Field field in date.Fields)
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

            if (name.AnyNameForms())
            {
                foreach (NameForm form in name.NameForms)
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
                foreach (NamePart part in form.Parts)
                {
                    part.Accept(this);
                }
            }

            if (form.AnyFields())
            {
                foreach (var field in form.Fields)
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
                foreach (var field in part.Fields)
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

            if (gender.AnyFields())
            {
                foreach (var field in gender.Fields)
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
