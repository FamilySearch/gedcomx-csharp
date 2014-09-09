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
        private static JsonSerializerSettings jsonSettings;

        static RestClientExtensions()
        {
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.ContractResolver = new CamelCaseContractResolver();
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public static IRestRequest Accept(this IRestRequest @this, object value)
        {
            var accept = new Parameter() { Name = "Accept", Type = ParameterType.HttpHeader, Value = value };
            var existing = @this.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == accept.Name);

            if (existing != null)
            {
                existing.Value = accept.Value;
            }
            else
            {
                @this.AddParameter(accept);
            }

            return @this;
        }

        public static IRestRequest ContentType(this IRestRequest @this, object value)
        {
            var contentType = new Parameter() { Name = "Content-Type", Type = ParameterType.HttpHeader, Value = value };
            var existing = @this.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == contentType.Name);

            if (existing != null)
            {
                existing.Value = contentType.Value;
            }
            else
            {
                @this.AddParameter(contentType);
            }

            return @this;
        }

        public static IRestRequest Build(this IRestRequest @this, Uri href, Method method)
        {
            return @this.Build(href.ToString(), method);
        }

        public static IRestRequest Build(this IRestRequest @this, string href, Method method)
        {
            @this.RequestFormat = @this.GetDataFormat();
            @this.Resource = href;
            @this.Method = method;

            return @this;
        }

        public static IRestRequest SetEntity<T>(this IRestRequest @this, T entity)
        {
            var dictionary = entity as System.Collections.IDictionary;

            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    String value = null;

                    if (dictionary[key] != null)
                    {
                        value = dictionary[key].ToString();
                    }

                    @this.AddParameter(key.ToString(), value);
                }
            }
            else
            {
                var contentType = @this.Parameters.FirstOrDefault(x => x.Name == "Content-Type" && x.Type == ParameterType.HttpHeader);

                if (contentType != null && contentType.Value != null)
                {
                    DataFormat format = @this.GetDataFormat();
                    String value = null;

                    if (format == DataFormat.Json)
                    {
                        value = JsonConvert.SerializeObject(entity, jsonSettings);
                    }
                    else if (format == DataFormat.Xml)
                    {
                        value = @this.XmlSerializer.Serialize(entity);
                    }

                    @this.AddParameter(new Parameter() { Name = contentType.Value.ToString(), Type = ParameterType.RequestBody, Value = value });
                }
                else
                {
                    @this.AddBody(entity);
                }
            }

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

            if (@this != null)
            {
                result = @this.toAsyncResponse<T>();
                var format = @this.GetDataFormat();

                if (@this.Content != null)
                {
                    if (format == DataFormat.Json)
                    {
                        result.Data = JsonConvert.DeserializeObject<T>(@this.Content, jsonSettings);
                    }
                    else if (format == DataFormat.Xml)
                    {
                        var deserializer = new RestSharp.Deserializers.XmlDeserializer();
                        result.Data = deserializer.Deserialize<T>(@this);
                    }
                }
            }

            return result;
        }

        private static DataFormat GetDataFormat(this IRestResponse @this)
        {
            DataFormat result = default(DataFormat);
            var contentType = @this.Headers.FirstOrDefault(x => x.Name == "Content-Type");

            if (contentType != null && contentType.Value != null)
            {
                result = GetDataFormat(contentType.Value.ToString(), result);
            }
            else if (@this.Request != null)
            {
                result = @this.Request.GetDataFormat();
            }

            return result;
        }

        private static DataFormat GetDataFormat(this IRestRequest @this)
        {
            DataFormat result = default(DataFormat);
            var contentType = @this.Parameters.FirstOrDefault(x => x.Name == "Content-Type" && x.Type == ParameterType.HttpHeader);

            if (contentType != null && contentType.Value != null)
            {
                result = GetDataFormat(contentType.Value.ToString(), @this.RequestFormat);
            }

            return result;
        }

        private static DataFormat GetDataFormat(String value, DataFormat @default)
        {
            DataFormat result = @default;

            if (value.IndexOf("json", StringComparison.OrdinalIgnoreCase) != -1)
            {
                result = DataFormat.Json;
            }
            else if (value.IndexOf("xml", StringComparison.OrdinalIgnoreCase) != -1)
            {
                result = DataFormat.Xml;
            }

            return result;
        }
    }
}
