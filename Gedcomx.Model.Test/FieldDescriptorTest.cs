using System.Xml.Serialization;

using Gx.Records;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="FieldDescriptor"/>
/// </summary>
[TestFixture]
public class FieldDescriptorTest
{
    [Test]
    public void FieldDescriptorEmpty()
    {
        FieldDescriptor sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void FieldDescriptorObjectInitialization()
    {
        FieldDescriptor sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // FieldDescriptor
            OriginalLabel = "originalLabel",
            Descriptions = { new() },
            Values = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(FieldDescriptor sut)
    {
        XmlSerializer serializer = new(typeof(FieldDescriptor));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(FieldDescriptor sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<FieldDescriptor>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
