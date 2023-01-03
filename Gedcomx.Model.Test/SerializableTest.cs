using System;
using System.Reflection;
using System.Xml.Serialization;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Genealogy.Model.Test;

public class SerializableTest
{
    private JsonSerializerSettings _jsonSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };


    [Test]
    [TestCaseSource(nameof(TestSerializableTypes))]
    public void Serialize(Type type)
    {
        var sut = Activator.CreateInstance(type);
        Assert.That(sut, Is.Not.Null);
        VerifyXmlSerialization(sut, type);
        VerifyJsonSerialization(sut, type);
    }

    private static void VerifyXmlSerialization(object sut, Type type)
    {
        var serializer = new XmlSerializer(type);
        using var stream = new MemoryStream();
        Assert.DoesNotThrow(() => serializer.Serialize(stream, sut));

        stream.Seek(0, SeekOrigin.Begin);
        var result = new StreamReader(stream).ReadToEnd();
        //Assert.That(result, Does.Contain("<" + type.Name + " ").IgnoreCase);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.DoesNotThrow(() => serializer.Deserialize(stream));
    }

    private void VerifyJsonSerialization(object sut, Type type)
    {
        Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(JsonConvert.SerializeObject(sut, _jsonSettings), type, _jsonSettings));
    }

    private static IEnumerable<TestCaseData> TestSerializableTypes()
    {
        var types = from t in Assembly.Load("Gedcomx.Model").GetTypes()
                    where t.IsClass && t.IsPublic && !t.IsAbstract && ((t.Attributes & TypeAttributes.Serializable) != 0)
                    select t;
        return from type in types
               select new TestCaseData(type).SetName($"Serialize({type.Name})");
        ;
    }
}
