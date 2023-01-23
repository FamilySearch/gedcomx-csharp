using System.Xml.Serialization;

using Gx.Agent;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="OnlineAccount"/>
    /// </summary>
    [TestFixture]
    public class OnlineAccountTest
    {
        [Test]
        public void OnlineAccountEmpty()
        {
            var sut = new OnlineAccount();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void OnlineAccountFilled()
        {
            var sut = new OnlineAccount
            {
                // ExtensibleData
                Id = "O-1",
                // OnlineAccount
                AccountName = "Peter Pan",
                ServiceHomepage = new Gx.Common.ResourceReference()
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(OnlineAccount sut)
        {
            var serializer = new XmlSerializer(typeof(OnlineAccount));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(OnlineAccount sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<OnlineAccount>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}

