using System.Xml.Serialization;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Gx.Agent.OnlineAccount"/>
    /// </summary>
    [TestFixture]
    public class OnlineAccountTest
    {
        [Test]
        public void OnlineAccountEmpty()
        {
            var sut = new Gx.Agent.OnlineAccount();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void OnlineAccountFilled()
        {
            var sut = new Gx.Agent.OnlineAccount
            {
                Id = "O-1",
                AccountName = "Peter Pan",
                ServiceHomepage = new Gx.Common.ResourceReference()
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Gx.Agent.OnlineAccount sut)
        {
            var serializer = new XmlSerializer(typeof(Gx.Agent.OnlineAccount));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Gx.Agent.OnlineAccount sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Gx.Agent.OnlineAccount>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}

