using System.Xml.Serialization;

using Gx.Records;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="RecordDescriptor"/>
/// </summary>
[TestFixture]
public class RecordDescriptorTest
{
    [Test]
    public void RecordDescriptorEmpty()
    {
        RecordDescriptor sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void RecordDescriptorObjectInitialization()
    {
        RecordDescriptor sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // RecordDescriptor
            Lang = "lang",
            Fields = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(RecordDescriptor sut)
    {
        XmlSerializer serializer = new(typeof(RecordDescriptor));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(RecordDescriptor sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<RecordDescriptor>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
