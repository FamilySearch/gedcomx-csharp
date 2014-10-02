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
        private readonly Dictionary<String, String> manifestAttributes;

        public GedcomxFile(FileInfo gedxFile, GedcomxEntryDeserializer deserializer)
        {
            this.gedxFile = gedxFile;
            this.gedxArc = ZipFile.OpenRead(gedxFile.FullName);
            this.deserializer = deserializer;
            this.manifestAttributes = ManifestAttributesParser.Parse(this.gedxArc).ToDictionary(x => x.Name, x => x.Value);
        }

        /**
         * Creates a new <code>GedcomxFile</code> to read from the specified file <code>zipFile</code>.
         *
         * @param zipFile the zip file to be read
         * @throws IOException if an I/O error has occurred
         */
        public GedcomxFile(FileInfo zipFile, params Type[] types)
            : this(zipFile, new DefaultXMLSerialization(types))
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
            return manifestAttributes.Where(x => x.Key == name).Select(x => x.Value).FirstOrDefault();
        }

        /**
         * Get the attributes that have been associated with this GEDCOM X file.
         *
         * @return The attributes.
         */
        public Dictionary<String, String> Attributes
        {
            get
            {
                return manifestAttributes;
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
                    yield return new GedcomxFileEntry(entry);
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
        public Object ReadResource(GedcomxFileEntry gedxEntry)
        {
            return this.deserializer.Deserialize(GetResourceStream(gedxEntry));
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
