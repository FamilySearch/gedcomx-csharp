using System.Xml.Serialization;

using Gx.Records;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="FieldValueDescriptor"/>
/// </summary>
[TestFixture]
public class FieldValueDescriptorTest
{
    [Test]
    public void FieldValueDescriptorEmpty()
    {
        FieldValueDescriptor sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void FieldValueDescriptorObjectInitialization()
    {
        FieldValueDescriptor sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // FieldValueDescriptor
            Optional = true,
            KnownType = FieldValueType.Original,
            LabelId = "labelId",
            DisplayLabels = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(FieldValueDescriptor sut)
    {
        XmlSerializer serializer = new(typeof(FieldValueDescriptor));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(FieldValueDescriptor sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<FieldValueDescriptor>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
