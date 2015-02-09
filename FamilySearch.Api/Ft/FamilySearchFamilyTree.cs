using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gx.Rs.Api;
using Gx.Types;
using Gx.Conclusion;
using Gx.Fs.Tree;
using Gx.Common;
using Gx.Links;
using Gx.Fs;
using FamilySearch.Api.Util;
using Tavis.UriTemplates;
using System.Net;
using Gedcomx.Support;

namespace FamilySearch.Api.Ft
{
    /// <summary>
    /// The FamilySearchFamilyTree is a collection of FamilySearch records and exposes management of those records.
    /// </summary>
    public class FamilySearchFamilyTree : FamilySearchCollectionState
    {
        /// <summary>
        /// The default production environment URI for this collection.
        /// </summary>
        public static readonly String URI = "https://familysearch.org/platform/collections/tree";
        /// <summary>
        /// The default sandbox environment URI for this collection.
        /// </summary>
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree";

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class using the production environment URI.
        /// </summary>
        public FamilySearchFamilyTree()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="sandbox">If set to <c>true</c> this will use the sandbox environment URI; otherwise, it will use production.</param>
        public FamilySearchFamilyTree(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        public FamilySearchFamilyTree(Uri uri)
            : this(uri, new FamilyTreeStateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchFamilyTree(Uri uri, FamilyTreeStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchFamilyTree(Uri uri, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private FamilySearchFamilyTree(IRestRequest request, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilySearchFamilyTree"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal FamilySearchFamilyTree(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
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
            return new FamilySearchFamilyTree(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        /// <summary>
        /// Creates a state instance without authentication. It will produce an access token, but only good for requests that do not need authentication.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A <see cref="FamilySearchFamilyTree"/> instance containing the REST API response.</returns>
        /// <remarks>See https://familysearch.org/developers/docs/guides/oauth2 for more information.</remarks>
        public FamilySearchFamilyTree AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchFamilyTree)this.AuthenticateViaOAuth2(formData);
        }

        /// <summary>
        /// Adds relationship to the collection.
        /// </summary>
        /// <param name="relationship">The relationship to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public override RelationshipState AddRelationship(Relationship relationship, params IStateTransitionOption[] options)
        {
            if (relationship.KnownType == RelationshipType.ParentChild)
            {
                throw new GedcomxApplicationException("FamilySearch Family Tree doesn't support adding parent-child relationships. You must instead add a child-and-parents relationship.");
            }
            return base.AddRelationship(relationship);
        }

        /// <summary>
        /// Adds the array of relationships to the collection.
        /// </summary>
        /// <param name="relationships">The relationships to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipsState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public override RelationshipsState AddRelationships(List<Relationship> relationships, params IStateTransitionOption[] options)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.KnownType == RelationshipType.ParentChild)
                {
                    throw new GedcomxApplicationException("FamilySearch Family Tree doesn't support adding parent-child relationships. You must instead add a child-and-parents relationship.");
                }
            }
            return base.AddRelationships(relationships);
        }

        /// <summary>
        /// Adds a child and parents relationship to the collection.
        /// </summary>
        /// <param name="child">The child to add in the relationship.</param>
        /// <param name="father">The father to add in the relationship.</param>
        /// <param name="mother">The mother to add in the relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(PersonState child, PersonState father, PersonState mother, params IStateTransitionOption[] options)
        {
            ChildAndParentsRelationship chap = new ChildAndParentsRelationship();
            chap.Child = new ResourceReference(child.GetSelfUri());
            if (father != null)
            {
                chap.Father = new ResourceReference(father.GetSelfUri());
            }
            if (mother != null)
            {
                chap.Mother = new ResourceReference(mother.GetSelfUri());
            }
            return AddChildAndParentsRelationship(chap, options);
        }

        /// <summary>
        /// Adds a child and parents relationship to the collection.
        /// </summary>
        /// <param name="chap">The child and parent relationship to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="ChildAndParentsRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(ChildAndParentsRelationship chap, params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("FamilySearch Family Tree at {0} didn't provide a 'relationships' link.", GetUri()));
            }

            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.ChildAndParentsRelationships = new List<ChildAndParentsRelationship>() { chap };
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            request.SetEntity(entity);
            return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Adds the child and parents relationships to the collection.
        /// </summary>
        /// <param name="chaps">The child and parent relationships to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipsState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if a link to the required resource cannot be found.</exception>
        public RelationshipsState AddChildAndParentsRelationships(List<ChildAndParentsRelationship> chaps, params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("FamilySearch Family Tree at {0} didn't provide a 'relationships' link.", GetUri()));
            }

            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.ChildAndParentsRelationships = chaps;
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.POST);
            request.SetEntity(entity);
            return ((FamilyTreeStateFactory)this.stateFactory).NewRelationshipsStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person by the specified ID.
        /// </summary>
        /// <param name="id">The ID of the person to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState ReadPersonById(String id, params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person by the specified ID and includes relationship details in the response.
        /// </summary>
        /// <param name="id">The ID of the person to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="FamilyTreePersonState"/> instance containing the REST API response.
        /// </returns>
        public FamilyTreePersonState ReadPersonWithRelationshipsById(String id, params IStateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON_WITH_RELATIONSHIPS);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("person", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying spouse relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredSpouseRelationship(UserState user, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, options);
        }

        /// <summary>
        /// Reads the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying parent relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredParentRelationship(UserState user, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, options);
        }

        /// <summary>
        /// Reads the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying spouse relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, person.Person.Id, options);
        }

        /// <summary>
        /// Reads the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying parent relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredParentRelationship(String treeUserId, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, person.Person.Id, options);
        }

        /// <summary>
        /// Reads the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying spouse relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredSpouseRelationship(String treeUserId, String personId, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, personId, options);
        }

        /// <summary>
        /// Reads the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>Tree users can have varying parent relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.</remarks>
        public IPreferredRelationshipState ReadPreferredParentRelationship(String treeUserId, String personId, params IStateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, personId, options);
        }

        /// <summary>
        /// Reads the preferred relationship for the specified person.
        /// </summary>
        /// <param name="rel">The rel name of the link to use to perform this operation.</param>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId" /> for which the preference will be read. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying relationship preferences; therefore, this method will only read what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        protected IPreferredRelationshipState ReadPreferredRelationship(String rel, String treeUserId, String personId, IStateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("uid", treeUserId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            IRestResponse response = Invoke(request, options);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            FamilySearchPlatform fsp = response.ToIRestResponse<FamilySearchPlatform>().Data;
            if (fsp != null && fsp.ChildAndParentsRelationships != null && fsp.ChildAndParentsRelationships.Count > 0)
            {
                return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, response, this.Client, this.CurrentAccessToken);
            }
            else
            {
                return (IPreferredRelationshipState)((FamilyTreeStateFactory)this.stateFactory).NewRelationshipStateInt(request, response, this.Client, this.CurrentAccessToken);
            }
        }

        /// <summary>
        /// Sets the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredSpouseRelationship(UserState user, FamilyTreePersonState person, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredParentRelationship(UserState user, FamilyTreePersonState person, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, person.Person.Id, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredParentRelationship(String treeUserId, FamilyTreePersonState person, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, person.Person.Id, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred spouse relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredSpouseRelationship(String treeUserId, String personId, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, personId, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred parent relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState UpdatePreferredParentRelationship(String treeUserId, String personId, IPreferredRelationshipState relationshipState, params IStateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, personId, relationshipState, options);
        }

        /// <summary>
        /// Sets the preferred relationship for the specified person.
        /// </summary>
        /// <param name="rel">The rel name of the link to use to perform this operation.</param>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId" /> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred relationship will be read.</param>
        /// <param name="relationshipState">The relationship state instance with the relationship to set as the preferred relationship.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying relationship preferences; therefore, this method will only set what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        protected FamilyTreePersonState UpdatePreferredRelationship(String rel, String treeUserId, String personId, IPreferredRelationshipState relationshipState, IStateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("uid", treeUserId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).AddHeader("Location", relationshipState.GetSelfUri()).Build(uri, Method.PUT);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Deletes the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredSpouseRelationship(UserState user, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(user.User.TreeUserId, person.Person.Id, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="user">The user for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredParentRelationship(UserState user, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(user.User.TreeUserId, person.Person.Id, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, person.Person.Id, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="person">The person, represented by the FamilyTreePersonState, for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredParentRelationship(String treeUserId, FamilyTreePersonState person, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, person.Person.Id, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred spouse relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred spouse relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying spouse relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredSpouseRelationship(String treeUserId, String personId, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, personId, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred parent relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId"/> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred parent relationship will be read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying parent relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        public FamilyTreePersonState DeletePreferredParentRelationship(String treeUserId, String personId, params IStateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, personId, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        /// <summary>
        /// Deletes the preferred relationship for the specified person.
        /// </summary>
        /// <param name="treeUserId">The <see cref="P:User.TreeUserId" /> for which the preference will be set. This is typically the current tree user. An API error may result if the user specified
        /// is someone other than the current tree user (due to a lack of permissions).</param>
        /// <param name="personId">The person ID for which the preferred relationship will be read.</param>
        /// <param name="rel">The rel name of the link to use to perform this operation.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="IPreferredRelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <remarks>
        /// Tree users can have varying relationship preferences; therefore, this method will only delete what the specified user prefers to
        /// see for the specified relationship.
        /// </remarks>
        protected FamilyTreePersonState DeletePreferredRelationship(String treeUserId, String personId, String rel, IStateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("uid", treeUserId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.DELETE);
            return (FamilyTreePersonState)((FamilyTreeStateFactory)this.stateFactory).NewPersonStateInt(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
