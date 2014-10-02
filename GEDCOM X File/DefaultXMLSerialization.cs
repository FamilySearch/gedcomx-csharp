using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    class DefaultXMLSerialization : GedcomxEntryDeserializer
    {
        private Type[] types;

        public DefaultXMLSerialization(params Type[] types)
        {
            // TODO: Complete member initialization
            this.types = types;
        }
    }
}
