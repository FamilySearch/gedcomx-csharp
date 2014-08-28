using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public static class RestClientExtensions
    {
        public static IRestRequest SetDataFormat(this IRestRequest @this, DataFormat format)
        {
            @this.RequestFormat = format;

            if (format == DataFormat.Xml)
            {
                // This is added since JSON doesn't currently require it
                @this.AddXmlAcceptHeader();
            }

            return @this;
        }

        public static IRestRequest AddXmlAcceptHeader(this IRestRequest @this)
        {
            @this.AddHeader("Accept", MediaTypes.GEDCOMX_XML_MEDIA_TYPE);

            return @this;
        }
    }
}
