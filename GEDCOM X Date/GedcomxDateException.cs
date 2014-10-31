using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents exceptions when working with GEDCOM X dates.
    /// </summary>
    [Serializable]
    public class GedcomxDateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateException"/> class.
        /// </summary>
        public GedcomxDateException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GedcomxDateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected GedcomxDateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public GedcomxDateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
