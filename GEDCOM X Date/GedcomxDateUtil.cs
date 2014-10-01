using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    public class GedcomxDateUtil
    {
        /**
         * Parse a String representation of a Formal GedcomX Date
         * @param date The GedcomX Date
         * @return A GedcomxDate
         */
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

        /**
         * Calculates the Duration between two dates
         * @param startDate The start date
         * @param endDate The end date
         * @return The duration
         */
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

        /**
         * Add a duration to a simple date
         * @param startDate The date to start from
         * @param duration The duration to add
         * @return a new simple date
         */
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

        /**
         * Multiple a duration by a fixed number
         * @param duration The duration to multiply
         * @param multiplier The amount to multiply by
         * @return The new, multiplied duration
         */
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

        /**
         * Ensures that both start and end have values where the other has values.
         * For example, if start has minutes but end does not, this function
         * will initialize minutes in end.
         * @param start The start date
         * @param end The end date
         */
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

        /**
         * Ensures that date has its properties initialized based on what the duration has.
         * For example, if date does not have minutes and duration does, this will
         * initialize minutes in the date.
         * @param date The start date
         * @param duration The duration
         */
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

        /**
         * A simplified representation of a date.
         * Used as a bag-o-properties when performing caluclations
         */
        protected class Date
        {
            public Int32? year = null;
            public Int32? month = null;
            public Int32? day = null;
            public Int32? hours = null;
            public Int32? minutes = null;
            public Int32? seconds = null;

            public Date() { }

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
