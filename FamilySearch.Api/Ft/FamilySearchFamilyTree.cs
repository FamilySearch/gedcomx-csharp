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

namespace FamilySearch.Api.Ft
{
    public class FamilySearchFamilyTree : FamilySearchCollectionState
    {
        public static readonly String URI = "https://familysearch.org/platform/collections/tree";
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree";

        public FamilySearchFamilyTree()
            : this(false)
        {
        }

        public FamilySearchFamilyTree(bool sandbox)
            : this(new Uri(sandbox ? SANDBOX_URI : URI))
        {
        }

        public FamilySearchFamilyTree(Uri uri)
            : this(uri, new FamilyTreeStateFactory())
        {
        }

        private FamilySearchFamilyTree(Uri uri, FamilyTreeStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private FamilySearchFamilyTree(Uri uri, IRestClient client, FamilyTreeStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private FamilySearchFamilyTree(IRestRequest request, IRestClient client, FamilyTreeStateFactory stateFactory)
            : this(request, client.Execute(request), client, null, stateFactory)
        {
        }

        internal FamilySearchFamilyTree(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new FamilySearchFamilyTree(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

        public FamilySearchFamilyTree AuthenticateViaUnauthenticatedAccess(String clientId, String ipAddress)
        {
            IDictionary<String, String> formData = new Dictionary<String, String>();
            formData.Add("grant_type", "unauthenticated_session");
            formData.Add("client_id", clientId);
            formData.Add("ip_address", ipAddress);

            return (FamilySearchFamilyTree)this.AuthenticateViaOAuth2(formData);
        }

        public override RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            if (relationship.KnownType == RelationshipType.ParentChild)
            {
                throw new GedcomxApplicationException("FamilySearch Family Tree doesn't support adding parent-child relationships. You must instead add a child-and-parents relationship.");
            }
            return base.AddRelationship(relationship);
        }

        public override RelationshipsState AddRelationships(List<Relationship> relationships, params StateTransitionOption[] options)
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

        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(PersonState child, PersonState father, PersonState mother, params StateTransitionOption[] options)
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

        public ChildAndParentsRelationshipState AddChildAndParentsRelationship(ChildAndParentsRelationship chap, params StateTransitionOption[] options)
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

        public RelationshipsState AddChildAndParentsRelationships(List<ChildAndParentsRelationship> chaps, params StateTransitionOption[] options)
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
            return ((FamilyTreeStateFactory)this.stateFactory).NewRelationshipsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public FamilyTreePersonState ReadPersonById(String id, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public FamilyTreePersonState ReadPersonWithRelationshipsById(String id, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSON_WITH_RELATIONSHIPS);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("person", id).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.GET);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PreferredRelationshipState ReadPreferredSpouseRelationship(UserState user, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, options);
        }

        public PreferredRelationshipState ReadPreferredParentRelationship(UserState user, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, options);
        }

        public PreferredRelationshipState ReadPreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, person.Person.Id, options);
        }

        public PreferredRelationshipState ReadPreferredParentRelationship(String treeUserId, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, person.Person.Id, options);
        }

        public PreferredRelationshipState ReadPreferredSpouseRelationship(String treeUserId, String personId, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, personId, options);
        }

        public PreferredRelationshipState ReadPreferredParentRelationship(String treeUserId, String personId, params StateTransitionOption[] options)
        {
            return ReadPreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, personId, options);
        }

        protected PreferredRelationshipState ReadPreferredRelationship(String rel, String treeUserId, String personId, StateTransitionOption[] options)
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
            if (fsp.ChildAndParentsRelationships != null && fsp.ChildAndParentsRelationships.Count > 0)
            {
                return ((FamilyTreeStateFactory)this.stateFactory).NewChildAndParentsRelationshipState(request, response, this.Client, this.CurrentAccessToken);
            }
            else
            {
                return ((FamilyTreeStateFactory)this.stateFactory).NewRelationshipState(request, response, this.Client, this.CurrentAccessToken);
            }
        }

        public FamilyTreePersonState UpdatePreferredSpouseRelationship(UserState user, FamilyTreePersonState person, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, relationshipState, options);
        }

        public FamilyTreePersonState UpdatePreferredParentRelationship(UserState user, FamilyTreePersonState person, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, user.User.TreeUserId, person.Person.Id, relationshipState, options);
        }

        public FamilyTreePersonState UpdatePreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, person.Person.Id, relationshipState, options);
        }

        public FamilyTreePersonState UpdatePreferredParentRelationship(String treeUserId, FamilyTreePersonState person, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, person.Person.Id, relationshipState, options);
        }

        public FamilyTreePersonState UpdatePreferredSpouseRelationship(String treeUserId, String personId, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_SPOUSE_RELATIONSHIP, treeUserId, personId, relationshipState, options);
        }

        public FamilyTreePersonState UpdatePreferredParentRelationship(String treeUserId, String personId, PreferredRelationshipState relationshipState, params StateTransitionOption[] options)
        {
            return UpdatePreferredRelationship(Rel.PREFERRED_PARENT_RELATIONSHIP, treeUserId, personId, relationshipState, options);
        }

        protected FamilyTreePersonState UpdatePreferredRelationship(String rel, String treeUserId, String personId, PreferredRelationshipState relationshipState, StateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("uid", treeUserId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).AddHeader("Location", relationshipState.GetSelfUri()).Build(uri, Method.PUT);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public FamilyTreePersonState DeletePreferredSpouseRelationship(UserState user, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(user.User.TreeUserId, person.Person.Id, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        public FamilyTreePersonState DeletePreferredParentRelationship(UserState user, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(user.User.TreeUserId, person.Person.Id, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        public FamilyTreePersonState DeletePreferredSpouseRelationship(String treeUserId, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, person.Person.Id, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        public FamilyTreePersonState DeletePreferredParentRelationship(String treeUserId, FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, person.Person.Id, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        public FamilyTreePersonState DeletePreferredSpouseRelationship(String treeUserId, String personId, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, personId, Rel.PREFERRED_SPOUSE_RELATIONSHIP, options);
        }

        public FamilyTreePersonState DeletePreferredParentRelationship(String treeUserId, String personId, params StateTransitionOption[] options)
        {
            return DeletePreferredRelationship(treeUserId, personId, Rel.PREFERRED_PARENT_RELATIONSHIP, options);
        }

        protected FamilyTreePersonState DeletePreferredRelationship(String treeUserId, String personId, String rel, StateTransitionOption[] options)
        {
            Link link = GetLink(rel);
            if (link == null || link.Template == null)
            {
                return null;
            }

            String template = link.Template;
            String uri = new UriTemplate(template).AddParameter("pid", personId).AddParameter("uid", treeUserId).Resolve();

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(uri, Method.DELETE);
            return ((FamilyTreeStateFactory)this.stateFactory).NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
