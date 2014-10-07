using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    public interface GedcomxEntrySerializer
    {
        /**
         * Serialize the resource to the specified output stream.
         *
         * @param resource the resource.
         * @param out the output stream.
         */
        void Serialize(Object resource, Stream stream);

        String Serialize(Object resource);

        /**
         * Whether the specified content type is a known content type and therefore doesn't need to be written to the entry attributes.
         *
         * @param contentType The content type.
         * @return Whether the content type is "known".
         */
        bool IsKnownContentType(String contentType);
    }
}
