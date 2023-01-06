using System.Xml.Serialization;

using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Agent"/>
    /// </summary>
    [TestFixture]
    public class AgentTest
    {
        [Test]
        public void AgentEmpty()
        {
            var sut = new Agent();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void AgentFilled()
        {
            var sut = new Agent
            {
                Id = "O-1",
                Names = { "Jane Doe" },
                Emails = { "example@example.org" },
                Homepage = new ResourceReference(),
                Openid = new ResourceReference(),
            };
            sut.Accounts.Add(new OnlineAccount());
            sut.Addresses.Add(new Address());
            sut.Identifiers.Add(new Identifier());
            sut.Phones.Add(new ResourceReference());

            Assert.That(sut.Names[0].Value, Is.EqualTo("Jane Doe"));
            Assert.That(sut.Emails[0].Resource, Is.EqualTo("mailto:example@example.org"));

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Agent sut)
        {
            var serializer = new XmlSerializer(typeof(Agent));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            Assert.That(result, Does.Contain("<agent "));
            Assert.That(result, Does.Contain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""));
            Assert.That(result, Does.Contain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            Assert.That(result, Does.Contain("xmlns=\"http://gedcomx.org/v1/\""));
            Assert.That(result.Contains("id"), Is.EqualTo(sut.Id != null));
            Assert.That(result.Contains("<account"), Is.EqualTo(sut.AnyAccounts()));
            Assert.That(result.Contains("<address"), Is.EqualTo(sut.AnyAddresses()));
            Assert.That(result.Contains("<email"), Is.EqualTo(sut.AnyEmails()));
            Assert.That(result.Contains("<homepage"), Is.EqualTo(sut.Homepage != null));
            Assert.That(result.Contains("<identifier"), Is.EqualTo(sut.AnyIdentifiers()));
            Assert.That(result.Contains("<name"), Is.EqualTo(sut.AnyNames()));
            Assert.That(result.Contains("<openid"), Is.EqualTo(sut.Openid != null));
            Assert.That(result.Contains("<phone"), Is.EqualTo(sut.AnyPhones()));
        }

        private static void VerifyJsonSerialization(Agent sut)
        {
            JsonSerializerSettings jsonSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Agent>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }
    }
}
