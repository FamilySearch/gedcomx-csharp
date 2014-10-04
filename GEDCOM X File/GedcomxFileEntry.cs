using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    public class GedcomxFileEntry
    {
        private readonly ZipArchiveEntry zipEntry;
        private readonly List<ManifestAttribute> attributes;

        /**
         * Constructs an instance of GedcomxFileEntry by wrapping a JarEntry from the underlying Jar file.
         *
         * @param jarEntry The JarEntry instance being wrapped.
         */
        public GedcomxFileEntry(ZipArchiveEntry entry, List<ManifestAttribute> attributes)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            this.zipEntry = entry;
            this.attributes = attributes;
        }

        /**
         * Gets the underlying JarEntry object used to construct this entry.
         *
         * @return The underlying JarEntry object used to construct this entry.
         */
        public ZipArchiveEntry ZipEntry
        {
            get
            {
                return zipEntry;
            }
        }

        /**
         * The content type of this entry.
         *
         * @return The content type of the part.
         */
        public String ContentType
        {
            get
            {
                return attributes != null ? attributes.Where(x => x.Name == "Content-Type").Select(x => x.Value).FirstOrDefault() : null;
            }
        }

        /**
         * Get the value of the specified per-entry attribute.
         *
         * @param name The name of the per-entry attribute.
         * @return The value of the per-entry attribute.
         */
        public String GetAttribute(String name)
        {
            return attributes != null ? attributes.Where(x => x.Name == name).Select(x => x.Value).FirstOrDefault() : null;
        }

        /**
         * Gets the attributes associated with this entry.
         *
         * @return The attributes associated with this entry.
         *
         * @throws IOException
         */
        public Dictionary<String, String> Attributes
        {
            get
            {
                return attributes != null ? attributes.ToDictionary(x => x.Name, x => x.Value) : null;
            }
        }
    }
}
