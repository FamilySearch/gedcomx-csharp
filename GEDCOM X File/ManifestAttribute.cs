using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    /// <summary>
    /// Represents a single attribute that can be used in a manifest file.
    /// </summary>
    public class ManifestAttribute
    {
        /// <summary>
        /// Gets the name of the current attribute.
        /// </summary>
        /// <value>
        /// The name of the current attribute.
        /// </value>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets the value of the current attribute.
        /// </summary>
        /// <value>
        /// The value of the current attribute.
        /// </value>
        public string Value { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this attribute.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this attribute.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }
    }
}
