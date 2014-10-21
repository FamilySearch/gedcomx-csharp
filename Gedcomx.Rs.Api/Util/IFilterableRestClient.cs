using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public interface IFilterableRestClient : IRestClient
    {
        void AddFilter(IFilter filter);
        IRestResponse Handle(IRestRequest request);
        bool FollowRedirects { get; set; }
    }
}
