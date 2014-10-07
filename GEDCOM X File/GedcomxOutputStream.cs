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
    public class GedcomxOutputStream : IDisposable
    {
        private readonly GedcomxEntrySerializer serializer;
        private readonly ZipArchive gedxOutputStream;
        private readonly ManifestAttributes mf;
        private int entryCount = 0;

        public GedcomxOutputStream(Stream gedxOutputStream, GedcomxEntrySerializer serializer)
        {
            this.serializer = serializer;
            this.gedxOutputStream = new ZipArchive(gedxOutputStream, ZipArchiveMode.Create, false, Encoding.UTF8);
            this.mf = new ManifestAttributes(this.gedxOutputStream);
            this.mf.MainAttributes.Put("Manifest-Version", "1.0");
        }

        /**
        * Constructs a GEDCOM X output stream.
        *
        * NOTE: This class uses the GedcomXFileJAXBContextFactory to create a JAXB context from which to derive the marshaller that is used to marshal resources into the output stream.
        * GedcomXFileJAXBContextFactory creates a context that includes some default resource classes.  The classes passed via this constructor will supplement these defaults; they will
        * not overwrite or replace these defaults.  Please see the documentation for GedcomXFileJAXBContextFactory to review the list of default classes.
        *
        * @param gedxOutputStream an output stream to which the GEDCOM X resources will appended
        * @param classes classes representing resources that will be marshaled (via JAXB) into the GEDCOM X output stream
        *
        * @throws IOException
        */
        public GedcomxOutputStream(Stream gedxOutputStream, params Type[] types)
            : this(gedxOutputStream, new DefaultXmlSerialization(types))
        {
        }

        /**
         * Add an attribute to the GEDCOM X output stream.
         *
         * @param name The name of the attribute.
         * @param value The value of the attribute.
         */
        public void AddAttribute(String name, String value)
        {
            this.mf.MainAttributes.Put(name, value);
        }

        /**
         * Add a resource to the GEDCOM X output stream.
         *
         * @param resource The resource.
         * @throws IOException
         */
        public void AddResource(Gx.Gedcomx resource)
        {
            AddResource(resource, DateTime.UtcNow);
        }

        /**
         * Add a resource to the GEDCOM X output stream.
         *
         * @param resource The resource.
         * @param lastModified timestamp when the resource was last modified (can be null)
         * @throws IOException
         */
        public void AddResource(Gx.Gedcomx resource, DateTime lastModified)
        {
            StringBuilder entryName = new StringBuilder("tree");
            if (this.entryCount > 0)
            {
                entryName.Append(this.entryCount);
            }
            entryName.Append(".xml");
            AddResource(entryName.ToString(), resource, lastModified);
        }

        /**
         * Add a resource to the GEDCOM X output stream.
         *
         * @param entryName The name by which this resource shall be known within the GEDCOM X file.
         * @param resource The resource.
         * @param lastModified timestamp when the resource was last modified (can be null)
         * @throws IOException
         */
        public void AddResource(String entryName, Gx.Gedcomx resource, DateTime lastModified)
        {
            AddResource(MediaTypes.GEDCOMX_XML_MEDIA_TYPE, entryName, resource, lastModified, null);
        }

        /**
         * Add a resource to the GEDCOM X output stream.
         *
         *
         * @param contentType The content type of the resource.
         * @param entryName The name by which this resource shall be known within the GEDCOM X file.
         * @param resource The resource.
         * @param lastModified timestamp when the resource was last modified (can be null)
         * @throws IOException
         */
        public void AddResource(String contentType, String entryName, Object resource, DateTime lastModified)
        {
            AddResource(contentType, entryName, resource, lastModified, null);
        }

        /**
         * Add a resource to the GEDCOM X output stream.
         *
         * @param contentType The content type of the resource.
         * @param entryName The name by which this resource shall be known within the GEDCOM X file.
         * @param resource The resource.
         * @param lastModified timestamp when the resource was last modified (can be null)
         * @param attributes The attributes of the resource.
         *
         * @throws IOException
         */
        public void AddResource(String contentType, String entryName, Object resource, DateTime lastModified, Dictionary<String, String> attributes)
        {
            if (contentType.Trim().Length == 0)
            {
                throw new ArgumentException("contentType must not be null or empty.", contentType);
            }

            entryName = entryName.Replace("\\\\", "/");
            entryName = entryName[0] == '/' ? entryName.Substring(1) : entryName;

            ZipArchiveEntry gedxEntry = gedxOutputStream.CreateEntry(entryName); // will throw a runtime exception if entryName is not okay
            List<ManifestAttribute> entryAttrs = new List<ManifestAttribute>();

            if (lastModified != null)
            {
                entryAttrs.Put("X-DC-modified", lastModified.ToUniversalTime().ToString("o"));
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

        /**
         * Closes the GEDCOM X output stream as well as the stream being filtered.
         *
         * @throws IOException
         */
        public void Close()
        {
            var entry = gedxOutputStream.CreateEntry(ManifestAttributes.MANIFEST_FULLNAME);

            using (var stream = entry.Open())
            {
                this.mf.Write(stream);
            }
        }

        public void Dispose()
        {
            this.Close();
            this.gedxOutputStream.Dispose();
        }
    }
}
