using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Source;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="PlaceDescription"/>
    /// </summary>
    [TestFixture]
    public class PlaceDescriptionTest
    {
        [Test]
        public void PlaceDescriptionEmpty()
        {
            var sut = new PlaceDescription();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void PersonObjectInitialization()
        {
            var sut = new PlaceDescription
            {
                // ExtensibleData
                Id = "A-1",
                // HypermediaEnabledData
                Links = { new Link(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
                // Conclusion
                KnownConfidence = ConfidenceLevel.Medium,
                SortKey = "sortKey",
                Lang = "lang",
                Attribution = new Attribution(),
                Sources = { new SourceDescription { Id = "S-1" }, new SourceReference() },
                Analysis = new ResourceReference(),
                Notes = { new Note() },
                // Subject
                Extracted = true,
                Evidence = { new EvidenceReference() },
                Media = { new SourceReference(), new SourceDescription() { Id = "D-1" } },
                Identifiers = { new Identifier() },
                // PlaceDescription
                Type = "type",
                Names = { new TextValue(), "textValue" },
                TemporalDescription = new DateInfo(),
                Latitude = 38.192353,
                Longitude = -76.904069,
                SpatialDescription = new ResourceReference(),
                Place = new ResourceReference(),
                Jurisdiction = new ResourceReference(),
                DisplayExtension = new PlaceDisplayProperties()
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(PlaceDescription sut)
        {
            var serializer = new XmlSerializer(typeof(PlaceDescription));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(PlaceDescription sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<PlaceDescription>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
