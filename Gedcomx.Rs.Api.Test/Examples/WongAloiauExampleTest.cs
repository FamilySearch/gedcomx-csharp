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
    public class WongAloiauExampleTest
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
            Agent cemetery = (Agent)new Agent().SetName("Lin Yee Chung Cemetery").SetAddress(new Address().SetCity("Honolulu").SetStateOrProvince("Hawaii")).SetId("A-2");

            //Hanyu Pinyin, the translator.
            Agent hanyuPinyin = (Agent)new Agent().SetName("HANYU Pinyin 王大年").SetEmail("example@example.org").SetId("A-3");

            //The attribution for this research.
            Attribution researchAttribution = new Attribution().SetContributor(janeDoe).SetModified(DateTime.Parse("2014-03-27"));

            //The attribution for the translation.
            Attribution translationAttribution = new Attribution().SetContributor(hanyuPinyin).SetModified(DateTime.Parse("2014-03-27"));

            //The grave stone.
            SourceDescription gravestoneDescription = (SourceDescription)new SourceDescription()
              .SetTitle("Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii")
              .SetCitation(new SourceCitation().SetValue("WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe."))
              .SetResourceType(ResourceType.PhysicalArtifact)
              .SetRepository(cemetery)
              .SetId("S-1");

            //The image of the grave stone.
            SourceDescription gravestoneImageDescription = (SourceDescription)new SourceDescription()
              .SetTitle("Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii")
              .SetCitation(new SourceCitation().SetValue("WONG Aloiau gravestone (digital photograph), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe."))
              .SetResourceType(ResourceType.DigitalArtifact)
              .SetSource(new SourceReference().SetDescription(gravestoneDescription))
              .SetId("S-2");

            //The transcription of the grave stone.
            Document transcription = (Document)new Document()
              .SetText("WONG ALOIAU\n" +
                      "NOV. 22, 1848 – AUG. 3, 1920\n" +
                      "中山  大字都  泮沙鄉\n" +
                      "生  於  前  清 戊申 年 十一 月 廿二（日）子   時\n" +
                      "終  於  民國  庚申 年     七月    十二 (日)    午    時\n" +
                      "先考  諱 羅有  字 容康 王 府 君 之 墓")
              .SetSource(gravestoneImageDescription)
              .SetLang("zh")
              .SetId("D-1");

            //The transcription described as a source.
            SourceDescription transcriptionDescription = (SourceDescription)new SourceDescription()
              .SetAbout("#" + transcription.Id)
              .SetTitle("Transcription of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii")
              .SetCitation(new SourceCitation().SetValue("WONG Aloiau gravestone (transcription), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe."))
              .SetResourceType(ResourceType.DigitalArtifact)
              .SetSource(new SourceReference().SetDescription(gravestoneImageDescription))
              .SetId("S-3");

            //The translation of the grave stone.
            Document translation = (Document)new Document()
              .SetText("WONG ALOIAU\n" +
                      "NOV. 22, 1848 – AUG. 3, 1920 [lunar dates]\n" +
                      "[Birthplace] [China, Guandong, ]Chung Shan, See Dai Doo, Pun Sha village\n" +
                      "[Date of birth] Born at former Qing 1848 year 11th month 22nd day 23-1 hour.\n" +
                      "[Life] ended at Republic of China year 1920 year 7th mo. 12th day 11-13 hour.\n" +
                      "Deceased father avoid [mention of] Lo Yau also known as Young Hong Wong [noble]residence ruler’s grave.")
              .SetSource(transcriptionDescription)
              .SetId("D-2");

            //The translation described as a source.
            SourceDescription translationDescription = (SourceDescription)new SourceDescription()
              .SetAbout("#" + translation.Id)
              .SetTitle("Translation of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii")
              .SetCitation(new SourceCitation().SetValue("WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe. Translation by HANYU Pinyin 王大年."))
              .SetAttribution(translationAttribution)
              .SetResourceType(ResourceType.DigitalArtifact)
              .SetSource(new SourceReference().SetDescription(transcriptionDescription))
              .SetId("S-4");

            //the birth.
            Fact birth = new Fact()
              .SetType(FactType.Birth)
              .SetDate(new DateInfo().SetOriginal("former Qing 1848 year 11th month 22nd day 23-1 hour").SetFormal("+1848-11-22"))
              .SetPlace(new PlaceReference().SetOriginal("Pun Sha Village, See Dai Doo, Chung Shan, Guangdong, China"));

            //the death.
            Fact death = new Fact()
              .SetType(FactType.Death)
              .SetDate(new DateInfo().SetOriginal("Republic of China year 1920 year 7th mo. 12th day 11-13 hour").SetFormal("+1920-08-03"));

            //the burial.
            Fact burial = new Fact()
              .SetType(FactType.Burial)
              .SetPlace(new PlaceReference().SetOriginal("Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii"));

            //the principal person
            Person aloiau = (Person)new Person().SetName("WONG Aloiau").SetGender(GenderType.Male).SetFact(birth).SetFact(death).SetFact(burial).SetExtracted(true).SetSource(translationDescription).SetId("P-1");

            //the father of the principal (with an aka name).
            Person father = (Person)new Person().SetName("Lo Yau").SetName(new Name().SetType(NameType.AlsoKnownAs).SetNameForm(new NameForm().SetFullText("Young Hong Wong"))).SetExtracted(true).SetSource(translationDescription).SetId("P-2");

            //the relationship.
            Relationship fatherRelationship = new Relationship().SetType(RelationshipType.ParentChild).SetPerson1(father).SetPerson2(aloiau);

            //Jane Doe's analysis.
            Document analysis = (Document)new Document().SetText("...Jane Doe's analysis document...").SetId("D-3");

            //Jane Doe's conclusions about a person.
            Person aloiauConclusion = (Person)new Person().SetEvidence(aloiau).SetAnalysis(analysis).SetId("C-1");

            Gx.Gedcomx gx = new Gx.Gedcomx()
              .SetAgent(janeDoe)
              .SetAgent(cemetery)
              .SetAgent(hanyuPinyin)
              .SetAttribution(researchAttribution)
              .SetSourceDescription(gravestoneDescription)
              .SetSourceDescription(gravestoneImageDescription)
              .SetDocument(transcription)
              .SetSourceDescription(transcriptionDescription)
              .SetDocument(translation)
              .SetSourceDescription(translationDescription)
              .SetPerson(aloiau)
              .SetPerson(father)
              .SetRelationship(fatherRelationship)
              .SetDocument(analysis)
              .SetPerson(aloiauConclusion);

            xmlSerializer.Deserialize<Gx.Gedcomx>(xmlSerializer.Serialize(gx));
            jsonSerializer.Deserialize<Gx.Gedcomx>(jsonSerializer.Serialize(gx));
        }
    }
}
