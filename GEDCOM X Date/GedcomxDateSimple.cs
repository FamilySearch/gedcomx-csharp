using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    public class GedcomxDateSimple : GedcomxDate
    {
        private Int32? year = null;
        private Int32? month = null;
        private Int32? day = null;
        private Int32? hours = null;
        private Int32? minutes = null;
        private Int32? seconds = null;
        private Int32? tzHours = null;
        private Int32? tzMinutes = null;

        /**
         * Instantiate a new Simple date based off of a formal date string.
         * @param date The date
         */
        public GedcomxDateSimple(String date)
        {
            ParseDate(date);
        }

        /**
         * Parse the date portion of the formal string
         * @param date The date string
         */
        private void ParseDate(String date)
        {
            // There is a minimum length of 5 characters
            if (date.Length < 5)
            {
                throw new GedcomxDateException("Invalid Date: Must have at least [+-]YYYY");
            }

            int end = date.Length;
            int offset = 0;
            String num;

            // Must start with a + or -
            if (date[offset] != '+' && date[offset] != '-')
            {
                throw new GedcomxDateException("Invalid Date: Must begin with + or -");
            }

            offset++;
            num = date[0] == '-' ? "-" : "";
            for (int i = 0; i < 4; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Year");
                }
                num += date[offset++];
            }

            year = Int32.Parse(num);

            if (offset == end)
            {
                return;
            }

            // If there is time
            if (date[offset] == 'T')
            {
                ParseTime(date.Substring(offset + 1));
                return;
            }

            // Month
            if (date[offset] != '-')
            {
                throw new GedcomxDateException("Invalid Date: Invalid Year-Month Separator");
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: Month must be 2 digits");
            }

            offset++;
            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Month");
                }
                num += date[offset++];
            }

            month = Int32.Parse(num);

            if (month < 1 || month > 12)
            {
                throw new GedcomxDateException("Invalid Date: Month must be between 1 and 12");
            }

            if (offset == end)
            {
                return;
            }

            // If there is time
            if (date[offset] == 'T')
            {
                ParseTime(date.Substring(offset + 1));
                return;
            }

            // Day
            if (date[offset] != '-')
            {
                throw new GedcomxDateException("Invalid Date: Invalid Month-Day Separator");
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: Day must be 2 digits");
            }

            offset++;
            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Day");
                }
                num += date[offset++];
            }

            day = Int32.Parse(num);

            if (day < 1)
            {
                throw new GedcomxDateException("Invalid Date: Day 0 does not exist");
            }

            int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
            if (day > daysInMonth)
            {
                throw new GedcomxDateException("Invalid Date: There are only " + daysInMonth + " days in Month " + month + " year " + year);
            }

            if (offset == end)
            {
                return;
            }

            if (date[offset] == 'T')
            {
                ParseTime(date.Substring(offset + 1));
            }
            else
            {
                throw new GedcomxDateException("Invalid Date: +YYYY-MM-DD must have T before time");
            }

        }

        /**
         * Parse the time portion of the formal string
         * @param date The date string (minus the date)
         */
        private void ParseTime(String date)
        {

            int offset = 0;
            int end = date.Length;
            String num;
            bool flag24 = false;

            // Always initialize the Timezone to the local offset.
            // It may be overridden if set
            TimeZone tz = TimeZone.CurrentTimeZone;
            double offsetInMillis = tz.GetUtcOffset(DateTime.Now).TotalMilliseconds;
            tzHours = Convert.ToInt32(offsetInMillis / 3600000);
            tzMinutes = Convert.ToInt32((offsetInMillis / 60000) % 60);

            // You must at least have hours
            if (end < 2)
            {
                throw new GedcomxDateException("Invalid Date: Hours must be 2 digits");
            }

            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Hours");
                }
                num += date[offset++];
            }

            hours = Int32.Parse(num);

            if (hours > 24)
            {
                throw new GedcomxDateException("Invalid Date: Hours must be between 0 and 24");
            }

            if (hours == 24)
            {
                flag24 = true;
            }

            if (offset == end)
            {
                return;
            }

            // If there is a timezone offset
            if (date[offset] == '+' || date[offset] == '-' || date[offset] == 'Z')
            {
                parseTimezone(date.Substring(offset)); // Don't remove the character when calling
                return;
            }

            if (date[offset] != ':')
            {
                throw new GedcomxDateException("Invalid Date: Invalid Hour-Minute Separator");
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: Minutes must be 2 digits");
            }

            offset++;
            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Minutes");
                }
                num += date[offset++];
            }

            minutes = Int32.Parse(num);

            if (minutes > 59)
            {
                throw new GedcomxDateException("Invalid Date: Minutes must be between 0 and 59");
            }

            if (flag24 && minutes != 0)
            {
                throw new GedcomxDateException("Invalid Date: Hours of 24 requires 00 Minutes");
            }

            if (offset == end)
            {
                return;
            }

            // If there is a timezone offset
            if (date[offset] == '+' || date[offset] == '-' || date[offset] == 'Z')
            {
                parseTimezone(date.Substring(offset)); // Don't remove the character when calling
                return;
            }

            if (date[offset] != ':')
            {
                throw new GedcomxDateException("Invalid Date: Invalid Minute-Second Separator");
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: Seconds must be 2 digits");
            }

            offset++;
            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Seconds");
                }
                num += date[offset++];
            }

            seconds = Int32.Parse(num);

            if (seconds > 59)
            {
                throw new GedcomxDateException("Invalid Date: Seconds must be between 0 and 59");
            }

            if (flag24 && seconds != 0)
            {
                throw new GedcomxDateException("Invalid Date: Hours of 24 requires 00 Seconds");
            }

            if (offset == end)
            {
                return;
            }
            else
            {
                parseTimezone(date.Substring(offset)); // Don't remove the character when calling
            }

        }

        /**
         * Parse the timezone portion of the formal string
         * @param date The date string (minus the date and time)
         */
        private void parseTimezone(String date)
        {
            int offset = 0;
            int end = date.Length;
            String num;

            // If Z we're done
            if (date[offset] == 'Z')
            {
                if (end == 1)
                {
                    tzHours = 0;
                    tzMinutes = 0;
                    return;
                }
                else
                {
                    throw new GedcomxDateException("Invalid Date: Malformed Timezone - No Characters allowed after Z");
                }
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: Malformed Timezone - tzHours must be [+-] followed by 2 digits");
            }

            // Must start with a + or -
            if (date[offset] != '+' && date[offset] != '-')
            {
                throw new GedcomxDateException("Invalid Date: TimeZone Hours must begin with + or -");
            }

            offset++;
            num = date[0] == '-' ? "-" : "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed tzHours");
                }
                num += date[offset++];
            }

            tzHours = Int32.Parse(num);
            // Set tzMinutes to clear out default local tz offset
            tzMinutes = 0;

            if (offset == end)
            {
                return;
            }

            if (date[offset] != ':')
            {
                throw new GedcomxDateException("Invalid Date: Invalid tzHour-tzMinute Separator");
            }

            if (end - offset < 3)
            {
                throw new GedcomxDateException("Invalid Date: tzSecond must be 2 digits");
            }

            offset++;
            num = "";
            for (int i = 0; i < 2; i++)
            {
                if (!Char.IsDigit(date[offset]))
                {
                    throw new GedcomxDateException("Invalid Date: Malformed tzMinutes");
                }
                num += date[offset++];
            }

            tzMinutes = Int32.Parse(num);

            if (offset == end)
            {
                return;
            }
            else
            {
                throw new GedcomxDateException("Invalid Date: Malformed Timezone - No characters allowed after tzSeconds");
            }

        }

        /**
         * Get the Date Type
         * @return The type
         */
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.SIMPLE;
            }
        }

        /**
         * Whether or not this date can be considered approximate
         * @return True if this is approximate
         */
        public override bool IsApproximate()
        {
                return false;
        }

        /**
         * Output the formal string for this date
         * @return The formal date string
         */
        public override String ToFormalString()
        {
            StringBuilder simple = new StringBuilder();

            simple.Append(year >= 0 ? "+" : "-").Append(String.Format("{0:0000}", Math.Abs(year.Value)));

            if (month != null)
            {
                simple.Append("-").Append(String.Format("{0:00}", month));
            }

            if (day != null)
            {
                simple.Append("-").Append(String.Format("{0:00}", day));
            }

            if (hours != null)
            {
                simple.Append("T").Append(String.Format("{0:00}", hours));

                if (minutes != null)
                {
                    simple.Append(":").Append(String.Format("{0:00}", minutes));
                }

                if (seconds != null)
                {
                    simple.Append(":").Append(String.Format("{0:00}", seconds));
                }

                // If we have time we always have tz
                if (tzHours == 0 && tzMinutes == 0)
                {
                    simple.Append("Z");
                }
                else
                {
                    simple.Append(tzHours >= 0 ? "+" : "-").Append(String.Format("{0:00}", Math.Abs(tzHours.Value)));
                    simple.Append(":").Append(String.Format("{0:00}", tzMinutes));
                }
            }



            return simple.ToString();
        }

        /**
         * Get the year
         * @return The Year
         */
        public Int32? Year
        {
            get
            {
                return year;
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
                return month;
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
                return day;
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
                return hours;
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
                return minutes;
            }
        }

        /**
         * Get the seconds
         * @return The seconds
         */
        public Int32? Seconds
        {
            get
            {
                return seconds;
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
                return tzHours;
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
                return tzMinutes;
            }
        }
    }
}
