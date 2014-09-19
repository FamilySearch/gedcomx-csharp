using Gx.Fs;
using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using System.Net;
using Gedcomx.Model;
using Gx.Fs.Tree;
using Gx.Conclusion;
using Gx.Common;
using Gx.Source;
using Gx.Links;
using FamilySearch.Api.Util;

namespace FamilySearch.Api.Ft
{
    public class ChildAndParentsRelationshipState : GedcomxApplicationState<FamilySearchPlatform>, PreferredRelationshipState
    {
        protected internal ChildAndParentsRelationshipState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
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

        protected override GedcomxApplicationState<FamilySearchPlatform> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new ChildAndParentsRelationshipState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        protected override FamilySearchPlatform LoadEntityConditionally(IRestResponse response)
        {
            if (Request.Method == Method.GET && (response.StatusCode == HttpStatusCode.OK
                                                              || response.StatusCode == HttpStatusCode.Gone)
                    || response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return LoadEntity(response);
            }
            else
            {
                return null;
            }
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Relationship;
            }
        }

        public ChildAndParentsRelationship Relationship
        {
            get
            {
                return Entity == null ? null : Entity.ChildAndParentsRelationships == null ? null : Entity.ChildAndParentsRelationships.FirstOrDefault();
            }
        }

        public Conclusion Conclusion
        {
            get
            {
                return FatherFact != null ? FatherFact : MotherFact != null ? MotherFact : null;
            }
        }

        public Fact FatherFact
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.FatherFacts == null ? null : relationship.FatherFacts.FirstOrDefault();
            }
        }

        public Fact MotherFact
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.MotherFacts == null ? null : relationship.MotherFacts.FirstOrDefault();
            }
        }

        public Note Note
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Notes == null ? null : relationship.Notes.FirstOrDefault();
            }
        }

        public SourceReference SourceReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Sources == null ? null : relationship.Sources.FirstOrDefault();
            }
        }

        public EvidenceReference EvidenceReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Evidence == null ? null : relationship.Evidence.FirstOrDefault();
            }
        }

        public SourceReference MediaReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Media == null ? null : relationship.Media.FirstOrDefault();
            }
        }

        public FamilySearchFamilyTree ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return (FamilySearchFamilyTree)((FamilyTreeStateFactory)this.stateFactory).NewCollectionStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
        }

        public ChildAndParentsRelationshipState LoadEmbeddedResources(params StateTransitionOption[] options)
        {
            IncludeEmbeddedResources(this.Entity, options);
            return this;
        }

        public ChildAndParentsRelationshipState LoadEmbeddedResources(String[] rels, params StateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed(link, this.Entity, options);
                }
            }
            return this;
        }

        public ChildAndParentsRelationshipState LoadConclusions(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        public ChildAndParentsRelationshipState LoadSourceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        public ChildAndParentsRelationshipState LoadMediaReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        public ChildAndParentsRelationshipState LoadEvidenceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        public ChildAndParentsRelationshipState LoadNotes(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        protected ChildAndParentsRelationship CreateEmptySelf()
        {
            ChildAndParentsRelationship relationship = new ChildAndParentsRelationship();
            relationship.Id = LocalSelfId;
            return relationship;
        }

        protected String LocalSelfId
        {
            get
            {
                ChildAndParentsRelationship me = Relationship;
                return me == null ? null : me.Id;
            }
        }

        public ChildAndParentsRelationshipState AddFatherFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddFatherFacts(new Fact[] { fact }, options);
        }

        public ChildAndParentsRelationshipState AddFatherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.FatherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        public ChildAndParentsRelationshipState UpdateFatherFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateFatherFacts(new Fact[] { fact }, options);
        }

        public ChildAndParentsRelationshipState UpdateFatherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.FatherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        public ChildAndParentsRelationshipState AddMotherFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddMotherFacts(new Fact[] { fact }, options);
        }

        public ChildAndParentsRelationshipState AddMotherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.MotherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        public ChildAndParentsRelationshipState UpdateMotherFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateMotherFacts(new Fact[] { fact }, options);
        }

        public ChildAndParentsRelationshipState UpdateMotherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.MotherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        protected ChildAndParentsRelationshipState UpdateConclusions(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.CONCLUSIONS);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { relationship };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteFact(Fact fact, params StateTransitionOption[] options)
        {
            Link link = fact.GetLink(Rel.CONCLUSION);
            link = link == null ? fact.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState AddSourceReference(SourceDescriptionState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri();
            return AddSourceReference(reference, options);
        }

        public ChildAndParentsRelationshipState AddSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference });
        }

        public ChildAndParentsRelationshipState AddSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        public ChildAndParentsRelationshipState UpdateSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        public ChildAndParentsRelationshipState UpdateSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        protected ChildAndParentsRelationshipState UpdateSourceReferences(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.SOURCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { relationship };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.SOURCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Source reference cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState AddMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        public ChildAndParentsRelationshipState AddMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        public ChildAndParentsRelationshipState UpdateMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        public ChildAndParentsRelationshipState UpdateMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        protected ChildAndParentsRelationshipState UpdateMediaReferences(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.MEDIA_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { relationship };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.MEDIA_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Media reference cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState AddEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public ChildAndParentsRelationshipState AddEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        public ChildAndParentsRelationshipState UpdateEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        public ChildAndParentsRelationshipState UpdateEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        protected ChildAndParentsRelationshipState UpdateEvidenceReferences(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.EVIDENCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { relationship };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState deleteEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.EVIDENCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Evidence reference cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState ReadNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be read: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState AddNote(Note note, params StateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        public ChildAndParentsRelationshipState AddNotes(Note[] notes, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship);
        }

        public ChildAndParentsRelationshipState UpdateNote(Note note, params StateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        public ChildAndParentsRelationshipState UpdateNotes(Note[] notes, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        protected ChildAndParentsRelationshipState UpdateNotes(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.NOTES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            FamilySearchPlatform gx = new FamilySearchPlatform();
            gx.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { relationship };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(gx).Build(target, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChangeHistoryState ReadChangeHistory(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CHANGE_HISTORY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChangeHistoryState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadChild(params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference child = relationship.Child;
            if (child == null || child.Resource == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(child.Resource, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadFather(params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference father = relationship.Father;
            if (father == null || father.Resource == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(father.Resource, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState UpdateFather(PersonState father, params StateTransitionOption[] options)
        {
            return UpdateFather(father.GetSelfUri(), options);
        }

        public ChildAndParentsRelationshipState UpdateFather(String fatherId, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Father = new ResourceReference(fatherId);
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddChildAndParentsRelationship(relationship);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteFather(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.FATHER_ROLE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadMother(params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference mother = relationship.Mother;
            if (mother == null || mother.Resource == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(mother.Resource, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState UpdateMother(PersonState mother, params StateTransitionOption[] options)
        {
            return UpdateMother(mother.GetSelfUri(), options);
        }

        public ChildAndParentsRelationshipState UpdateMother(String motherId, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Mother = new ResourceReference(motherId);
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddChildAndParentsRelationship(relationship);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState DeleteMother(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.MOTHER_ROLE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public ChildAndParentsRelationshipState Restore(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RESTORE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
