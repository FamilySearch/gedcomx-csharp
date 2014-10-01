using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    public class GedcomxDateDuration : GedcomxDate
    {
        private Int32? years = null;
        private Int32? months = null;
        private Int32? days = null;
        private Int32? hours = null;
        private Int32? minutes = null;
        private Int32? seconds = null;

        /**
         * Create a new duration from the formal string
         * @param str The formal duration string
         */
        public GedcomxDateDuration(String str)
        {

            // Durations must start with P
            if (str == null || str.Length < 1 || str[0] != 'P')
            {
                throw new GedcomxDateException("Invalid Duration: Must start with P");
            }

            String duration = str.Substring(1);

            if (duration.Length < 1)
            {
                throw new GedcomxDateException("Invalid Duration: You must have a duration value");
            }

            // 5.3.2 allows for NON normalized durations
            // We assume that if there is a space, it is non-normalized
            if (duration.Contains(" "))
            {
                // When we implement non normalized durations we can call parseNonNormalizedDuration(duration)
                throw new GedcomxDateException("Invalid Duration: Non normalized durations are not implemented yet");
            }
            else
            {
                ParseNormalizedDuration(duration);
            }
        }

        /**
         * Parse the normalized duration
         * @param duration the formal duration string
         */
        private void ParseNormalizedDuration(String duration)
        {
            String currentNum = "";
            bool inTime = false;
            HashSet<String> seen = new HashSet<String>();
            List<String> valid = new List<string>() { "Y", "Mo", "D", "T", "H", "Mi", "S" };

            foreach (char character in duration)
            {
                if (Char.IsDigit(character))
                {
                    currentNum += character;
                    continue;
                }

                switch (character)
                {
                    case 'Y':
                        if (currentNum.Length < 1)
                        {
                            throw new GedcomxDateException("Invalid Duration: Invalid years");
                        }
                        if (seen.Contains("Y"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Duplicate years");
                        }
                        if (!valid.Contains("Y"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Years out of order");
                        }
                        this.years = Int32.Parse(currentNum);
                        seen.Add("Y");
                        valid = valid.Skip(valid.IndexOf("Y") + 1).ToList();
                        currentNum = "";
                        break;
                    case 'M':
                        if (inTime)
                        {
                            if (currentNum.Length < 1)
                            {
                                throw new GedcomxDateException("Invalid Duration: Invalid minutes");
                            }
                            if (seen.Contains("Mi"))
                            {
                                throw new GedcomxDateException("Invalid Duration: Duplicate minutes");
                            }
                            if (!valid.Contains("Mi"))
                            {
                                throw new GedcomxDateException("Invalid Duration: Minutes out of order");
                            }
                            this.minutes = Int32.Parse(currentNum);
                            seen.Add("Mi");
                            valid = valid.Skip(valid.IndexOf("Mi") + 1).ToList();
                            currentNum = "";
                        }
                        else
                        {
                            if (currentNum.Length < 1)
                            {
                                throw new GedcomxDateException("Invalid Duration: Invalid months");
                            }
                            if (seen.Contains("Mo"))
                            {
                                throw new GedcomxDateException("Invalid Duration: Duplicate months");
                            }
                            if (!valid.Contains("Mo"))
                            {
                                throw new GedcomxDateException("Invalid Duration: Months out of order");
                            }
                            this.months = Int32.Parse(currentNum);
                            seen.Add("Mo");
                            valid = valid.Skip(valid.IndexOf("Mo") + 1).ToList();
                            currentNum = "";
                        }
                        break;
                    case 'D':
                        if (currentNum.Length < 1)
                        {
                            throw new GedcomxDateException("Invalid Duration: Invalid days");
                        }
                        if (seen.Contains("D"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Duplicate days");
                        }
                        if (!valid.Contains("D"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Days out of order");
                        }
                        this.days = Int32.Parse(currentNum);
                        seen.Add("D");
                        valid = valid.Skip(valid.IndexOf("D") + 1).ToList();
                        currentNum = "";
                        break;
                    case 'H':
                        if (!inTime)
                        {
                            throw new GedcomxDateException("Invalid Duration: Missing T before hours");
                        }
                        if (currentNum.Length < 1)
                        {
                            throw new GedcomxDateException("Invalid Duration: Invalid hours");
                        }
                        if (seen.Contains("H"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Duplicate hours");
                        }
                        if (!valid.Contains("H"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Hours out of order");
                        }
                        this.hours = Int32.Parse(currentNum);
                        seen.Add("H");
                        valid = valid.Skip(valid.IndexOf("H") + 1).ToList();
                        currentNum = "";
                        break;
                    case 'S':
                        if (!inTime)
                        {
                            throw new GedcomxDateException("Invalid Duration: Missing T before seconds");
                        }
                        if (currentNum.Length < 1)
                        {
                            throw new GedcomxDateException("Invalid Duration: Invalid seconds");
                        }
                        if (seen.Contains("S"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Duplicate seconds");
                        }
                        // You cannot have seconds out of order because it's last
                        this.seconds = Int32.Parse(currentNum);
                        seen.Add("S");
                        valid = new List<String>();
                        currentNum = "";
                        break;
                    case 'T':
                        if (seen.Contains("T"))
                        {
                            throw new GedcomxDateException("Invalid Duration: Duplicate T");
                        }
                        inTime = true;
                        seen.Add("T");
                        valid = valid.Skip(valid.IndexOf("T") + 1).ToList();
                        break;
                    default:
                        throw new GedcomxDateException("Invalid Duration: Unknown letter " + character);
                }
            }

            // If there is any leftover we have an invalid duration
            if (!currentNum.Equals(""))
            {
                throw new GedcomxDateException("Invalid Duration: No letter after " + currentNum);
            }

        }

        /**
         * The type of this date
         * @return The date type
         */
        public override GedcomxDateType Type
        {
            get
            {
                return GedcomxDateType.DURATION;
            }
        }

        /**
         * A Duration is NEVER Approximate
         * @return True if the duration is approximate (It never is)
         */
        public override bool IsApproximate()
        {
            return false;
        }

        /**
         * The formal string representation of the duration
         * @return The formal string
         */
        public override String ToFormalString()
        {
            StringBuilder duration = new StringBuilder("P");

            if (years != null)
            {
                duration.Append(years).Append('Y');
            }

            if (months != null)
            {
                duration.Append(months).Append('M');
            }

            if (days != null)
            {
                duration.Append(days).Append('D');
            }

            if (hours != null || minutes != null || seconds != null)
            {
                duration.Append('T');

                if (hours != null)
                {
                    duration.Append(hours).Append('H');
                }

                if (minutes != null)
                {
                    duration.Append(minutes).Append('M');
                }

                if (seconds != null)
                {
                    duration.Append(seconds).Append('S');
                }
            }

            return duration.ToString();
        }

        /**
         * Get the years
         * @return The Years
         */
        public Int32? Years
        {get{
            return years;
        }}

        /**
         * Get the months
         * @return The Months
         */
        public Int32? Months
        {
            get
            {
                return months;
            }
        }

        /**
         * Get the days
         * @return The Days
         */
        public Int32? Days
        {
            get
            {
                return days;
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
         * @return The Seconds
         */
        public Int32? Seconds
        {
            get
            {
                return seconds;
            }
        }
    }
}
