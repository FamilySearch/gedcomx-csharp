using Gedcomx.File;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class GedcomxFileTests
    {
        [Test]
        public void TestReadingJarFile()
        {
            var file = TestBacking.WriteBytesToDisk(Resources.TestJar);
            var fi = new FileInfo(file);
            GedcomxFile test = new GedcomxFile(fi);

            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Attributes);
            Assert.AreEqual(2, test.Attributes.Count);
        }
    }
}
