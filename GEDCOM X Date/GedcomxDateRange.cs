using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents a GEDCOM X date range.
    /// </summary>
    public class GedcomxDateRange : GedcomxDate
    {
        private bool approximate = false;
        private GedcomxDateSimple start = null;
        private GedcomxDateDuration duration = null;
        private GedcomxDateSimple end = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateRange"/> class.
        /// </summary>
        /// <param name="date">The formal date string that describes a GEDCOM X date range.</param>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Thrown if the formal date is null, empty, or does not meet the expected format.
        /// </exception>
        public GedcomxDateRange(String date)
        {

            if (date == null || date.Length < 1)
            {
                throw new GedcomxDateException("Invalid Range");
            }

            String range = date;

            // If range starts with A it is recurring
            if (date[0] == 'A')
            {
                approximate = true;
                range = date.Substring(1);
            }

            // / is required
            if (!date.Contains("/"))
            {
                throw new GedcomxDateException("Invalid Range: / is required");
            }

            /*
             * range -> parts
             * / -> []
             * +1000/ -> ["+1000"]
             * /+1000 -> ["","+1000"]
             * +1000/+2000 -> ["+1000","+2000"]
             */
            String[] parts = range.Split('/');

            if (parts.Length < 1 || parts.Length > 2)
            {
                throw new GedcomxDateException("Invalid Range: One or two parts are required");
            }

            if (!parts[0].Equals(""))
            {
                try
                {
                    start = new GedcomxDateSimple(parts[0]);
                }
                catch (GedcomxDateException e)
                {
                    throw new GedcomxDateException(e.Message + " in Range Start Date");
                }
            }

            if (parts.Length == 2)
            {
                if (parts[1][0] == 'P')
                {
                    if (start == null)
                    {
                        throw new GedcomxDateException("Invalid Range: A range may not end with a duration if missing a start date");
                    }
                    try
                    {
                        duration = new GedcomxDateDuration(parts[1]);
                    }
                    catch (GedcomxDateException e)
                    {
                        throw new GedcomxDateException(e.Message + " in Range End Duration");
                    }
                    // Use the duration to calculate the end date
                    end = GedcomxDateUtil.AddDuration(start, duration);
                }
                else
                {
                    try
                    {
                        end = new GedcomxDateSimple(parts[1]);
                    }
                    catch (GedcomxDateException e)
                    {
                        throw new GedcomxDateException(e.Message + " in Range End Date");
                    }
                    if (start != null)
                    {
                        duration = GedcomxDateUtil.GetDuration(start, end);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the simple start date of the current date range.
        /// </summary>
        /// <value>
        /// The simple start date of the current date range.
        /// </value>
        public GedcomxDateSimple Start
        {
            get
            {
                return start;
            }
        }

        /// <summary>
        /// Gets the duration between the start and end dates.
        /// </summary>
        /// <value>
        /// The duration between the start and end dates.
        /// </value>
        public GedcomxDateDuration Duration
        {
            get
            {
                return duration;
            }
        }

        /// <summary>
        /// Gets the simple end date of the current date range.
        /// </summary>
        /// <value>
        /// The simple end date of the current date range.
        /// </value>
        public GedcomxDateSimple End
        {
            get
            {
                return end;
            }
        }

        /// <summary>
        /// Gets the type of GEDCOM X date. This property always returns RANGE for this instance.
        /// </summary>
        /// <value>
        /// The type of GEDCOM X date.
        /// </value>
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.RANGE;
            }
        }

        /// <summary>
        /// Determines whether this date is approximate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this date is approximate; otherwise, <c>false</c>.
        /// </value>
        public override bool IsApproximate
        {
            get
            {
                return approximate;
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
                StringBuilder range = new StringBuilder();

                if (approximate)
                {
                    range.Append('A');
                }

                if (start != null)
                {
                    range.Append(start.FormalString);
                }

                range.Append('/');

                if (duration != null)
                {
                    range.Append(duration.FormalString);
                }
                else if (end != null)
                {
                    range.Append(end.FormalString);
                }

                return range.ToString();
            }
        }
    }
}
