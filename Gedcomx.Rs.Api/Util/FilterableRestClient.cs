using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class FilterableRestClient : RestClient, IFilterableRestClient
    {
        private List<IFilter> filters;

        public FilterableRestClient()
            : this(null)
        {
        }

        public FilterableRestClient(string baseUrl)
            : base(baseUrl)
        {
            filters = new List<IFilter>();
        }

        public void AddFilter(IFilter filter)
        {
            filters.Add(filter);
        }

        public IRestResponse Handle(IRestRequest request)
        {
            foreach (var filter in filters)
            {
                filter.Handle((IRestClient)this, request);
            }

            return this.Execute(request);
        }
    }
}
