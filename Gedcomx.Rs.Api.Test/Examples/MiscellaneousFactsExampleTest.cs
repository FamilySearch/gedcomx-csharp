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
    public class MiscellaneousFactsExampleTest
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
        public void TestCensusAndResidenceLikeFacts()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.Census, "...", "..."))
              .SetFact(new Fact(FactType.Emigration, "...", "..."))
              .SetFact(new Fact(FactType.Immigration, "...", "..."))
              .SetFact(new Fact(FactType.LandTransaction, "...", "..."))
              .SetFact(new Fact(FactType.MoveTo, "...", "..."))
              .SetFact(new Fact(FactType.MoveFrom, "...", "..."))
              .SetFact(new Fact(FactType.Residence, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestMilitaryServiceFacts()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.MilitaryAward, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryDischarge, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryDraftRegistration, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryInduction, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryService, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestEducationAndOccupationFacts()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.Apprenticeship, "...", "..."))
              .SetFact(new Fact(FactType.Education, "...", "..."))
              .SetFact(new Fact(FactType.Occupation, "...", "..."))
              .SetFact(new Fact(FactType.Retirement, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestReligiousOrCulturalFacts()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.AdultChristening, "...", "..."))
              .SetFact(new Fact(FactType.Baptism, "...", "..."))
              .SetFact(new Fact(FactType.BarMitzvah, "...", "..."))
              .SetFact(new Fact(FactType.BatMitzvah, "...", "..."))
              .SetFact(new Fact(FactType.Caste, "...", "..."))
              .SetFact(new Fact(FactType.Christening, "...", "..."))
              .SetFact(new Fact(FactType.Circumcision, "...", "..."))
              .SetFact(new Fact(FactType.Clan, "...", "..."))
              .SetFact(new Fact(FactType.Confirmation, "...", "..."))
              .SetFact(new Fact(FactType.Excommunication, "...", "..."))
              .SetFact(new Fact(FactType.FirstCommunion, "...", "..."))
              .SetFact(new Fact(FactType.Nationality, "...", "..."))
              .SetFact(new Fact(FactType.Ordination, "...", "..."))
              .SetFact(new Fact(FactType.Religion, "...", "..."))
              .SetFact(new Fact(FactType.Yahrzeit, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestFactQualifiers()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.Christening, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Religion, "Catholic")))
              .SetFact(new Fact(FactType.Census, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Age, "44")))
              .SetFact(new Fact(FactType.Death, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Cause, "Heart failure")));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestCustomFact()
        {
            Person person = new Person()
              .SetFact(new Fact().SetType("data:,Eagle%20Scout").SetPlace(new PlaceReference().SetOriginal("...")).SetDate(new DateInfo().SetOriginal("...")));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }

        [Test]
        public void TestRelationshipFacts()
        {
            Relationship couple = new Relationship()
              .SetType(RelationshipType.Couple)
              .SetFact(new Fact(FactType.CivilUnion, "...", "..."))
              .SetFact(new Fact(FactType.DomesticPartnership, "...", "..."))
              .SetFact(new Fact(FactType.Divorce, "...", "..."))
              .SetFact(new Fact(FactType.Marriage, "...", "..."))
              .SetFact(new Fact(FactType.MarriageBanns, "...", "..."))
              .SetFact(new Fact(FactType.MarriageContract, "...", "..."))
              .SetFact(new Fact(FactType.MarriageLicense, "...", "..."));

            Relationship parentChild = new Relationship()
              .SetType(RelationshipType.ParentChild)
              .SetFact(new Fact(FactType.AdoptiveParent, "...", "..."))
              .SetFact(new Fact(FactType.BiologicalParent, "...", "..."))
              .SetFact(new Fact(FactType.FosterParent, "...", "..."))
              .SetFact(new Fact(FactType.GuardianParent, "...", "..."))
              .SetFact(new Fact(FactType.StepParent, "...", "..."));

            Gx.Gedcomx gx = new Gx.Gedcomx().SetRelationship(couple).SetRelationship(parentChild);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }
    }
}
