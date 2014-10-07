using Gedcomx.File;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            using (GedcomxFile test = new GedcomxFile(fi))
            {
                Assert.IsNotNull(test);
                Assert.IsNotNull(test.Attributes);
                Assert.AreEqual(2, test.Attributes.Count);
                Assert.IsNotNull(test.GetAttribute("Manifest-Version"));
                Assert.IsNotNull(test.Entries);
                Assert.Greater(test.Entries.Count(), 0);

                foreach (var entry in test.Entries)
                {
                    using (var stream = test.GetResourceStream(entry))
                    {
                        Assert.IsNotNull(stream);
                        Assert.IsTrue(stream.CanRead);
                    }
                }
            }
        }

        [Test]
        public void TestXmlSerialization()
        {
            var serializer = new DefaultXmlSerialization();
            var gxExpected = TestBacking.GetGedcomxObject();

            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(gxExpected, ms);
                bytes = ms.ToArray();
            }

            var gxActual = serializer.Deserialize<Gx.Gedcomx>(new MemoryStream(bytes));

            var comparer = new CompareLogic();
            var differences = comparer.Compare(gxExpected, gxActual);

            Assert.AreEqual(0, differences.Differences.Count);
        }

        [Test]
        public void TestManifestParsing()
        {
            var file = TestBacking.WriteBytesToDisk(Resources.TestJar);
            var fi = new FileInfo(file);
            var results = ManifestAttributes.Parse(ZipFile.OpenRead(file));

            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Count);
            Assert.IsNull(results[results.Keys.Where(x => x == "META-INF/").Single()]);
            Assert.AreEqual(2, results[results.Keys.Where(x => x == ManifestAttributes.MANIFEST_FULLNAME).Single()].Count);
            Assert.AreEqual(3, results[results.Keys.Where(x => x == "Test1.txt").Single()].Count);
            Assert.AreEqual(3, results[results.Keys.Where(x => x == "Test4.txt").Single()].Count);
        }

        [Test]
        public void TestFileWriting()
        {
            string fileName;

            using (var ms = new MemoryStream())
            {
                var file = new GedcomxOutputStream(ms, new DefaultXmlSerialization());
                var gx = new Gx.Gedcomx();

                gx.Persons = new List<Gx.Conclusion.Person>();
                gx.Persons.Add(TestBacking.GetCreateMalePerson());

                file.AddResource(gx);

                file.Dispose();

                fileName = Path.GetTempFileName();
                var output = System.IO.File.OpenWrite(fileName);
                var bytes = ms.ToArray();
                output.Write(bytes, 0, bytes.Length);
                output.Flush();
                output.Close();
                output.Dispose();
            }

            // Verify basic reading of the zip file created above
            using (var zip = ZipFile.OpenRead(fileName))
            {
                Assert.AreEqual(2, zip.Entries.Count);
            }
        }
    }
}
