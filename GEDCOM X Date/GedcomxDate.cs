using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Date
{
    public abstract class GedcomxDate
    {
        /**
         * Return the type of date
         * @return The Type
         */
        public abstract GedcomxDateType Type { get; }

        /**
         * Whether or not this date is approximate
         * @return True if this date is approximate
         */
        public abstract bool IsApproximate();

        /**
         * The formal representation of this date
         * @return The formal string
         */
        public abstract String ToFormalString();
    }
}
