using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents an approximate GEDCOM X date.
    /// </summary>
    public class GedcomxDateApproximate : GedcomxDate
    {
        private GedcomxDateSimple simpleDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateApproximate"/> class.
        /// </summary>
        /// <param name="date">The formal duration string that describes a GEDCOM X date approximation.</param>
        /// <exception cref="GedcomxDateException">Thrown if the specified date is null, empty, or does not begin with 'A' (as required by a formal date string).</exception>
        public GedcomxDateApproximate(String date)
        {
            if (date == null || date.Length < 1 || date[0] != 'A')
            {
                throw new GedcomxDateException("Invalid Approximate Date: Must start with A");
            }

            simpleDate = new GedcomxDateSimple(date.Substring(1));

        }

        /// <summary>
        /// Gets the underlying simple date.
        /// </summary>
        /// <value>
        /// The underlying simple date.
        /// </value>
        public GedcomxDateSimple SimpleDate
        {
            get
            {
                return simpleDate;
            }
        }

        /// <summary>
        /// Gets the type of GEDCOM X date. This property always returns APPROXIMATE for this instance.
        /// </summary>
        /// <value>
        /// The type of GEDCOM X date.
        /// </value>
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.APPROXIMATE;
            }
        }

        /// <summary>
        /// Determines whether this date is approximate. This method always returns <c>true</c> for this instance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this date is approximate; otherwise, <c>false</c>.
        /// </value>
        public override bool IsApproximate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The formal representation of this date.
        /// </summary>
        /// <value>
        /// The formal representation of this date.
        /// </value>
        public override String FormalString
        {
            get
            {
                return "A" + simpleDate.FormalString;
            }
        }

        /// <summary>
        /// Gets the year of the current date if it exists.
        /// </summary>
        /// <value>
        /// The year of the current date if it exists.
        /// </value>
        public Int32? Year
        {
            get
            {
                return simpleDate.Year;
            }
        }

        /// <summary>
        /// Gets the month of the current date if it exists.
        /// </summary>
        /// <value>
        /// The month of the current date if it exists.
        /// </value>
        public Int32? Month
        {
            get
            {
                return simpleDate.Month;
            }
        }

        /// <summary>
        /// Gets the day of the current date if it exists.
        /// </summary>
        /// <value>
        /// The day of the current date if it exists.
        /// </value>
        public Int32? Day
        {
            get
            {
                return simpleDate.Day;
            }
        }

        /// <summary>
        /// Gets the hours of the current date if it exists.
        /// </summary>
        /// <value>
        /// The hours of the current date if it exists.
        /// </value>
        public Int32? Hours
        {
            get
            {
                return simpleDate.Hours;
            }
        }

        /// <summary>
        /// Gets the minutes of the current date if it exists.
        /// </summary>
        /// <value>
        /// The minutes of the current date if it exists.
        /// </value>
        public Int32? Minutes
        {
            get
            {
                return simpleDate.Minutes;
            }
        }

        /// <summary>
        /// Gets the seconds of the current date if it exists.
        /// </summary>
        /// <value>
        /// The seconds of the current date if it exists.
        /// </value>
        public Int32? Seconds
        {
            get
            {
                return simpleDate.Seconds;
            }
        }

        /// <summary>
        /// Gets the timezone hours of the current date if it exists.
        /// </summary>
        /// <value>
        /// The timezone hours of the current date if it exists.
        /// </value>
        public Int32? TzHours
        {
            get
            {
                return simpleDate.TzHours;
            }
        }

        /// <summary>
        /// Gets the timezone minutes of the current date if it exists.
        /// </summary>
        /// <value>
        /// The timezone minutes of the current date if it exists.
        /// </value>
        public Int32? TzMinutes
        {
            get
            {
                return simpleDate.TzMinutes;
            }
        }
    }
}
