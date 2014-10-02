using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Gedcomx.File
{
    public class GedcomxFileEntry
    {
        public GedcomxFileEntry(ZipArchiveEntry entry)
        {
            ZipEntry = entry;
        }

        public ZipArchiveEntry ZipEntry { get; private set; }
    }
}
