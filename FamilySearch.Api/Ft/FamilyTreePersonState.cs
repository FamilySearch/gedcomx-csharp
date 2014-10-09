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
    public class FamilyTreePersonState : PersonState
    {
        public FamilyTreePersonState(Uri uri)
            : this(uri, new FamilyTreeStateFactory())
        {
        }

        private FamilyTreePersonState(Uri uri, FamilyTreeStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        private FamilyTreePersonState(Uri uri, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private FamilyTreePersonState(IRestRequest request, IFilterableRestClient client, FamilyTreeStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        protected internal FamilyTreePersonState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilyTreeStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilyTreePersonState(request, response, client, this.CurrentAccessToken, (FamilyTreeStateFactory)this.stateFactory);
        }

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

        protected override Gx.Gedcomx LoadEntity(IRestResponse response)
        {
            FamilySearchPlatform result = null;

            if (response != null)
            {
                result = response.ToIRestResponse<FamilySearchPlatform>().Data;
            }

            return result;
        }

        public List<Person> Persons
        {
            get
            {
                return Entity == null ? null : Entity.Persons;
            }
        }

        public List<ChildAndParentsRelationship> ChildAndParentsRelationships
        {
            get
            {
                return Entity == null ? null : ((FamilySearchPlatform)Entity).ChildAndParentsRelationships;
            }
        }

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

        public FamilyTreePersonState LoadDiscussionReferences(params StateTransitionOption[] options)
        {
            return (FamilyTreePersonState)base.LoadEmbeddedResources(new String[] { Rel.DISCUSSION_REFERENCES }, options);
        }

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

        public FamilyTreePersonState AddDiscussionReference(DiscussionState discussion, params StateTransitionOption[] options)
        {
            DiscussionReference reference = new DiscussionReference();
            reference.Resource = discussion.GetSelfUri();
            return AddDiscussionReference(reference, options);
        }

        public FamilyTreePersonState AddDiscussionReference(DiscussionReference reference, params StateTransitionOption[] options)
        {
            return AddDiscussionReference(new DiscussionReference[] { reference }, options);
        }

        public FamilyTreePersonState AddDiscussionReference(DiscussionReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            foreach (DiscussionReference @ref in refs)
            {
                person.AddExtensionElement(@ref, "discussion-references", true);
            }
            return UpdateDiscussionReference(person, options);
        }

        public FamilyTreePersonState UpdateDiscussionReference(DiscussionReference reference, params StateTransitionOption[] options)
        {
            return UpdateDiscussionReference(new DiscussionReference[] { reference }, options);
        }

        public FamilyTreePersonState UpdateDiscussionReference(DiscussionReference[] refs, params StateTransitionOption[] options)
        {
            Person person = CreateEmptySelf();
            foreach (DiscussionReference @ref in refs)
            {
                person.AddExtensionElement(@ref, "discussion-references", true);
            }
            return UpdateDiscussionReference(person, options);
        }

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

        public PersonMergeState ReadMergeOptions(FamilyTreePersonState candidate, params StateTransitionOption[] options)
        {
            return TransitionToPersonMerge(Method.OPTIONS, candidate, options);
        }

        public PersonMergeState ReadMergeAnalysis(FamilyTreePersonState candidate, params StateTransitionOption[] options)
        {
            return TransitionToPersonMerge(Method.GET, candidate, options);
        }

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

        public PersonNonMatchesState AddNonMatch(FamilyTreePersonState person, params StateTransitionOption[] options)
        {
            return AddNonMatch(person.Person, options);
        }

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
