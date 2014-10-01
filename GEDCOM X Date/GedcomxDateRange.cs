using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    public class GedcomxDateRange : GedcomxDate
    {
        private bool approximate = false;
        private GedcomxDateSimple start = null;
        private GedcomxDateDuration duration = null;
        private GedcomxDateSimple end = null;

        /**
         * Instantiate a new Range date from the formal string
         * @param date The formal date string
         */
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

            /**
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

        /**
         * Get the start of the range
         * @return The Start Date
         */
        public GedcomxDateSimple Start
        {
            get
            {
                return start;
            }
        }

        /**
         * Get the duration between the start and end dates
         * @return The Duration
         */
        public GedcomxDateDuration Duration
        {
            get
            {
                return duration;
            }
        }

        /**
         * Get the end of the range
         * @return The End Date
         */
        public GedcomxDateSimple End
        {
            get
            {
                return end;
            }
        }

        /**
         * Get the type of this date
         * @return The Date Type
         */
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.RANGE;
            }
        }

        /**
         * Whether or not this date is considered approximate
         * @return True if it this date is approximate
         */
        public override bool IsApproximate()
        {
            return approximate;
        }

        /**
         * Return the formal string for this date
         * @return The formal string
         */
        public override String ToFormalString()
        {
            StringBuilder range = new StringBuilder();

            if (approximate)
            {
                range.Append('A');
            }

            if (start != null)
            {
                range.Append(start.ToFormalString());
            }

            range.Append('/');

            if (duration != null)
            {
                range.Append(duration.ToFormalString());
            }
            else if (end != null)
            {
                range.Append(end.ToFormalString());
            }

            return range.ToString();
        }
    }
}
