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
    /// <summary>
    /// The ChildAndParentsRelationshipState exposes management and other FamilySearch specific functions for a children and parents.
    /// </summary>
    public class ChildAndParentsRelationshipState : GedcomxApplicationState<FamilySearchPlatform>, PreferredRelationshipState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildAndParentsRelationshipState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal ChildAndParentsRelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public override String SelfRel
        {
            get
            {
                return Rel.RELATIONSHIP;
            }
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new ChildAndParentsRelationshipState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was a GET and the response status is OK or GONE.</remarks>
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

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Relationship;
            }
        }

        /// <summary>
        /// Gets the first relationship from <see cref="P:FamilySearchPlatform.ChildAndParentsRelationships"/> of the current <see cref="FamilySearchPlatform"/>.
        /// </summary>
        /// <value>
        /// The first relationship from <see cref="P:FamilySearchPlatform.ChildAndParentsRelationships"/> of the current <see cref="FamilySearchPlatform"/>.
        /// </value>
        public ChildAndParentsRelationship Relationship
        {
            get
            {
                return Entity == null ? null : Entity.ChildAndParentsRelationships == null ? null : Entity.ChildAndParentsRelationships.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first conclusion for this relationship (see remarks).
        /// </summary>
        /// <value>
        /// The first conclusion for this relationship.
        /// </value>
        /// <remarks>The <see cref="FatherFact"/> is returned first if it is not null; otherwise, <see cref="MotherFact"/> is returned.</remarks>
        public Conclusion Conclusion
        {
            get
            {
                return FatherFact != null ? FatherFact : MotherFact != null ? MotherFact : null;
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.FatherFacts"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.FatherFacts"/> from the current <see cref="Relationship"/>.
        /// </value>
        public Fact FatherFact
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.FatherFacts == null ? null : relationship.FatherFacts.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.MotherFacts"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.MotherFacts"/> from the current <see cref="Relationship"/>.
        /// </value>
        public Fact MotherFact
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.MotherFacts == null ? null : relationship.MotherFacts.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.Notes"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.Notes"/> from the current <see cref="Relationship"/>.
        /// </value>
        public Note Note
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Notes == null ? null : relationship.Notes.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.Sources"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.Sources"/> from the current <see cref="Relationship"/>.
        /// </value>
        public SourceReference SourceReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Sources == null ? null : relationship.Sources.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.Evidence"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.Evidence"/> from the current <see cref="Relationship"/>.
        /// </value>
        public EvidenceReference EvidenceReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Evidence == null ? null : relationship.Evidence.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first <see cref="P:Relationship.Media"/> from the current <see cref="Relationship"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="P:Relationship.Media"/> from the current <see cref="Relationship"/>.
        /// </value>
        public SourceReference MediaReference
        {
            get
            {
                ChildAndParentsRelationship relationship = Relationship;
                return relationship == null ? null : relationship.Media == null ? null : relationship.Media.FirstOrDefault();
            }
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilySearchFamilyTree"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Creates a REST API request (with appropriate authentication headers).
        /// </summary>
        /// <param name="rel">This parameter is currently unused.</param>
        /// <returns>
        /// A REST API requeset (with appropriate authentication headers).
        /// </returns>
        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
        }

        /// <summary>
        /// Loads all embedded resources for which the current FamilySearchPlatform has links.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadEmbeddedResources(params StateTransitionOption[] options)
        {
            IncludeEmbeddedResources<FamilySearchPlatform>(this.Entity, options);
            return this;
        }

        /// <summary>
        /// Loads the embedded resources for the specified links.
        /// </summary>
        /// <param name="rels">The array of link names for which the current Gedcomx will be queried, and loaded if the links are present.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadEmbeddedResources(String[] rels, params StateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed<FamilySearchPlatform>(link, this.Entity, options);
                }
            }
            return this;
        }

        /// <summary>
        /// Loads conclusions for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadConclusions(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        /// <summary>
        /// Loads source references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadSourceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads media references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadMediaReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        /// <summary>
        /// Loads evidence references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadEvidenceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads notes for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState LoadNotes(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        /// <summary>
        /// Instantiates a new <see cref="ChildAndParentsRelationship"/> and only sets the <see cref="P:ChildAndParentsRelationship.Id"/> to the current relationship's ID.
        /// </summary>
        /// <returns>A new <see cref="ChildAndParentsRelationship"/> with a matching relationship ID for the current relationship ID.</returns>
        protected ChildAndParentsRelationship CreateEmptySelf()
        {
            ChildAndParentsRelationship relationship = new ChildAndParentsRelationship();
            relationship.Id = LocalSelfId;
            return relationship;
        }

        /// <summary>
        /// Gets the current <see cref="P:ChildAndParentsRelationship.Id"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="P:ChildAndParentsRelationship.Id"/>
        /// </value>
        protected String LocalSelfId
        {
            get
            {
                ChildAndParentsRelationship me = Relationship;
                return me == null ? null : me.Id;
            }
        }

        /// <summary>
        /// Adds a father fact to the current relationship.
        /// </summary>
        /// <param name="fact">The fact to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddFatherFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddFatherFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Add the specified father facts to the current relationship.
        /// </summary>
        /// <param name="facts">The facts to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddFatherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.FatherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Update the specified father fact on the current relationship.
        /// </summary>
        /// <param name="fact">The fact to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateFatherFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateFatherFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Update the specified father facts on the current relationship.
        /// </summary>
        /// <param name="facts">The facts to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateFatherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.FatherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Adds a mother fact to the current relationship.
        /// </summary>
        /// <param name="fact">The fact to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddMotherFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddMotherFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Adds the specified mother facts to the current relationship.
        /// </summary>
        /// <param name="facts">The facts to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddMotherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.MotherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Updates the specified mother fact on the relationship.
        /// </summary>
        /// <param name="fact">The fact to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMotherFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateMotherFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Updates the specified mother facts on the current relationship.
        /// </summary>
        /// <param name="facts">The facts to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMotherFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.MotherFacts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Updates the specified conclusions on the current relationship.
        /// </summary>
        /// <param name="relationship">The relationship to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Deletes the specified fact from the relationship.
        /// </summary>
        /// <param name="fact">The fact to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this relationship does not have a link to the resource.</exception>
        public ChildAndParentsRelationshipState DeleteFact(Fact fact, params StateTransitionOption[] options)
        {
            Link link = fact.GetLink(Rel.CONCLUSION);
            link = link == null ? fact.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedGedcomxRequest()).Build(link.Href, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a source reference from the <see cref="SourceDescriptionState"/> to the current relationship.
        /// </summary>
        /// <param name="source">The source reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddSourceReference(SourceDescriptionState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri();
            return AddSourceReference(reference, options);
        }

        /// <summary>
        /// Adds a source reference to the current relationship.
        /// </summary>
        /// <param name="reference">The reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference });
        }

        /// <summary>
        /// Adds the specified source references to the current relationship.
        /// </summary>
        /// <param name="refs">The source references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the specified source reference on the relationship.
        /// </summary>
        /// <param name="reference">The source reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the specified source references on the relationship.
        /// </summary>
        /// <param name="refs">The source references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the source references on the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with source references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Deletes the specified source reference from the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
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

        /// <summary>
        /// Adds a media reference to the current relationship.
        /// </summary>
        /// <param name="reference">The <see cref="SourceReference"/> that is a media reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds media references to the current relationship.
        /// </summary>
        /// <param name="refs">The array of source references that are media references to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        /// <summary>
        /// Updates the media reference for the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the media references for the current relationship.
        /// </summary>
        /// <param name="refs">The source references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        /// <summary>
        /// Updates the media references of the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with media references that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Deletes the specified media reference from the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
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

        /// <summary>
        /// Adds an evidence reference to the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the evidence references to the current relationship.
        /// </summary>
        /// <param name="refs">The evidence references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        /// <summary>
        /// Update the evidence reference for the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the evidence references for the current relationship.
        /// </summary>
        /// <param name="refs">The evidence references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the evidence references for the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with evidence references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Deletes the evidence reference from the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public ChildAndParentsRelationshipState DeleteEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
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

        /// <summary>
        /// Reads the specified note.
        /// </summary>
        /// <param name="note">The note to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
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

        /// <summary>
        /// Adds a note to the current relationship.
        /// </summary>
        /// <param name="note">The note to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddNote(Note note, params StateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Add the notes to the current relationship.
        /// </summary>
        /// <param name="notes">The notes to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddNotes(Note[] notes, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship);
        }

        /// <summary>
        /// Updates the specified note for the current relationship.
        /// </summary>
        /// <param name="note">The note to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateNote(Note note, params StateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Update the specified notes for the current relationship.
        /// </summary>
        /// <param name="notes">The notes to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateNotes(Note[] notes, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        /// <summary>
        /// Update the notes on the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with the notes to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Delete the specified note from the current relationship.
        /// </summary>
        /// <param name="note">The note to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
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

        /// <summary>
        /// Reads the change history of the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChangeHistoryState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Reads the child from the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Reads the father from the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Sets the father of the current relationship to the father specified.
        /// </summary>
        /// <param name="father">The father to use in the current relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateFather(PersonState father, params StateTransitionOption[] options)
        {
            return UpdateFather(father.GetSelfUri(), options);
        }

        /// <summary>
        /// Sets the father of the current relationship to the father specified.
        /// </summary>
        /// <param name="fatherId">The father URI to use in the current relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateFather(String fatherId, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Father = new ResourceReference(fatherId);
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddChildAndParentsRelationship(relationship);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the current father from the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Reads the mother from the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Sets the mother of the current relationship to the mother specified.
        /// </summary>
        /// <param name="mother">The mother to use in the current relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMother(PersonState mother, params StateTransitionOption[] options)
        {
            return UpdateMother(mother.GetSelfUri(), options);
        }

        /// <summary>
        /// Sets the mother of the current relationship to the mother specified.
        /// </summary>
        /// <param name="motherId">The mother URI to use in the current relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState UpdateMother(String motherId, params StateTransitionOption[] options)
        {
            ChildAndParentsRelationship relationship = CreateEmptySelf();
            relationship.Mother = new ResourceReference(motherId);
            FamilySearchPlatform fsp = new FamilySearchPlatform();
            fsp.AddChildAndParentsRelationship(relationship);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(fsp).Build(GetSelfUri(), Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the current mother from the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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

        /// <summary>
        /// Restores the current relationship (if it is currently deleted).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
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
