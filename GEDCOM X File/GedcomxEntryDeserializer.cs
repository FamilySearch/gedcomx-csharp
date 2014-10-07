using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    public interface GedcomxEntryDeserializer
    {
        /**
         * Deserialize the resource from the specified input stream.
         *
         * @param in the input stream.
         * @return the resource.
         */
        T Deserialize<T>(Stream stream);

        T Deserialize<T>(String value);
    }
}
