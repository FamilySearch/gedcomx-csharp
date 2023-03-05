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
    /// Test calss for <see cref="Name"/>
    /// </summary>
    [TestFixture]
    public class NameTest
    {
        [Test]
        public void NameEmpty()
        {
            var sut = new Name();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void NameFilled()
        {
            var sut = new Name
            {
                // ExtensibleData
                Id = "N-1",
                // Conclusion
                KnownConfidence = ConfidenceLevel.Medium,
                SortKey = "sort key",
                Lang = "lang",
                Attribution = new Attribution(),
                Sources = { new SourceReference(), new SourceDescription() { Id = "S-1" } },
                Analysis = new ResourceReference(),
                Notes = { new Note() },
                // Name
                KnownType = NameType.BirthName,
                Preferred = true,
                Date = new DateInfo(),
                NameForms = { new NameForm() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Name sut)
        {
            var serializer = new XmlSerializer(typeof(Name));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Name sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Name>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
