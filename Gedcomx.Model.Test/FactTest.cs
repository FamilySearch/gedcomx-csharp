using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Source;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Fact"/>
    /// </summary>
    [TestFixture]
    public class FactTest
    {
        [Test]
        public void FactEmpty()
        {
            var sut = new Fact();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void FactFilled()
        {
            var sut = new Fact
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
                // Fact
                Primary = true,
                KnownType = FactType.Adoption,
                Date = new DateInfo(),
                Place = new PlaceReference(),
                Value = "value",
                Qualifiers = { new Qualifier() },
                Fields = { new Field() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Fact sut)
        {
            var serializer = new XmlSerializer(typeof(Fact));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Fact sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Fact>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
