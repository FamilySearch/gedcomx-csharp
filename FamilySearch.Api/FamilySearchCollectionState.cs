using Gx.Rs.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.Rs.Api.Util;
using Gx.Links;
using Tavis.UriTemplates;
using Gx.Conclusion;
using Gx.Common;
using FamilySearch.Api.Util;
using Gx.Fs.Discussions;
using Gx.Fs;

namespace FamilySearch.Api
{
    public class FamilySearchCollectionState : CollectionState
    {
        public FamilySearchCollectionState(Uri uri)
            : this(uri, new FamilySearchStateFactory())
        {
        }

        private FamilySearchCollectionState(Uri uri, FamilySearchStateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClientInt(uri), stateFactory)
        {
        }

        private FamilySearchCollectionState(Uri uri, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private FamilySearchCollectionState(IRestRequest request, IFilterableRestClient client, FamilySearchStateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        protected internal FamilySearchCollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, FamilySearchStateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gx.Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new FamilySearchCollectionState(request, response, client, this.CurrentAccessToken, (FamilySearchStateFactory)this.stateFactory);
        }

        public DateInfo NormalizeDate(String date, params StateTransitionOption[] options)
        {
            Link normalizedDateLink = GetLink(Rel.NORMALIZED_DATE);
            if (normalizedDateLink == null || normalizedDateLink.Template == null)
            {
                return null;
            }
            String template = normalizedDateLink.Template;
            String uri = new UriTemplate(template).AddParameter("date", date).Resolve();

            IRestRequest request = CreateRequest().Accept(MediaTypes.TEXT_PLAIN).Build(uri, Method.GET);
            IRestResponse response = Invoke(request, options);
            DateInfo dateValue = new DateInfo();
            dateValue.Original = date;
            dateValue.AddNormalizedExtension(new TextValue(response.ToIRestResponse<String>().Data));
            if (response.Headers != null)
            {
                dateValue.Formal = response.Headers.Where(x => x.Name == "Location").Select(x => x.Value as string).FirstOrDefault();
            }
            return dateValue;
        }

        public UserState ReadCurrentUser(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.CURRENT_USER);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewUserState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonMatchResultsState SearchForPersonMatches(GedcomxPersonSearchQueryBuilder query, params StateTransitionOption[] options)
        {
            return SearchForPersonMatches(query.Build(), options);
        }

        public PersonMatchResultsState SearchForPersonMatches(String query, params StateTransitionOption[] options)
        {
            Link searchLink = GetLink(Rel.PERSON_MATCHES_QUERY);
            if (searchLink == null || searchLink.Template == null)
            {
                return null;
            }
            String template = searchLink.Template;

            String uri = new UriTemplate(template).AddParameter("q", query).Resolve();

            IRestRequest request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewPersonMatchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public DiscussionsState ReadDiscussions(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.DISCUSSIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).Build(link.Href, Method.GET);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public DiscussionState AddDiscussion(Discussion discussion, params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.DISCUSSIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException("Unable to add discussion: missing link.");
            }

            FamilySearchPlatform entity = new FamilySearchPlatform();
            entity.AddDiscussion(discussion);
            IRestRequest request = RequestUtil.ApplyFamilySearchConneg(CreateAuthenticatedRequest()).SetEntity(entity).Build(link.Href, Method.POST);
            return ((FamilySearchStateFactory)this.stateFactory).NewDiscussionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
