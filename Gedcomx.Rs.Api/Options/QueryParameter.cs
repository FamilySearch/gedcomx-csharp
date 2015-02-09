using Flurl;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Options
{
    /// <summary>
    /// Represents a generic query string parameter to use in REST API requests.
    /// </summary>
    public class QueryParameter : IStateTransitionOption
    {
        /// <summary>
        /// The access token query parameter.
        /// </summary>
        public static readonly String ACCESS_TOKEN = "access_token";
        /// <summary>
        /// The count query parameter.
        /// </summary>
        public static readonly String COUNT = "count";
        /// <summary>
        /// The generations query parameter.
        /// </summary>
        public static readonly String GENERATIONS = "generations";
        /// <summary>
        /// The search query parameter.
        /// </summary>
        public static readonly String SEARCH_QUERY = "q";
        /// <summary>
        /// The start query parameter.
        /// </summary>
        public static readonly String START = "start";

        private readonly bool replace;
        private readonly String name;
        private readonly String[] values;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter"/> class.
        /// </summary>
        /// <param name="name">The name of the query string parameter.</param>
        /// <param name="values">The string value to use in the new query parameter.</param>
        public QueryParameter(String name, params String[] values)
            : this(false, name, values)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter" /> class.
        /// </summary>
        /// <param name="replace">
        /// Determines whether a parameter with the same name is to be replaced or appended to. If set to <c>true</c>, any existing query string
        /// parameters of the same name will be replaced by this parameter; otherwise, the values specified here will be appened to the existing
        /// parameter of the same name. This parameter is ignored if it is the parameter name is unique.
        /// </param>
        /// <param name="name">The name of the query string parameter.</param>
        /// <param name="values">The string value to use in the new query parameter.</param>
        public QueryParameter(bool replace, String name, params String[] values)
        {
            this.replace = replace;
            this.name = name;
            this.values = values.Length > 0 ? values : new String[] { };
        }

        /// <summary>
        /// This method adds the current parameter to the specified REST API request.
        /// </summary>
        /// <param name="request">The REST API request that will be modified.</param>
        public void Apply(IRestRequest request)
        {
            var url = new Url(request.Resource);
            var query = url.QueryParams.ToList();

            if (this.replace)
            {
                query.RemoveAll(x => x.Key == this.name);
            }

            foreach (var value in values)
            {
                query.Add(new KeyValuePair<string, object>(this.name, value));
            }

            request.Resource = url.Path + "?" + String.Join("&", query.Select(x => x.Key + "=" + x.Value));
        }

        /// <summary>
        /// Creates an access token query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the access token query string parameter. See remarks.</param>
        /// <returns>An access token query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to send an access token via the query string, rather than an authorization header in REST API requests.
        /// </remarks>
        public static QueryParameter AccessToken(String value)
        {
            return new QueryParameter(true, ACCESS_TOKEN, value);
        }

        /// <summary>
        /// Creates a count query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the count query string parameter. See remarks.</param>
        /// <returns>A count query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to control the maximum number search results.
        /// </remarks>
        public static QueryParameter Count(int value)
        {
            return new QueryParameter(true, COUNT, value.ToString());
        }

        /// <summary>
        /// Creates a generations query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the generations query string parameter. See remarks.</param>
        /// <returns>A generations query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to control how far back an ancestry query should search.
        /// </remarks>
        public static QueryParameter Generations(int value)
        {
            return new QueryParameter(true, GENERATIONS, value.ToString());
        }

        /// <summary>
        /// Creates a search query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the search query string parameter. See remarks.</param>
        /// <returns>A search query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to create a search request, such as what might be built by <see cref="Gx.Rs.Api.Util.GedcomxPersonSearchQueryBuilder"/>
        /// or <see cref="Gx.Rs.Api.Util.GedcomxPlaceSearchQueryBuilder"/>.
        /// </remarks>
        public static QueryParameter SearchQuery(String value)
        {
            return new QueryParameter(true, SEARCH_QUERY, value);
        }

        /// <summary>
        /// Creates a start query string parameter.
        /// </summary>
        /// <param name="value">The value to use in the start query string parameter. See remarks.</param>
        /// <returns>A start query string parameter.</returns>
        /// <remarks>
        /// This could be used, for example, to control where search results should start.
        /// </remarks>
        public static QueryParameter Start(int value)
        {
            return new QueryParameter(true, START, value.ToString());
        }
    }
}
