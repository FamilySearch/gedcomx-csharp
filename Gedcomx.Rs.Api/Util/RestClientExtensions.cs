using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Extensions;
using Newtonsoft.Json;
using Gedcomx.Model.Util;
using System.IO;
using Gedcomx.File;

namespace Gx.Rs.Api.Util
{
    public static class RestClientExtensions
    {
        private static GedcomxEntrySerializer XmlSerializer;
        private static GedcomxEntryDeserializer XmlDeserializer;
        private static GedcomxEntrySerializer JsonSerializer;
        private static GedcomxEntryDeserializer JsonDeserializer;

        static RestClientExtensions()
        {
            XmlSerializer = new DefaultXmlSerialization();
            XmlDeserializer = (GedcomxEntryDeserializer)XmlSerializer;
            JsonSerializer = new DefaultJsonSerialization();
            JsonDeserializer = (GedcomxEntryDeserializer)JsonSerializer;
        }

        public static IRestRequest Accept(this IRestRequest @this, object value)
        {
            var accept = new Parameter() { Name = "Accept", Type = ParameterType.HttpHeader, Value = value };
            return @this.SetParameter(accept);
        }

        public static IRestRequest ContentType(this IRestRequest @this, object value)
        {
            var contentType = new Parameter() { Name = "Content-Type", Type = ParameterType.HttpHeader, Value = value };
            return @this.SetParameter(contentType);
        }

        public static IRestRequest Build(this IRestRequest @this, string href, Method method)
        {
            return @this.Build(new Uri(href), method);
        }

        public static IRestRequest Build(this IRestRequest @this, Uri uri, Method method)
        {
            @this.RequestFormat = @this.GetDataFormat();
            @this.Resource = uri.PathAndQuery;
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
                var formatHeader = @this.GetHeaders().Get("Content-Type").FirstOrDefault() ?? @this.GetHeaders().Get("Accept").FirstOrDefault();

                if (formatHeader != null && formatHeader.Value != null)
                {
                    DataFormat format = @this.GetDataFormat();
                    String value = null;

                    if (format == DataFormat.Json)
                    {
                        value = JsonSerializer.Serialize(entity);
                    }
                    else if (format == DataFormat.Xml)
                    {
                        value = XmlSerializer.Serialize(entity);
                    }

                    @this.AddParameter(new Parameter() { Name = formatHeader.Value.ToString(), Type = ParameterType.RequestBody, Value = value });
                }
                else
                {
                    // This is a backup option, but is probably a bad idea. Throw exception?
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
                        result.Data = JsonDeserializer.Deserialize<T>(@this.Content);
                    }
                    else if (format == DataFormat.Xml)
                    {
                        result.Data = XmlDeserializer.Deserialize<T>(@this.Content);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<Parameter> Get(this IEnumerable<Parameter> @this, String name)
        {
            return @this.Where(x => x.Name == name);
        }

        public static IEnumerable<Parameter> GetHeaders(this IRestRequest @this)
        {
            return @this.Parameters.Where(x => x.Type == ParameterType.HttpHeader);
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

        public static IRestRequest AcceptLanguage(this IRestRequest @this, String value)
        {
            var accept = new Parameter() { Name = "Accept-Language", Type = ParameterType.HttpHeader, Value = value };
            return @this.SetParameter(accept);
        }

        private static IRestRequest SetParameter(this IRestRequest @this, Parameter parameter)
        {
            var existing = @this.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == parameter.Name);

            if (existing != null)
            {
                existing.Value = parameter.Value;
            }
            else
            {
                @this.AddParameter(parameter);
            }

            return @this;
        }
    }
}
