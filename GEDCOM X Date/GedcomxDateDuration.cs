using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    /// <summary>
    /// Represents a GEDCOM X date span, measured from years to seconds.
    /// </summary>
    public class GedcomxDateDuration : GedcomxDate
    {
        private Int32? years = null;
        private Int32? months = null;
        private Int32? days = null;
        private Int32? hours = null;
        private Int32? minutes = null;
        private Int32? seconds = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxDateDuration"/> class.
        /// </summary>
        /// <param name="str">The formal duration string that describes a GEDCOM X duration.</param>
        /// <exception cref="GedcomxDateException">
        /// Thrown if the formal string is null, empty, or does not begin with 'P'.
        /// or
        /// Thrown if the formal string does not have a duration specified (the string after 'P').
        /// or
        /// Thrown if the formal string is a 5.3.2 non-normalized date (these are not yet implemented).
        /// </exception>
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

        /// <summary>
        /// Parses the formal duration string and populates the results in this current instance.
        /// </summary>
        /// <param name="duration">The formal duration string to be parsed.</param>
        /// <exception cref="GedcomxDateException">
        /// Thrown if any of the current parsing expectations fail. A specific reason will be included at runtime.
        /// </exception>
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
                return GedcomxDateType.DURATION;
            }
        }

        /// <summary>
        /// Determines whether this date is approximate.  This method always returns <c>false</c> for this instance.
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
        /// The formal representation of this duration.
        /// </summary>
        /// <value>
        /// The formal representation of this duration.
        /// </value>
        public override String FormalString
        {
            get
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
        }

        /// <summary>
        /// Gets the years of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The years of the current duration if applicable.
        /// </value>
        public Int32? Years
        {
            get
            {
                return years;
            }
        }

        /// <summary>
        /// Gets the months of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The months of the current duration if applicable.
        /// </value>
        public Int32? Months
        {
            get
            {
                return months;
            }
        }

        /// <summary>
        /// Gets the days of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The days of the current duration if applicable.
        /// </value>
        public Int32? Days
        {
            get
            {
                return days;
            }
        }

        /// <summary>
        /// Gets the hours of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The hours of the current duration if applicable.
        /// </value>
        public Int32? Hours
        {
            get
            {
                return hours;
            }
        }

        /// <summary>
        /// Gets the minutes of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The minutes of the current duration if applicable.
        /// </value>
        public Int32? Minutes
        {
            get
            {
                return minutes;
            }
        }

        /// <summary>
        /// Gets the seconds of the current duration if applicable.
        /// </summary>
        /// <value>
        /// The seconds of the current duration if applicable.
        /// </value>
        public Int32? Seconds
        {
            get
            {
                return seconds;
            }
        }
    }
}
