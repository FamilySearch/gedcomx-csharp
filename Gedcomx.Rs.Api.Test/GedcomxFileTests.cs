using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;

using Gedcomx.File;

using KellermanSoftware.CompareNetObjects;

using NUnit.Framework;

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
                Assert.That(test, Is.Not.Null);
                Assert.That(test.Attributes, Is.Not.Null);
                Assert.That(test.Attributes, Has.Count.EqualTo(2));
                Assert.That(test.GetAttribute("Manifest-Version"), Is.Not.Null);
                Assert.That(test.Entries, Is.Not.Null);
                Assert.That(test.Entries.Count(), Is.GreaterThan(0));

                foreach (var entry in test.Entries)
                {
                    using (var stream = test.GetResourceStream(entry))
                    {
                        Assert.That(stream, Is.Not.Null);
                        Assert.That(stream.CanRead, Is.True);
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

            Assert.That(differences.Differences, Is.Empty);
        }

        [Test]
        public void TestManifestParsing()
        {
            var file = TestBacking.WriteBytesToDisk(Resources.SampleGEDX);
            var fi = new FileInfo(file);
            var results = ManifestAttributes.Parse(ZipFile.OpenRead(file));

            Assert.That(results, Is.Not.Null);
            Assert.That(results, Has.Count.EqualTo(4));
            Assert.That(results[results.Keys.Where(x => x == ManifestAttributes.MANIFEST_FULLNAME).Single()], Has.Count.EqualTo(2));
            Assert.That(results[results.Keys.Where(x => x == "tree.xml").Single()], Has.Count.EqualTo(3));
            Assert.That(results[results.Keys.Where(x => x == "person1.png").Single()], Has.Count.EqualTo(3));
            Assert.That(results[results.Keys.Where(x => x == "person2.png").Single()], Has.Count.EqualTo(3));
        }

        [Test]
        public void TestFileWriting()
        {
            string fileName;

            using (var ms = new MemoryStream())
            {
                var file = new GedcomxOutputStream(ms, new DefaultXmlSerialization());
                var gx = new Gx.Gedcomx
                {
                    Persons = new List<Gx.Conclusion.Person>()
                };
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
                Assert.That(zip.Entries, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void TestFileReading()
        {
            var file = TestBacking.WriteBytesToDisk(Resources.SampleGEDX);
            var fi = new FileInfo(file);
            using (GedcomxFile test = new GedcomxFile(fi))
            {
                Assert.That(test.Entries.Count(), Is.EqualTo(4));

                var gedxEntry = test.Entries.Where(x => x.ZipEntry.FullName == "tree.xml").Single();
                var imageEntry1 = test.Entries.Where(x => x.ZipEntry.FullName == "person1.png").Single();
                var imageEntry2 = test.Entries.Where(x => x.ZipEntry.FullName == "person2.png").Single();

                Assert.That(gedxEntry.GetAttribute("Content-Type"), Is.EqualTo("application/x-gedcomx-v1+xml"));
                Assert.That(imageEntry1.GetAttribute("Content-Type"), Is.EqualTo("image/png"));
                Assert.That(imageEntry2.GetAttribute("Content-Type"), Is.EqualTo("image/png"));

                var gedx = test.ReadResource<Gx.Gedcomx>(gedxEntry);
                var image1 = test.GetResourceStream(imageEntry1);
                var image2 = test.GetResourceStream(imageEntry2);

                Assert.That(gedx.Persons, Has.Count.EqualTo(4));
                Assert.That(gedx.Relationships, Has.Count.EqualTo(4));
                Assert.DoesNotThrow(() => new Bitmap(image1));
                Assert.DoesNotThrow(() => new Bitmap(image2));
            }
        }
    }
}
