using System.Xml.Serialization;

using Gx.Records;

using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="Field"/>
/// </summary>
[TestFixture]
public class FieldTest
{
    [Test]
    public void FieldEmpty()
    {
        Field sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void FieldObjectInitialization()
    {
        Field sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // Field
            KnownType = FieldType.Abusua,
            Values = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(Field sut)
    {
        XmlSerializer serializer = new(typeof(Field));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(Field sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Field>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
