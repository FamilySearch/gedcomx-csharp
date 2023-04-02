using System.Xml.Serialization;

using Gx.Source;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="SourceReference"/>
/// </summary>
[TestFixture]
public class SourceReferenceTest
{
    [Test]
    public void SourceReferenceEmpty()
    {
        SourceReference sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void SourceReferenceObjectInitialization()
    {
        SourceReference sut = new()
        {
            // ExtensibleData
            Id = "SR-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // SourceReference
            DescriptionRef = "DescriptionRef",
            Attribution = new(),
            Qualifiers = { new() },
            Tags = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(SourceReference sut)
    {
        XmlSerializer serializer = new(typeof(SourceReference));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(SourceReference sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<SourceReference>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
