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
                // ExtensibleData
                Id = "A-1",
                // HypermediaEnabledData
                Links = { new Link(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
                // Agent
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
            result.ShouldContain(sut);
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
