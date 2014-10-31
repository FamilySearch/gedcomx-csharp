using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    /// <summary>
    /// The supported types of GEDCOM X dates.
    /// </summary>
    public enum GedcomxDateType
    {
        /// <summary>
        /// A simple GEDCOM X date
        /// </summary>
        SIMPLE,
        /// <summary>
        /// An approximate GEDCOM X date
        /// </summary>
        APPROXIMATE,
        /// <summary>
        /// A GEDCOM X date range
        /// </summary>
        RANGE,
        /// <summary>
        /// A GEDCOM X date duration or timespan
        /// </summary>
        DURATION,
        /// <summary>
        /// A recurring GEDCOM X date
        /// </summary>
        RECURRING,
    }
}
