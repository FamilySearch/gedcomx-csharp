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
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Tree")]
        Tree,

        /**
         * The FamilySearch Record Set.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Records")]
        Records,

        /**
         * The FamilySearch User-Submitted Trees.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Trees")]
        Lls,

        /**
         * The FamilySearch Temple System.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Temple")]
        Tss,

        // TODO: @XmlUnknownQNameEnumValue
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/OTHER")]
        OTHER,
    }
}
