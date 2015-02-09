using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model.Rt
{
    public interface IGedcomxModelVisitor
    {
        void VisitGedcomx(Gx.Gedcomx gx);

        void VisitDocument(Document document);

        void VisitPlaceDescription(PlaceDescription place);

        void VisitEvent(Event @event);

        void VisitEventRole(EventRole role);

        void VisitAgent(Agent agent);

        void VisitSourceDescription(SourceDescription sourceDescription);

        void VisitSourceCitation(SourceCitation citation);

        void VisitCollection(Collection collection);

        void VisitRecordDescriptor(RecordDescriptor recordDescriptor);

        void VisitField(Field field);

        void VisitFieldValue(FieldValue fieldValue);

        void VisitRelationship(Relationship relationship);

        void VisitPerson(Person person);

        void VisitFact(Fact fact);

        void VisitPlaceReference(PlaceReference place);

        void VisitDate(DateInfo date);

        void VisitName(Name name);

        void VisitNameForm(NameForm form);

        void VisitNamePart(NamePart part);

        void VisitGender(Gender gender);

        void VisitSourceReference(SourceReference sourceReference);

        void VisitNote(Note note);

        void VisitEvidenceReference(EvidenceReference evidenceReference);
    }
}
