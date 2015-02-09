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
    /// Provides methods for working with GEDCOM X files.
    /// </summary>
    public class GedcomxFile : IDisposable
    {
        private readonly FileInfo gedxFile;
        private readonly ZipArchive gedxArc;
        private readonly IGedcomxEntryDeserializer deserializer;
        private readonly ManifestAttributes attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxFile"/> class.
        /// </summary>
        /// <param name="gedxFile">The GEDCOM X file.</param>
        /// <param name="deserializer">The deserializer to use for deserializing data streams from the file.</param>
        public GedcomxFile(FileInfo gedxFile, IGedcomxEntryDeserializer deserializer)
        {
            this.gedxFile = gedxFile;
            this.gedxArc = ZipFile.OpenRead(gedxFile.FullName);
            this.deserializer = deserializer;
            this.attributes = ManifestAttributes.Parse(this.gedxArc);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomxFile"/> class. Creates a new <c>GedcomxFile</c> to read from the specified <c>zipFile</c>.
        /// </summary>
        /// <param name="zipFile">The zip file to be read.</param>
        /// <param name="types">The types the serializer is to know about for serialization.</param>
        public GedcomxFile(FileInfo zipFile, params Type[] types)
            : this(zipFile, new DefaultXmlSerialization(types))
        {
        }

        /// <summary>
        /// Get the value of the specified attribute for this GEDCOM X file.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>The value of the requested attribute if it is found; otherwise, <c>null</c>.</returns>
        public String GetAttribute(String name)
        {
            var collection = attributes != null ? attributes.MainAttributes : null;
            return collection != null ? collection.Where(x => x.Name == name).Select(x => x.Value).FirstOrDefault() : null;
        }

        /// <summary>
        /// Gets the attributes that have been associated with this GEDCOM X file.
        /// </summary>
        /// <value>
        /// The attributes that have been associated with this GEDCOM X file.
        /// </value>
        public List<ManifestAttribute> Attributes
        {
            get
            {
                return attributes != null ? attributes.MainAttributes : null;
            }
        }

        /// <summary>
        /// Gets the entries found in this GEDCOM X file.
        /// </summary>
        /// <value>
        /// The entries found in this GEDCOM X file.
        /// </value>
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

        /// <summary>
        /// Gets the stream for the specified zip file entry. The stream access will depend on the mode of the zip file.
        /// </summary>
        /// <param name="gedxEntry">The entry that contains the desired resource.</param>
        /// <returns>The stream for the specified zip file entry. The stream access will depend on the mode of the zip file.</returns>
        public Stream GetResourceStream(GedcomxFileEntry gedxEntry)
        {
            return gedxEntry.ZipEntry.Open();
        }

        /// <summary>
        /// Deserializes the specified file entry to the requested object type.
        /// </summary>
        /// <typeparam name="T">The type the object will be deserialized to.</typeparam>
        /// <param name="gedxEntry">The file entry to deserialize.</param>
        /// <returns>An instance of T representing the object deserialized.</returns>
        public T ReadResource<T>(GedcomxFileEntry gedxEntry)
        {
            return this.deserializer.Deserialize<T>(GetResourceStream(gedxEntry));
        }


        /// <summary>
        /// Closes the GEDCOM X file by specifically calling <see cref="M:Dispose()"/>.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.gedxArc.Dispose();
        }
    }
}
