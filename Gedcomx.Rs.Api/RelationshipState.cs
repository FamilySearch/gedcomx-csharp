using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gedcomx.Model;
using Gx.Common;
using Gx.Source;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The RelationshipState exposes management functions for a relationship.
    /// </summary>
    public class RelationshipState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal RelationshipState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new RelationshipState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override ISupportsLinks MainDataElement
        {
            get
            {
                return Relationship;
            }
        }

        /// <summary>
        /// Gets the first <see cref="Relationship"/> of the current <see cref="P:Gedcomx.Relationships"/>.
        /// </summary>
        /// <value>
        /// The first <see cref="Relationship"/> of the current <see cref="P:Gedcomx.Reltionships"/>.
        /// </value>
        public Relationship Relationship
        {
            get
            {
                return Entity == null ? null : Entity.Relationships == null ? null : Entity.Relationships.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the <see cref="Fact"/>, which is a conclusion. This is simply another property for <see cref="Fact"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Fact"/>, which is a conclusion.
        /// </value>
        public Conclusion.Conclusion Conclusion
        {
            get
            {
                return Fact;
            }
        }

        /// <summary>
        /// Gets the first fact from <see cref="P:Relationship.Fact"/>.
        /// </summary>
        /// <value>
        /// The first fact from <see cref="P:Relationship.Fact"/>.
        /// </value>
        public Fact Fact
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Facts == null ? null : relationship.Facts.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first note from <see cref="P:Relationship.Notes"/>.
        /// </summary>
        /// <value>
        /// The first note from <see cref="P:Relationship.Notes"/>.
        /// </value>
        public Note Note
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Notes == null ? null : relationship.Notes.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first source reference from <see cref="P:Relationship.Sources"/>.
        /// </summary>
        /// <value>
        /// The first source reference from <see cref="P:Relationship.Sources"/>.
        /// </value>
        public SourceReference SourceReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Sources == null ? null : relationship.Sources.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first evidence reference from <see cref="P:Relationship.Evidence"/>.
        /// </summary>
        /// <value>
        /// The first evidence reference from <see cref="P:Relationship.Evidence"/>.
        /// </value>
        public EvidenceReference EvidenceReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Evidence == null ? null : relationship.Evidence.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first media reference from <see cref="P:Relationship.Media"/>.
        /// </summary>
        /// <value>
        /// The first media reference from <see cref="P:Relationship.Media"/>.
        /// </value>
        public SourceReference MediaReference
        {
            get
            {
                Relationship relationship = Relationship;
                return relationship == null ? null : relationship.Media == null ? null : relationship.Media.FirstOrDefault();
            }
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person1 specified by this relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadPerson1(params IStateTransitionOption[] options)
        {
            Relationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference person1 = relationship.Person1;
            if (person1 == null || person1.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(person1.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person2 specified by this relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadPerson2(params IStateTransitionOption[] options)
        {
            Relationship relationship = Relationship;
            if (relationship == null)
            {
                return null;
            }

            ResourceReference person2 = relationship.Person2;
            if (person2 == null || person2.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(person2.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Loads all embedded resources for which the current Gedcomx has links.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadEmbeddedResources(params IStateTransitionOption[] options)
        {
            IncludeEmbeddedResources<Gedcomx>(this.Entity);
            return this;
        }

        /// <summary>
        /// Loads the embedded resources for the specified links.
        /// </summary>
        /// <param name="rels">The array of link names for which the current Gedcomx will be queried, and loaded if the links are present.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadEmbeddedResources(String[] rels, params IStateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed<Gedcomx>(link, this.Entity, options);
                }
            }
            return this;
        }

        /// <summary>
        /// Loads conclusions for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadConclusions(params IStateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        /// <summary>
        /// Loads source references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadSourceReferences(params IStateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads media references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadMediaReferences(params IStateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        /// <summary>
        /// Loads evidence references for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadEvidenceReferences(params IStateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads notes for the current relationship.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState LoadNotes(params IStateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        /// <summary>
        /// Instantiates a new <see cref="Relationship"/> and only sets the <see cref="P:Relationship.Id"/> to the current relationship's ID.
        /// </summary>
        /// <returns>A new <see cref="Relationship"/> with a matching relationship ID for the current relationship ID.</returns>
        protected Relationship CreateEmptySelf()
        {
            Relationship relationship = new Relationship();
            relationship.Id = LocalSelfId;
            return relationship;
        }

        /// <summary>
        /// Gets the current <see cref="P:Relationship.Id"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="P:Relationship.Id"/>
        /// </value>
        protected String LocalSelfId
        {
            get
            {
                Relationship me = Relationship;
                return me == null ? null : me.Id;
            }
        }

        /// <summary>
        /// Adds a fact to the current relationship.
        /// </summary>
        /// <param name="fact">The fact to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddFact(Fact fact, params IStateTransitionOption[] options)
        {
            return AddFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Adds facts to the current relationship.
        /// </summary>
        /// <param name="facts">The facts to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddFacts(Fact[] facts, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Facts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Updates the fact of the current relationship.
        /// </summary>
        /// <param name="fact">The fact to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateFact(Fact fact, params IStateTransitionOption[] options)
        {
            return UpdateFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Updates the facts of the current relationship.
        /// </summary>
        /// <param name="facts">The facts to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateFacts(Fact[] facts, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Facts = facts.ToList();
            return UpdateConclusions(relationship, options);
        }

        /// <summary>
        /// Updates the conclusions of the specified <see cref="Relationship"/>.
        /// </summary>
        /// <param name="relationship">The relationship with conclusions that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected RelationshipState UpdateConclusions(Relationship relationship, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.CONCLUSIONS);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the fact of the current relationship.
        /// </summary>
        /// <param name="fact">The fact to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState DeleteFact(Fact fact, params IStateTransitionOption[] options)
        {
            Link link = fact.GetLink(Rel.CONCLUSION);
            link = link == null ? fact.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the specified source reference (in the <see cref="SourceDescriptionState"/>) to the current relationship.
        /// </summary>
        /// <param name="source">The source reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddSourceReference(SourceDescriptionState source, params IStateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri();
            return AddSourceReference(reference, options);
        }

        /// <summary>
        /// Adds the specified source reference to the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddSourceReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the specified source references to the current relationship.
        /// </summary>
        /// <param name="refs">The source references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddSourceReferences(SourceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the specified source reference for the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateSourceReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the specified source references for the current relationship.
        /// </summary>
        /// <param name="refs">The source references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateSourceReferences(SourceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Sources = refs.ToList();
            return UpdateSourceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the source references for the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with source references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected RelationshipState UpdateSourceReferences(Relationship relationship, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.SOURCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified source reference from the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RelationshipState DeleteSourceReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.SOURCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Source reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a media reference to the current relationship.
        /// </summary>
        /// <param name="description">The <see cref="SourceDescriptionState"/> that represents the media reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddMediaReference(SourceDescriptionState description, params IStateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = description.GetSelfUri();
            return AddMediaReference(reference, options);
        }

        /// <summary>
        /// Adds a media reference to the current relationship.
        /// </summary>
        /// <param name="reference">The <see cref="SourceReference"/> that is a media reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddMediaReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds media references to the current relationship.
        /// </summary>
        /// <param name="refs">The array of source references that are media references to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddMediaReferences(SourceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        /// <summary>
        /// Updates the media reference for the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateMediaReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the media references for the current relationship.
        /// </summary>
        /// <param name="refs">The source references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateMediaReferences(SourceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Media = refs.ToList();
            return UpdateMediaReferences(relationship, options);
        }

        /// <summary>
        /// Updates the media references of the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with media references that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected RelationshipState UpdateMediaReferences(Relationship relationship, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.MEDIA_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified media reference from the current relationship.
        /// </summary>
        /// <param name="reference">The source reference to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RelationshipState DeleteMediaReference(SourceReference reference, params IStateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.MEDIA_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Media reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds an evidence reference to the current relationship.
        /// </summary>
        /// <param name="evidence">The evidence reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddEvidenceReference(RelationshipState evidence, params IStateTransitionOption[] options)
        {
            EvidenceReference reference = new EvidenceReference();
            reference.Resource = evidence.GetSelfUri();
            return AddEvidenceReference(reference, options);
        }

        /// <summary>
        /// Adds an evidence reference to the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddEvidenceReference(EvidenceReference reference, params IStateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the evidence references to the current relationship.
        /// </summary>
        /// <param name="refs">The evidence references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddEvidenceReferences(EvidenceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        /// <summary>
        /// Update the evidence reference for the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateEvidenceReference(EvidenceReference reference, params IStateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the evidence references for the current relationship.
        /// </summary>
        /// <param name="refs">The evidence references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateEvidenceReferences(EvidenceReference[] refs, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Evidence = refs.ToList();
            return UpdateEvidenceReferences(relationship, options);
        }

        /// <summary>
        /// Updates the evidence references for the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with evidence references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected RelationshipState UpdateEvidenceReferences(Relationship relationship, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.EVIDENCE_REFERENCES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the evidence reference from the current relationship.
        /// </summary>
        /// <param name="reference">The evidence reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RelationshipState DeleteEvidenceReference(EvidenceReference reference, params IStateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.EVIDENCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Evidence reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the specified note.
        /// </summary>
        /// <param name="note">The note to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RelationshipState ReadNote(Note note, params IStateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be read: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a note to the current relationship.
        /// </summary>
        /// <param name="note">The note to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddNote(Note note, params IStateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Add the notes to the current relationship.
        /// </summary>
        /// <param name="notes">The notes to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddNotes(Note[] notes, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        /// <summary>
        /// Updates the specified note for the current relationship.
        /// </summary>
        /// <param name="note">The note to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateNote(Note note, params IStateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Update the specified notes for the current relationship.
        /// </summary>
        /// <param name="notes">The notes to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState UpdateNotes(Note[] notes, params IStateTransitionOption[] options)
        {
            Relationship relationship = CreateEmptySelf();
            relationship.Notes = notes.ToList();
            return UpdateNotes(relationship, options);
        }

        /// <summary>
        /// Update the notes on the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship with the notes to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        protected RelationshipState UpdateNotes(Relationship relationship, params IStateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link conclusionsLink = GetLink(Rel.NOTES);
            if (conclusionsLink != null && conclusionsLink.Href != null)
            {
                target = conclusionsLink.Href;
            }

            Gedcomx gx = new Gedcomx();
            gx.Relationships = new List<Relationship>() { relationship };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Delete the specified note from the current relationship.
        /// </summary>
        /// <param name="note">The note to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RelationshipState DeleteNote(Note note, params IStateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
