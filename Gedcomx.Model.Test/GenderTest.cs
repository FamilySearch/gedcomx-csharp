using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Gx.Common;
using Gx.Conclusion;
using Gx.Source;

using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test;
/// <summary>
/// Test calss for <see cref="Gender"/>
/// </summary>
[TestFixture]
public class GenderTest
{
    [Test]
    public void GenderEmpty()
    {
        Gender sut = new();

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    [Test]
    public void GenderObjectInitialization()
    {
        Gender sut = new()
        {
            // ExtensibleData
            Id = "G-1",
            // Conclusion
            KnownConfidence = ConfidenceLevel.Medium,
            SortKey = "sort key",
            Lang = "lang",
            Attribution = new Attribution(),
            Sources = { new SourceReference(), new SourceDescription() { Id = "S-1" } },
            Analysis = new ResourceReference(),
            Notes = { new Note() },
            // Gender
            KnownType = GenderType.Female,
            Fields = { new() }
        };

        VerifyXmlSerialization(sut);
        VerifyJsonSerialization(sut);
    }

    private static void VerifyXmlSerialization(Gender sut)
    {
        XmlSerializer serializer = new(typeof(Gender));
        using MemoryStream stream = new();
        serializer.Serialize(stream, sut);

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        result.ShouldContain(sut);
    }

    private static void VerifyJsonSerialization(Gender sut)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Gender>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
    }
}
