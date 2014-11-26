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
    public class GedcomxLocalReferenceResolver : GedcomxModelVisitorBase
    {
        private readonly String resourceId;
        protected ExtensibleData resource;

        public GedcomxLocalReferenceResolver(String resourceId)
        {
            this.resourceId = resourceId;
        }

        public static ExtensibleData Resolve(ResourceReference @ref, Gx.Gedcomx document)
        {
            if (@ref.Resource == null)
            {
                return null;
            }

            return Resolve(@ref.Resource, document);
        }

        public static ExtensibleData Resolve(String @ref, Gx.Gedcomx document)
        {
            if (!@ref.ToString().StartsWith("#"))
            {
                return null;
            }

            GedcomxLocalReferenceResolver visitor = new GedcomxLocalReferenceResolver(@ref.ToString().Substring(1));
            document.Accept(visitor);
            return visitor.Resource;
        }

        public ExtensibleData Resource
        {
            get
            {
                return resource;
            }
        }

        protected void BindIfNeeded(ExtensibleData candidate)
        {
            if (resource == null && this.resourceId.Equals(candidate.Id))
            {
                this.resource = candidate;
            }
        }

        public override void VisitGedcomx(Gx.Gedcomx gx)
        {
            BindIfNeeded(gx);
            base.VisitGedcomx(gx);
        }

        public override void VisitDocument(Document document)
        {
            BindIfNeeded(document);
            base.VisitDocument(document);
        }

        public override void VisitPlaceDescription(PlaceDescription place)
        {
            BindIfNeeded(place);
            base.VisitPlaceDescription(place);
        }

        public override void VisitEvent(Event @event)
        {
            BindIfNeeded(@event);
            base.VisitEvent(@event);
        }

        public override void VisitEventRole(EventRole role)
        {
            BindIfNeeded(role);
            base.VisitEventRole(role);
        }

        public override void VisitAgent(Agent agent)
        {
            BindIfNeeded(agent);
            base.VisitAgent(agent);
        }

        public override void VisitSourceDescription(SourceDescription sourceDescription)
        {
            BindIfNeeded(sourceDescription);
            base.VisitSourceDescription(sourceDescription);
        }

        public override void VisitSourceCitation(SourceCitation citation)
        {
            BindIfNeeded(citation);
            base.VisitSourceCitation(citation);
        }

        public override void VisitCollection(Collection collection)
        {
            BindIfNeeded(collection);
            base.VisitCollection(collection);
        }

        public override void VisitRecordDescriptor(RecordDescriptor recordDescriptor)
        {
            BindIfNeeded(recordDescriptor);
            base.VisitRecordDescriptor(recordDescriptor);
        }

        public void visitField(Field field)
        {
            BindIfNeeded(field);
            base.VisitField(field);
        }

        public override void VisitFieldValue(FieldValue fieldValue)
        {
            BindIfNeeded(fieldValue);
            base.VisitFieldValue(fieldValue);
        }

        public override void VisitRelationship(Relationship relationship)
        {
            BindIfNeeded(relationship);
            base.VisitRelationship(relationship);
        }

        protected override void VisitConclusion(Conclusion conclusion)
        {
            BindIfNeeded(conclusion);
            base.VisitConclusion(conclusion);
        }

        protected override void VisitSubject(Subject subject)
        {
            BindIfNeeded(subject);
            base.VisitSubject(subject);
        }

        public override void VisitPerson(Person person)
        {
            BindIfNeeded(person);
            base.VisitPerson(person);
        }

        public override void VisitFact(Fact fact)
        {
            BindIfNeeded(fact);
            base.VisitFact(fact);
        }

        public override void VisitPlaceReference(PlaceReference place)
        {
            BindIfNeeded(place);
            base.VisitPlaceReference(place);
        }

        public override void VisitDate(DateInfo date)
        {
            BindIfNeeded(date);
            base.VisitDate(date);
        }

        public override void VisitName(Name name)
        {
            BindIfNeeded(name);
            base.VisitName(name);
        }

        public override void VisitNameForm(NameForm form)
        {
            BindIfNeeded(form);
            base.VisitNameForm(form);
        }

        public override void VisitNamePart(NamePart part)
        {
            BindIfNeeded(part);
            base.VisitNamePart(part);
        }

        public override void VisitGender(Gender gender)
        {
            BindIfNeeded(gender);
            base.VisitGender(gender);
        }

        public override void VisitSourceReference(SourceReference sourceReference)
        {
            BindIfNeeded(sourceReference);
            base.VisitSourceReference(sourceReference);
        }

        public override void VisitNote(Note note)
        {
            BindIfNeeded(note);
            base.VisitNote(note);
        }

        public override void VisitEvidenceReference(EvidenceReference evidenceReference)
        {
            BindIfNeeded(evidenceReference);
            base.VisitEvidenceReference(evidenceReference);
        }
    }
}
