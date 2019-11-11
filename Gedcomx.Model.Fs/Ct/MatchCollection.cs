using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
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
        [XmlEnum("http://gedcomx.org/Tree")]
        Tree,

        /**
         * The FamilySearch Record Set.
         */
        [XmlEnum("http://gedcomx.org/Records")]
        Records,

        /**
         * The FamilySearch User-Submitted Trees.
         */
        [XmlEnum("http://gedcomx.org/Trees")]
        Lls,

        /**
         * The FamilySearch Temple System.
         */
        [XmlEnum("http://gedcomx.org/Temple")]
        Tss,

        // TODO: @XmlUnknownQNameEnumValue
        [XmlEnum("http://gedcomx.org/OTHER")]
        OTHER,
    }
}
