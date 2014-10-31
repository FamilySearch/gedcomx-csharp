using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    /// <summary>
    /// A helper utility to manipulating and working with various GEDCOM X dates and strings.
    /// </summary>
    public class GedcomxDateUtil
    {
        /**
         * Parse a String representation of a Formal GedcomX Date
         * @param date The GedcomX Date
         * @return A GedcomxDate
         */
        /// <summary>
        /// Parses the specified formal date string representing a GEDCOM X date of any kind.
        /// </summary>
        /// <param name="date">The formal date string to parse.</param>
        /// <returns></returns>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Throw if the formal date string is null or empty.
        /// </exception>
        public static GedcomxDate Parse(String date)
        {
            if (date == null || date.Equals(""))
            {
                throw new GedcomxDateException("Invalid Date");
            }

            if (date[0] == 'R')
            {
                return new GedcomxDateRecurring(date);
            }
            else if (date.Contains("/"))
            {
                return new GedcomxDateRange(date);
            }
            else if (date[0] == 'A')
            {
                return new GedcomxDateApproximate(date);
            }
            else
            {
                return new GedcomxDateSimple(date);
            }
        }

        /// <summary>
        /// Gets the duration between the two specified GEDCOM X dates.
        /// </summary>
        /// <param name="startDate">The simple start date.</param>
        /// <param name="endDate">The simple end date.</param>
        /// <returns>A <see cref="GedcomxDateDuration"/> representing the duration between the two specified dates.</returns>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Thrown if one of the input dates is null
        /// or
        /// Thrown if the start date occurs after the end date, or is equal to the end time.
        /// </exception>
        public static GedcomxDateDuration GetDuration(GedcomxDateSimple startDate, GedcomxDateSimple endDate)
        {

            if (startDate == null || endDate == null)
            {
                throw new GedcomxDateException("Start and End must be simple dates");
            }

            Date start = new Date(startDate, true);
            Date end = new Date(endDate, true);
            bool hasTime = false;
            StringBuilder duration = new StringBuilder();

            ZipDates(start, end);

            // Build the duration backwards so we can grab the correct diff
            // Also we need to roll everything up so we don't generate an invalid max year
            if (end.seconds != null)
            {
                while (end.seconds - start.seconds < 0)
                {
                    end.minutes -= 1;
                    end.seconds += 60;
                }
                if (end.seconds - start.seconds > 0)
                {
                    hasTime = true;
                    duration.Insert(0, 'S').Insert(0, String.Format("{0:00}", end.seconds - start.seconds));
                }
            }

            if (end.minutes != null)
            {
                while (end.minutes - start.minutes < 0)
                {
                    end.hours -= 1;
                    end.minutes += 60;
                }
                if (end.minutes - start.minutes > 0)
                {
                    hasTime = true;
                    duration.Insert(0, 'M').Insert(0, String.Format("{0:00}", end.minutes - start.minutes));
                }
            }

            if (end.hours != null)
            {
                while (end.hours - start.hours < 0)
                {
                    end.day -= 1;
                    end.hours += 24;
                }
                if (end.hours - start.hours > 0)
                {
                    hasTime = true;
                    duration.Insert(0, 'H').Insert(0, String.Format("{0:00}", end.hours - start.hours));
                }
            }

            if (hasTime)
            {
                duration.Insert(0, 'T');
            }

            if (end.day != null)
            {
                while (end.day - start.day < 0)
                {
                    end.day += DateTime.DaysInMonth(end.year.Value, end.month.Value);
                    end.month -= 1;
                    if (end.month < 1)
                    {
                        end.year -= 1;
                        end.month += 12;
                    }
                }
                if (end.day - start.day > 0)
                {
                    duration.Insert(0, 'D').Insert(0, String.Format("{0:00}", end.day - start.day));
                }
            }

            if (end.month != null)
            {
                while (end.month - start.month < 0)
                {
                    end.year -= 1;
                    end.month += 12;
                }
                if (end.month - start.month > 0)
                {
                    duration.Insert(0, 'M').Insert(0, String.Format("{0:00}", end.month - start.month));
                }
            }

            if (end.year - start.year > 0)
            {
                duration.Insert(0, 'Y').Insert(0, String.Format("%04d", end.year - start.year));
            }

            String finalDuration = duration.ToString();

            if (end.year - start.year < 0 || duration.Equals(""))
            {
                throw new GedcomxDateException("Start Date must be less than End Date");
            }

            return new GedcomxDateDuration("P" + finalDuration);
        }

        /// <summary>
        /// Adds a duration to the specified simple date and returns the resulting simple date.
        /// </summary>
        /// <param name="startDate">The start date that will have the specified duration added.</param>
        /// <param name="duration">The duration to add to the specified simple date.</param>
        /// <returns>The <see cref="GedcomxDateSimple"/> date resulting from adding the duration to the specified date. </returns>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Throw if the start date is null
        /// or
        /// Thrown if the duration is null
        /// or
        /// Thrown if the resulting end year is beyond 9999.
        /// </exception>
        public static GedcomxDateSimple AddDuration(GedcomxDateSimple startDate, GedcomxDateDuration duration)
        {

            if (startDate == null)
            {
                throw new GedcomxDateException("Invalid Start Date");
            }

            if (duration == null)
            {
                throw new GedcomxDateException("Invalid Duration");
            }

            Date end = new Date(startDate, false);
            StringBuilder endString = new StringBuilder();

            // Initialize all the values we need in end based on the duration
            ZipDuration(end, duration);

            // Add Timezone offset to endString
            if (startDate.TzHours != null)
            {
                endString.Append(startDate.TzHours >= 0 ? "+" : "-").Append(String.Format("{0:00}", Math.Abs(startDate.TzHours.Value)));
                endString.Append(":").Append(String.Format("{0:00}", startDate.TzMinutes));
            }

            if (end.seconds != null)
            {
                if (duration.Seconds != null)
                {
                    end.seconds += duration.Seconds;
                }
                while (end.seconds >= 60)
                {
                    end.seconds -= 60;
                    end.minutes += 1;
                }
                endString.Insert(0, String.Format("{0:00}", end.seconds)).Insert(0, ":");
            }

            if (end.minutes != null)
            {
                if (duration.Minutes != null)
                {
                    end.minutes += duration.Minutes;
                }
                while (end.minutes >= 60)
                {
                    end.minutes -= 60;
                    end.hours += 1;
                }
                endString.Insert(0, String.Format("{0:00}", end.minutes)).Insert(0, ":");
            }

            if (end.hours != null)
            {
                if (duration.Hours != null)
                {
                    end.hours += duration.Hours;
                }
                while (end.hours >= 24)
                {
                    end.hours -= 24;
                    end.day += 1;
                }
                endString.Insert(0, String.Format("{0:00}", end.hours)).Insert(0, "T");
            }

            if (end.day != null)
            {
                if (duration.Days != null)
                {
                    end.day += duration.Days;
                }
                while (end.day >= DateTime.DaysInMonth(end.year.Value, end.month.Value))
                {
                    end.month += 1;
                    if (end.month > 12)
                    {
                        end.month -= 12;
                        end.year += 1;
                    }
                    end.day -= DateTime.DaysInMonth(end.year.Value, end.month.Value);
                }
                endString.Insert(0, String.Format("{0:00}", end.day)).Insert(0, "-");
            }

            if (end.month != null)
            {
                if (duration.Months != null)
                {
                    end.month += duration.Months;
                }
                while (end.month > 12)
                {
                    end.month -= 12;
                    end.year += 1;
                }
                endString.Insert(0, String.Format("{0:00}", end.month)).Insert(0, "-");
            }

            if (duration.Years != null)
            {
                end.year += duration.Years;
            }

            // After adding months to this year we could have bumped into or out of a non leap year
            // TODO fix this

            if (end.year > 9999)
            {
                throw new GedcomxDateException("New date out of range");
            }

            if (end.year != null)
            {
                endString.Insert(0, String.Format("%04d", Math.Abs(end.year.Value))).Insert(0, end.year >= 0 ? "+" : "-");
            }

            return new GedcomxDateSimple(endString.ToString());
        }

        /// <summary>
        /// Multiplies the duration by a fixed number.
        /// </summary>
        /// <param name="duration">The duration to be multiplied.</param>
        /// <param name="multiplier">The fixed number to multiply the duration.</param>
        /// <returns>The resulting <see cref="GedcomxDateDuration"/> after multiplying the specified duration by the fixed number.</returns>
        /// <exception cref="Gedcomx.Date.GedcomxDateException">
        /// Thrown if the duration is null
        /// or
        /// Thrown if the multiplier is negative or zero.
        /// </exception>
        public static GedcomxDateDuration MultiplyDuration(GedcomxDateDuration duration, int multiplier)
        {

            if (duration == null)
            {
                throw new GedcomxDateException("Invalid Duration");
            }

            if (multiplier <= 0)
            {
                throw new GedcomxDateException("Invalid Multiplier");
            }

            StringBuilder newDuration = new StringBuilder("P");

            if (duration.Years != null)
            {
                newDuration.Append(duration.Years * multiplier).Append('Y');
            }

            if (duration.Months != null)
            {
                newDuration.Append(duration.Months * multiplier).Append('M');
            }

            if (duration.Days != null)
            {
                newDuration.Append(duration.Days * multiplier).Append('D');
            }

            if (duration.Hours != null || duration.Minutes != null || duration.Seconds != null)
            {
                newDuration.Append('T');

                if (duration.Hours != null)
                {
                    newDuration.Append(duration.Hours * multiplier).Append('H');
                }

                if (duration.Minutes != null)
                {
                    newDuration.Append(duration.Minutes * multiplier).Append('M');
                }

                if (duration.Seconds != null)
                {
                    newDuration.Append(duration.Seconds * multiplier).Append('S');
                }

            }

            return new GedcomxDateDuration(newDuration.ToString());
        }

        /// <summary>
        /// Ensures both specified dates have matching attributes. See remarks for more information.
        /// </summary>
        /// <param name="start">The start date to evaluate.</param>
        /// <param name="end">The end date to evaluate.</param>
        /// <remarks>
        /// If the start date has minutes specified but the end date does not have minutes specified,
        /// this method will initialize minutes on the end date.
        /// </remarks>
        protected static void ZipDates(Date start, Date end)
        {
            if (start.month == null && end.month != null)
            {
                start.month = 1;
            }
            if (start.month != null && end.month == null)
            {
                end.month = 1;
            }

            if (start.day == null && end.day != null)
            {
                start.day = 1;
            }
            if (start.day != null && end.day == null)
            {
                end.day = 1;
            }

            if (start.hours == null && end.hours != null)
            {
                start.hours = 0;
            }
            if (start.hours != null && end.hours == null)
            {
                end.hours = 0;
            }

            if (start.minutes == null && end.minutes != null)
            {
                start.minutes = 0;
            }
            if (start.minutes != null && end.minutes == null)
            {
                end.minutes = 0;
            }

            if (start.seconds == null && end.seconds != null)
            {
                start.seconds = 0;
            }
            if (start.seconds != null && end.seconds == null)
            {
                end.seconds = 0;
            }
        }

        /// <summary>
        /// Ensures the specified date has matching attributes based off the duration attributes. See remarks for more information.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="duration">The duration to use for evaluation.</param>
        /// <remarks>
        /// If the specified duration has minutes and the specified date does not, this will initialize the minutes on
        /// the specified date.
        /// 
        /// Furthermore, for the level of granularity specified in the duration (e.g., down to the minute but not second),
        /// this method ensures the specified date has the same level of granularity and greater. So if the specified duration
        /// has minutes set, but the specified date does not have minutes or some greater unit not set, this will initialize
        /// the minutes on the specified date and all other levels of granularity greater than minutes. Thus hours would be set
        /// if not already, and so on. It's important to note, however, that just like <see cref="ZipDates"/>, this method will
        /// not initialize any unit for which a value is already set.
        /// 
        /// Note: This method only writes to the specified date, and only reads the duration.
        /// </remarks>
        protected static void ZipDuration(Date date, GedcomxDateDuration duration)
        {
            bool seconds = false;
            bool minutes = false;
            bool hours = false;
            bool days = false;
            bool months = false;

            if (duration.Seconds != null)
            {
                seconds = true;
                minutes = true;
                hours = true;
                days = true;
                months = true;
            }
            else if (duration.Minutes != null)
            {
                minutes = true;
                hours = true;
                days = true;
                months = true;
            }
            else if (duration.Hours != null)
            {
                hours = true;
                days = true;
                months = true;
            }
            else if (duration.Days != null)
            {
                days = true;
                months = true;
            }
            else if (duration.Months != null)
            {
                months = true;
            }
            else
            {
                return;
            }

            if (seconds && date.seconds == null)
            {
                date.seconds = 0;
            }

            if (minutes && date.minutes == null)
            {
                date.minutes = 0;
            }

            if (hours && date.hours == null)
            {
                date.hours = 0;
            }

            if (days && date.day == null)
            {
                date.day = 1;
            }

            if (months && date.month == null)
            {
                date.month = 1;
            }
        }

        /// <summary>
        /// A simplified representation of a date. Used as a bag of properties when performing caluclations.
        /// </summary>
        protected class Date
        {
            /// <summary>
            /// The year this date represents if applicable.
            /// </summary>
            public Int32? year = null;
            /// <summary>
            /// The month this date represents if applicable.
            /// </summary>
            public Int32? month = null;
            /// <summary>
            /// The day this date represents if applicable.
            /// </summary>
            public Int32? day = null;
            /// <summary>
            /// The hours this date represents if applicable.
            /// </summary>
            public Int32? hours = null;
            /// <summary>
            /// The minutes this date represents if applicable.
            /// </summary>
            public Int32? minutes = null;
            /// <summary>
            /// The seconds this date represents if applicable.
            /// </summary>
            public Int32? seconds = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="Date"/> class.
            /// </summary>
            public Date() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="Date"/> class.
            /// </summary>
            /// <param name="simple">The simple GEDCOM X date.</param>
            /// <param name="adjustTimezone">
            /// If set to <c>true</c> the resulting date hours and minutes will have the timezone hours and minutes added. See remarks for more information.
            /// </param>
            /// <remarks>
            /// The timezone adjustment is only applied when the adjustTimezone parameter is <c>true</c> and the incoming simple date hours and minutes are set. Thus,
            /// if the simple date hours are null, no adjustment will be made to the hours. Likewise, if the simple date minutes are null, no adjustment will be made
            /// to minutes.
            /// </remarks>
            public Date(GedcomxDateSimple simple, bool adjustTimezone)
            {
                year = simple.Year;
                month = simple.Month;
                day = simple.Day;
                hours = simple.Hours;
                minutes = simple.Minutes;
                seconds = simple.Seconds;

                if (adjustTimezone)
                {
                    if (hours != null && simple.TzHours != null)
                    {
                        hours += simple.TzHours;
                    }

                    if (minutes != null && simple.TzMinutes != null)
                    {
                        minutes += simple.TzMinutes;
                    }
                }
            }
        }
    }
}
