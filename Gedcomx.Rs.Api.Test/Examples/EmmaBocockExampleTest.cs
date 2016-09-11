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
    public class EmmaBocockExampleTest
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
            Agent contributor = (Agent)new Agent().SetName("Jane Doe").SetEmail("example@example.org").SetId("A-1");
            Agent repository = (Agent)new Agent().SetName("General Registry Office, Southport").SetId("A-2");
            Attribution attribution = new Attribution().SetContributor(contributor).SetModified(DateTime.Parse("2014-03-07"));
            SourceDescription sourceDescription = (SourceDescription)new SourceDescription()
            .SetTitle("Birth Certificate of Emma Bocock, 23 July 1843, General Registry Office")
            .SetCitation(new SourceCitation().SetValue("England, birth certificate for Emma Bocock, born 23 July 1843; citing 1843 Birth in District and Sub-district of Ecclesall-Bierlow in the County of York, 303; General Registry Office, Southport."))
            .SetResourceType(ResourceType.PhysicalArtifact)
            .SetCreated(DateTime.Parse("1843-07-27"))
            .SetRepository(repository)
            .SetId("S-1");
            Fact birth = new Fact()
            .SetType(FactType.Birth)
            .SetDate(new DateInfo().SetOriginal("23 June 1843"))
            .SetPlace(new PlaceReference().SetOriginal("Broadfield Bar, Abbeydale Road, Ecclesall-Bierlow, York, England, United Kingdom"));
            Person emma = (Person)new Person().SetName("Emma Bocock").SetGender(GenderType.Female).SetFact(birth).SetExtracted(true).SetSource(sourceDescription).SetId("P-1");
            Person father = (Person)new Person().SetName("William Bocock").SetFact(new Fact().SetType(FactType.Occupation).SetValue("Toll Collector")).SetExtracted(true).SetSource(sourceDescription).SetId("P-2");
            Person mother = (Person)new Person().SetName("Sarah Bocock formerly Brough").SetExtracted(true).SetSource(sourceDescription).SetId("P-3");
            Relationship fatherRelationship = new Relationship().SetType(RelationshipType.ParentChild).SetPerson1(father).SetPerson2(emma);
            Relationship motherRelationship = new Relationship().SetType(RelationshipType.ParentChild).SetPerson1(mother).SetPerson2(emma);
            Document analysis = (Document)new Document().SetText("...Jane Doe's analysis document...").SetId("D-1");
            Person emmaConclusion = (Person)new Person().SetEvidence(emma).SetAnalysis(analysis).SetId("C-1");
            Gx.Gedcomx gx = new Gx.Gedcomx()
            .SetAgent(contributor)
            .SetAgent(repository)
            .SetAttribution(attribution)
            .SetSourceDescription(sourceDescription)
            .SetPerson(emma)
            .SetPerson(father)
            .SetPerson(mother)
            .SetRelationship(fatherRelationship)
            .SetRelationship(motherRelationship)
            .SetDocument(analysis)
            .SetPerson(emmaConclusion);
            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }
    }
}
