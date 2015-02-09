using Gedcomx.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    /// <summary>
    /// Represents a GEDCOM X file stream. Since the underlying data is a zip file, this class simply wraps some of the zip file access.
    /// </summary>
    public class GedcomxOutputStream : IDisposable
    {
        private readonly IGedcomxEntrySerializer serializer;
        private readonly ZipArchive gedxOutputStream;
        private readonly ManifestAttributes mf;
        private int entryCount = 0;
        private bool closed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxOutputStream"/> class.
        /// </summary>
        /// <param name="gedxOutputStream">The underlying data stream this GEDCOM X will be written to.</param>
        /// <param name="serializer">The serializer to use when adding objects to this GEDCOM X file.</param>
        public GedcomxOutputStream(Stream gedxOutputStream, IGedcomxEntrySerializer serializer)
        {
            this.serializer = serializer;
            this.gedxOutputStream = new ZipArchive(gedxOutputStream, ZipArchiveMode.Create, false, Encoding.UTF8);
            this.mf = new ManifestAttributes(this.gedxOutputStream);
            this.mf.MainAttributes.Put("Manifest-Version", "1.0");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxOutputStream"/> class.
        /// </summary>
        /// <param name="gedxOutputStream">The underlying data stream this GEDCOM X will be written to.</param>
        /// <param name="types">The types the serializer is to know about for serialization.</param>
        public GedcomxOutputStream(Stream gedxOutputStream, params Type[] types)
            : this(gedxOutputStream, new DefaultXmlSerialization(types))
        {
        }

        /// <summary>
        /// Adds the specified attribute name and value to this GEDCOM X file (such as the content type, etc).
        /// </summary>
        /// <param name="name">The name of the attribute to add.</param>
        /// <param name="value">The value of the attribute to add.</param>
        public void AddAttribute(String name, String value)
        {
            this.mf.MainAttributes.Put(name, value);
        }

        /// <summary>
        /// Adds the specified GEDCOM X entity to the current GEDCOM X file.
        /// </summary>
        /// <param name="resource">The entity to add.</param>
        public void AddResource(Gx.Gedcomx resource)
        {
            AddResource(resource, DateTime.UtcNow);
        }

        /// <summary>
        /// Adds the specified GEDCOM X entity to the current GEDCOM X file.
        /// </summary>
        /// <param name="resource">The entity to add.</param>
        /// <param name="lastModified">The last modified to specify for the entity being added.</param>
        public void AddResource(Gx.Gedcomx resource, DateTime? lastModified)
        {
            StringBuilder entryName = new StringBuilder("tree");
            if (this.entryCount > 0)
            {
                entryName.Append(this.entryCount);
            }
            entryName.Append(".xml");
            AddResource(entryName.ToString(), resource, lastModified);
        }

        /// <summary>
        /// Adds the specified GEDCOM X entity to the current GEDCOM X file.
        /// </summary>
        /// <param name="entryName">The name by which this entity shall be known within the GEDCOM X file.</param>
        /// <param name="resource">The entity to add.</param>
        /// <param name="lastModified">The last modified to specify for the entity being added.</param>
        public void AddResource(String entryName, Gx.Gedcomx resource, DateTime? lastModified)
        {
            AddResource(MediaTypes.GEDCOMX_XML_MEDIA_TYPE, entryName, resource, lastModified, null);
        }

        /// <summary>
        /// Adds the specified GEDCOM X entity to the current GEDCOM X file.
        /// </summary>
        /// <param name="contentType">The content type of the entity.</param>
        /// <param name="entryName">The name by which this entity shall be known within the GEDCOM X file.</param>
        /// <param name="resource">The entity to add.</param>
        /// <param name="lastModified">The last modified to specify for the entity being added.</param>
        public void AddResource(String contentType, String entryName, Object resource, DateTime? lastModified)
        {
            AddResource(contentType, entryName, resource, lastModified, null);
        }

        /// <summary>
        /// Adds the specified GEDCOM X entity to the current GEDCOM X file.
        /// </summary>
        /// <param name="contentType">The content type of the entity.</param>
        /// <param name="entryName">The name by which this entity shall be known within the GEDCOM X file.</param>
        /// <param name="resource">The entity to add.</param>
        /// <param name="lastModified">The last modified to specify for the entity being added.</param>
        /// <param name="attributes">The attributes of the specified entity (such as content type, etc).</param>
        /// <exception cref="System.ArgumentException">Thrown if the specified content type is null or empty.</exception>
        public void AddResource(String contentType, String entryName, Object resource, DateTime? lastModified, Dictionary<String, String> attributes)
        {
            if (String.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType must not be null or empty.", contentType);
            }

            entryName = entryName.Replace("\\\\", "/");
            entryName = entryName[0] == '/' ? entryName.Substring(1) : entryName;

            ZipArchiveEntry gedxEntry = gedxOutputStream.CreateEntry(entryName); // will throw a runtime exception if entryName is not okay
            List<ManifestAttribute> entryAttrs = new List<ManifestAttribute>();

            if (lastModified != null)
            {
                entryAttrs.Put("X-DC-modified", lastModified.Value.ToUniversalTime().ToString("o"));
            }

            if (!IsKnownContentType(contentType))
            {
                entryAttrs.Put("Content-Type", contentType);
            }

            if (attributes != null)
            {
                foreach (KeyValuePair<String, String> entry in attributes)
                {
                    entryAttrs.Put(entry.Key, entry.Value);
                }
            }

            if (entryAttrs.Any())
            {
                this.mf.Put(gedxEntry.FullName, entryAttrs);
            }

            using (var stream = gedxEntry.Open())
            {
                this.serializer.Serialize(resource, stream);
            }
            this.entryCount++;
        }

        private bool IsKnownContentType(String contentType)
        {
            return this.serializer.IsKnownContentType(contentType);
        }

        /// <summary>
        /// Adds the manifest entry and marks this instance as closed.
        /// </summary>
        public void Close()
        {
            if (!closed)
            {
                var entry = gedxOutputStream.CreateEntry(ManifestAttributes.MANIFEST_FULLNAME);

                using (var stream = entry.Open())
                {
                    this.mf.Write(stream);
                }

                closed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Close();
            this.gedxOutputStream.Dispose();
        }
    }
}
