using Gedcomx.File;
using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Source;
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
    public class GeorgeMarthaWashingtonExampleTest
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
        public void TestExample()
        {
            PlaceDescription popesCreek = CreatePopesCreek();
            PlaceDescription mountVernon = CreateMountVernon();
            PlaceDescription chestnutGrove = CreateChestnutGrove();
            Person george = CreateGeorge(popesCreek, mountVernon);
            Person martha = CreateMartha(chestnutGrove, mountVernon);
            Relationship marriage = CreateMarriage(george, martha);
            List<SourceDescription> sources = CiteGeorgeMarthaAndMarriage(george, martha, marriage);
            Agent contributor = CreateContributor();
            Gx.Gedcomx gx = new Gx.Gedcomx();
            gx.Persons = new List<Person>() { george, martha };
            gx.SetRelationship(marriage);
            gx.SourceDescriptions = sources;
            gx.SetAgent(contributor);
            gx.SetAttribution(new Attribution());
            gx.Attribution.SetContributor(new ResourceReference());
            gx.Attribution.Contributor.SetResource("#" + contributor.Id);
            gx.Places = new List<PlaceDescription>() { popesCreek, mountVernon, chestnutGrove };

            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        private PlaceDescription CreatePopesCreek()
        {
            PlaceDescription place = new PlaceDescription();
            place.SetId("888");
            place.SetLatitude(38.192353);
            place.SetLongitude(-76.904069);
            place.SetName("Pope's Creek, Westmoreland, Virginia, United States");
            return place;
        }

        private PlaceDescription CreateMountVernon()
        {
            PlaceDescription place = new PlaceDescription();
            place.SetId("999");
            place.SetLatitude(38.721144);
            place.SetLongitude(-77.109461);
            place.SetName("Mount Vernon, Fairfax County, Virginia, United States");
            return place;
        }

        private PlaceDescription CreateChestnutGrove()
        {
            PlaceDescription place = new PlaceDescription();
            place.SetId("KKK");
            place.SetLatitude(37.518304);
            place.SetLongitude(-76.984148);
            place.SetName("Chestnut Grove, New Kent, Virginia, United States");
            return place;
        }

        private Agent CreateContributor()
        {
            Agent agent = new Agent();
            agent.SetId("GGG-GGGG");
            agent.SetName("Ryan Heaton");
            return agent;
        }

        private Person CreateGeorge(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            Person person = new Person();
            person.SetGender(new Gender(GenderType.Male));

            Fact fact = new Fact();
            fact.SetId("123");
            fact.SetType(FactType.Birth);

            fact.SetDate(new DateInfo());
            fact.Date.SetOriginal("February 22, 1732");
            fact.Date.SetFormal("+1732-02-22");

            fact.SetPlace(new PlaceReference());
            fact.Place.SetOriginal(birthPlace.Names[0].Value.ToLower());
            fact.Place.DescriptionRef = "#" + birthPlace.Id;

            person.AddFact(fact);

            fact = new Fact();
            fact.SetId("456");
            fact.SetType(FactType.Death);

            fact.SetDate(new DateInfo());
            fact.Date.SetOriginal("December 14, 1799");
            fact.Date.SetFormal("+1799-12-14T22:00:00");

            fact.SetPlace(new PlaceReference());
            fact.Place.SetOriginal(deathPlace.Names[0].Value.ToLower());
            fact.Place.DescriptionRef = "#" + deathPlace.Id;

            person.AddFact(fact);

            List<Name> names = new List<Name>();
            Name name = new Name();
            NameForm nameForm = new NameForm();
            nameForm.SetFullText("George Washington");
            List<NamePart> parts = new List<NamePart>();
            NamePart part = new NamePart();
            part.SetType(NamePartType.Given);
            part.SetValue("George");
            parts.Add(part);
            part = new NamePart();
            part.SetType(NamePartType.Surname);
            part.SetValue("Washington");
            parts.Add(part);
            nameForm.Parts = parts;
            name.SetNameForm(nameForm);
            name.SetId("789");
            names.Add(name);
            person.Names = names;

            person.SetId("BBB-BBBB");

            return person;
        }

        private Person CreateMartha(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            Person person = new Person();
            person.SetGender(new Gender(GenderType.Male));

            Fact fact = new Fact();
            fact.SetId("321");
            fact.SetType(FactType.Birth);

            fact.SetDate(new DateInfo());
            fact.Date.SetOriginal("June 2, 1731");
            fact.Date.SetFormal("+1731-06-02");

            fact.SetPlace(new PlaceReference());
            fact.Place.SetOriginal(birthPlace.Names[0].Value.ToLower());
            fact.Place.DescriptionRef = "#" + birthPlace.Id;

            person.AddFact(fact);

            fact = new Fact();
            fact.SetId("654");
            fact.SetType(FactType.Death);

            fact.SetDate(new DateInfo());
            fact.Date.SetOriginal("May 22, 1802");
            fact.Date.SetFormal("+1802-05-22");

            fact.SetPlace(new PlaceReference());
            fact.Place.SetOriginal(deathPlace.Names[0].Value.ToLower());
            fact.Place.DescriptionRef = "#" + deathPlace.Id;

            person.AddFact(fact);

            List<Name> names = new List<Name>();
            Name name = new Name();
            NameForm nameForm = new NameForm();
            nameForm.SetFullText("Martha Dandridge Custis");
            List<NamePart> parts = new List<NamePart>();
            NamePart part = new NamePart();
            part.SetType(NamePartType.Given);
            part.SetValue("Martha Dandridge");
            parts.Add(part);
            part = new NamePart();
            part.SetType(NamePartType.Surname);
            part.SetValue("Custis");
            parts.Add(part);
            nameForm.Parts = parts;
            name.SetNameForm(nameForm);
            name.SetId("987");
            names.Add(name);
            person.Names = names;

            person.SetId("CCC-CCCC");

            return person;
        }

        private Relationship CreateMarriage(Person george, Person martha)
        {
            Relationship relationship = new Relationship();
            relationship.SetId("DDD-DDDD");
            relationship.SetPerson1(new ResourceReference("#" + george.Id));
            relationship.SetPerson2(new ResourceReference("#" + martha.Id));
            Fact marriage = new Fact();
            marriage.SetDate(new DateInfo());
            marriage.Date.SetOriginal("January 6, 1759");
            marriage.Date.SetFormal("+01-06-1759");
            marriage.SetPlace(new PlaceReference());
            marriage.Place.SetOriginal("White House Plantation");
            relationship.SetFact(marriage);
            return relationship;
        }

        private List<SourceDescription> CiteGeorgeMarthaAndMarriage(Person george, Person martha, Relationship relationship)
        {
            SourceDescription georgeSource = new SourceDescription();
            georgeSource.SetId("EEE-EEEE");
            georgeSource.SetAbout("http://en.wikipedia.org/wiki/George_washington");
            SourceCitation georgeCitation = new SourceCitation();
            georgeCitation.SetValue("\"George Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012.");
            georgeSource.SetCitation(georgeCitation);

            SourceDescription marthaSource = new SourceDescription();
            marthaSource.SetId("FFF-FFFF");
            marthaSource.SetAbout("http://en.wikipedia.org/wiki/Martha_washington");
            SourceCitation marthaCitation = new SourceCitation();
            marthaCitation.SetValue("\"Martha Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012.");
            marthaSource.SetCitation(marthaCitation);

            SourceReference reference = new SourceReference();
            reference.SetDescriptionRef("#" + georgeSource.Id);
            george.SetSource(reference);

            reference = new SourceReference();
            reference.SetDescriptionRef("#" + marthaSource.Id);
            martha.SetSource(reference);

            relationship.SetSource(reference);

            return new List<SourceDescription>() { georgeSource, marthaSource };
        }
    }
}
