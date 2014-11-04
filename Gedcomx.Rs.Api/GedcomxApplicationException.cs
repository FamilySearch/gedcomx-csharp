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
    /// <summary>
    /// Represents an exception within the FamilySearch GEDCOM X application.
    /// </summary>
    [Serializable]
    public class GedcomxApplicationException : Exception
    {
        /// <summary>
        /// Gets the response associated with the exception if applicable.
        /// </summary>
        /// <value>
        /// The response associated with the exception if applicable.
        /// </value>
        public IRestResponse Response { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        public GedcomxApplicationException()
            : this(null as string)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GedcomxApplicationException(string message)
            : this(message, null as Exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public GedcomxApplicationException(string message, Exception innerException)
            : this(message, innerException, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="response">The REST API response that is associated with this exception.</param>
        public GedcomxApplicationException(IRestResponse response)
            : this(null as string, response)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="response">The REST API response that is associated with this exception.</param>
        public GedcomxApplicationException(string message, IRestResponse response)
            : this(message, null, response)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxApplicationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="response">The REST API response that is associated with this exception.</param>
        public GedcomxApplicationException(string message, Exception innerException, IRestResponse response)
            : base(message, innerException)
        {
            Response = response;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
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

        /// <summary>
        /// Gets the list of warning header values from the associated REST API response if it is available.
        /// </summary>
        /// <value>
        /// The list of warning header values from the associated REST API response if it is available.
        /// </value>
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
