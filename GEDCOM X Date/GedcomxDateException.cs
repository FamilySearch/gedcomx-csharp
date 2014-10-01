using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    [Serializable]
    public class GedcomxDateException : Exception
    {
        public GedcomxDateException()
            : base()
        {
        }

        public GedcomxDateException(string message)
            : base(message)
        {
        }

        protected GedcomxDateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GedcomxDateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
