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
    public class SamuelHamExampleTest
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
            //Jane Doe, the researcher.
            Agent janeDoe = (Agent)new Agent().SetName("Jane Doe").SetEmail("example@example.org").SetId("A-1");

            //Lin Yee Chung Cemetery
            Agent fhl = (Agent)new Agent().SetName("Family History Library").SetAddress(new Address().SetCity("Salt Lake City").SetStateOrProvince("Utah")).SetId("A-2");

            //The attribution for this research.
            Attribution researchAttribution = new Attribution().SetContributor(janeDoe).SetModified(DateTime.Parse("2014-04-25"));

            //The parish register.
            SourceDescription recordDescription = (SourceDescription)new SourceDescription()
              .SetTitle("Marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England")
              .SetDescription("Marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837.")
              .SetCitation(new SourceCitation().SetValue("Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86."))
              .SetResourceType(ResourceType.PhysicalArtifact)
              .SetRepository(fhl)
              .SetId("S-1");

            //The transcription of the grave stone.
            Document transcription = (Document)new Document()
              .SetType(DocumentType.Transcription)
              .SetText("Samuel Ham of the parish of Honiton and Elizabeth Spiller\n" +
                      "were married this 3rd day of November 1828 by David Smith\n" +
                      "Stone, Pl Curate,\n" +
                      "In the Presence of\n" +
                      "Jno Pain.\n" +
                      "R.G. Halls.  Peggy Hammet.\n" +
                      "No. 86.")
              .SetSource(recordDescription)
              .SetLang("en")
              .SetId("D-1");

            //The transcription described as a source.
            SourceDescription transcriptionDescription = (SourceDescription)new SourceDescription()
              .SetAbout("#" + transcription.Id)
              .SetTitle("Transcription of marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England")
              .SetDescription("Transcription of marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837.")
              .SetCitation(new SourceCitation().SetValue("Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86."))
              .SetResourceType(ResourceType.DigitalArtifact)
              .SetSource(new SourceReference().SetDescription(recordDescription))
              .SetId("S-2");

            //the marriage fact.
            Fact marriage = new Fact()
              .SetType(FactType.Marriage)
              .SetDate(new DateInfo().SetOriginal("3 November 1828").SetFormal("+1828-11-03"))
              .SetPlace(new PlaceReference().SetOriginal("Wilton St George, Wilton, Somerset, England"));

            //the groom's residence.
            Fact samsResidence = new Fact()
              .SetType(FactType.Residence)
              .SetDate(new DateInfo().SetOriginal("3 November 1828").SetFormal("+1828-11-03"))
              .SetPlace(new PlaceReference().SetOriginal("parish of Honiton, Devon, England"));

            //the groom's residence.
            Fact lizsResidence = new Fact()
              .SetType(FactType.Residence)
              .SetDate(new DateInfo().SetOriginal("3 November 1828").SetFormal("+1828-11-03"))
              .SetPlace(new PlaceReference().SetOriginal("parish of Wilton, Somerset, England"));

            //the groom
            Person sam = (Person)new Person().SetName("Samuel Ham").SetGender(GenderType.Male).SetFact(samsResidence).SetExtracted(true).SetSource(transcriptionDescription).SetId("P-1");

            //the bride.
            Person liz = (Person)new Person().SetName("Elizabeth Spiller").SetGender(GenderType.Female).SetFact(lizsResidence).SetExtracted(true).SetSource(transcriptionDescription).SetId("P-2");

            //witnesses
            Person witness1 = (Person)new Person().SetName("Jno. Pain").SetExtracted(true).SetSource(transcriptionDescription).SetId("P-3");
            Person witness2 = (Person)new Person().SetName("R.G. Halls").SetExtracted(true).SetSource(transcriptionDescription).SetId("P-4");
            Person witness3 = (Person)new Person().SetName("Peggy Hammet").SetExtracted(true).SetSource(transcriptionDescription).SetId("P-5");

            //officiator
            Person officiator = (Person)new Person().SetName("David Smith Stone").SetExtracted(true).SetSource(transcriptionDescription).SetId("P-6");

            //the relationship.
            Relationship marriageRelationship = (Relationship)new Relationship().SetType(RelationshipType.Couple).SetPerson1(sam).SetPerson2(liz).SetFact(marriage).SetExtracted(true);

            //the marriage event
            Event marriageEvent = (Event)new Event(EventType.Marriage)
              .SetDate(new DateInfo().SetOriginal("3 November 1828").SetFormal("+1828-11-03"))
              .SetPlace(new PlaceReference().SetOriginal("Wilton St George, Wilton, Somerset, England"))
              .SetRole(new EventRole().SetPerson(sam).SetType(EventRoleType.Principal))
              .SetRole(new EventRole().SetPerson(liz).SetType(EventRoleType.Principal))
              .SetRole(new EventRole().SetPerson(witness1).SetType(EventRoleType.Witness))
              .SetRole(new EventRole().SetPerson(witness2).SetType(EventRoleType.Witness))
              .SetRole(new EventRole().SetPerson(witness3).SetType(EventRoleType.Witness))
              .SetRole(new EventRole().SetPerson(officiator).SetType(EventRoleType.Official))
              .SetExtracted(true)
              .SetId("E-1");

            //Jane Doe's analysis.
            Document analysis = (Document)new Document().SetText("...Jane Doe's analysis document...").SetId("D-2");

            //Jane Doe's conclusions about a person.
            Person samConclusion = (Person)new Person().SetEvidence(sam).SetAnalysis(analysis).SetId("C-1");

            Gx.Gedcomx gx = new Gx.Gedcomx()
              .SetAgent(janeDoe)
              .SetAgent(fhl)
              .SetAttribution(researchAttribution)
              .SetSourceDescription(recordDescription)
              .SetDocument(transcription)
              .SetSourceDescription(transcriptionDescription)
              .SetPerson(sam)
              .SetPerson(liz)
              .SetPerson(witness1)
              .SetPerson(witness2)
              .SetPerson(witness3)
              .SetPerson(officiator)
              .SetRelationship(marriageRelationship)
              .SetEvent(marriageEvent)
              .SetDocument(analysis)
              .SetPerson(samConclusion);

            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }
    }
}
