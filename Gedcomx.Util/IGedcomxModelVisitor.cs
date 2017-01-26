using System;
using Gx;
using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Records;
using Gx.Source;
using Gx.Types;

namespace Gx.Util
{
    /// <summary>
    /// Visitor interface for GEDCOM X data.
    /// </summary>
    public interface IGedcomxModelVisitor
    {
        /// <summary>
        /// Visits the record set.
        /// </summary>
        /// <param name="rs">The record set to visit.</param>
        void VisitRecordSet (RecordSet rs);

        /// <summary>
        /// Visits the <see cref="Gx.Gedcomx"/> entity.
        /// </summary>
        /// <param name="gx">The <see cref="Gx.Gedcomx"/> entity to visit.</param>
        void VisitGedcomx (Gedcomx gx);

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        void VisitDocument (Document document);

        /// <summary>
        /// Visits the place description.
        /// </summary>
        /// <param name="place">The place description to visit.</param>
        void VisitPlaceDescription (PlaceDescription place);

        /// <summary>
        /// Visits the event.
        /// </summary>
        /// <param name="e">The event to visit.</param>
        void VisitEvent (Event e);

        /// <summary>
        /// Visits the event role.
        /// </summary>
        /// <param name="role">The event role to visit.</param>
        void VisitEventRole (EventRole role);

        /// <summary>
        /// Visits the agent.
        /// </summary>
        /// <param name="agent">The agent to visit.</param>
        void VisitAgent (Gx.Agent.Agent agent);

        /// <summary>
        /// Visits the source description.
        /// </summary>
        /// <param name="sourceDescription">The source description to visit.</param>
        void VisitSourceDescription (SourceDescription sourceDescription);

        /// <summary>
        /// Visits the source citation.
        /// </summary>
        /// <param name="citation">The source citation to visit.</param>
        void VisitSourceCitation (SourceCitation citation);

        /// <summary>
        /// Visits the collection.
        /// </summary>
        /// <param name="collection">The collection to visit.</param>
        void VisitCollection (Collection collection);

        /// <summary>
        /// Visits the facet.
        /// </summary>
        /// <param name="facet">The facet to visit.</param>
        void VisitFacet (Facet facet);

        /// <summary>
        /// Visits the record descriptor.
        /// </summary>
        /// <param name="recordDescriptor">The record descriptor to visit.</param>
        void VisitRecordDescriptor (RecordDescriptor recordDescriptor);

        /// <summary>
        /// Visits the field.
        /// </summary>
        /// <param name="field">The field to visit.</param>
        void VisitField (Field field);

        /// <summary>
        /// Visits the field value.
        /// </summary>
        /// <param name="fieldValue">The field value to visit.</param>
        void VisitFieldValue (FieldValue fieldValue);

        /// <summary>
        /// Visits the relationship.
        /// </summary>
        /// <param name="relationship">The relationship to visit.</param>
        void VisitRelationship (Relationship relationship);

        /// <summary>
        /// Visits the person.
        /// </summary>
        /// <param name="person">The person to visit.</param>
        void VisitPerson (Person person);

        /// <summary>
        /// Visits the fact.
        /// </summary>
        /// <param name="fact">The fact to visit.</param>
        void VisitFact (Fact fact);

        /// <summary>
        /// Visits the place reference.
        /// </summary>
        /// <param name="place">The place reference to visit.</param>
        void VisitPlaceReference (PlaceReference place);

        /// <summary>
        /// Visits the date.
        /// </summary>
        /// <param name="date">The date to visit.</param>
        void VisitDate (DateInfo date);

        /// <summary>
        /// Visits the name.
        /// </summary>
        /// <param name="name">The name to visit.</param>
        void VisitName (Name name);

        /// <summary>
        /// Visits the name form.
        /// </summary>
        /// <param name="form">The name form to visit.</param>
        void VisitNameForm (NameForm form);

        /// <summary>
        /// Visits the name part.
        /// </summary>
        /// <param name="part">The name part to visit.</param>
        void VisitNamePart (NamePart part);

        /// <summary>
        /// Visits the gender.
        /// </summary>
        /// <param name="gender">The gender to visit.</param>
        void VisitGender (Gender gender);

        /// <summary>
        /// Visits the source reference.
        /// </summary>
        /// <param name="sourceReference">The source reference to visit.</param>
        void VisitSourceReference (SourceReference sourceReference);

        /// <summary>
        /// Visits the note.
        /// </summary>
        /// <param name="note">The note to visit.</param>
        void VisitNote (Note note);
    }
}

