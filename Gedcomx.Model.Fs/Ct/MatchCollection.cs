using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Fs.Ct
{
    public enum MatchCollection
    {
        /**
         * The FamilySearch Family Tree.
         */
        Tree,

        /**
         * The FamilySearch Record Set.
         */
        Records,

        /**
         * The FamilySearch User-Submitted Trees.
         */
        Lls,

        /**
         * The FamilySearch Temple System.
         */
        Tss,

        // TODO: @XmlUnknownQNameEnumValue
        OTHER,
    }

    /// <remarks>
    /// Utility class for converting to/from the QNames associated with MatchCollection.
    /// </remarks>
    /// <summary>
    /// Utility class for converting to/from the QNames associated with MatchCollection.
    /// </summary>
    public static class MatchCollectionQNameUtil
    {

        /// <summary>
        /// Get the known MatchCollection for a given QName. If the QName isn't a known QName, MatchCollection.OTHER will be returned.
        /// </summary>
        public static MatchCollection ConvertFromKnownQName(string qname)
        {
            if (qname != null)
            {
                if ("http://gedcomx.org/Tree".Equals(qname))
                {
                    return MatchCollection.Tree;
                }
                if ("http://gedcomx.org/Records".Equals(qname))
                {
                    return MatchCollection.Records;
                }
                if ("http://gedcomx.org/Trees".Equals(qname))
                {
                    return MatchCollection.Lls;
                }
                if ("http://gedcomx.org/Temple".Equals(qname))
                {
                    return MatchCollection.Tss;
                }
            }
            return MatchCollection.OTHER;
        }

        /// <summary>
        /// Convert the known MatchCollection to a QName. If MatchCollection.OTHER, an ArgumentException will be thrown.
        /// </summary>
        public static string ConvertToKnownQName(MatchCollection known)
        {
            switch (known)
            {
                case MatchCollection.Tree:
                    return "http://gedcomx.org/Tree";
                case MatchCollection.Records:
                    return "http://gedcomx.org/Records";
                case MatchCollection.Lls:
                    return "http://gedcomx.org/Trees";
                case MatchCollection.Tss:
                    return "http://gedcomx.org/Temple";
                default:
                    throw new System.ArgumentException("No known QName for: " + known, "known");
            }
        }
    }
}
