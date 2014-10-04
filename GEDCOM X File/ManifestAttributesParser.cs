using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    public static class ManifestAttributesParser
    {
        public static Dictionary<ZipArchiveEntry, List<ManifestAttribute>> Parse(ZipArchive zip)
        {
            var result = new Dictionary<ZipArchiveEntry, List<ManifestAttribute>>();
            var manifest = zip.GetEntry("META-INF/MANIFEST.MF");
            var tempAttributes = new List<ManifestAttribute>();
            var mainAttributesParsed = false;
            ZipArchiveEntry entry = null;

            if (zip != null && zip.Entries != null)
            {
                foreach (var file in zip.Entries)
                {
                    result.Add(file, null);
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
                                    result[entry] = tempAttributes.ToList(); // Make a copy so the reference won't clear this assignment
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
    }
}
