using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gedcomx.Model.Util;

namespace Gx.Types
{
    public enum FactQualifierType
    {
        /**
         * The age of a person at the event described by the fact.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Age")]
        Age,

        /**
         * The cause of a specific fact, such as the cause of death.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Cause")]
        Cause,

        /**
         * The religion associated with a religious event such as a baptism or excommunication.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Religion")]
        Religion,

        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/OTHER")]
        OTHER,
    }
}
