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
    /// Test calss for <see cref="Event"/>
    /// </summary>
    [TestFixture]
    public class EventTest
    {
        [Test]
        public void EventEmpty()
        {
            var sut = new Event();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void EventFilled()
        {
            var sut = new Event
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
                // Event
                KnownType = EventType.Adoption,
                Date = new DateInfo(),
                Place = new PlaceReference(),
                Roles = { new EventRole() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Event sut)
        {
            var serializer = new XmlSerializer(typeof(Event));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Event sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
