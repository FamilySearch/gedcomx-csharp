using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents a recurring GEDCOM X date.
    /// </summary>
    public class GedcomxDateRecurring : GedcomxDate
    {

        private Int32? count = null;
        private GedcomxDateRange range;
        private GedcomxDateSimple end = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateRecurring"/> class.
        /// </summary>
        /// <param name="date">The recurring date string that describes a recurring GEDCOM X date.</param>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Thrown if the formal date string is null, empty, or fails to meet the expected format. The specific reason will be given at runtime.
        /// </exception>
        public GedcomxDateRecurring(String date)
        {
            if (date == null || date.Length < 3)
            {
                throw new GedcomxDateException("Invalid Recurring Date");
            }

            if (date[0] != 'R')
            {
                throw new GedcomxDateException("Invalid Recurring Date: Must start with R");
            }

            String[] parts = date.Split('/');

            if (parts.Length != 3)
            {
                throw new GedcomxDateException("Invalid Recurring Date: Must contain 3 parts");
            }

            // We must have a start and end
            if (parts[1].Equals("") || parts[2].Equals(""))
            {
                throw new GedcomxDateException("Invalid Recurring Date: Range must have a start and an end");
            }

            String countNum = parts[0].Substring(1);
            char[] countNumChars = parts[0].Substring(1).ToCharArray();

            if (countNumChars.Length > 0)
            {
                foreach (char c in countNumChars)
                {
                    if (!Char.IsDigit(c))
                    {
                        throw new GedcomxDateException("Invalid Recurring Date: Malformed Count");
                    }
                }
                count = Int32.Parse(countNum);
            }
            try
            {
                range = new GedcomxDateRange(parts[1] + "/" + parts[2]);
            }
            catch (GedcomxDateException e)
            {
                throw new GedcomxDateException(e.Message + " in Recurring Range");
            }

            // If we have a count set end
            if (count != null)
            {
                end = GetNth(count.Value);
            }
        }

        /// <summary>
        /// Gets the count of recurring instances if applicable.
        /// </summary>
        /// <value>
        /// The count of recurring instances if applicable.
        /// </value>
        public Int32? Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// Gets the <see cref="GedcomxDateRange"/> range of the current recurrences.
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        public GedcomxDateRange Range
        {
            get
            {
                return range;
            }
        }

        /// <summary>
        /// Gets the simple start date of the recurring period.
        /// </summary>
        /// <value>
        /// The simple start date of the recurring period.
        /// </value>
        public GedcomxDateSimple Start
        {
            get
            {
                return range.Start;
            }
        }

        /// <summary>
        /// Gets the <see cref="GedcomxDateDuration"/> of the recurring period.
        /// </summary>
        /// <value>
        /// The the <see cref="GedcomxDateDuration"/> of the recurring period.
        /// </value>
        public GedcomxDateDuration Duration
        {
            get
            {
                return range.Duration;
            }
        }

        /// <summary>
        /// Gets the simple end date of the recurring period.
        /// </summary>
        /// <value>
        /// The simple end date of the recurring period.
        /// </value>
        public GedcomxDateSimple End
        {
            get
            {
                return end;
            }
        }

        /// <summary>
        /// Gets the nth instance of this recurring date.
        /// </summary>
        /// <param name="count">The nth instance to retrieve.</param>
        /// <returns>The simple date of the nth instance.</returns>
        public GedcomxDateSimple GetNth(Int32 count)
        {
            GedcomxDateDuration duration = GedcomxDateUtil.MultiplyDuration(range.Duration, count);

            return GedcomxDateUtil.AddDuration(range.Start, duration);
        }

        /// <summary>
        /// Gets the type of GEDCOM X date. This property always returns RECURRING for this instance.
        /// </summary>
        /// <value>
        /// The type of GEDCOM X date.
        /// </value>
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.RECURRING;
            }
        }

        /// <summary>
        /// Determines whether this date is approximate. This method always returns <c>false</c> for this instance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this date is approximate; otherwise, <c>false</c>.
        /// </value>
        public override bool IsApproximate
        {
            get
            {
                return false;
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
                if (count != null)
                {
                    return "R" + count + "/" + range.FormalString;
                }
                else
                {
                    return "R/" + range.FormalString;
                }
            }
        }
    }
}