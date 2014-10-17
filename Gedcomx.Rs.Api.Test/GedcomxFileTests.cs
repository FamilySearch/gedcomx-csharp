using Gedcomx.File;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var file = TestBacking.WriteBytesToDisk(Resources.SampleGEDX);
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
            var gxExpected = TestBacking.GetGedcomxObjectForDeepCompare();

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
            var file = TestBacking.WriteBytesToDisk(Resources.SampleGEDX);
            var fi = new FileInfo(file);
            var results = ManifestAttributes.Parse(ZipFile.OpenRead(file));

            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(2, results[results.Keys.Where(x => x == ManifestAttributes.MANIFEST_FULLNAME).Single()].Count);
            Assert.AreEqual(3, results[results.Keys.Where(x => x == "tree.xml").Single()].Count);
            Assert.AreEqual(3, results[results.Keys.Where(x => x == "person1.png").Single()].Count);
            Assert.AreEqual(3, results[results.Keys.Where(x => x == "person2.png").Single()].Count);
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

        [Test]
        public void TestFileReading()
        {
            var file = TestBacking.WriteBytesToDisk(Resources.SampleGEDX);
            var fi = new FileInfo(file);
            using (GedcomxFile test = new GedcomxFile(fi))
            {
                Assert.AreEqual(4, test.Entries.Count());

                var gedxEntry = test.Entries.Where(x => x.ZipEntry.FullName == "tree.xml").Single();
                var imageEntry1 = test.Entries.Where(x => x.ZipEntry.FullName == "person1.png").Single();
                var imageEntry2 = test.Entries.Where(x => x.ZipEntry.FullName == "person2.png").Single();

                Assert.AreEqual("application/x-gedcomx-v1+xml", gedxEntry.GetAttribute("Content-Type"));
                Assert.AreEqual("image/png", imageEntry1.GetAttribute("Content-Type"));
                Assert.AreEqual("image/png", imageEntry2.GetAttribute("Content-Type"));

                var gedx = test.ReadResource<Gx.Gedcomx>(gedxEntry);
                var image1 = test.GetResourceStream(imageEntry1);
                var image2 = test.GetResourceStream(imageEntry2);

                Assert.AreEqual(4, gedx.Persons.Count);
                Assert.AreEqual(4, gedx.Relationships.Count);
                Assert.DoesNotThrow(() => new Bitmap(image1));
                Assert.DoesNotThrow(() => new Bitmap(image2));
            }
        }
    }
}
