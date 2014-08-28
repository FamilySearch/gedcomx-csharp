using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
    }
}
