using Gx.Atom;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /**
 * A paged {@link org.gedcomx.atom.Feed feed} iterator.
 * <p/>
 * The HTTP GET requests can be customized by using a custom {@link WebResourceProvider} and/or adding {@link
 * WebResourceBuilderExtension}s.
 *
 * @see <a href="http://tools.ietf.org/search/rfc5005#section-3">Paged Feeds</a>
 */
    public class PagedFeedEnumerator : IEnumerator<Feed>
    {
        /**
         * The default {@link WebResourceProvider} which simply calls {@link com.sun.jersey.api.client.Client#resource(String)}.
         */
        public static IWebResourceProvider DEFAULT_WEB_RESOURCE_PROVIDER = new WebResourceProviderImpl();
        private readonly List<WebResourceBuilderExtension> extensions = new List<WebResourceBuilderExtension>();
        private IRestClient client;
        private IWebResourceProvider webResourceProvider = DEFAULT_WEB_RESOURCE_PROVIDER;
        private String first = null;
        private String last = null;
        private String previous = null;
        private String next = null;
        private Feed current;

        private PagedFeedEnumerator(String uri)
        {
            next = uri;
        }

        private PagedFeedEnumerator(Feed feed)
        {
            LoadHRefsFromFeed(feed);
        }

        /**
         * Creates a new paged feed iterator using the specified URI.
         *
         * @param uri the {@link org.gedcomx.common.URI} to use to get the initial (next) paged feed document and from which
         *            future values of first, last, previous, and next hypertext references will be acquired.
         * @return a new {@link PagedFeedIterator}
         */
        public static PagedFeedEnumerator FromUri(String uri)
        {
            return new PagedFeedEnumerator(uri);
        }

        /**
         * Creates a new paged feed iterator using the specified {@link org.gedcomx.atom.Feed}.
         *
         * @param feed the feed document from which the initial first, last, previous, and next hypertext references will be
         *             acquired.
         * @return a new {@link PagedFeedIterator}
         */
        public static PagedFeedEnumerator FromFeed(Feed feed)
        {
            return new PagedFeedEnumerator(feed);
        }

        /**
         * Gets the hypertext reference from the specified {@link org.gedcomx.atom.Feed} for the specified rel link.
         *
         * @param feed the source feed document
         * @param rel  the desired rel
         * @return the hypertext reference, if it exists; otherwise {@code null}
         */
        public static String GetLinkRelHref(Feed feed, String rel)
        {
            Link link = feed.GetLink(rel);
            return link == null ? null : link.Href;
        }

        public IList<WebResourceBuilderExtension> WebResourceBuilderExtensions
        {
            get
            {
                return this.extensions.AsReadOnly();
            }
        }

        /**
         * Adds a {@link WebResourceBuilderExtension} in order to add cookies, header values, etc. to the paged feed document
         * GET request.
         *
         * @param extension the extension to add
         */
        public void AddWebResourceBuilderExtension(WebResourceBuilderExtension extension)
        {
            this.extensions.Add(extension);
        }

        /**
         * Removes a {@link WebResourceBuilderExtension}.
         *
         * @param extension the extension to remove
         */
        public void RemoveWebResourceBuilderExtension(WebResourceBuilderExtension extension)
        {
            this.extensions.Remove(extension);
        }

        public void ClearWebResourceBuilderExtensions()
        {
            this.extensions.Clear();
        }

        /**
         * Adds a {@link WebResourceBuilderExtension} in order to add cookies, header values, etc. to the paged feed document
         * GET request.
         *
         * @param extension the extension
         * @return a reference to this {@link PagedFeedIterator} for fluent configuration chaining
         */
        public PagedFeedEnumerator WithWebResourceBuilderExtension(WebResourceBuilderExtension extension)
        {
            AddWebResourceBuilderExtension(extension);
            return this;
        }

        /**
         * Gets the {@link com.sun.jersey.api.client.Client} being used to get paged feed documents.
         *
         * @return the {@link com.sun.jersey.api.client.Client} being used to get paged feed documents.
         */
        public IRestClient Client
        {
            get
            {
                if (client == null)
                {
                    WithClient(new RestClient());
                }
                return client;
            }
            set
            {
                client = value;
            }
        }

        /**
         * Sets the {@link com.sun.jersey.api.client.Client} to use to get paged feed documents.
         *
         * @param client the {@link com.sun.jersey.api.client.Client} to use to get paged feed documents.
         * @return a reference to this {@link PagedFeedIterator} for fluent configuration chaining
         */
        public PagedFeedEnumerator WithClient(IRestClient client)
        {
            Client = client;
            return this;
        }

        /**
         * Gets the {@link WebResourceProvider}.
         *
         * @return the {@link WebResourceProvider}.
         * @see #DEFAULT_WEB_RESOURCE_PROVIDER
         */
        public IWebResourceProvider WebResourceProvider
        {
            get
            {
                return webResourceProvider;
            }
            set
            {
                webResourceProvider = value;
            }
        }

        /**
         * Sets the {@link WebResourceProvider}.
         *
         * @param webResourceProvider the desired {@link WebResourceProvider}.
         * @return a reference to this {@link PagedFeedIterator} for fluent configuration chaining
         * @see #DEFAULT_WEB_RESOURCE_PROVIDER
         */
        public PagedFeedEnumerator WithWebResourceProvider(WebResourceProviderImpl webResourceProvider)
        {
            WebResourceProvider = webResourceProvider;
            return this;
        }

        public bool HasFirst
        {
            get
            {
                return first != null;
            }
        }

        public String FirstHRef
        {
            get
            {
                return first;
            }
        }

        public Feed First()
        {
            if (!HasFirst)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(first);
        }

        public String LastHRef
        {
            get
            {
                return last;
            }
        }

        public bool HasLast
        {
            get
            {
                return last != null;
            }
        }

        public Feed Last()
        {
            if (!HasLast)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(last);
        }

        public String PreviousHRef
        {
            get
            {
                return previous;
            }
        }

        public bool HasPrevious
        {
            get
            {
                return previous != null;
            }
        }

        public Feed Previous()
        {
            if (!HasPrevious)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(previous);
        }

        public String NextHRef
        {
            get
            {
                return next;
            }
        }

        public bool HasNext
        {
            get
            {
                return next != null;
            }
        }

        public Feed Next()
        {
            if (!HasNext)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(next);
        }

        private void LoadHRefsFromFeed(Feed feed)
        {
            first = GetLinkRelHref(feed, "first");
            last = GetLinkRelHref(feed, "last");
            previous = GetLinkRelHref(feed, "previous");
            next = GetLinkRelHref(feed, "next");
        }

        private Feed GetFeed(String uri)
        {
            Feed result = null;
            IRestRequest request = webResourceProvider.Provide(Client, uri)
                .Accept(MediaTypes.APPLICATION_ATOM_XML_TYPE);
            foreach (WebResourceBuilderExtension extension in this.extensions)
            {
                extension.Extend(request);
            }
            IRestResponse clientResponse = Client.Execute(request);
            HttpStatusCode status = clientResponse.StatusCode;
            switch (status)
            {
                case HttpStatusCode.OK:
                    Feed feed = clientResponse.ToIRestResponse<Feed>().Data;
                    LoadHRefsFromFeed(feed);
                    result = feed;
                    break;
                case HttpStatusCode.NoContent:
                    break;
                default:
                    throw new InvalidOperationException(clientResponse.StatusDescription);
            }

            current = result;

            return result;
        }

        /**
         * Interface for providing {@link com.sun.jersey.api.client.WebResource}s to use for fetching paged feed documents.
         */
        public interface IWebResourceProvider
        {
            /**
             * Provide the {@link com.sun.jersey.api.client.WebResource} to use for fetching a paged feed document.
             *
             * @param client the configured client to use
             * @param uri    the specified {@link org.gedcomx.common.URI} for the first, last, previous, and/or next paged feed
             *               document
             * @return a {@link com.sun.jersey.api.client.WebResource} for acquiring the desired paged feed document
             */
            IRestRequest Provide(IRestClient client, String uri);
        }

        public class WebResourceProviderImpl : IWebResourceProvider
        {
            public IRestRequest Provide(IRestClient client, String uri)
            {
                client.BaseUrl = new Uri(uri).GetBaseUrl();
                return new RestRequest(uri);
            }
        }

        /**
         * Interface for extending HTTP GET requests (e.g. add cookies, header values, etc.).
         */
        public interface WebResourceBuilderExtension
        {
            /**
             * Extends a {@link com.sun.jersey.api.client.WebResource.Builder}.
             *
             * @param builder the base {@link com.sun.jersey.api.client.WebResource.Builder}
             * @return the extended {@link com.sun.jersey.api.client.WebResource.Builder}
             */
            IRestRequest Extend(IRestRequest requeset);
        }

        public Feed Current
        {
            get { return current; }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            bool result = HasNext;

            if (result)
            {
                Next();
            }

            return result;
        }

        public void Reset()
        {
            if (HasFirst)
            {
                First();
            }
        }
    }
}
