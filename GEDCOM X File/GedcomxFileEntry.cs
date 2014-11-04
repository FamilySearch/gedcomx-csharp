using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    /// <summary>
    /// Represents a single file withing a GEDCOM X file.
    /// </summary>
    public class GedcomxFileEntry
    {
        private readonly ZipArchiveEntry zipEntry;
        private readonly List<ManifestAttribute> attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxFileEntry"/> class.
        /// </summary>
        /// <param name="entry">The zip archive this file belongs to.</param>
        /// <param name="attributes">The attributes belonging to this file (such as content type, etc).</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the entry parameter is <c>null</c>.</exception>
        public GedcomxFileEntry(ZipArchiveEntry entry, List<ManifestAttribute> attributes)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            this.zipEntry = entry;
            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the zip file this file belongs to.
        /// </summary>
        /// <value>
        /// The zip file this file belongs to.
        /// </value>
        public ZipArchiveEntry ZipEntry
        {
            get
            {
                return zipEntry;
            }
        }

        /// <summary>
        /// Gets the content type of this file.
        /// </summary>
        /// <value>
        /// The content type of this file.
        /// </value>
        public String ContentType
        {
            get
            {
                return attributes != null ? attributes.Where(x => x.Name == "Content-Type").Select(x => x.Value).FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// Gets the value of the attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute for which the value will be retrieved.</param>
        /// <returns>If an attribute with the specified name is found the value of said attribute will be returned; otherwise, <c>null</c> will be returned.</returns>
        public String GetAttribute(String name)
        {
            return attributes != null ? attributes.Where(x => x.Name == name).Select(x => x.Value).FirstOrDefault() : null;
        }

        /// <summary>
        /// Gets the attributes belonging to this file (such as content type, etc).
        /// </summary>
        /// <value>
        /// The attributes belonging to this file (such as content type, etc).
        /// </value>
        public Dictionary<String, String> Attributes
        {
            get
            {
                return attributes != null ? attributes.ToDictionary(x => x.Name, x => x.Value) : null;
            }
        }
    }
}
