using System.Xml.Serialization;

using Gx.Records;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="Collection"/>
/// </summary>
[TestFixture]
public class CollectionTest
{
    [Test]
    public void CollectionEmpty()
    {
        Collection sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void CollectionObjectInitialization()
    {
        Collection sut = new()
        {
            // ExtensibleData
            Id = "A-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // Collection
            Lang = "en",
            Title = "title",
            Size = 5,
            Content = { new() },
            Attribution = new()
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(Collection sut)
    {
        XmlSerializer serializer = new(typeof(Collection));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(Collection sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Collection>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
