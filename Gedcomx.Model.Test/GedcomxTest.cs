﻿using System.Xml.Serialization;

using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Model.Collections;
using Gx.Source;
using Gx.Types;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    /// <summary>
    /// Test calss for <see cref="Gx.Gedcomx"/>
    /// </summary>
    [TestFixture]
    public class GedcomxTest
    {
        [Test]
        public void GedcomxEmpty()
        {
            var sut = new Gx.Gedcomx();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void EmmaBocockExampleTest()
        {
            Agent contributor = new()
            {
                Id = "A-1",
                Names = { "Jane Doe" },
                Emails = { "example@example.org" }
            };
            Agent repository = new()
            {
                Id = "A-2",
                Names = { "General Registry Office, Southport" }
            };
            Attribution attribution = new()
            {
                Modified = DateTime.Parse("2014-03-07")
            };
            attribution.SetContributor(contributor);
            SourceDescription sourceDescription = new()
            {
                Id = "S-1",
                Titles = { "Birth Certificate of Emma Bocock, 23 July 1843, General Registry Office" },
                Citations = { new SourceCitation() { Value = "England, birth certificate for Emma Bocock, born 23 July 1843; citing 1843 Birth in District and Sub-district of Ecclesall-Bierlow in the County of York, 303; General Registry Office, Southport." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Created = DateTime.Parse("1843-07-27"),
                Repository = repository
            };
            Fact birth = new()
            {
                KnownType = FactType.Birth,
                Date = new DateInfo() { Original = "23 June 1843" },
                Place = new PlaceReference() { Original = "Broadfield Bar, Abbeydale Road, Ecclesall-Bierlow, York, England, United Kingdom" }
            };
            Person emma = new()
            {
                Id = "P-1",
                Names = { "Emma Bocock" },
                Gender = new Gender(GenderType.Female),
                Facts = { birth },
                Extracted = true,
                Sources = { sourceDescription }
            };
            Person father = new()
            {
                Id = "P-2",
                Names = { "William Bocock" },
                Facts = { new Fact() { KnownType = FactType.Occupation, Value = "Toll Collector" } },
                Extracted = true,
                Sources = { sourceDescription }
            };
            Person mother = new()
            {
                Id = "P-3",
                Names = { "Sarah Bocock formerly Brough" },
                Extracted = true,
                Sources = { sourceDescription }
            };
            Relationship fatherRelationship = new() { KnownType = RelationshipType.ParentChild, Person1 = father, Person2 = emma };
            Relationship motherRelationship = new() { KnownType = RelationshipType.ParentChild, Person1 = mother, Person2 = emma };
            Document analysis = (Document)new Document().SetText("...Jane Doe's analysis document...").SetId("D-1");
            Person emmaConclusion = new()
            {
                Id = "C-1",
                Evidence = { emma },
                Analysis = analysis,
            };
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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void SamuelHamExampleTest()
        {
            //Jane Doe, the researcher.
            Agent janeDoe = new()
            {
                Id = "A-1",
                Names = { "Jane Doe" },
                Emails = { "example@example.org" }
            };

            //Lin Yee Chung Cemetery
            Agent fhl = new()
            {
                Id = "A-2",
                Names = { "Family History Librarye" },
                Addresses = { new Address().SetCity("Salt Lake City").SetStateOrProvince("Utah") }
            };

            //The attribution for this research.
            Attribution researchAttribution = new()
            {
                Modified = DateTime.Parse("2014-04-25")
            };
            researchAttribution.SetContributor(janeDoe);

            //The parish register.
            SourceDescription recordDescription = new()
            {
                Id = "S-1",
                Titles = { "Marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England" },
                Descriptions = { "Marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837." },
                Citations = { new SourceCitation() { Value = "Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Repository = fhl
            };

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
            SourceDescription transcriptionDescription = new()
            {
                Id = "S-2",
                About = "#" + transcription.Id,
                Titles = { "Transcription of marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England" },
                Descriptions = { "Transcription of marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837." },
                Citations = { new SourceCitation() { Value = "Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { new SourceReference().SetDescription(recordDescription) }
            };

            //the marriage fact.
            Fact marriage = new()
            {
                KnownType = FactType.Marriage,
                Date = new DateInfo() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new PlaceReference() { Original = "Wilton St George, Wilton, Somerset, England" }
            };

            //the groom's residence.
            Fact samsResidence = new()
            {
                KnownType = FactType.Residence,
                Date = new DateInfo() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new PlaceReference() { Original = "parish of Honiton, Devon, England" }
            };

            //the groom's residence.
            Fact lizsResidence = new()
            {
                KnownType = FactType.Residence,
                Date = new DateInfo() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new PlaceReference() { Original = "parish of Wilton, Somerset, England" }
            };

            //the groom
            Person sam = new()
            {
                Id = "P-1",
                Names = { "Samuel Ham" },
                Facts = { samsResidence },
                Extracted = true,
                Sources = { transcriptionDescription }
            };

            //the bride.
            Person liz = new()
            {
                Id = "P-2",
                Names = { "Elizabeth Spiller" },
                Facts = { lizsResidence },
                Extracted = true,
                Sources = { transcriptionDescription }
            };

            //witnesses
            Person witness1 = new()
            {
                Id = "P-3",
                Names = { "Jno. Pain" },
                Extracted = true,
                Sources = { transcriptionDescription }
            };
            Person witness2 = new()
            {
                Id = "P-4",
                Names = { "R.G. Halls" },
                Extracted = true,
                Sources = { transcriptionDescription }
            };
            Person witness3 = new()
            {
                Id = "P-5",
                Names = { "Peggy Hammet" },
                Extracted = true,
                Sources = { transcriptionDescription }
            };

            //officiator
            Person officiator = new()
            {
                Id = "P-6",
                Names = { "David Smith Stone" },
                Extracted = true,
                Sources = { transcriptionDescription }
            };

            //the relationship.
            Relationship marriageRelationship = new() { KnownType = RelationshipType.Couple, Person1 = sam, Person2 = liz, Facts = { marriage }, Extracted = true };

            //the marriage event
            Event marriageEvent = (Event)new Event(EventType.Marriage)
              .SetDate(new DateInfo() { Original = "3 November 1828" }.SetFormal("+1828-11-03"))
              .SetPlace(new PlaceReference() { Original = "Wilton St George, Wilton, Somerset, England" })
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
            Person samConclusion = new()
            {
                Id = "C-1",
                Evidence = { sam },
                Analysis = analysis,
            };

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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void GeorgeMarthaWashingtonExampleTest()
        {
            PlaceDescription popesCreek = CreatePopesCreek();
            PlaceDescription mountVernon = CreateMountVernon();
            PlaceDescription chestnutGrove = CreateChestnutGrove();
            Person george = CreateGeorge(popesCreek, mountVernon);
            Person martha = CreateMartha(chestnutGrove, mountVernon);
            Relationship marriage = CreateMarriage(george, martha);
            List<SourceDescription> sources = CiteGeorgeMarthaAndMarriage(george, martha, marriage);
            Agent contributor = CreateContributor();
            Gx.Gedcomx gx = new()
            {
                Persons = new List<Person>() { george, martha }
            };
            gx.SetRelationship(marriage);
            gx.SourceDescriptions = sources;
            gx.SetAgent(contributor);
            gx.SetAttribution(new Attribution());
            gx.Attribution.SetContributor(new ResourceReference());
            gx.Attribution.Contributor.SetResource("#" + contributor.Id);
            gx.Places = new List<PlaceDescription>() { popesCreek, mountVernon, chestnutGrove };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void CensusAndResidenceLikeFactsTest()
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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MilitaryServiceFactsTest()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.MilitaryAward, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryDischarge, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryDraftRegistration, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryInduction, "...", "..."))
              .SetFact(new Fact(FactType.MilitaryService, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void EducationAndOccupationFactsTest()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.Apprenticeship, "...", "..."))
              .SetFact(new Fact(FactType.Education, "...", "..."))
              .SetFact(new Fact(FactType.Occupation, "...", "..."))
              .SetFact(new Fact(FactType.Retirement, "...", "..."));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void ReligiousOrCulturalFactsTest()
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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void CustomFactTest()
        {
            Person person = new Person()
              .SetFact(new Fact().SetType("data:,Eagle%20Scout").SetPlace(new PlaceReference() { Original = "..." }).SetDate(new DateInfo() { Original = "..." }));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void RelationshipFactsTest()
        {
            Relationship couple = new()
            {
                KnownType = RelationshipType.Couple,
                Facts = {
                    new Fact(FactType.CivilUnion, "...", "..."),
                    new Fact(FactType.DomesticPartnership, "...", "..."),
                    new Fact(FactType.Divorce, "...", "..."),
                    new Fact(FactType.Marriage, "...", "..."),
                    new Fact(FactType.MarriageBanns, "...", "..."),
                    new Fact(FactType.MarriageContract, "...", "..."),
                    new Fact(FactType.MarriageLicense, "...", "...")
                }
            };

            Relationship parentChild = new()
            {
                KnownType = RelationshipType.ParentChild,
                Facts = {
                    new Fact(FactType.AdoptiveParent, "...", "..."),
                    new Fact(FactType.BiologicalParent, "...", "..."),
                    new Fact(FactType.FosterParent, "...", "..."),
                    new Fact(FactType.GuardianParent, "...", "..."),
                    new Fact(FactType.StepParent, "...", "...")
                }
            };

            Gx.Gedcomx gx = new Gx.Gedcomx().SetRelationship(couple).SetRelationship(parentChild);

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void BasicWesternNameTest()
        {
            NameForm nameForm = new NameForm("John Fitzgerald Kennedy")
              .SetLang("en")
              .SetPart(NamePartType.Given, "John")
              .SetPart(NamePartType.Given, "Fitzgerald")
              .SetPart(NamePartType.Surname, "Kennedy");
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleJapaneseFormsTest()
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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleNamePartsOnePartPerTypeTest()
        {
            NameForm nameForm = new NameForm("José Eduardo Santos Tavares Melo Silva")
              .SetLang("pt-BR")
              .SetPart(NamePartType.Given, "José Eduardo")
              .SetPart(NamePartType.Surname, "Santos Tavares Melo Silva");
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleNamePartsMultiplePartsPerTypeTest()
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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void PatronymicTest()
        {
            NameForm nameForm = new NameForm("Björk Guðmundsdóttir")
              .SetLang("is")
              .SetPart(NamePartType.Given, "Björk")
              .SetPart(new NamePart().SetValue("Guðmundsdóttir").SetQualifier(new Qualifier(NamePartQualifierType.Patronymic)));
            Name name = new Name().SetNameForm(nameForm);

            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(new Person().SetName(name));

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void FactQualifiersTest()
        {
            Person person = new Person()
              .SetFact(new Fact(FactType.Christening, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Religion, "Catholic")))
              .SetFact(new Fact(FactType.Census, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Age, "44")))
              .SetFact(new Fact(FactType.Death, "...", "...").SetQualifier(new Qualifier(FactQualifierType.Cause, "Heart failure")));
            Gx.Gedcomx gx = new Gx.Gedcomx().SetPerson(person);

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void WongAloiauExampleTest()
        {
            //Jane Doe, the researcher.
            Agent janeDoe = new()
            {
                Id = "A-1",
                Names = { "Jane Doe" },
                Emails = { "example@example.org" }
            };

            //Lin Yee Chung Cemetery
            Agent cemetery = new()
            {
                Id = "A-2",
                Names = { "Lin Yee Chung Cemetery" },
                Addresses = { new Address().SetCity("Honolulu").SetStateOrProvince("Hawaii") }
            };

            //Hanyu Pinyin, the translator.
            Agent hanyuPinyin = new()
            {
                Id = "A-3",
                Names = { "HANYU Pinyin 王大年" },
                Emails = { "example@example.org" }
            };

            //The attribution for this research.
            Attribution researchAttribution = new()
            {
                Modified = DateTime.Parse("2014-03-27")
            };
            researchAttribution.SetContributor(janeDoe);

            //The attribution for the translation.
            Attribution translationAttribution = new()
            {
                Modified = DateTime.Parse("2014-03-27")
            };
            translationAttribution.SetContributor(hanyuPinyin);

            //The grave stone.
            SourceDescription gravestoneDescription = new()
            {
                Id = "S-1",
                Titles = { "Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new SourceCitation() { Value = "WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Repository = cemetery
            };

            //The image of the grave stone.
            SourceDescription gravestoneImageDescription = new()
            {
                Id = "S-2",
                Titles = { "Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new SourceCitation() { Value = "WONG Aloiau gravestone (digital photograph), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { new SourceReference().SetDescription(gravestoneDescription) }
            };

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
            SourceDescription transcriptionDescription = new()
            {
                Id = "S-3",
                About = "#" + transcription.Id,
                Titles = { "Transcription of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new SourceCitation() { Value = "WONG Aloiau gravestone (transcription), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { new SourceReference().SetDescription(gravestoneImageDescription) }
            };

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
            SourceDescription translationDescription = new()
            {
                Id = "S-4",
                About = "#" + translation.Id,
                Titles = { "Translation of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new SourceCitation() { Value = "WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe. Translation by HANYU Pinyin 王大年." } },
                Attribution = translationAttribution,
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { new SourceReference().SetDescription(transcriptionDescription) }
            };

            //the birth.
            Fact birth = new()
            {
                KnownType = FactType.Birth,
                Date = new DateInfo() { Original = "former Qing 1848 year 11th month 22nd day 23-1 hour", Formal = "+1848-11-22" },
                Place = new PlaceReference() { Original = "Pun Sha Village, See Dai Doo, Chung Shan, Guangdong, China" }
            };

            //the death.
            Fact death = new()
            {
                KnownType = FactType.Death,
                Date = new DateInfo() { Original = "Republic of China year 1920 year 7th mo. 12th day 11-13 hour", Formal = "+1920-08-03" }
            };

            //the burial.
            Fact burial = new()
            {
                KnownType = FactType.Burial,
                Place = new PlaceReference() { Original = "Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" }
            };

            //the principal person
            Person aloiau = new()
            {
                Id = "P-1",
                Names = { "WONG Aloiau" },
                Gender = new Gender(GenderType.Male),
                Facts = { birth, death, burial },
                Extracted = true,
                Sources = { translationDescription }
            };

            //the father of the principal (with an aka name).
            Person father = new()
            {
                Id = "P-2",
                Names = { "Lo Yau", new Name() { KnownType = NameType.AlsoKnownAs }.SetNameForm(new NameForm().SetFullText("Young Hong Wong")) },
                Extracted = true,
                Sources = { translationDescription }
            };

            //the relationship.
            Relationship fatherRelationship = new() { KnownType = RelationshipType.ParentChild, Person1 = father, Person2 = aloiau };

            //Jane Doe's analysis.
            Document analysis = (Document)new Document().SetText("...Jane Doe's analysis document...").SetId("D-3");

            //Jane Doe's conclusions about a person.
            Person aloiauConclusion = new()
            {
                Id = "C-1",
                Evidence = { aloiau },
                Analysis = analysis
            };

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

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void GedcomxWithRelationships()
        {
            var sut = new Gx.Gedcomx
            {
                Persons = new List<Person> { new Person() },
                Relationships = new List<Relationship> { new Relationship() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Gx.Gedcomx sut)
        {
            var serializer = new XmlSerializer(typeof(Gx.Gedcomx));
            using var stream = new MemoryStream();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            Assert.That(result, Does.Contain("<gedcomx "));
            Assert.That(result, Does.Contain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""));
            Assert.That(result, Does.Contain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            Assert.That(result, Does.Contain("xmlns=\"http://gedcomx.org/v1/\""));
            Assert.That(result.Contains("<lang "), Is.EqualTo(sut.Lang != null));
            Assert.That(result.Contains("<description "), Is.EqualTo(sut.DescriptionRef != null));
            Assert.That(result.Contains("<profile"), Is.EqualTo(sut.Profile != null));
            Assert.That(result.Contains("<attribution"), Is.EqualTo(sut.Attribution != null));
            Assert.That(result.Contains("<person"), Is.EqualTo(sut.Persons != null && sut.Persons.Count > 0));
            Assert.That(result.Contains("<relationship "), Is.EqualTo(sut.Relationships != null && sut.Relationships.Count > 0));
            Assert.That(result.Contains("<sourceDescription "), Is.EqualTo(sut.SourceDescriptions != null && sut.SourceDescriptions.Count > 0));
            Assert.That(result.Contains("<agent "), Is.EqualTo(sut.Agents != null && sut.Agents.Count > 0));
            Assert.That(result.Contains("<event "), Is.EqualTo(sut.Events != null && sut.Events.Count > 0));
            Assert.That(result.Contains("<place "), Is.EqualTo(sut.Places != null && sut.Places.Count > 0));
            Assert.That(result.Contains("<document "), Is.EqualTo(sut.Documents != null && sut.Documents.Count > 0));
            Assert.That(result.Contains("<collection "), Is.EqualTo(sut.Collections != null && sut.Collections.Count > 0));
            Assert.That(result.Contains("<field "), Is.EqualTo(sut.Fields != null && sut.Fields.Count > 0));
            Assert.That(result.Contains("<recordDescriptor "), Is.EqualTo(sut.RecordDescriptors != null && sut.RecordDescriptors.Count > 0));
        }

        private static void VerifyJsonSerialization(Gx.Gedcomx sut)
        {
            JsonSerializerSettings jsonSettings = new(); ;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Gx.Gedcomx>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }

        private static PlaceDescription CreatePopesCreek()
        {
            PlaceDescription place = new();
            place.SetId("888");
            place.SetLatitude(38.192353);
            place.SetLongitude(-76.904069);
            place.SetName("Pope's Creek, Westmoreland, Virginia, United States");
            return place;
        }

        private static PlaceDescription CreateMountVernon()
        {
            PlaceDescription place = new();
            place.SetId("999");
            place.SetLatitude(38.721144);
            place.SetLongitude(-77.109461);
            place.SetName("Mount Vernon, Fairfax County, Virginia, United States");
            return place;
        }

        private static PlaceDescription CreateChestnutGrove()
        {
            PlaceDescription place = new();
            place.SetId("KKK");
            place.SetLatitude(37.518304);
            place.SetLongitude(-76.984148);
            place.SetName("Chestnut Grove, New Kent, Virginia, United States");
            return place;
        }

        private static Agent CreateContributor()
        {
            Agent agent = new();
            agent.SetId("GGG-GGGG");
            agent.SetName("Ryan Heaton");
            return agent;
        }

        private static Person CreateGeorge(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            Person person = new();
            person.SetGender(new Gender(GenderType.Male));

            Fact fact = new()
            {
                Id = "123",
                KnownType = FactType.Birth,
                Date = new DateInfo() { Original = "February 22, 1732", Formal = "+1732-02-22" },
                Place = new PlaceReference() { Original = birthPlace.Names[0].Value.ToLower() }.SetDescription(birthPlace)
            };

            person.SetFact(fact);

            fact = new()
            {
                Id = "456",
                KnownType = FactType.Death,
                Date = new DateInfo() { Original = "December 14, 1799", Formal = "+1799-12-14T22:00:00" },
                Place = new PlaceReference() { Original = deathPlace.Names[0].Value.ToLower() }.SetDescription(deathPlace)
            };

            person.SetFact(fact);

            Names names = new();
            Name name = new();
            NameForm nameForm = new();
            nameForm.SetFullText("George Washington");
            List<NamePart> parts = new();
            NamePart part = new();
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

        private static Person CreateMartha(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            Person person = new();
            person.SetGender(new Gender(GenderType.Male));

            Fact fact = new()
            {
                Id = "321",
                KnownType = FactType.Birth,
                Date = new DateInfo() { Original = "June 2, 1731", Formal = "+1731-06-02" },
                Place = new PlaceReference() { Original = birthPlace.Names[0].Value.ToLower() }.SetDescription(birthPlace)
            };

            person.SetFact(fact);

            fact = new()
            {
                Id = "654",
                KnownType = FactType.Death,
                Date = new DateInfo() { Original = "May 22, 1802", Formal = "+1802-05-22" },
                Place = new PlaceReference() { Original = deathPlace.Names[0].Value.ToLower() }.SetDescription(deathPlace)
            };

            person.SetFact(fact);

            Names names = new();
            Name name = new();
            NameForm nameForm = new();
            nameForm.SetFullText("Martha Dandridge Custis");
            List<NamePart> parts = new();
            NamePart part = new();
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

        private static Relationship CreateMarriage(Person george, Person martha)
        {
            Relationship relationship = new();
            relationship.SetId("DDD-DDDD");
            relationship.SetPerson1(new ResourceReference("#" + george.Id));
            relationship.SetPerson2(new ResourceReference("#" + martha.Id));
            Fact marriage = new();
            marriage.SetDate(new DateInfo());
            marriage.Date.SetOriginal("January 6, 1759");
            marriage.Date.SetFormal("+01-06-1759");
            marriage.SetPlace(new PlaceReference());
            marriage.Place.SetOriginal("White House Plantation");
            relationship.SetFact(marriage);
            return relationship;
        }

        private static List<SourceDescription> CiteGeorgeMarthaAndMarriage(Person george, Person martha, Relationship relationship)
        {
            SourceDescription georgeSource = new();
            georgeSource.SetId("EEE-EEEE");
            georgeSource.SetAbout("http://en.wikipedia.org/wiki/George_washington");
            SourceCitation georgeCitation = new();
            georgeCitation.SetValue("\"George Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012.");
            georgeSource.SetCitation(georgeCitation);

            SourceDescription marthaSource = new();
            marthaSource.SetId("FFF-FFFF");
            marthaSource.SetAbout("http://en.wikipedia.org/wiki/Martha_washington");
            SourceCitation marthaCitation = new();
            marthaCitation.SetValue("\"Martha Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012.");
            marthaSource.SetCitation(marthaCitation);

            SourceReference reference = new();
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