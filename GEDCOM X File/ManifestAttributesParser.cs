using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    internal static class ManifestAttributesParser
    {
        public static IEnumerable<ManifestAttribute> Parse(ZipArchive zip)
        {
            var manifest = zip.GetEntry("META-INF/MANIFEST.MF");
            var result = new List<ManifestAttribute>();

            if (manifest != null)
            {
                using (var stream = new StreamReader(manifest.Open()))
                {
                    while (!stream.EndOfStream)
                    {
                        var attribute = Parse(stream.ReadLine());

                        if (attribute != null)
                        {
                            result.Add(attribute);
                        }
                    }
                }
            }

            return result;
        }

        private static ManifestAttribute Parse(string attribute)
        {
            ManifestAttribute result = null;

            if (!string.IsNullOrEmpty(attribute) && attribute.IndexOf(":") != -1)
            {
                var kvp = attribute.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (kvp != null && kvp.Length == 2 && kvp.All(x => !string.IsNullOrEmpty(x)))
                {
                    result = new ManifestAttribute();
                    result.Name = kvp[0].Trim();
                    result.Value = kvp[1].Trim();
                }
            }

            return result;
        }
    }
}
