using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Records;

using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="NamePart"/>
    /// </summary>
    [TestFixture]
    public class NamePartTest
    {
        [Test]
        public void NamePartEmpty()
        {
            var sut = new NamePart();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void NamePartFilled()
        {
            var sut = new NamePart
            {
                // ExtensibleData
                Id = "A-1",
                // NamePart
                Value = "John",
                KnownType = NamePartType.Given,
                Fields = { new Field() },
                Qualifiers = { new Qualifier() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(NamePart sut)
        {
            var serializer = new XmlSerializer(typeof(NamePart));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(NamePart sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<NamePart>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
