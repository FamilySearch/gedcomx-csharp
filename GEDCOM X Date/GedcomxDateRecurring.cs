using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    public class GedcomxDateRecurring : GedcomxDate
    {

        private Int32? count = null;
        private GedcomxDateRange range;
        private GedcomxDateSimple end = null;

        /**
         * Instantiate a new Recurring date from the formal date string
         * @param date The formal date string
         */
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

        /**
         * Get the count
         * @return The Count
         */
        public Int32? Count
        {
            get
            {
                return count;
            }
        }

        /**
         * Get the range
         * @return The Range
         */
        public GedcomxDateRange Range
        {
            get
            {
                return range;
            }
        }

        /**
         * Get the start date
         * @return The Start Date
         */
        public GedcomxDateSimple Start
        {
            get
            {
                return range.Start;
            }
        }

        /**
         * Get the duration
         * @return The Duration
         */
        public GedcomxDateDuration Duration
        {
            get
            {
                return range.Duration;
            }
        }

        /**
         * Get the end date
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
         * Get the nth instance of this recurring date
         * @param count The nth instance
         * @return The date of the nth instance
         */
        public GedcomxDateSimple GetNth(Int32 count)
        {

            GedcomxDateDuration duration = GedcomxDateUtil.MultiplyDuration(range.Duration, count);

            return GedcomxDateUtil.AddDuration(range.Start, duration);
        }

        /**
         * Get the date type
         * @return The Date Type
         */
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.RECURRING;
            }
        }

        /**
         * Whether or not this date is considered approximate
         * @return True if this date is approximate
         */
        public override bool IsApproximate()
        {
            return false;
        }

        /**
         * Return the formal string for this date
         * @return The formal date string
         */
        public override String ToFormalString()
        {
            if (count != null)
            {
                return "R" + count + "/" + range.ToFormalString();
            }
            else
            {
                return "R/" + range.ToFormalString();
            }
        }
    }
}
