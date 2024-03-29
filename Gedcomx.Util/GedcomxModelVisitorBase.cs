﻿using System.Collections;

using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Source;

namespace Gx.Util
{
    /// <summary>
    /// Basic visitor logic for the GEDCOM X model.
    /// </summary>
    public class GedcomxModelVisitorBase : IGedcomxModelVisitor
    {
        /// <summary>
        /// The context stack of objects that are currently being visited.
        /// </summary>
        protected readonly Stack contextStack = new Stack();

        /// <summary>
        /// Visits the record set.
        /// </summary>
        /// <param name="rs">The record set to visit.</param>
        public virtual void VisitRecordSet(RecordSet rs)
        {
            this.contextStack.Push(rs);
            if (rs.AnyRecords())
            {
                foreach (var record in rs.Records)
                {
                    VisitGedcomx(record);
                }
            }

            //we're going to avoid visiting the metadata so that visitors don't get confused about the context
            //of the visit.
            //if (rs.Metadata != null) {
            //		visitGedcomx(rs.Metadata);
            //}
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the <see cref="Gx.Gedcomx"/> entity.
        /// </summary>
        /// <param name="gx">The <see cref="Gx.Gedcomx"/> to visit.</param>
        public virtual void VisitGedcomx(Gedcomx gx)
        {
            this.contextStack.Push(gx);

            if (gx.AnyPersons())
            {
                foreach (var person in gx.Persons)
                {
                    if (person != null)
                    {
                        VisitPerson(person);
                    }
                }
            }

            if (gx.AnyRelationships())
            {
                foreach (var relationship in gx.Relationships)
                {
                    if (relationship != null)
                    {
                        VisitRelationship(relationship);
                    }
                }
            }

            if (gx.AnySourceDescriptions())
            {
                foreach (var sourceDescription in gx.SourceDescriptions)
                {
                    if (sourceDescription != null)
                    {
                        VisitSourceDescription(sourceDescription);
                    }
                }
            }

            if (gx.AnyAgents())
            {
                foreach (var agent in gx.Agents)
                {
                    if (agent != null)
                    {
                        VisitAgent(agent);
                    }
                }
            }

            if (gx.AnyEvents())
            {
                foreach (var e in gx.Events)
                {
                    if (e != null)
                    {
                        VisitEvent(e);
                    }
                }
            }

            if (gx.AnyPlaces())
            {
                foreach (var place in gx.Places)
                {
                    if (place != null)
                    {
                        VisitPlaceDescription(place);
                    }
                }
            }

            if (gx.AnyDocuments())
            {
                foreach (var document in gx.Documents)
                {
                    if (document != null)
                    {
                        VisitDocument(document);
                    }
                }
            }

            if (gx.AnyFields())
            {
                foreach (var field in gx.Fields)
                {
                    if (field != null)
                    {
                        VisitField(field);
                    }
                }
            }

            if (gx.AnyRecordDescriptors())
            {
                foreach (var rd in gx.RecordDescriptors)
                {
                    if (rd != null)
                    {
                        VisitRecordDescriptor(rd);
                    }
                }
            }

            if (gx.AnyCollections())
            {
                foreach (var collection in gx.Collections)
                {
                    if (collection != null)
                    {
                        VisitCollection(collection);
                    }
                }
            }

            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        public virtual void VisitDocument(Document document)
        {
            this.contextStack.Push(document);
            VisitConclusion(document);
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the place description.
        /// </summary>
        /// <param name="place">The place description to visit.</param>
        public virtual void VisitPlaceDescription(PlaceDescription place)
        {
            this.contextStack.Push(place);
            VisitSubject(place);
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the event.
        /// </summary>
        /// <param name="e">The event to visit.</param>
        public virtual void VisitEvent(Event e)
        {
            this.contextStack.Push(e);

            VisitSubject(e);

            var date = e.Date;
            if (date != null)
            {
                VisitDate(date);
            }

            var place = e.Place;
            if (place != null)
            {
                VisitPlaceReference(place);
            }

            if (e.AnyRoles())
            {
                foreach (var role in e.Roles)
                {
                    VisitEventRole(role);
                }
            }

            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the event role.
        /// </summary>
        /// <param name="role">The event role to visit.</param>
        public virtual void VisitEventRole(EventRole role)
        {
            this.contextStack.Push(role);
            VisitConclusion(role);
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the agent.
        /// </summary>
        /// <param name="agent">The agent to visit.</param>
        /// <remarks>This specific class implementation does not currently visit the agent.</remarks>
        public virtual void VisitAgent(Gx.Agent.Agent agent)
        {
            //no-op.
        }

        /// <summary>
        /// Visits the source description.
        /// </summary>
        /// <param name="sourceDescription">The source description to visit.</param>
        public virtual void VisitSourceDescription(SourceDescription sourceDescription)
        {
            this.contextStack.Push(sourceDescription);
            if (sourceDescription.AnySources())
            {
                foreach (var source in sourceDescription.Sources)
                {
                    VisitSourceReference(source);
                }
            }

            if (sourceDescription.AnyNotes())
            {
                foreach (var note in sourceDescription.Notes)
                {
                    VisitNote(note);
                }
            }

            if (sourceDescription.AnyCitations())
            {
                foreach (var citation in sourceDescription.Citations)
                {
                    VisitSourceCitation(citation);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the source citation.
        /// </summary>
        /// <param name="citation">The source citation to visit.</param>
        /// <remarks>This specific class implementation does not currently visit the source citation.</remarks>
        public virtual void VisitSourceCitation(SourceCitation citation)
        {
            //no-op.
        }

        /// <summary>
        /// Visits the collection.
        /// </summary>
        /// <param name="collection">The collection to visit.</param>
        public virtual void VisitCollection(Collection collection)
        {
            this.contextStack.Push(collection);
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the facet.
        /// </summary>
        /// <param name="facet">The facet to visit.</param>
        public virtual void VisitFacet(Facet facet)
        {
            this.contextStack.Push(facet);
            if (facet.AnyFacets())
            {
                foreach (var f in facet.Facets)
                {
                    VisitFacet(f);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the record descriptor.
        /// </summary>
        /// <param name="recordDescriptor">The record descriptor to visit.</param>
        /// <remarks>This specific class implementation does not currently visit the record descriptor.</remarks>
        public virtual void VisitRecordDescriptor(RecordDescriptor recordDescriptor)
        {
            //no-op.
        }

        /// <summary>
        /// Visits the field.
        /// </summary>
        /// <param name="field">The field to visit.</param>
        public virtual void VisitField(Field field)
        {
            this.contextStack.Push(field);

            if (field.AnyValues())
            {
                foreach (var v in field.Values)
                {
                    VisitFieldValue(v);
                }
            }

            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the field value.
        /// </summary>
        /// <param name="fieldValue">The field value to visit.</param>
        public virtual void VisitFieldValue(FieldValue fieldValue)
        {
            this.contextStack.Push(fieldValue);
            VisitConclusion(fieldValue);
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the relationship.
        /// </summary>
        /// <param name="relationship">The relationship to visit.</param>
        public virtual void VisitRelationship(Relationship relationship)
        {
            this.contextStack.Push(relationship);
            VisitSubject(relationship);

            if (relationship.AnyFacts())
            {
                foreach (var fact in relationship.Facts)
                {
                    VisitFact(fact);
                }
            }

            if (relationship.AnyFields())
            {
                foreach (var field in relationship.Fields)
                {
                    VisitField(field);
                }
            }

            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the person.
        /// </summary>
        /// <param name="person">The person to visit.</param>
        public virtual void VisitPerson(Person person)
        {
            this.contextStack.Push(person);
            VisitSubject(person);

            if (person.Gender != null)
            {
                VisitGender(person.Gender);
            }

            if (person.AnyNames())
            {
                foreach (var name in person.Names)
                {
                    VisitName(name);
                }
            }

            if (person.AnyFacts())
            {
                foreach (var fact in person.Facts)
                {
                    VisitFact(fact);
                }
            }

            if (person.AnyFields())
            {
                foreach (var field in person.Fields)
                {
                    VisitField(field);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the fact.
        /// </summary>
        /// <param name="fact">The fact to visit.</param>
        public virtual void VisitFact(Fact fact)
        {
            this.contextStack.Push(fact);
            VisitConclusion(fact);
            if (fact.Date != null)
            {
                VisitDate(fact.Date);
            }

            if (fact.Place != null)
            {
                VisitPlaceReference(fact.Place);
            }

            if (fact.AnyFields())
            {
                foreach (var field in fact.Fields)
                {
                    VisitField(field);
                }
            }

            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the place reference.
        /// </summary>
        /// <param name="place">The place reference to visit.</param>
        public virtual void VisitPlaceReference(PlaceReference place)
        {
            this.contextStack.Push(place);
            if (place.AnyFields())
            {
                foreach (var field in place.Fields)
                {
                    VisitField(field);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the date.
        /// </summary>
        /// <param name="date">The date to visit.</param>
        public virtual void VisitDate(DateInfo date)
        {
            this.contextStack.Push(date);
            if (date.AnyFields())
            {
                foreach (var field in date.Fields)
                {
                    VisitField(field);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the name.
        /// </summary>
        /// <param name="name">The name to visit.</param>
        public virtual void VisitName(Name name)
        {
            this.contextStack.Push(name);
            VisitConclusion(name);

            if (name.AnyNameForms())
            {
                foreach (var form in name.NameForms)
                {
                    VisitNameForm(form);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the name form.
        /// </summary>
        /// <param name="form">The name form to visit.</param>
        public virtual void VisitNameForm(NameForm form)
        {
            this.contextStack.Push(form);
            if (form.AnyParts())
            {
                foreach (var part in form.Parts)
                {
                    VisitNamePart(part);
                }
            }

            if (form.AnyFields())
            {
                foreach (var field in form.Fields)
                {
                    VisitField(field);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the name part.
        /// </summary>
        /// <param name="part">The name part to visit.</param>
        public virtual void VisitNamePart(NamePart part)
        {
            this.contextStack.Push(part);
            if (part.AnyFields())
            {
                foreach (var field in part.Fields)
                {
                    VisitField(field);
                }
            }
            this.contextStack.Pop();
        }

        /// <summary>
        /// Visits the gender.
        /// </summary>
        /// <param name="gender">The gender to visit.</param>
        public virtual void VisitGender(Gender gender)
        {
            this.contextStack.Push(gender);
            VisitConclusion(gender);

            if (gender.AnyFields())
            {
                foreach (var field in gender.Fields)
                {
                    VisitField(field);
                }
            }

            this.contextStack.Pop();

        }

        /// <summary>
        /// Visits the source reference.
        /// </summary>
        /// <param name="sourceReference">The source reference to visit.</param>
        /// <remarks>This specific class implementation does not currently visit the source reference.</remarks>
        public virtual void VisitSourceReference(SourceReference sourceReference)
        {
            //no-op.
        }

        /// <summary>
        /// Visits the note.
        /// </summary>
        /// <param name="note">The note to visit.</param>
        /// <remarks>This specific class implementation does not currently visit the note.</remarks>
        public virtual void VisitNote(Note note)
        {
            //no-op.
        }

        /// <summary>
        /// Visits the subject.
        /// </summary>
        /// <param name="subject">The subject to visit.</param>
        protected void VisitSubject(Subject subject)
        {
            VisitConclusion(subject);

            if (subject.AnyMedia())
            {
                foreach (var reference in subject.Media)
                {
                    VisitSourceReference(reference);
                }
            }
        }

        /// <summary>
        /// Visits the conclusion.
        /// </summary>
        /// <param name="conclusion">The conclusion to visit.</param>
        protected void VisitConclusion(Gx.Conclusion.Conclusion conclusion)
        {
            if (conclusion.AnySources())
            {
                foreach (var sourceReference in conclusion.Sources)
                {
                    VisitSourceReference(sourceReference);
                }
            }

            if (conclusion.AnyNotes())
            {
                foreach (var note in conclusion.Notes)
                {
                    VisitNote(note);
                }
            }
        }
    }
}

