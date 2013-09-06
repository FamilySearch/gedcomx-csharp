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
        void VisitRecordSet (RecordSet rs);

        void VisitGedcomx (Gedcomx gx);

        void VisitDocument (Document document);
		
        void VisitPlaceDescription (PlaceDescription place);
		
        void VisitEvent (Event e);
		
        void VisitEventRole (EventRole role);
		
        void VisitAgent (Gx.Agent.Agent agent);
		
        void VisitSourceDescription (SourceDescription sourceDescription);
		
        void VisitSourceCitation (SourceCitation citation);
		
        void VisitCollection (Collection collection);
		
        void VisitFacet (Facet facet);
		
        void VisitRecordDescriptor (RecordDescriptor recordDescriptor);
		
        void VisitField (Field field);

        void VisitFieldValue (FieldValue fieldValue);
		
        void VisitRelationship (Relationship relationship);
		
        void VisitPerson (Person person);
		
        void VisitFact (Fact fact);
		
        void VisitPlaceReference (PlaceReference place);
		
        void VisitDate (DateInfo date);
		
        void VisitName (Name name);
		
        void VisitNameForm (NameForm form);
		
        void VisitNamePart (NamePart part);
		
        void VisitGender (Gender gender);
		
        void VisitSourceReference (SourceReference sourceReference);
		
        void VisitNote (Note note);
    }
}

