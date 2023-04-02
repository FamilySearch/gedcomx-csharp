using System.Xml.Serialization;

using Gx.Records;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="RecordSet"/>
/// </summary>
[TestFixture]
public class RecordSetTest
{
    [Test]
    public void RecordSetEmpty()
    {
        RecordSet sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void RecordSetObjectInitialization()
    {
        RecordSet sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // RecordSet
            Lang = "lang",
            Metadata = new(),
            Records = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(RecordSet sut)
    {
        XmlSerializer serializer = new(typeof(RecordSet));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(RecordSet sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<RecordSet>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
