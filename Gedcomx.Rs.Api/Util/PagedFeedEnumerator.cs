using Gedcomx.Support;
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
    /// <summary>
    /// A paged <see cref="Gx.Atom.Feed"/> enumerator. See remarks.
    /// </summary>
    /// <remarks>
    /// The HTTP GET requests can be customized by using a custom <see cref="IWebResourceProvider"/> and/or adding
    /// <see cref="IWebResourceBuilderExtension"/>s.
    /// 
    /// @see <a href="http://tools.ietf.org/search/rfc5005#section-3">Paged Feeds</a>
    /// </remarks>
    public class PagedFeedEnumerator : IEnumerator<Feed>
    {
        /// <summary>
        /// The default <see cref="WebResourceProvider"/> which simply sets the base URI on the REST API client
        /// and sets the resource on the REST API request.
        /// </summary>
        public static IWebResourceProvider DEFAULT_WEB_RESOURCE_PROVIDER = new WebResourceProviderImpl();
        private readonly List<IWebResourceBuilderExtension> extensions = new List<IWebResourceBuilderExtension>();
        private IFilterableRestClient client;
        private IWebResourceProvider webResourceProvider = DEFAULT_WEB_RESOURCE_PROVIDER;
        private String first = null;
        private String last = null;
        private String previous = null;
        private String next = null;
        private Feed current;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedFeedEnumerator"/> class.
        /// </summary>
        /// <param name="uri">The URI to use to get the paged feed.</param>
        private PagedFeedEnumerator(String uri)
        {
            next = uri;
        }

        private PagedFeedEnumerator(Feed feed)
        {
            LoadHRefsFromFeed(feed);
        }

        /**
         * 
         *
         * @param uri 
         * @return a 
         */
        /// <summary>
        /// Creates a new paged feed iterator using the specified URI.
        /// </summary>
        /// <param name="uri">The URI to use to get the initial (next) paged feed document and from which future values of first, last, previous,
        /// and next hypertext references will be acquired.</param>
        /// <returns>A new <see cref="PagedFeedEnumerator"/>.</returns>
        public static PagedFeedEnumerator FromUri(String uri)
        {
            return new PagedFeedEnumerator(uri);
        }

        /// <summary>
        /// Creates a new paged feed iterator using the specified <see cref="Feed"/>.
        /// </summary>
        /// <param name="feed">The feed document from which the initial first, last, previous, and next hypertext references will be acquired.</param>
        /// <returns>A new <see cref="PagedFeedEnumerator"/>.</returns>
        public static PagedFeedEnumerator FromFeed(Feed feed)
        {
            return new PagedFeedEnumerator(feed);
        }

        /// <summary>
        /// Gets the hypertext reference from the specified <see cref="Feed"/> for the specified rel link.
        /// </summary>
        /// <param name="feed">The source feed document.</param>
        /// <param name="rel">The desired rel link in the specified feed.</param>
        /// <returns>The hypertext reference, if it exists; otherwise <c>null</c>.</returns>
        public static String GetLinkRelHref(Feed feed, String rel)
        {
            Link link = feed.GetLink(rel);
            return link == null ? null : link.Href;
        }

        /// <summary>
        /// Gets the web resource builder extensions that will be used to modify REST API requests just before retrieving the feed.
        /// </summary>
        /// <value>
        /// The web resource builder extensions that will be used to modify REST API requests just before retrieving the feed.
        /// </value>
        public IList<IWebResourceBuilderExtension> WebResourceBuilderExtensions
        {
            get
            {
                return this.extensions.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds a <see cref="IWebResourceBuilderExtension"/> in order to add cookies, header values, etc. to the paged feed document GET request.
        /// </summary>
        /// <param name="extension">The extension to add.</param>
        public void AddWebResourceBuilderExtension(IWebResourceBuilderExtension extension)
        {
            this.extensions.Add(extension);
        }

        /// <summary>
        /// Removes the specified <see cref="IWebResourceBuilderExtension"/>.
        /// </summary>
        /// <param name="extension">The extension to remove.</param>
        public void RemoveWebResourceBuilderExtension(IWebResourceBuilderExtension extension)
        {
            this.extensions.Remove(extension);
        }

        /// <summary>
        /// Removes all <see cref="IWebResourceBuilderExtension"/>s.
        /// </summary>
        public void ClearWebResourceBuilderExtensions()
        {
            this.extensions.Clear();
        }

        /// <summary>
        /// Adds a <see cref="IWebResourceBuilderExtension"/> in order to add cookies, header values, etc. to the paged feed document GET request.
        /// </summary>
        /// <param name="extension">The extension to add.</param>
        /// <returns>A reference to this <see cref="PagedFeedEnumerator"/> for fluent configuration chaining.</returns>
        public PagedFeedEnumerator WithWebResourceBuilderExtension(IWebResourceBuilderExtension extension)
        {
            AddWebResourceBuilderExtension(extension);
            return this;
        }

        /// <summary>
        /// Gets the <see cref="IFilterableRestClient"/> being used to get paged feed documents.
        /// </summary>
        /// <value>
        /// The <see cref="IFilterableRestClient"/> being used to get paged feed documents..
        /// </value>
        public IFilterableRestClient Client
        {
            get
            {
                if (client == null)
                {
                    WithClient(new FilterableRestClient());
                }
                return client;
            }
            set
            {
                client = value;
            }
        }

        /**
         * 
         *
         * @param client t
         * @return a 
         */
        /// <summary>
        /// Sets the <see cref="IFilterableRestClient"/> to use to get paged feed documents.
        /// </summary>
        /// <param name="client">The <see cref="IFilterableRestClient"/> to use to get paged feed documents.</param>
        /// <returns>A reference to this <see cref="PagedFeedEnumerator"/> for fluent configuration chaining.</returns>
        public PagedFeedEnumerator WithClient(IFilterableRestClient client)
        {
            Client = client;
            return this;
        }

        /// <summary>
        /// Gets or sets the web resource provider.
        /// </summary>
        /// <value>
        /// The web resource provider.
        /// </value>
        /// <remarks>
        /// Upon instantiation, this defaults to <see cref="DEFAULT_WEB_RESOURCE_PROVIDER"/>.
        /// </remarks>
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

        /// <summary>
        /// Sets the <see cref="WebResourceProvider"/> .
        /// </summary>
        /// <param name="webResourceProvider">The desired <see cref="WebResourceProvider"/>.</param>
        /// <returns>A reference to this <see cref="PagedFeedEnumerator"/> for fluent configuration chaining.</returns>
        public PagedFeedEnumerator WithWebResourceProvider(WebResourceProviderImpl webResourceProvider)
        {
            WebResourceProvider = webResourceProvider;
            return this;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a FIRST link.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has a FIRST link; otherwise, <c>false</c>.
        /// </value>
        public bool HasFirst
        {
            get
            {
                return first != null;
            }
        }

        /// <summary>
        /// Gets the FIRST link.
        /// </summary>
        /// <value>
        /// The FIRST link.
        /// </value>
        public String FirstHRef
        {
            get
            {
                return first;
            }
        }

        /// <summary>
        /// Gets the FIRST paged results, if available.
        /// </summary>
        /// <returns>The FIRST paged results, if available</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if a FIRST link is available. Check <see cref="HasFirst"/> to prevent undesired behavior.</exception>
        public Feed First()
        {
            if (!HasFirst)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(first);
        }

        /// <summary>
        /// Gets the LAST link
        /// </summary>
        /// <value>
        /// The LAST link.
        /// </value>
        public String LastHRef
        {
            get
            {
                return last;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a LAST link.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has a LAST link; otherwise, <c>false</c>.
        /// </value>
        public bool HasLast
        {
            get
            {
                return last != null;
            }
        }

        /// <summary>
        /// Gets the LAST paged results, if available.
        /// </summary>
        /// <returns>The LAST paged results, if available</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if a LAST link is available. Check <see cref="HasLast"/> to prevent undesired behavior.</exception>
        public Feed Last()
        {
            if (!HasLast)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(last);
        }

        /// <summary>
        /// Gets the PREVIOUS link.
        /// </summary>
        /// <value>
        /// The PREVIOUS link.
        /// </value>
        public String PreviousHRef
        {
            get
            {
                return previous;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a PREVIOUS link.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a PREVIOUS link; otherwise, <c>false</c>.
        /// </value>
        public bool HasPrevious
        {
            get
            {
                return previous != null;
            }
        }

        /// <summary>
        /// Gets the PREVIOUS paged results, if available.
        /// </summary>
        /// <returns>The PREVIOUS paged results, if available</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if a PREVIOUS link is available. Check <see cref="HasPrevious"/> to prevent undesired behavior.</exception>
        public Feed Previous()
        {
            if (!HasPrevious)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(previous);
        }

        /// <summary>
        /// Gets the NEXT link.
        /// </summary>
        /// <value>
        /// The NEXT link.
        /// </value>
        public String NextHRef
        {
            get
            {
                return next;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a NEXT link.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has a NEXT link; otherwise, <c>false</c>.
        /// </value>
        public bool HasNext
        {
            get
            {
                return next != null;
            }
        }

        /// <summary>
        /// Gets the NEXT paged results, if available.
        /// </summary>
        /// <returns>The NEXT paged results, if available</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if a NEXT link is available. Check <see cref="HasNext"/> to prevent undesired behavior.</exception>
        public Feed Next()
        {
            if (!HasNext)
            {
                throw new InvalidOperationException();
            }
            return GetFeed(next);
        }

        /// <summary>
        /// Loads available links into this instance.
        /// </summary>
        /// <param name="feed">The feed from which links will be extracted.</param>
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
            foreach (IWebResourceBuilderExtension extension in this.extensions)
            {
                extension.Extend(request);
            }
            IRestResponse clientResponse = Client.Handle(request);
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

        /// <summary>
        /// Interface for providing <see cref="WebResourceProviderImpl"/>s to use for fetching paged feed documents.
        /// </summary>
        public interface IWebResourceProvider
        {
            /// <summary>
            /// Provide the WebResourceProviderImpl to use for fetching a paged feed document.
            /// </summary>
            /// <param name="client">The REST API client to use for API calls.</param>
            /// <param name="uri">The specified URI for the first, last, previous, and/or next paged feed document.</param>
            /// <returns>A <see cref="WebResourceProviderImpl"/> for acquiring the desired paged feed document.</returns>
            IRestRequest Provide(IFilterableRestClient client, String uri);
        }

        /// <summary>
        /// A basic implementation of the <see cref="IWebResourceProvider"/> interface, which simply sets the base URI on the REST API client
        /// and sets the resource on the REST API request.
        /// </summary>
        public class WebResourceProviderImpl : IWebResourceProvider
        {
            /// <summary>
            /// Provide the WebResourceProviderImpl to use for fetching a paged feed document.
            /// </summary>
            /// <param name="client">The REST API client to use for API calls.</param>
            /// <param name="uri">The specified URI for the first, last, previous, and/or next paged feed document.</param>
            /// <returns>
            /// A <see cref="WebResourceProviderImpl" /> for acquiring the desired paged feed document.
            /// </returns>
            public IRestRequest Provide(IFilterableRestClient client, String uri)
            {
                client.BaseUrl = new Uri(uri).GetBaseUrl();
                return new RestRequest(uri);
            }
        }

        /// <summary>
        /// Interface for extending HTTP GET requests (e.g. add cookies, header values, etc.).
        /// </summary>
        public interface IWebResourceBuilderExtension
        {
            /// <summary>
            /// Extends a REST API request.
            /// </summary>
            /// <param name="requeset">The requeset to extend.</param>
            /// <returns>The extended REST API request.</returns>
            IRestRequest Extend(IRestRequest requeset);
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public Feed Current
        {
            get { return current; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            bool result = HasNext;

            if (result)
            {
                Next();
            }

            return result;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            if (HasFirst)
            {
                First();
            }
        }
    }
}
