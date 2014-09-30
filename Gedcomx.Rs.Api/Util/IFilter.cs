using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public interface IFilter
    {
        void Handle(IRestClient client, IRestRequest request);
    }
}
