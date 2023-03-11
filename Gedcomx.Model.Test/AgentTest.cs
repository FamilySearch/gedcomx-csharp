using System.Xml.Serialization;

using Gx.Agent;
using Gx.Common;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="Agent"/>
/// </summary>
[TestFixture]
public class AgentTest
{
    [Test]
    public void AgentEmpty()
    {
        Agent sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void AgentObjectInitialization()
    {
        Agent sut = new()
        {
            // ExtensibleData
            Id = "A-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // Agent
            Accounts = { new() },
            Addresses = { new() },
            Emails = { "example@example.org" },
            Homepage = new(),
            Identifiers = { new() },
            Names = { "Jane Doe" },
            Openid = new(),
            Phones = { new() }
        };

        Assert.That(sut.Names[0].Value, Is.EqualTo("Jane Doe"));
        Assert.That(sut.Emails[0].Resource, Is.EqualTo("mailto:example@example.org"));

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void SetAccountTest()
    {
        Agent agent = new();
        agent.Accounts.Add(new());
        agent.Addresses.Add(new());
        agent.Emails.Add(new ResourceReference());
        agent.Homepage = new();
        agent.Identifiers.Add(new());
        agent.Names.Add(new TextValue());
        agent.Openid = new();
        agent.Phones.Add(new());
        agent.Id = "id";
        agent.Links.Add(new());

        VerifyXmlSerialization(agent);
        VerifyJsonSerialization(agent);
    }

    private static void VerifyXmlSerialization(Agent sut)
    {
        XmlSerializer serializer = new(typeof(Agent));
        using MemoryStream stream = new();
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
