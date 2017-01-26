using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
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
        [XmlEnum("http://gedcomx.org/Age")]
        Age,

        /**
         * The cause of a specific fact, such as the cause of death.
         */
        [XmlEnum("http://gedcomx.org/Cause")]
        Cause,

        /**
         * The religion associated with a religious event such as a baptism or excommunication.
         */
        [XmlEnum("http://gedcomx.org/Religion")]
        Religion,

        [XmlEnum("http://gedcomx.org/OTHER")]
        OTHER,
    }
}
