using Gedcomx.File;
using Gx.Common;
using Gx.Conclusion;
using Gx.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test.Examples
{
    [TestFixture(Category = "Examples")]
    public class NamesExampleTest
    {
        private DefaultXmlSerialization xmlSerializer;
        private DefaultJsonSerialization jsonSerializer;

        [OneTimeSetUp]
        public void Initialize()
        {
            xmlSerializer = new DefaultXmlSerialization();
            jsonSerializer = new DefaultJsonSerialization();
        }

        [Test]
        public void TestBasicWesternName()
        {
            NameForm nameForm = new NameForm("John Fitzgerald Kennedy")
              .SetLang("en")
              .SetPart(NamePartType.Given, "John")
              .SetPart(NamePartType.Given, "Fitzgerald")
              .SetPart(NamePartType.Surname, "Kennedy");
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestMultipleJapaneseForms()
        {
            NameForm kanji = new NameForm("山田太郎")
              .SetLang("ja-Hani")
              .SetPart(NamePartType.Surname, "山田")
              .SetPart(NamePartType.Given, "太郎");
            NameForm katakana = new NameForm("ヤマダタロー")
              .SetLang("ja-Kana")
              .SetPart(NamePartType.Surname, "ヤマダ")
              .SetPart(NamePartType.Given, "タロー");
            NameForm romanized = new NameForm("Yamada Tarō")
              .SetLang("ja-Latn")
              .SetPart(NamePartType.Surname, "Tarō")
              .SetPart(NamePartType.Given, "Yamada");
            Name name = new Name().SetNameForm(kanji).SetNameForm(katakana).SetNameForm(romanized);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestMultipleNamePartsOnePartPerType()
        {
            NameForm nameForm = new NameForm("José Eduardo Santos Tavares Melo Silva")
              .SetLang("pt-BR")
              .SetPart(NamePartType.Given, "José Eduardo")
              .SetPart(NamePartType.Surname, "Santos Tavares Melo Silva");
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestMultipleNamePartsMultiplePartsPerType()
        {
            NameForm nameForm = new NameForm("José Eduardo Santos Tavares Melo Silva")
              .SetLang("pt-BR")
              .SetPart(NamePartType.Given, "José")
              .SetPart(NamePartType.Given, "Eduardo")
              .SetPart(NamePartType.Surname, "Santos")
              .SetPart(NamePartType.Surname, "Tavares")
              .SetPart(NamePartType.Surname, "Melo")
              .SetPart(NamePartType.Surname, "Silva");
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestPatronymic()
        {
            NameForm nameForm = new NameForm("Björk Guðmundsdóttir")
              .SetLang("is")
              .SetPart(NamePartType.Given, "Björk")
              .SetPart(new NamePart().SetValue("Guðmundsdóttir").SetQualifier(new Qualifier(NamePartQualifierType.Patronymic)));
            Name name = new Name().SetNameForm(nameForm);
            
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }
    }
}
