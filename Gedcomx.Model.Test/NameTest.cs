using System.Xml.Serialization;

using Gx.Conclusion;
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
            Name sut = new();

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
                Attribution = new(),
                Sources = { new(), new() { Id = "S-1" } },
                Analysis = new(),
                Notes = { new() },
                // Name
                KnownType = NameType.BirthName,
                Preferred = true,
                Date = new(),
                NameForms = { new() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Name sut)
        {
            XmlSerializer serializer = new(typeof(Name));
            using MemoryStream stream = new();
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
