using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using System.Net;
using Gx.Fs;
using Gx.Conclusion;
using Gx.Fs.Tree;
using FamilySearch.Api.Util;
using Gx.Links;
using Gx.Source;
using Tavis.UriTemplates;
using Gedcomx.Support;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The FamilyTreePersonState exposes management and other FamilySearch specific functions for a person.
    /// </summary>
    public class FamilyTreePersonState : PersonState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI of the person.</param>
        public FamilyTreePersonState(Uri uri)
            : this(uri, new FamilyTreeStateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilyTreePersonState(Uri uri, FamilyTreeStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilyTreePersonState(Uri uri, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilyTreePersonState(IRestRequest request, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyTreePersonState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal FamilyTreePersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Clone.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <returns>
        /// A <see cref="GedcomxApplicationState{T}"/> instance, of type <see cref="Gx.Gedcomx"/>, containing the REST API response.
        /// </returns>
        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilyTreePersonState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Loads the entity from the REST API response if the response should have data.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>Conditional returns the entity from the REST API response if the response should have data.</returns>
        /// <remarks>The REST API response should have data if the invoking request was a GET and the response status is OK, GONE, or PRECONDITIONFAILED.</remarks>
        protected override Gx.Gedcomx LoadEntityConditionally(IRestResponse response)
        {
            if ((Request.Method == Method.GET) && (response.StatusCode == HttpStatusCode.OK
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
        /// Returns the <see cref="Gx.Gedcomx"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="Gx.Gedcomx"/> from the REST API response.</returns>
        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            FamilySearchPlatform result = null;

            if (response != null)
            {
                result = response.ToIRestResponse<FamilySearchPlatform>().Data;
            }

            return result;
        }

        /// <summary>
        /// Gets the persons from the current entity. This is shorthand for getting <see cref="P:Gx.Gedcomx.Persons"/>.
        /// </summary>
        /// <value>
        /// The persons from the current entity.
        /// </value>
        public List<Person> Persons
        {
            get
            {
                return Entity == null ? null : Entity.Persons;
            }
        }

        /// <summary>
        /// Gets the child and parents relationships.
        /// </summary>
        /// <value>
        /// The child and parents relationships.
        /// </value>
        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

        /// <summary>
        /// Gets the child and parents relationships to the children.
        /// </summary>
        /// <value>
        /// The child and parents relationships to the children.
        /// </value>
        public List<ChildAndParentsRelationship> ChildAndParentsRelationshipsToChildren
        {
            get
            {
                List<ChildAndParentsRelationship> relationships = ChildAndParentsRelationships;
                relationships = relationships == null ? null : new List<ChildAndParentsRelationship>(relationships);
                if (relationships != null)
                {
                    foreach (var relationship in relationships)
                    {
                        if (RefersToMe(relationship.Child))
                        {
                            relationships.Remove(relationship);
                        }
                    }
                }
                return relationships;
            }
        }

        /// <summary>
        /// Gets the child and parents relationships to the parents.
        /// </summary>
        /// <value>
        /// The child and parents relationships to the parents.
        /// </value>
        public List<ChildAndParentsRelationship> ChildAndParentsRelationshipsToParents
        {
            get
            {
                List<ChildAndParentsRelationship> relationships = ChildAndParentsRelationships;
                relationships = relationships == null ? null : new List<ChildAndParentsRelationship>(relationships);
                if (relationships != null)
                {
                    foreach (var relationship in relationships)
                    {
                        if (RefersToMe(relationship.Father) || RefersToMe(relationship.Mother))
                        {
                            relationships.Remove(relationship);
                        }
                    }
                }
                return relationships;
            }
        }

        /// <summary>
        /// Creates a REST API request (with appropriate authentication headers).
        /// </summary>
        /// <param name="rel">If the value is equal to the discussion references link, the resulting request is built with accept and content-type headers of "application/x-fs-v1+json"; otherwise, "application/x-gedcomx-v1+json" is used.</param>
        /// <returns>
        /// A REST API requeset (with appropriate authentication headers).
        /// </returns>
        protected override IRestRequest CreateRequestForEmbeddedResource(String rel)
        {
            if (Rel.DISCUSSION_REFERENCES.Equals(rel))
            {
                return RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest());
            }
            else
            {
                return base.CreateRequestForEmbeddedResource(rel);
            }
        }

        /// <summary>
        /// Loads all discussion references for the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState LoadDiscussionReferences(params StateTransitionOption[] options)
        {
            return (FamilyTreePersonState)base.LoadEmbeddedResources(new String[] { Rel.DISCUSSION_REFERENCES }, options);
        }

        /// <summary>
        /// Reads the portraits of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionsState ReadPortraits(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PORTRAITS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewSourceDescriptionsStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the portrait of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns></returns>
        public IRestResponse ReadPortrait(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PORTRAIT);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return Invoke(request, options);
        }

        /// <summary>
        /// Adds a discussion reference to the current person.
        /// </summary>
        /// <param name="discussion">The discussion state with a discussion reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState AddDiscussionReference(DiscussionState discussion, params StateTransitionOption[] options)
        {
            DiscussionReference reference = new DiscussionReference();
            reference.Resource = discussion.GetSelfUri();
            return AddDiscussionReference(reference, options);
        }

        /// <summary>
        /// Adds a discussion reference to the current person.
        /// </summary>
        /// <param name="reference">The discussion reference to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState AddDiscussionReference(DiscussionReference reference, params StateTransitionOption[] options)
        {
            return AddDiscussionReference(new DiscussionReference[] { reference }, options);
        }

        /// <summary>
        /// Adds the specified discussion references to the current person.
        /// </summary>
        /// <param name="refs">The discussion references to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState AddDiscussionReference(DiscussionReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            foreach (DiscussionReference @ref in refs)
            {
                person.AddExtensionElement(@ref, "discussion-references", true);
            }
            return UpdateDiscussionReference(person, options);
        }

        /// <summary>
        /// Updates the specified discussion reference for the current person.
        /// </summary>
        /// <param name="reference">The discussion reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState UpdateDiscussionReference(DiscussionReference reference, params StateTransitionOption[] options)
        {
            return UpdateDiscussionReference(new DiscussionReference[] { reference }, options);
        }

        /// <summary>
        /// Update the discussion references for the current person.
        /// </summary>
        /// <param name="refs">The discussion references to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState UpdateDiscussionReference(DiscussionReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            foreach (DiscussionReference @ref in refs)
            {
                person.AddExtensionElement(@ref, "discussion-references", true);
            }
            return UpdateDiscussionReference(person, options);
        }

        /// <summary>
        /// Updates the discussion reference on the specified person.
        /// </summary>
        /// <param name="person">The person with a discussion reference to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState UpdateDiscussionReference(Person person, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link discussionsLink = GetLink(Rel.DISCUSSION_REFERENCES);
            if (discussionsLink != null && discussionsLink.Href != null)
            {
                target = discussionsLink.Href;
            }

            Gx.Gedcomx gx = new Gx.Gedcomx();
            gx.Persons = new List<Person>() { person };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedGedcomxRequest()).SetEntity(gx).Build(target, Method.POST);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the specified discussion reference from the current person.
        /// </summary>
        /// <param name="reference">The discussion reference to delete.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="GedcomxApplicationException">Discussion reference cannot be deleted: missing link.</exception>
        public FamilyTreePersonState DeleteDiscussionReference(DiscussionReference reference, params StateTransitionOption[] options)
        {
            Link link = reference.GetLink(Rel.DISCUSSION_REFERENCE);
            link = link == null ? reference.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Discussion reference cannot be deleted: missing link.");
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedGedcomxRequest()).Build(link.Href, Method.DELETE);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the child and parents relationship of the current person.
        /// </summary>
        /// <param name="relationship">The relationship to be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState ReadChildAndParentsRelationship(ChildAndParentsRelationship relationship, params StateTransitionOption[] options)
        {
            Link link = relationship.GetLink(Rel.RELATIONSHIP);
            link = link == null ? relationship.GetLink(Rel.SELF) : link;
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the change history of the current person.
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
        /// Reads all matches of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMatchResultsState"/> instance containing the REST API response.
        /// </returns>
        public PersonMatchResultsState ReadMatches(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.MATCHES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonMatchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Restore the current person (if it is currently deleted).
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState Restore(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RESTORE);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the merge options for merging the candidate with the current person.
        /// </summary>
        /// <param name="candidate">The candidate to be evaluated for merge options.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        public PersonMergeState ReadMergeOptions(FamilyTreePersonState candidate, params StateTransitionOption[] options)
        {
            return TransitionToPersonMerge(Method.OPTIONS, candidate, options);
        }

        /// <summary>
        /// Reads the merge analysis of the specified person, after already having been merged with the current person.
        /// </summary>
        /// <param name="candidate">The candidate that was merged with the current person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        public PersonMergeState ReadMergeAnalysis(FamilyTreePersonState candidate, params StateTransitionOption[] options)
        {
            return TransitionToPersonMerge(Method.GET, candidate, options);
        }

        /// <summary>
        /// Prepares a merge state for the specified person and the current person.
        /// </summary>
        /// <param name="method">The HTTP method to use for the operation.</param>
        /// <param name="candidate">The person which will be merged with the current person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonMergeState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The current <see cref="P:FamilyTreePersonState.Person"/> or <see cref="P:Person.Id"/> was null.
        /// or
        /// The specified candidate.Person or candidate.Person.Id was null.
        /// </exception>
        protected PersonMergeState TransitionToPersonMerge(Method method, FamilyTreePersonState candidate, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.MERGE);
            if (link == null || link.Template == null)
            {
                return null;
            }

            Person person = Person;
            if (person == null || person.Id == null)
            {
                throw new ArgumentException("Cannot read merge options: no person id available.");
            }
            String personId = person.Id;

            person = candidate.Person;
            if (person == null || person.Id == null)
            {
                throw new ArgumentException("Cannot read merge options: no person id provided on the candidate.");
            }
            String candidateId = person.Id;

            String template = link.Template;

            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("dpid", candidateId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, method);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonMergeState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Declares a possible duplicate of the current person as not a duplicate.
        /// </summary>
        /// <param name="person">The <see cref="FamilyTreePersonState"/> with a person that was possibly a duplicate of the current person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
        public PersonNonMatchesState AddNonMatch(FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return AddNonMatch(person.Person, options);
        }

        /// <summary>
        /// Declares a possible duplicate of the current person as not a duplicate.
        /// </summary>
        /// <param name="person">The person that was possibly a duplicate of the current person.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
        public PersonNonMatchesState AddNonMatch(Person person, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.NOT_A_MATCHES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            Gx.Gedcomx entity = new Gx.Gedcomx();
            entity.AddPerson(person);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(entity).Build(link.Href, Method.POST);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonNonMatchesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads all declared non matches of the current person.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonNonMatchesState"/> instance containing the REST API response.
        /// </returns>
        public PersonNonMatchesState ReadNonMatches(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.NOT_A_MATCHES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonNonMatchesState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
