using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Extensions;
using Newtonsoft.Json;
using Gedcomx.Model.Util;
using System.IO;

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

        public static IRestRequest Build(this IRestRequest @this, Uri href, Method method)
        {
            return @this.Build(href.ToString(), method);
        }

        public static IRestRequest Build(this IRestRequest @this, string href, Method method)
        {
            @this.Resource = href;
            @this.Method = method;

            return @this;
        }

        public static IRestRequest SetEntity<T>(this IRestRequest @this, T entity)
        {
#warning Need to resolve .entity(entity) pattern
            @this.AddObject(entity);

            return @this;
        }

        public static bool HasClientError(this IRestResponse @this)
        {
            var code = (int)@this.StatusCode;

            return code >= 400 && code < 500;
        }

        public static bool HasServerError(this IRestResponse @this)
        {
            var code = (int)@this.StatusCode;

            return code >= 500 && code < 600;
        }

        public static IRestResponse<T> ToIRestResponse<T>(this IRestResponse @this)
        {
            IRestResponse<T> result = null;

            if (@this != null && !string.IsNullOrEmpty(@this.Content) && @this.Request != null)
            {
                result = @this.toAsyncResponse<T>();

                if (@this.Request.RequestFormat == DataFormat.Json)
                {
                    var settings = new JsonSerializerSettings();
                    settings.ContractResolver = new CamelCaseContractResolver();
                    var deserializer = Newtonsoft.Json.JsonSerializer.CreateDefault(settings);

                    using (var reader = new JsonTextReader(new StringReader(@this.Content)))
                    {
                        result.Data = deserializer.Deserialize<T>(reader);
                    }
                }
                else if (@this.Request.RequestFormat == DataFormat.Xml)
                {
                    var deserializer = new RestSharp.Deserializers.XmlDeserializer();
                    result.Data = deserializer.Deserialize<T>(@this);
                }
            }

            return result;
        }
    }
}
