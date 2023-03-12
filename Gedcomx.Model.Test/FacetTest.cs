using System.Xml.Serialization;

using Gx.Records;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;

/// <summary>
/// Test calss for <see cref="Facet"/>
/// </summary>
[TestFixture]
public class FacetTEst
{
    [Test]
    public void FacetEmpty()
    {
        Facet sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void FacetObjectInitialization()
    {
        Facet sut = new()
        {
            // ExtensibleData
            Id = "F-1",
            // HypermediaEnabledData
            Links = { new(), { "rel", new Uri("https://www.familysearch.org/platform/collections/tree") }, { "rel", "template" } },
            // Facet
            KnownType = FacetType.Volume,
            Title = "title",
            Key = "key",
            Facets = { new() },
            Values = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(Facet sut)
    {
        XmlSerializer serializer = new(typeof(Facet));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(Facet sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Facet>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
