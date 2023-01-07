using System.Xml.Serialization;

using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Links;

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
        public void AgentObjectInitialization()
        {
            var sut = new Agent
            {
                Id = "A-1",
                Links = { new Link(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
                Accounts = { new OnlineAccount() },
                Addresses = { new Address() },
                Emails = { "example@example.org" },
                Homepage = new ResourceReference(),
                Identifiers = { new Identifier() },
                Names = { "Jane Doe" },
                Openid = new ResourceReference(),
                Phones = { new ResourceReference() },
            };

            Assert.That(sut.Names[0].Value, Is.EqualTo("Jane Doe"));
            Assert.That(sut.Emails[0].Resource, Is.EqualTo("mailto:example@example.org"));

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void SetAccountTest()
        {
            var agent = new Agent();
            agent.Accounts.Add(new OnlineAccount());
            agent.Addresses.Add(new Address());
            agent.Emails.Add(new ResourceReference());
            agent.Homepage = new ResourceReference();
            agent.Identifiers.Add(new Identifier());
            agent.Names.Add(new TextValue());
            agent.Openid = new ResourceReference();
            agent.Phones.Add(new ResourceReference());
            agent.Id = "id";
            agent.Links.Add(new Link());

            VerifyXmlSerialization(agent);
            VerifyJsonSerialization(agent);
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
            Assert.That(result.Contains("<link"), Is.EqualTo(sut.AnyLinks()));
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
