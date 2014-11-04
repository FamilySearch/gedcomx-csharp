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
    /// <summary>
    /// An extension helper class for RestSharp classes, primarily <see cref="IRestClient"/>, <see cref="IRestRequest"/>, and <see cref="IRestResponse"/>.
    /// </summary>
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

        /// <summary>
        /// Adds an accept haeder to the specified REST API request.
        /// </summary>
        /// <param name="this">The REST API request to have the header applied.</param>
        /// <param name="value">The value of the header to be applied.</param>
        /// <returns>The REST API request with the specified accept header applied.</returns>
        /// <remarks>
        /// The accept header will be updated if it already exists; otherwise, it will be added.
        /// </remarks>
        public static IRestRequest Accept(this IRestRequest @this, object value)
        {
            var accept = new Parameter() { Name = "Accept", Type = ParameterType.HttpHeader, Value = value };
            return @this.SetParameter(accept);
        }

        /// <summary>
        /// Adds a content type header to the specified REST API request.
        /// </summary>
        /// <param name="this">The REST API request to have the header applied.</param>
        /// <param name="value">The value of the header to be applied.</param>
        /// <returns>The REST API request with the specified content type header applied.</returns>
        /// <remarks>
        /// The content type header will be updated if it already exists; otherwise, it will be added.
        /// </remarks>
        public static IRestRequest ContentType(this IRestRequest @this, object value)
        {
            var contentType = new Parameter() { Name = "Content-Type", Type = ParameterType.HttpHeader, Value = value };
            return @this.SetParameter(contentType);
        }

        /// <summary>
        /// Builds the specified REST API request by setting the request format, method, and resource.
        /// </summary>
        /// <param name="this">The REST API request to build.</param>
        /// <param name="href">The URI the REST API request should use. At a minimum, it needs the <see cref="P:Uri.PathAndQuery"/>.</param>
        /// <param name="method">The method the specified REST API request should use.</param>
        /// <returns>
        /// A REST API request with the request format, method, and resource initialized. After this method has been called, the returned result is
        /// now ready to be executed by <see cref="O:IRestClient.Execute"/>.
        /// </returns>
        public static IRestRequest Build(this IRestRequest @this, string href, Method method)
        {
            return @this.Build(new Uri(href), method);
        }

        /// <summary>
        /// Builds the specified REST API request by setting the request format, method, and resource.
        /// </summary>
        /// <param name="this">The REST API request to build.</param>
        /// <param name="uri">The URI the REST API request should use. At a minimum, it needs the <see cref="P:Uri.PathAndQuery" />.</param>
        /// <param name="method">The method the specified REST API request should use.</param>
        /// <returns>
        /// A REST API request with the request format, method, and resource initialized. After this method has been called, the returned result is
        /// now ready to be executed by <see cref="O:IRestClient.Execute" />.
        /// </returns>
        public static IRestRequest Build(this IRestRequest @this, Uri uri, Method method)
        {
            @this.RequestFormat = @this.GetDataFormat();
            @this.Resource = uri.PathAndQuery;
            @this.Method = method;

            return @this;
        }

        /// <summary>
        /// Sets the specified entity as a parameter on the <see cref="IRestRequest"/>. See remarks.
        /// </summary>
        /// <typeparam name="T">The type of entity to be set.</typeparam>
        /// <param name="this">The <see cref="IRestRequest"/> to have the entity embedded in the request.</param>
        /// <param name="entity">The entity to embed in the <see cref="IRestRequest"/>.</param>
        /// <returns>A <see cref="IRestRequest"/> with the specified entity embedded.</returns>
        /// <remarks>
        /// This method typically adds the specified entity as a body parameter; however, in the case of <see cref="T:IDictionary"/> objects,
        /// and derivatives, it is broken into key value pairs, and each key value pair is added as a regular parameter via the
        /// <see cref="M:IRestRequest(String, Object)"/> method.
        /// </remarks>
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

        /// <summary>
        /// Determines whether the server response status code indicates a client side error (status code >= 400 and &lt; 500).
        /// </summary>
        /// <param name="this">The <see cref="IRestResponse"/> to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the response status code is >= 400 and &lt; 500; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasClientError(this IRestResponse @this)
        {
            var code = (int)@this.StatusCode;

            return code >= 400 && code < 500;
        }

        /// <summary>
        /// Determines whether the server response status code indicates a server side error (status code >= 500 and &lt; 600).
        /// </summary>
        /// <param name="this">The <see cref="IRestResponse"/> to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the response status code is >= 500 and &lt; 600; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasServerError(this IRestResponse @this)
        {
            var code = (int)@this.StatusCode;

            return code >= 500 && code < 600;
        }

        /// <summary>
        /// Converts the specified <see cref="IRestResponse"/> into a strongly typed REST API response.
        /// </summary>
        /// <typeparam name="T">The type to cast the response to. See remarks.</typeparam>
        /// <param name="this">The <see cref="IRestResponse"/> which will be strongly typed.</param>
        /// <returns>
        /// A strongly typed REST API response.
        /// </returns>
        /// <remarks>
        /// This method will attempt to deserialize the response data using either <see cref="DefaultXmlSerialization"/>
        /// or <see cref="DefaultJsonSerialization"/>. The type used is determined by <see cref="GetDataFormat(IRestResponse)"/>.
        /// </remarks>
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

        /// <summary>
        /// Gets the parameters from the specified collection with the specified name.
        /// </summary>
        /// <param name="this">The collection of parameters to be evaluated.</param>
        /// <param name="name">The name of the parameter being sought.</param>
        /// <returns>The collection of parameters that satisfy the search condition.</returns>
        public static IEnumerable<Parameter> Get(this IEnumerable<Parameter> @this, String name)
        {
            return @this.Where(x => x.Name == name);
        }

        /// <summary>
        /// Gets the collection of REST API request headers.
        /// </summary>
        /// <param name="this">The REST API request from which the headers will be retrieved.</param>
        /// <returns>The collection of REST API request headers.</returns>
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

        /// <summary>
        /// Adds an accept language to the specified REST API request.
        /// </summary>
        /// <param name="this">The REST API request to have the header applied.</param>
        /// <param name="value">The value of the header to be applied.</param>
        /// <returns>The REST API request with the specified accept language header applied.</returns>
        /// <remarks>
        /// The accept language header will be updated if it already exists; otherwise, it will be added.
        /// </remarks>
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
