using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    /// <summary>
    /// An interface exposing the ability serialize an arbitrary object.
    /// </summary>
    public interface IGedcomxEntrySerializer
    {
        /// <summary>
        /// Serializes the specified object to the specified stream.
        /// </summary>
        /// <param name="resource">The object to be serialized.</param>
        /// <param name="stream">The stream that will contain the serialization result.</param>
        void Serialize(Object resource, Stream stream);

        /// <summary>
        /// Serializes the specified object and returns the string result.
        /// </summary>
        /// <param name="resource">The object to be serialized.</param>
        /// <returns>A string representation of the serialized object.</returns>
        String Serialize(Object resource);

        /// <summary>
        /// Determines whether the content type is known to the serializer. This is currently not used in any meaningful way.
        /// </summary>
        /// <param name="contentType">The content type to check.</param>
        /// <returns>
        /// <c>true</c> if the content type is known to the serializer; otherwise, false.
        /// </returns>
        bool IsKnownContentType(String contentType);
    }
}
