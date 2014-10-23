using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Types
{
    public enum FactQualifierType
    {
        /**
         * The age of a person at the event described by the fact.
         */
        Age,

        /**
         * The cause of a specific fact, such as the cause of death.
         */
        Cause,

        /**
         * The religion associated with a religious event such as a baptism or excommunication.
         */
        Religion,

        OTHER,
    }

    public static class FactQualifierTypeUtil
    {
        public static FactQualifierType ConvertFromKnownQName(string qname)
        {
            if (qname != null)
            {
                if ("http://gedcomx.org/Age".Equals(qname))
                {
                    return FactQualifierType.Age;
                }
                if ("http://gedcomx.org/Cause".Equals(qname))
                {
                    return FactQualifierType.Cause;
                }
                if ("http://gedcomx.org/Religion".Equals(qname))
                {
                    return FactQualifierType.Religion;
                }
            }
            return FactQualifierType.OTHER;
        }

        public static string ConvertToKnownQName(FactQualifierType known)
        {
            switch (known)
            {
                case FactQualifierType.Age:
                    return "http://gedcomx.org/Age";
                case FactQualifierType.Cause:
                    return "http://gedcomx.org/Cause";
                case FactQualifierType.Religion:
                    return "http://gedcomx.org/Religion";
                default:
                    throw new System.ArgumentException("No known QName for: " + known, "known");
            }
        }
    }
}
