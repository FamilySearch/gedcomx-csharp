using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Source;

using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Document"/>
    /// </summary>
    [TestFixture]
    public class DocumentTest
    {
        [Test]
        public void DocumentEmpty()
        {
            var sut = new Document();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void DocumentFilled()
        {
            var sut = new Document
            {
                // ExtensibleData
                Id = "F-1",
                // Conclusion
                KnownConfidence = ConfidenceLevel.Medium,
                SortKey = "sort key",
                Lang = "lang",
                Attribution = new Attribution(),
                Sources = { new SourceReference(), new SourceDescription() { Id = "S-1" } },
                Analysis = new ResourceReference(),
                Notes = { new Note() },
                // Document
                TextType = "text type",
                Extracted = true,
                KnownType = DocumentType.Transcription,
                Text = "text"
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Document sut)
        {
            var serializer = new XmlSerializer(typeof(Document));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Document sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Document>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
