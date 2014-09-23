using Gx.Rs.Api.Util;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Gx.Rs.Api
{
    [Serializable]
    public class GedcomxApplicationException : Exception
    {
        public IRestResponse Response { get; private set; }

        protected GedcomxApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                foreach (var member in info)
                {
                    if (member.Name == "Response")
                    {
                        Response = member.Value as IRestResponse;
                    }
                }
            }
        }

        public GedcomxApplicationException()
            : this(null as string)
        {
        }

        public GedcomxApplicationException(string message)
            : this(message, null as Exception)
        {
        }

        public GedcomxApplicationException(string message, Exception innerException)
            : this(message, innerException, null)
        {
        }

        public GedcomxApplicationException(IRestResponse response)
            : this(null as string, response)
        {
        }

        public GedcomxApplicationException(string message, IRestResponse response)
            : this(message, null, response)
        {
        }

        public GedcomxApplicationException(string message, Exception innerException, IRestResponse response)
            : base(message, innerException)
        {
            Response = response;
        }

        public override string Message
        {
            get
            {
                String message = base.Message;
                StringBuilder builder = new StringBuilder(message == null ? "Error processing GEDCOM X request." : message);
                List<HttpWarning> warnings = Warnings;
                if (message != null || warnings.Count > 0)
                {
                    if (warnings != null)
                    {
                        foreach (HttpWarning warning in warnings)
                        {
                            builder.Append("\nWarning: ").Append(warning.Message);
                        }
                    }
                }

                String body = null;
                if (this.Response != null)
                {
                    try
                    {
                        body = this.Response.ToIRestResponse<JObject>().Data.ToString();
                    }
                    catch (Exception)
                    {
                        //unable to get the response body...
                        body = "(error response body unavailable)";
                    }
                }
                if (body != null)
                {
                    builder.Append('\n').Append(body);
                }

                return builder.ToString();
            }
        }

        public List<HttpWarning> Warnings
        {
            get
            {
                List<HttpWarning> warnings = null;

                if (this.Response != null)
                {
                    IEnumerable<Parameter> values = this.Response.Headers.Get("Warning");
                    if (values != null && values.Any())
                    {
                        warnings = new List<HttpWarning>();
                        foreach (Parameter value in values)
                        {
                            warnings.AddRange(HttpWarning.Parse(value));
                        }
                    }
                }

                return warnings;
            }
        }
    }
}
