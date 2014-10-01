using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    public class GedcomxDateApproximate : GedcomxDate
    {
        private GedcomxDateSimple simpleDate;

        /**
         * Instantiate a new approximate date
         * @param date The formal date string
         */
        public GedcomxDateApproximate(String date)
        {

            if (date == null || date.Length < 1 || date[0] != 'A')
            {
                throw new GedcomxDateException("Invalid Approximate Date: Must start with A");
            }

            simpleDate = new GedcomxDateSimple(date.Substring(1));

        }

        /**
         * Return the underlying simple date
         * @return The Simple Date
         */
        public GedcomxDateSimple SimpleDate
        {
            get
            {
                return simpleDate;
            }
        }

        /**
         * The type of this date
         * @return The Type
         */
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.APPROXIMATE;
            }
        }

        /**
         * Whether or not this date is approximate
         * @return True
         */
        public override bool IsApproximate()
        {
            return true;
        }

        /**
         * Returns the formal representation of this date
         * @return The formal String
         */
        public override String ToFormalString()
        {
            return "A" + simpleDate.ToFormalString();
        }

        /**
         * Get the year
         * @return The Year
         */
        public Int32? Year
        {
            get
            {
                return simpleDate.Year;
            }
        }

        /**
         * Get the month
         * @return The Month
         */
        public Int32? Month
        {
            get
            {
                return simpleDate.Month;
            }
        }

        /**
         * Get the day
         * @return The Day
         */
        public Int32? Day
        {
            get
            {
                return simpleDate.Day;
            }
        }

        /**
         * Get the hours
         * @return The Hours
         */
        public Int32? Hours
        {
            get
            {
                return simpleDate.Hours;
            }
        }

        /**
         * Get the minutes
         * @return The Minutes
         */
        public Int32? Minutes
        {
            get
            {
                return simpleDate.Minutes;
            }
        }

        /**
         * Get the seconds
         * @return The Seconds
         */
        public Int32? Seconds
        {
            get
            {
                return simpleDate.Seconds;
            }
        }

        /**
         * Get the timezone hours
         * @return The Timezone Hours
         */
        public Int32? TzHours
        {
            get
            {
                return simpleDate.TzHours;
            }
        }

        /**
         * Get the timezone minutes
         * @return The Timezone Minutes
         */
        public Int32? TzMinutes
        {
            get
            {
                return simpleDate.TzMinutes;
            }
        }
    }
}
