using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Test calss for <see cref="NameForm"/>
    /// </summary>
    [TestFixture]
    public class NameFormTest
    {
        [Test]
        public void NameFormEmpty()
        {
            var sut = new NameForm();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void NameFormFilled()
        {
            var sut = new NameForm
            {
                // ExtensibleData
                Id = "A-1",
                // NameForm
                Lang = "en",
                FullText = "John Fitzgerald Kennedy",
                Parts = { new NamePart(NamePartType.Given, "John") },
                Fields = { new Field() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(NameForm sut)
        {
            var serializer = new XmlSerializer(typeof(NameForm));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(NameForm sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<NameForm>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
