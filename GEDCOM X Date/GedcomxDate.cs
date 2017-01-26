using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents a basic GEDCOM X date.
    /// </summary>
    public abstract class GedcomxDate
    {
        /// <summary>
        /// Gets the type of GEDCOM X date.
        /// </summary>
        /// <value>
        /// The type of GEDCOM X date.
        /// </value>
        public abstract GedcomxDateType Type { get; }

        /// <summary>
        /// Determines whether this date is approximate.
        /// </summary>
        /// <value><c>true</c> if this date is approximate; otherwise, <c>false</c>.</value>
        public abstract bool IsApproximate { get; }

        /// <summary>
        /// The formal representation of this date.
        /// </summary>
        /// <value>The formal representation of this date.</value>
        public abstract String FormalString { get; }
    }
}
