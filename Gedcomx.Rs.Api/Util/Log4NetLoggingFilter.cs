using log4net;
using log4net.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class Log4NetLoggingFilter : IFilter
    {
        private ILog logger = LogManager.GetLogger(typeof(Log4NetLoggingFilter));

        public void Handle(IRestClient client, IRestRequest request)
        {
            logger.Debug(string.Format("{0} {1}{2}", request.Method, client.BaseUrl, request.Resource));
        }
    }
}
