using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    /// <summary>
    /// An interface exposing the ability deserialize an arbitrary stream or string.
    /// </summary>
    public interface GedcomxEntryDeserializer
    {
        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T">The type of object the specified stream represents.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <returns>An instance of T upon successful deserialization.</returns>
        T Deserialize<T>(Stream stream);

        /// <summary>
        /// Deserializes the specified string.
        /// </summary>
        /// <typeparam name="T">The type of object the specified string represents.</typeparam>
        /// <param name="value">The string to be deserialized.</param>
        /// <returns>An instance of T upon successful deserialization.</returns>
        T Deserialize<T>(String value);
    }
}
