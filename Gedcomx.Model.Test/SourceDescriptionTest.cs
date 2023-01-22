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
    /// Test calss for <see cref="SourceDescription"/>
    /// </summary>
    [TestFixture]
    public class SourceDescriptionTest
    {
        [Test]
        public void GedcomxEmpty()
        {
            var sut = new SourceDescription();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void AgentObjectInitialization()
        {
            var sut = new SourceDescription
            {
                Id = "A-1",
                Links = { new Link(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
                About = "about text",
                Lang = "language",
                MediaType = "type of media",
                SortKey = "key to sort",
                KnownResourceType = ResourceType.PhysicalArtifact,
                Citations = { new SourceCitation().SetValue("citation") },
                Mediator = new ResourceReference(),
                Sources = { new SourceReference() },
                Analysis = new ResourceReference(),
                ComponentOf = new SourceReference(),
                Titles = { "first title" },
                TitleLabel = new TextValue("label of title"),
                Notes = { new Note().SetText("note") },
                Descriptions = { new TextValue("description") },
                Identifiers = { new Identifier() },
                Coverage = { new Coverage() },
                Rights = { "first rights" },
                Fields = { new Field() },
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(SourceDescription sut)
        {
            var serializer = new XmlSerializer(typeof(SourceDescription));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(SourceDescription sut)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings(); ;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<SourceDescription>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
