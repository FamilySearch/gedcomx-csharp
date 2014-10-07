using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    public class GedcomxFile : IDisposable
    {
        private readonly FileInfo gedxFile;
        private readonly ZipArchive gedxArc;
        private readonly GedcomxEntryDeserializer deserializer;
        private readonly ManifestAttributes attributes;

        public GedcomxFile(FileInfo gedxFile, GedcomxEntryDeserializer deserializer)
        {
            this.gedxFile = gedxFile;
            this.gedxArc = ZipFile.OpenRead(gedxFile.FullName);
            this.deserializer = deserializer;
            this.attributes = ManifestAttributes.Parse(this.gedxArc);
        }

        /**
         * Creates a new <code>GedcomxFile</code> to read from the specified file <code>zipFile</code>.
         *
         * @param zipFile the zip file to be read
         * @throws IOException if an I/O error has occurred
         */
        public GedcomxFile(FileInfo zipFile, params Type[] types)
            : this(zipFile, new DefaultXmlSerialization(types))
        {
        }

        /**
         * Get the value of the specified attribute for this GEDCOM X file.
         *
         * @param name The attribute name.
         * @return The attribute value.
         */
        public String GetAttribute(String name)
        {
            var collection = attributes != null ? attributes.MainAttributes : null;
            return collection != null ? collection.Where(x => x.Name == name).Select(x => x.Value).FirstOrDefault() : null;
        }

        /**
         * Get the attributes that have been associated with this GEDCOM X file.
         *
         * @return The attributes.
         */
        public List<ManifestAttribute> Attributes
        {
            get
            {
                return attributes != null ? attributes.MainAttributes : null;
            }
        }

        /**
         * Get the entries found in this GEDCOM X file.
         *
         * @return The GEDCOM X file entries.
         */
        public IEnumerable<GedcomxFileEntry> Entries
        {
            get
            {
                foreach (var entry in this.gedxArc.Entries)
                {
                    var collection = attributes != null ? attributes[entry.FullName] : null;
                    yield return new GedcomxFileEntry(entry, collection);
                }
            }
        }

        /**
         * Get the input stream of the resource in the given entry.
         *
         * @param gedxEntry The entry that contains the desired resource.
         * @return The input stream that constitutes the nature of the resource.
         */
        public Stream GetResourceStream(GedcomxFileEntry gedxEntry)
        {
            return gedxEntry.ZipEntry.Open();
        }

        /**
         * Unmarshal the resource contained in the given entry as an object.
         *
         * @param gedxEntry The entry that contains the desired resource.
         * @return The resource.
         *
         * @throws IOException If there was a problem unmarshalling the resource.
         */
        public T ReadResource<T>(GedcomxFileEntry gedxEntry)
        {
            return this.deserializer.Deserialize<T>(GetResourceStream(gedxEntry));
        }


        /**
         * Closes the GEDCOM X file.
         *
         * @throws IOException
         */
        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            this.gedxArc.Dispose();
        }
    }
}
