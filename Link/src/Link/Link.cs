using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tavis.UriTemplates;

namespace Tavis
{
    /// <summary>
    /// Link class augments the base LinkRfc class with abilities to:
    ///     - create HttpRequestMessage
    ///     - attach link hints
    ///     - attach response handling behaviour
    ///     - support for URI templates
    /// 
    /// This class can be subclassed with attributes and behaviour that is specific to a particular link relation
    /// </summary>
    public class Link : LinkRfc
    {
        public bool AddNonTemplatedParametersToQueryString { get; set; }

        /// <summary>
        /// The HTTP method to be used when following this link, or creating a HTTPRequestMessage
        /// </summary>
        public HttpMethod Method { get; set; }
        
        /// <summary>
        /// The HTTPContent to be sent with the HTTP request when following this link or creating a HTTPRequestMessage
        /// </summary>
        public HttpContent Content { get; set; }
        
      
        /// <summary>
        /// The Request headers to be used when following this link or creating a HttpRequestMessage.
        /// </summary>
        /// <remarks>
        /// HttpRequestMessage instances can only be used once.  Using a link class as a factory for HttpRequestMessages makes it easier to make multiple similar requests.  
        /// </remarks>
        public HttpRequestHeaders RequestHeaders
        {
            get
            {
                if (_requestHeaders == null)
                {
                    var dummyMessage = new HttpRequestMessage();  // Create fake request because RequestHeaders constructor is internal
                    _requestHeaders = dummyMessage.Headers;
                }
                return _requestHeaders;
            }
            
        }

        /// <summary>
        /// A handler with knowledge of how to process the response to following a link.  
        /// </summary>
        /// <remarks>
        /// The use of reponse handlers is completely optional.  They become valuable when media type deserializers use the LinkFactory which is has behaviours pre-registered
        /// </remarks>
        public IHttpResponseHandler HttpResponseHandler { get; set; }

        /// <summary>
        /// Create an instance of a link.  
        /// </summary>
        /// <remarks>
        /// The empty constructor makes it easier for deserializers to create links.
        /// </remarks>
        public Link() : base()
        {
            Method = HttpMethod.Get;
            Relation = LinkHelper.GetLinkRelationTypeName(GetType());

        }

        /// <summary>
        /// Create an HTTPRequestMessage based on the information stored in the link.
        /// </summary>
        /// <remarks>This method can be overloaded to provide custom behaviour when creating the link.  </remarks>
        /// <returns></returns>
        public virtual HttpRequestMessage CreateRequest()
        {
            Uri resolvedTarget = GetResolvedTarget();

            var requestMessage = new HttpRequestMessage()
                                     {
                                         Method = Method,
                                         RequestUri = resolvedTarget,
                                         Content = Content
                                     };

            CopyDefaultHeaders(requestMessage);

            requestMessage = ApplyHints(requestMessage);
            
            return requestMessage;
        }

        private HttpRequestMessage ApplyHints(HttpRequestMessage requestMessage)
        {
            foreach (var hint in _Hints.Values)
            {
                if (hint.ConfigureRequest != null)
                {
                    requestMessage = hint.ConfigureRequest(hint, requestMessage);
                }
            }
            return requestMessage;
        }

        protected void CopyDefaultHeaders(HttpRequestMessage requestMessage)
        {
            if (_requestHeaders != null) // If _requestheaders were never accessed then there is nothing to copy
            {
                foreach (var httpRequestHeader in RequestHeaders)
                {
                    requestMessage.Headers.Add(httpRequestHeader.Key, httpRequestHeader.Value);
                }
            }

            requestMessage.Headers.Referrer = Context;

        }

        /// <summary>
        /// Entry point for triggering the execution of the assigned HttpResponseHandler if one exists
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage responseMessage)
        {
            if (HttpResponseHandler != null)
            {
                return HttpResponseHandler.HandleAsync(this, responseMessage);
            }
            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(responseMessage);
            return tcs.Task;
        }

    
        /// <summary>
        /// Returns list of URI Template parameters specified in the Target URI
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetParameterNames()
        {
            var uriTemplate = new UriTemplate(Target.OriginalString);
            return uriTemplate.GetParameterNames();
        }


        /// <summary>
        /// Resolves the URI Template defined in the Target URI using the assigned URI parameters
        /// </summary>
        /// <returns></returns>
        public Uri GetResolvedTarget()
        {
            if (Target != null )
            {
                var uriTemplate = new UriTemplate(Target.OriginalString);

                if (AddNonTemplatedParametersToQueryString)
                {
                    var templateParameters = uriTemplate.GetParameterNames();
                    if (_Parameters.Any(p => !templateParameters.Contains(p.Key)))
                    {
                        AddParametersAsTemplate();
                        uriTemplate = new UriTemplate(Target.OriginalString);
                    }
                }

                if (Target.OriginalString.Contains("{"))
                {
                    
                    ApplyParameters(uriTemplate);
                    var resolvedTarget = new Uri(uriTemplate.Resolve(), UriKind.RelativeOrAbsolute);
                    return resolvedTarget;
                }
                
            }
            
            return Target;
        }

        /// <summary>
        /// Returns list of parameters assigned to the link
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LinkParameter> GetParameters()
        {
            return _Parameters.Values;
        }

        /// <summary>
        /// Add a hint to the link.  These hints can be used for serializing into representations on the server, or used to modify the behaviour of the CreateRequestMessage method
        /// </summary>
        /// <param name="hint"></param>
        public void AddHint(Hint hint)
        {
            _Hints.Add(hint.Name, hint);
        }

        /// <summary>
        /// Returns a list of assigned link hints
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Hint> GetHints()
        {
            return _Hints.Values;
        }

        /// <summary>
        /// Assign parameter value for use with URI templates
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="identifier">URL of documentation for this parameter</param>
        public void SetParameter(string name, object value, Uri identifier)
        {
            _Parameters[name] = new LinkParameter() { Name = name, Value = value, Identifier = identifier };
        }

        /// <summary>
        /// Assign parameter value for use with URI templates
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParameter(string name, object value)
        {
            _Parameters[name] = new LinkParameter() {Name = name, Value = value};
        }

        /// <summary>
        /// Remove URI template parameter
        /// </summary>
        /// <param name="name"></param>
        public void UnsetParameter(string name)
        {
            _Parameters.Remove(name);
        }

        /// <summary>
        /// Update target URI with query parameter tokens based on assigned parameters
        /// </summary>
        public void AddParametersAsTemplate(bool? replaceQueryString = null)
        {
            var queryTokens = String.Join(",", _Parameters.Keys.
                    Where(k => !Target.OriginalString.Contains("{" + k +"}"))
                    .Select(p => p).ToArray());

            // If query string already contains a parameter, then assume replace.
            replaceQueryString = replaceQueryString ?? _Parameters.Keys.Any(k => Target.Query.Contains(k + "="));

            string queryStringTemplate = null;
            if (replaceQueryString == true || String.IsNullOrEmpty(Target.Query))
            {
                queryStringTemplate = "{?" + queryTokens + "}";
            }
            else
            {
                queryStringTemplate =  "{&" + queryTokens + "}";
            }

            var targetUri = Target.OriginalString;

            if (replaceQueryString == true)
            {
                var queryStart = targetUri.IndexOf("?");
                if (queryStart >= 0)
                {
                    targetUri = targetUri.Substring(0, queryStart);
                }
            }

            Target = new Uri(targetUri + queryStringTemplate);
        }


        public void CreateParametersFromQueryString()
        {
            var reg = new Regex(@"([-A-Za-z0-9._~]*)=([^&]*)&?");		// Unreserved characters: http://tools.ietf.org/html/rfc3986#section-2.3
            foreach (Match m in reg.Matches(Target.Query))
            {
                string key = m.Groups[1].Value.ToLowerInvariant();
                string value = m.Groups[2].Value;
                SetParameter(key,value);
            }
        }

        private void ApplyParameters(UriTemplate uriTemplate)
        {
            foreach (var parameter in _Parameters)
            {
                if (parameter.Value.Value is IEnumerable<string>)
                {
                    uriTemplate.SetParameter(parameter.Key, (IEnumerable<string>)parameter.Value.Value);
                }
                else if (parameter.Value.Value is IDictionary<string, string>)
                {
                    uriTemplate.SetParameter(parameter.Key, (IDictionary<string, string>)parameter.Value.Value);
                }
                else
                {
                    uriTemplate.SetParameter(parameter.Key, parameter.Value.Value.ToString());
                }
            }
        }


        

        private HttpRequestHeaders _requestHeaders;
        private readonly Dictionary<string, LinkParameter> _Parameters = new Dictionary<string, LinkParameter>();
        private readonly Dictionary<string, Hint> _Hints = new Dictionary<string, Hint>();

    }
}
