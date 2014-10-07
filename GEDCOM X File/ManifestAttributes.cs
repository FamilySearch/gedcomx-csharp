using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    public class ManifestAttributes : Dictionary<string, List<ManifestAttribute>>
    {
        public const string MANIFEST_FOLDER = "META-INF";
        public const string MANIFEST_FILE = "MANIFEST.MF";
        public static readonly string MANIFEST_FULLNAME = MANIFEST_FOLDER + "/" + MANIFEST_FILE;
        private ZipArchive zip;

        public ManifestAttributes(ZipArchive zipArchive)
        {
            zip = zipArchive;
        }

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

        public List<ManifestAttribute> MainAttributes
        {
            get
            {
                return GetEntryAttributes(MANIFEST_FULLNAME);
            }
        }

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

    public static class ManifestAttributesExtensions
    {
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
