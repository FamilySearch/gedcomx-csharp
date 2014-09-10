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
    public class QueryParameter : StateTransitionOption
    {
        public static readonly String ACCESS_TOKEN = "access_token";
        public static readonly String COUNT = "count";
        public static readonly String GENERATIONS = "generations";
        public static readonly String SEARCH_QUERY = "q";
        public static readonly String START = "start";

        private readonly bool replace;
        private readonly String name;
        private readonly String[] values;

        public QueryParameter(String name, params String[] values)
            : this(false, name, values)
        {
        }

        public QueryParameter(bool replace, String name, params String[] values)
        {
            this.replace = replace;
            this.name = name;
            this.values = values.Length > 0 ? values : new String[] { };
        }

        public void Apply(IRestRequest request)
        {
            var url = new Url(request.Resource);
            var query = url.QueryParams.ToList();

            foreach (var value in values)
            {
                query.Add(new KeyValuePair<string, object>(this.name, value));
            }

            request.Resource = url.Path + "?" + String.Join("&", query.Select(x => x.Key + "=" + x.Value));
        }

        public static QueryParameter accessToken(String value)
        {
            return new QueryParameter(true, ACCESS_TOKEN, value);
        }

        public static QueryParameter count(int value)
        {
            return new QueryParameter(true, COUNT, value.ToString());
        }

        public static QueryParameter generations(int value)
        {
            return new QueryParameter(true, GENERATIONS, value.ToString());
        }

        public static QueryParameter searchQuery(String value)
        {
            return new QueryParameter(true, SEARCH_QUERY, value);
        }

        public static QueryParameter start(int value)
        {
            return new QueryParameter(true, START, value.ToString());
        }
    }
}
