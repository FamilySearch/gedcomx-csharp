using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Common;
using Gx.Source;
using Gx.Links;
using Gx.Records;
using Gedcomx.Model;
using RestSharp.Extensions;
using Gedcomx.Support;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The PersonState exposes management functions for a person.
    /// </summary>
    public class PersonState : GedcomxApplicationState<Gedcomx>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI of the person.</param>
        public PersonState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI of the person.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private PersonState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI of the person.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private PersonState(Uri uri, IFilterableRestClient client, StateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal PersonState(IRestRequest request, IFilterableRestClient client, StateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonState" /> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal PersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
                return Rel.PERSON;
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
            return new PersonState(request, response, client, this.CurrentAccessToken, this.stateFactory);
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
                return Entity == null ? null : Entity.Persons == null ? null : Entity.Persons.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the person from the main <see cref="Gedcomx"/> entity.
        /// </summary>
        /// <value>
        /// The person from the main <see cref="Gedcomx"/> entity.
        /// </value>
        public Person Person
        {
            get
            {
                return (Person)MainDataElement;
            }
        }

        /// <summary>
        /// Gets the relationships from the main <see cref="Gedcomx"/> entity.
        /// </summary>
        /// <returns>The relationships from the main <see cref="Gedcomx"/> entity</returns>
        public List<Relationship> GetRelationships()
        {
            return Entity == null ? null : Entity.Relationships;
        }

        /// <summary>
        /// Gets the spouse relationships from <see cref="GetRelationships"/>.
        /// </summary>
        /// <returns>The spouse relationships from <see cref="GetRelationships"/>.</returns>
        public List<Relationship> GetSpouseRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it during enumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.Couple)
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        /// <summary>
        /// Gets the child relationships from <see cref="GetRelationships"/>.
        /// </summary>
        /// <returns>The child relationships from <see cref="GetRelationships"/>.</returns>
        public List<Relationship> GetChildRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it during enumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.ParentChild || !RefersToMe(relationship.Person1))
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        /// <summary>
        /// Gets the parent relationships from <see cref="GetRelationships"/>.
        /// </summary>
        /// <returns>The parent relationships from <see cref="GetRelationships"/>.</returns>
        public List<Relationship> GetParentRelationships()
        {
            List<Relationship> relationships = GetRelationships();
            relationships = relationships == null ? null : new List<Relationship>(relationships);
            if (relationships != null)
            {
                foreach (var relationship in relationships.ToList() /*Make a copy to modify it duringenumeration*/)
                {
                    if (relationship.KnownType != Types.RelationshipType.ParentChild || !RefersToMe(relationship.Person2))
                    {
                        relationships.Remove(relationship);
                    }
                }
            }
            return relationships;
        }

        /// <summary>
        /// Determines if the specified <see cref="ResourceReference"/> refers to this state instance.
        /// </summary>
        /// <param name="ref">The resource reference to check.</param>
        /// <returns>True, if the specified resource reference refers to this state instance; otherwise, false.</returns>
        protected bool RefersToMe(ResourceReference @ref)
        {
            return @ref != null && @ref.Resource != null && @ref.Resource.ToString().Equals("#" + LocalSelfId);
        }

        /// <summary>
        /// Gets the display properties of current <see cref="Person"/>.
        /// </summary>
        /// <returns>The display properties of current <see cref="Person"/>.</returns>
        public DisplayProperties GetDisplayProperties()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.DisplayExtension;
        }

        /// <summary>
        /// Gets the first conclusion of the current <see cref="Person"/>.
        /// </summary>
        /// <returns>The first conclusion of the current <see cref="Person"/>.</returns>
        /// <remarks>
        /// The order of conclusions scanned are:
        /// <list type="number">
        ///     <item><see cref="P:Person.Names"/></item>
        ///     <item><see cref="P:Person.Gender"/></item>
        ///     <item><see cref="P:Person.Facts"/></item>
        /// </list>
        /// </remarks>
        public Gx.Conclusion.Conclusion GetConclusion()
        {
            return GetName() != null ? GetName() : GetGender() != null ? (Gx.Conclusion.Conclusion)GetGender() : GetFact() != null ? GetFact() : null;
        }

        /// <summary>
        /// Gets the first name from <see cref="P:Person.Names"/>.
        /// </summary>
        /// <returns>The first name from <see cref="P:Person.Names"/>.</returns>
        public Name GetName()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Names == null ? null : person.Names.FirstOrDefault();
        }

        /// <summary>
        /// Gets the gender from <see cref="P:Person.Gender"/>.
        /// </summary>
        /// <returns>The gender from <see cref="P:Person.Gender"/>.</returns>
        public Gender GetGender()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Gender;
        }

        /// <summary>
        /// Gets the first fact from <see cref="P:Person.Facts"/>.
        /// </summary>
        /// <returns>The first fact from <see cref="P:Person.Facts"/>.</returns>
        public Fact GetFact()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Facts == null ? null : person.Facts.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first note from <see cref="P:Person.Notes"/>.
        /// </summary>
        /// <returns>The first note from <see cref="P:Person.Notes"/>.</returns>
        public Note GetNote()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Notes == null ? null : person.Notes.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first source reference from <see cref="P:Person.Sources"/>.
        /// </summary>
        /// <returns>The first source reference from <see cref="P:Person.Sources"/>.</returns>
        public SourceReference GetSourceReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Sources == null ? null : person.Sources.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first evidence reference from <see cref="P:Person.Evidence"/>.
        /// </summary>
        /// <returns>The first evidence reference from <see cref="P:Person.Evidence"/>.</returns>
        public EvidenceReference GetEvidenceReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Evidence == null ? null : person.Evidence.FirstOrDefault();
        }

        /// <summary>
        /// Gets the persona reference. This is just another method for GetEvidenceReference().
        /// </summary>
        /// <returns>The persona reference from <see cref="GetEvidenceReference"/>.</returns>
        public EvidenceReference GetPersonaReference()
        {
            return GetEvidenceReference();
        }

        /// <summary>
        /// Gets the first media reference from <see cref="P:Person.Media"/>.
        /// </summary>
        /// <returns>The first media reference from <see cref="P:Person.Media"/>.</returns>
        public SourceReference GetMediaReference()
        {
            Person person = (Person)MainDataElement;
            return person == null ? null : person.Media == null ? null : person.Media.FirstOrDefault();
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the ancestry of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="AncestryResultsState"/> instance containing the REST API response.
        /// </returns>
        public AncestryResultsState ReadAncestry(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.ANCESTRY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewAncestryResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the descendancy of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="DescendancyResultsState"/> instance containing the REST API response.
        /// </returns>
        public DescendancyResultsState ReadDescendancy(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.DESCENDANCY);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewDescendancyResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Loads all embedded resources for which the current Gedcomx has links.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadEmbeddedResources(params StateTransitionOption[] options)
        {
            IncludeEmbeddedResources<Gedcomx>(this.Entity, options);
            return this;
        }

        /// <summary>
        /// Loads the embedded resources for the specified links.
        /// </summary>
        /// <param name="rels">The array of link names for which the current Gedcomx will be queried, and loaded if the links are present.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadEmbeddedResources(String[] rels, params StateTransitionOption[] options)
        {
            foreach (String rel in rels)
            {
                Link link = this.GetLink(rel);
                if (this.Entity != null && link != null && link.Href != null)
                {
                    Embed<Gedcomx>(link, this.Entity, options);
                }
            }
            return this;
        }

        /// <summary>
        /// Loads conclusions for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadConclusions(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CONCLUSIONS }, options);
        }

        /// <summary>
        /// Loads source references for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadSourceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SOURCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads media references for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadMediaReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.MEDIA_REFERENCES }, options);
        }

        /// <summary>
        /// Loads evidence references for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadEvidenceReferences(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.EVIDENCE_REFERENCES }, options);
        }

        /// <summary>
        /// Loads persona references for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadPersonaReferences(params StateTransitionOption[] options)
        {
            return LoadEvidenceReferences(options);
        }

        /// <summary>
        /// Loads notes for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadNotes(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.NOTES }, options);
        }

        /// <summary>
        /// Load parent relationships for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadParentRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.PARENT_RELATIONSHIPS }, options);
        }

        /// <summary>
        /// Load spouse relationships for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadSpouseRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.SPOUSE_RELATIONSHIPS }, options);
        }

        /// <summary>
        /// Load child relationships for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState LoadChildRelationships(params StateTransitionOption[] options)
        {
            return LoadEmbeddedResources(new String[] { Rel.CHILD_RELATIONSHIPS }, options);
        }

        /// <summary>
        /// Instantiates a new <see cref="Person"/> and only sets the <see cref="P:Person.Id"/> to the current person's ID.
        /// </summary>
        /// <returns>A new <see cref="Person"/> with a matching person ID for the current person ID.</returns>
        protected Person CreateEmptySelf()
        {
            Person person = new Person();
            person.Id = LocalSelfId;
            return person;
        }

        /// <summary>
        /// Gets the current <see cref="P:Person.Id" />.
        /// </summary>
        /// <value>
        /// The current <see cref="P:Person.Id"/>
        /// </value>
        protected String LocalSelfId
        {
            get
            {
                Person me = (Person)MainDataElement;
                return me == null ? null : me.Id;
            }
        }

        /// <summary>
        /// Updates the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>A <see cref="PersonState"/> instance containing the REST API response.</returns>
        public PersonState Update(Person person, params StateTransitionOption[] options)
        {
            if (this.GetLink(Rel.CONCLUSIONS) != null && (person.Names != null || person.Facts != null || person.Gender != null))
            {
                UpdateConclusions(person);
            }

            if (this.GetLink(Rel.EVIDENCE_REFERENCES) != null && person.Evidence != null)
            {
                UpdateEvidenceReferences(person);
            }

            if (this.GetLink(Rel.MEDIA_REFERENCES) != null && person.Media != null)
            {
                UpdateMediaReferences(person);
            }

            if (this.GetLink(Rel.SOURCE_REFERENCES) != null && person.Sources != null)
            {
                UpdateSourceReferences(person);
            }

            if (this.GetLink(Rel.NOTES) != null && person.Notes != null)
            {
                UpdateNotes(person);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a gender to the current person.
        /// </summary>
        /// <param name="gender">The gender to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddGender(Gender gender, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Gender = gender;
            return UpdateConclusions(person, options);
        }

        /// <summary>
        /// Adds a name to the current person.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddName(Name name, params StateTransitionOption[] options)
        {
            return AddNames(new Name[] { name }, options);
        }

        /// <summary>
        /// Adds names to the current person.
        /// </summary>
        /// <param name="names">The names to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddNames(Name[] names, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Names = names.ToList();
            return UpdateConclusions(person, options);
        }

        /// <summary>
        /// Adds a fact to the current person.
        /// </summary>
        /// <param name="fact">The fact to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddFact(Fact fact, params StateTransitionOption[] options)
        {
            return AddFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Adds facts to the current person.
        /// </summary>
        /// <param name="facts">The facts to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Facts = facts.ToList();
            return UpdateConclusions(person, options);
        }

        /// <summary>
        /// Updates the gender of the current person.
        /// </summary>
        /// <param name="gender">The gender to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateGender(Gender gender, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Gender = gender;
            return UpdateConclusions(person, options);
        }

        /// <summary>
        /// Updates the name of the current person.
        /// </summary>
        /// <param name="name">The name to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateName(Name name, params StateTransitionOption[] options)
        {
            return UpdateNames(new Name[] { name }, options);
        }

        /// <summary>
        /// Updates the names of the current person.
        /// </summary>
        /// <param name="names">The names to udpate.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateNames(Name[] names, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Names = names.ToList();
            return UpdateConclusions(person);
        }

        /// <summary>
        /// Updates the fact of the current person.
        /// </summary>
        /// <param name="fact">The fact to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateFact(Fact fact, params StateTransitionOption[] options)
        {
            return UpdateFacts(new Fact[] { fact }, options);
        }

        /// <summary>
        /// Updates the facts of the current person.
        /// </summary>
        /// <param name="facts">The facts to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateFacts(Fact[] facts, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Facts = facts.ToList();
            return UpdateConclusions(person, options);
        }

        /// <summary>
        /// Updates the conclusions of the specified <see cref="Person"/>.
        /// </summary>
        /// <param name="person">The person with conclusions that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateConclusions(Person person, params StateTransitionOption[] options)
        {
            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };

            return UpdateConclusions(gx, options);
        }

        /// <summary>
        /// Updates conclusions of the specified <see cref="Gedcomx"/>.
        /// </summary>
        /// <param name="gx">The Gedcomx with conclusions that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateConclusions(Gedcomx gx, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.CONCLUSIONS);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the name of the current person.
        /// </summary>
        /// <param name="name">The name to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState DeleteName(Name name, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(name, options);
        }

        /// <summary>
        /// Deletes the gender of the current person.
        /// </summary>
        /// <param name="gender">The gender to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState DeleteGender(Gender gender, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(gender, options);
        }

        /// <summary>
        /// Deletes the fact of the current person.
        /// </summary>
        /// <param name="fact">The fact to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState DeleteFact(Fact fact, params StateTransitionOption[] options)
        {
            return DoDeleteConclusion(fact, options);
        }

        /// <summary>
        /// Deletes the specified conclusion.
        /// </summary>
        /// <param name="conclusion">The conclusion to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        protected PersonState DoDeleteConclusion(Gx.Conclusion.Conclusion conclusion, params StateTransitionOption[] options)
        {
            Link link = conclusion.GetLink(Rel.CONCLUSION);
            link = link == null ? conclusion.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Conclusion cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the specified source reference (in the <see cref="SourceDescriptionState"/>) to the current person.
        /// </summary>
        /// <param name="source">The source reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddSourceReference(SourceDescriptionState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri().ToString();
            return AddSourceReference(reference, options);
        }

        /// <summary>
        /// Adds the specified source reference (in the <see cref="RecordState"/>) to the current person.
        /// </summary>
        /// <param name="source">The source reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddSourceReference(RecordState source, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = source.GetSelfUri().ToString();
            return AddSourceReference(reference, options);
        }

        /// <summary>
        /// Adds the specified source reference to the current person.
        /// </summary>
        /// <param name="reference">The source reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddSourceReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the specified source references to the current person.
        /// </summary>
        /// <param name="refs">The source references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Sources = refs.ToList();
            return UpdateSourceReferences(person, options);
        }

        /// <summary>
        /// Updates the specified source reference for the current person.
        /// </summary>
        /// <param name="reference">The source reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateSourceReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the specified source references for the current person.
        /// </summary>
        /// <param name="refs">The source references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateSourceReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Sources = refs.ToList();
            return UpdateSourceReferences(person, options);
        }

        /// <summary>
        /// Updates the source references for the specified person.
        /// </summary>
        /// <param name="person">The person with source references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateSourceReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.SOURCE_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified source reference from the current person.
        /// </summary>
        /// <param name="reference">The source reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState DeleteSourceReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.SOURCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Source reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads artifacts from the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionsState ReadArtifacts(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.ARTIFACTS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds an artifact to the current person.
        /// </summary>
        /// <param name="artifact">The artifact to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState AddArtifact(DataSource artifact, params StateTransitionOption[] options)
        {
            return AddArtifact(null, artifact, options);
        }

        /// <summary>
        /// Adds an artifact to the current person with the specified source description.
        /// </summary>
        /// <param name="description">The source description for the artifact being added.</param>
        /// <param name="artifact">The artifact to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState AddArtifact(SourceDescription description, DataSource artifact, params StateTransitionOption[] options)
        {
            return CollectionState.AddArtifact(this, description, artifact, options);
        }

        /// <summary>
        /// Adds a media reference to the current person.
        /// </summary>
        /// <param name="description">The <see cref="SourceDescriptionState"/> that represents the media reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddMediaReference(SourceDescriptionState description, params StateTransitionOption[] options)
        {
            SourceReference reference = new SourceReference();
            reference.DescriptionRef = description.GetSelfUri();
            return AddMediaReference(reference, options);
        }

        /// <summary>
        /// Adds a media reference to the current person.
        /// </summary>
        /// <param name="reference">The <see cref="SourceReference"/> that is a media reference to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return AddMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds media references to the current person.
        /// </summary>
        /// <param name="refs">The array of source references that are media references to be added.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Media = refs.ToList();
            return UpdateMediaReferences(person, options);
        }

        /// <summary>
        /// Updates the media reference for the current person.
        /// </summary>
        /// <param name="reference">The source reference to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            return UpdateMediaReferences(new SourceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the media references for the current person.
        /// </summary>
        /// <param name="refs">The source references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateMediaReferences(SourceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Media = refs.ToList();
            return UpdateMediaReferences(person, options);
        }

        /// <summary>
        /// Updates the media references of the specified person.
        /// </summary>
        /// <param name="person">The person with media references that will be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateMediaReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.MEDIA_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified media reference from the current person.
        /// </summary>
        /// <param name="reference">The source reference to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState DeleteMediaReference(SourceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.MEDIA_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Media reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds an evidence reference to the current person.
        /// </summary>
        /// <param name="evidence">The evidence reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddEvidenceReference(PersonState evidence, params StateTransitionOption[] options)
        {
            EvidenceReference reference = new EvidenceReference();
            reference.Resource = evidence.GetSelfUri().ToString();
            return AddEvidenceReference(reference, options);
        }

        /// <summary>
        /// Adds an evidence reference to the current person.
        /// </summary>
        /// <param name="reference">The evidence reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the evidence references to the current person.
        /// </summary>
        /// <param name="refs">The evidence references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Evidence = refs.ToList();
            return UpdateEvidenceReferences(person, options);
        }

        /// <summary>
        /// Update the evidence reference for the current person.
        /// </summary>
        /// <param name="reference">The evidence reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(new EvidenceReference[] { reference }, options);
        }

        /// <summary>
        /// Updates the evidence references for the current person.
        /// </summary>
        /// <param name="refs">The evidence references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateEvidenceReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Evidence = refs.ToList();
            return UpdateEvidenceReferences(person, options);
        }

        /// <summary>
        /// Updates the evidence references for the specified person.
        /// </summary>
        /// <param name="person">The person with evidence references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateEvidenceReferences(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.EVIDENCE_REFERENCES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the evidence reference from the current person.
        /// </summary>
        /// <param name="reference">The evidence reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState DeleteEvidenceReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.EVIDENCE_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Evidence reference cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a persona reference to the current person.
        /// </summary>
        /// <param name="persona">The persona reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPersonaReference(PersonState persona, params StateTransitionOption[] options)
        {
            return AddEvidenceReference(persona, options);
        }

        /// <summary>
        /// Adds a persona reference to the current person.
        /// </summary>
        /// <param name="reference">The persona reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return AddEvidenceReference(reference, options);
        }

        /// <summary>
        /// Adds the persona references to the current person.
        /// </summary>
        /// <param name="refs">The persona references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPersonaReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            return AddEvidenceReferences(refs, options);
        }

        /// <summary>
        /// Updates the persona reference for the current person.
        /// </summary>
        /// <param name="reference">The persona reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdatePersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReference(reference, options);
        }

        /// <summary>
        /// Updates the persona references for the current person.
        /// </summary>
        /// <param name="refs">The persona references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdatePersonaReferences(EvidenceReference[] refs, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(refs, options);
        }

        /// <summary>
        /// Updates the persona references for the specified person.
        /// </summary>
        /// <param name="person">The person with persona references to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdatePersonaReferences(Person person, params StateTransitionOption[] options)
        {
            return UpdateEvidenceReferences(person, options);
        }

        /// <summary>
        /// Deletes the specified persona reference for the current person.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState DeletePersonaReference(EvidenceReference reference, params StateTransitionOption[] options)
        {
            return DeleteEvidenceReference(reference, options);
        }

        /// <summary>
        /// Reads the specified note.
        /// </summary>
        /// <param name="note">The note to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState ReadNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be read: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds a note to the current person.
        /// </summary>
        /// <param name="note">The note to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddNote(Note note, params StateTransitionOption[] options)
        {
            return AddNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Add the notes to the current person.
        /// </summary>
        /// <param name="notes">The notes to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Notes = notes.ToList();
            return UpdateNotes(person, options);
        }

        /// <summary>
        /// Updates the specified note for the current person.
        /// </summary>
        /// <param name="note">The note to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateNote(Note note, params StateTransitionOption[] options)
        {
            return UpdateNotes(new Note[] { note }, options);
        }

        /// <summary>
        /// Update the specified notes for the current person.
        /// </summary>
        /// <param name="notes">The notes to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateNotes(Note[] notes, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            person.Notes = notes.ToList();
            return UpdateNotes(person, options);
        }

        /// <summary>
        /// Update the notes on the specified person.
        /// </summary>
        /// <param name="person">The person with the notes to be updated.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState UpdateNotes(Person person, params StateTransitionOption[] options)
        {
            Uri target = new Uri(GetSelfUri());
            Link link = this.GetLink(Rel.NOTES);
            if (link != null && link.Href != null)
            {
                target = new Uri(link.Href);
            }

            Gedcomx gx = new Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(gx).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Delete the specified note from the current person.
        /// </summary>
        /// <param name="note">The note to be deleted.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState DeleteNote(Note note, params StateTransitionOption[] options)
        {
            Link link = note.GetLink(Rel.NOTE);
            link = link == null ? note.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Note cannot be deleted: missing link.");
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.DELETE);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState ReadRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the relative in the specified relationship.
        /// </summary>
        /// <param name="relationship">The relationship to use reading the relative.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadRelative(Relationship relationship, params StateTransitionOption[] options)
        {
            ResourceReference reference = null;
            if (RefersToMe(relationship.Person1))
            {
                reference = relationship.Person2;
            }
            else if (RefersToMe(relationship.Person2))
            {
                reference = relationship.Person1;
            }
            if (reference == null || reference.Resource == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(reference.Resource, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the first spouse of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadFirstSpouse(params StateTransitionOption[] options)
        {
            return ReadSpouse(0, options);
        }

        /// <summary>
        /// Reads the spouse at the specified index of the current person.
        /// </summary>
        /// <param name="index">The index of the spouse to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadSpouse(int index, params StateTransitionOption[] options)
        {
            List<Relationship> spouseRelationships = GetSpouseRelationships();
			if (spouseRelationships == null || spouseRelationships.Count <= index)
            {
                return null;
            }
            return ReadSpouse(spouseRelationships[index], options);
        }

        /// <summary>
        /// Reads the spouse from the specified relationship of the current person.
        /// </summary>
        /// <param name="relationship">The relationship to use for reading the spouse.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadSpouse(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        /// <summary>
        /// Reads the spouses of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonSpousesState"/> instance containing the REST API response.
        /// </returns>
        public PersonSpousesState ReadSpouses(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SPOUSES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonSpousesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the specified person as a spouse to the current person.
        /// </summary>
        /// <param name="person">The person to add as a spouse.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if the current person's collection could not be read or produced a client or server error.</exception>
        public RelationshipState AddSpouse(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddSpouseRelationship(this, person, options);
        }

        /// <summary>
        /// Reads the first child of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadFirstChild(params StateTransitionOption[] options)
        {
            return ReadChild(0, options);
        }

        /// <summary>
        /// Reads the child at the specified index of the current person.
        /// </summary>
        /// <param name="index">The index of the child to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadChild(int index, params StateTransitionOption[] options)
        {
            List<Relationship> childRelationships = GetChildRelationships();
            if (childRelationships.Count <= index)
            {
                return null;
            }
            return ReadChild(childRelationships[index], options);
        }

        /// <summary>
        /// Reads the child from the specified relationship of the current person.
        /// </summary>
        /// <param name="relationship">The relationship to use for reading the child.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadChild(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        /// <summary>
        /// Reads the children of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonChildrenState"/> instance containing the REST API response.
        /// </returns>
        public PersonChildrenState ReadChildren(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.CHILDREN);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonChildrenState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the specified person as a child to the current person.
        /// </summary>
        /// <param name="person">The person to add as a child.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if the current person's collection could not be read or produced a client or server error.</exception>
        public RelationshipState AddChild(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddParentChildRelationship(this, person, options);
        }

        /// <summary>
        /// Reads the first parent of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadFirstParent(params StateTransitionOption[] options)
        {
            return ReadParent(0, options);
        }

        /// <summary>
        /// Reads the parent at the specified index of the current person.
        /// </summary>
        /// <param name="index">The index of the parent to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadParent(int index, params StateTransitionOption[] options)
        {
            List<Relationship> parentRelationships = GetParentRelationships();
            if (parentRelationships.Count <= index)
            {
                return null;
            }
            return ReadParent(parentRelationships[index], options);
        }

        /// <summary>
        /// Reads the parent from the specified relationship of the current person.
        /// </summary>
        /// <param name="relationship">The relationship to use for reading the parent.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadParent(Relationship relationship, params StateTransitionOption[] options)
        {
            return ReadRelative(relationship, options);
        }

        /// <summary>
        /// Reads the parents of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonParentsState"/> instance containing the REST API response.
        /// </returns>
        public PersonParentsState ReadParents(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PARENTS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonParentsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the specified person as a parent to the current person.
        /// </summary>
        /// <param name="person">The person to add as a parent.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Thrown if the current person's collection could not be read or produced a client or server error.</exception>
        public RelationshipState AddParent(PersonState person, params StateTransitionOption[] options)
        {
            CollectionState collection = ReadCollection();
            if (collection == null || collection.HasError())
            {
                throw new GedcomxApplicationException("Unable to add relationship: collection unavailable.");
            }

            return collection.AddParentChildRelationship(person, this, options);
        }
    }
}
