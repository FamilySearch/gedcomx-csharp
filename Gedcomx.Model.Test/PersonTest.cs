using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Records;
using Gx.Source;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Person"/>
    /// </summary>
    [TestFixture]
    public class PersonTest
    {
        [Test]
        public void PersonEmpty()
        {
            var sut = new Person();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void PersonObjectInitialization()
        {
            var sut = new Person
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
                // Person
                Principal = false,
                Private = true,
                Living = true,
                Gender = new Gender(GenderType.Female),
                Names = { "Emma Bocock" },
                Facts = { new Fact() },
                Fields = { new Field() },
                DisplayExtension = new DisplayProperties(),
                DiscussionReferences = { new DiscussionReference() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Person sut)
        {
            var serializer = new XmlSerializer(typeof(Person));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Person sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Person>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
