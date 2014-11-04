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
    /// Represents a collection of manifest attributes.
    /// </summary>
    public class ManifestAttributes : Dictionary<string, List<ManifestAttribute>>
    {
        /// <summary>
        /// The folder where the manifest file must go.
        /// </summary>
        public const string MANIFEST_FOLDER = "META-INF";
        /// <summary>
        /// The name of the manifest file.
        /// </summary>
        public const string MANIFEST_FILE = "MANIFEST.MF";
        /// <summary>
        /// The full path to the manifest file (this path is fully qualified in terms of a zip file path).
        /// </summary>
        public static readonly string MANIFEST_FULLNAME = MANIFEST_FOLDER + "/" + MANIFEST_FILE;
        private ZipArchive zip;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestAttributes"/> class.
        /// </summary>
        /// <param name="zipArchive">The zip file this manifest belongs to.</param>
        public ManifestAttributes(ZipArchive zipArchive)
        {
            zip = zipArchive;
        }

        /// <summary>
        /// Searches the specified zip file for the manifest file and returns the attribute declarations it contains.
        /// </summary>
        /// <param name="zip">The zip file to be evaluated.</param>
        /// <returns>A <see cref="ManifestAttributes"/> with the discovered manifest file attribute declarations.</returns>
        public static ManifestAttributes Parse(ZipArchive zip)
        {
            var result = new ManifestAttributes(zip);
            var manifest = zip.GetEntry(MANIFEST_FULLNAME);
            var tempAttributes = new List<ManifestAttribute>();
            var mainAttributesParsed = false;
            ZipArchiveEntry entry = null;

            if (zip != null && zip.Entries != null)
            {
                foreach (var file in zip.Entries)
                {
                    result.Add(file.FullName, null);
                }
            }

            if (manifest != null)
            {
                using (var stream = new StreamReader(manifest.Open()))
                {
                    while (!stream.EndOfStream)
                    {
                        var line = stream.ReadLine();
                        var attribute = Parse(line);

                        if (attribute != null)
                        {
                            tempAttributes.Add(attribute);
                        }

                        if (line == string.Empty)
                        {
                            var entryName = tempAttributes.FirstOrDefault(x => x.Name == "Name");

                            if ((entryName != null && !string.IsNullOrEmpty(entryName.Value)) || !mainAttributesParsed)
                            {
                                entry = entryName != null ? zip.GetEntry(entryName.Value) : manifest;

                                if (entry == manifest)
                                {
                                    mainAttributesParsed = true;
                                }

                                if (entry != null)
                                {
                                    result[entry.FullName] = tempAttributes.ToList(); // Make a copy so the reference won't clear this assignment
                                    tempAttributes.Clear();
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static ManifestAttribute Parse(string attribute)
        {
            ManifestAttribute result = null;
            int index;

            if (!string.IsNullOrEmpty(attribute) && (index = attribute.IndexOf(":")) != -1)
            {
                var key = attribute.Substring(0, index);
                var value = attribute.Substring(index + 1);

                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                {
                    result = new ManifestAttribute();
                    result.Name = key.Trim();
                    result.Value = value.Trim();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the attributes associated with the zip file.
        /// </summary>
        /// <value>
        /// The attributes associated with the zip file.
        /// </value>
        public List<ManifestAttribute> MainAttributes
        {
            get
            {
                return GetEntryAttributes(MANIFEST_FULLNAME);
            }
        }

        /// <summary>
        /// Associates the specified attributes to the specified file entry.
        /// </summary>
        /// <param name="entryName">The name of the file entry to which the specified attributes will be associated.</param>
        /// <param name="attributes">The attributes to associate with the specified file entry.</param>
        /// <remarks>
        /// If the file entry does not exist, a new entry record will be made. If the file entry exists, but does not have attributes,
        /// the specified attributes will become the only associated attributes. If the file entry exists, and other attributes also
        /// exist, these attributes will be appended at the end of the attribute list for the said file entry.
        /// </remarks>
        public void Put(String entryName, List<ManifestAttribute> attributes)
        {
            if (!string.IsNullOrWhiteSpace(entryName))
            {
                attributes = attributes ?? new List<ManifestAttribute>();

                if (!this.ContainsKey(entryName))
                {
                    this.Add(entryName, new List<ManifestAttribute>());
                }

                var existing = GetEntryAttributes(entryName);

                if (existing != null)
                {
                    existing.AddRange(attributes);
                }
                else
                {
                    this[entryName] = attributes;
                }
            }
        }

        /// <summary>
        /// Gets the list of attributes for the specified file entry.
        /// </summary>
        /// <param name="entryName">The name of the file entry for which the list of attributes will be retrieved.</param>
        /// <returns>The list of attributes for the specified file entry.</returns>
        private List<ManifestAttribute> GetEntryAttributes(String entryName)
        {
            if (!this.ContainsKey(entryName))
            {
                this.Add(entryName, new List<ManifestAttribute>());
            }

            if (this[entryName] == null)
            {
                this[entryName] = new List<ManifestAttribute>();
            }

            return this[entryName];
        }

        /// <summary>
        /// Writes the current attributes collection to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which the attributes will be written.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the stream parameter is <c>null</c>.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the specified stream <see cref="P:Stream.CanWrite"/> is false.</exception>
        public void Write(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanWrite)
            {
                throw new InvalidOperationException("Unable to write to stream.");
            }

            var manifest = GetManifest();
            var builder = new StringBuilder();

            WriteEntryAttributes(manifest, builder);
            foreach (var entry in this.Keys)
            {
                if (entry != manifest)
                {
                    builder.Append("Name: ");
                    builder.AppendLine(entry);
                    WriteEntryAttributes(entry, builder);
                }
            }

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteEntryAttributes(String entryName, StringBuilder builder)
        {
            foreach (var attribute in GetEntryAttributes(entryName))
            {
                builder.Append(attribute);
                builder.AppendLine();
            }

            builder.AppendLine();
        }

        private string GetManifest()
        {
            var result = MANIFEST_FULLNAME;

            if (!this.Keys.Contains(result))
            {
                InitializeDefaultManifest();
            }

            return result;
        }

        private void InitializeDefaultManifest()
        {
            this.Add(MANIFEST_FULLNAME, new List<ManifestAttribute>());

            this[MANIFEST_FULLNAME].Add(new ManifestAttribute() { Name = "Manifest-Version", Value = "1.0" });
        }
    }

    /// <summary>
    /// An extensions helper class for use with <see cref="ManifestAttributes"/>.
    /// </summary>
    public static class ManifestAttributesExtensions
    {
        /// <summary>
        /// Sets the value of the attribute with the specified name from the specified collection of attributes. If the specified collection
        /// of attributes does not have a matching attribute by name, a new one will be added.
        /// </summary>
        /// <param name="this">The list of attributes to search for an attribute with the specified name.</param>
        /// <param name="name">The name of the attribute being sought.</param>
        /// <param name="value">The value to use for the specified attribute.</param>
        /// <remarks>
        /// If the list of attributes does not contain an attribute with a matching name as specified by the name parameter, this method will add a new
        /// <see cref="ManifestAttribute"/> to the list and use the specified name and value for that attribute. If the collection does have an attribute
        /// with a matching name as specified by the name parameter, this method will update the existing attribute value. It is important to note, however,
        /// that this method will only use the first found attribute; therefore, if there are multiple attributes with identical names, only the first
        /// attribute will be updated and the others will be ignored.
        /// </remarks>
        public static void Put(this List<ManifestAttribute> @this, String name, String value)
        {
            var existing = @this.FirstOrDefault(x => x.Name == name);

            if (existing != null)
            {
                existing.Value = value;
            }
            else
            {
                @this.Add(new ManifestAttribute() { Name = name, Value = value });
            }
        }
    }
}
