using System.Xml.Serialization;

using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
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
            Gx.Gedcomx sut = new();

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        [Test]
        public void GedcomxFilled()
        {
            Gx.Gedcomx sut = new()
            {
                Lang = "en",
                DescriptionRef = "description",
                Profile = "profile",
                Attribution = new(),
                Persons = { new() },
                Relationships = { new() },
                SourceDescriptions = { new() },
                Agents = { new() },
                Events = { new() },
                Places = { new() },
                Documents = { new() },
                Collections = { new() },
                Fields = { new() },
                RecordDescriptors = { new() },
            };

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
                Modified = DateTime.Parse("2014-03-07"),
                Contributor = contributor
            };
            SourceDescription sourceDescription = new()
            {
                Id = "S-1",
                Titles = { "Birth Certificate of Emma Bocock, 23 July 1843, General Registry Office" },
                Citations = { new() { Value = "England, birth certificate for Emma Bocock, born 23 July 1843; citing 1843 Birth in District and Sub-district of Ecclesall-Bierlow in the County of York, 303; General Registry Office, Southport." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Created = DateTime.Parse("1843-07-27"),
                Repository = repository
            };
            Fact birth = new()
            {
                KnownType = FactType.Birth,
                Date = new() { Original = "23 June 1843" },
                Place = new() { Original = "Broadfield Bar, Abbeydale Road, Ecclesall-Bierlow, York, England, United Kingdom" }
            };
            Person emma = new()
            {
                Id = "P-1",
                Names = { "Emma Bocock" },
                Gender = new(GenderType.Female),
                Facts = { birth },
                Extracted = true,
                Sources = { sourceDescription }
            };
            Person father = new()
            {
                Id = "P-2",
                Names = { "William Bocock" },
                Facts = { new() { KnownType = FactType.Occupation, Value = "Toll Collector" } },
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
            Document analysis = new() { Id = "D-1", Text = "...Jane Doe's analysis document..." };
            Person emmaConclusion = new()
            {
                Id = "C-1",
                Evidence = { emma },
                Analysis = analysis,
            };
            Gx.Gedcomx gx = new()
            {
                Attribution = attribution,
                Persons = { emma, father, mother, emmaConclusion },
                Relationships = { fatherRelationship, motherRelationship },
                SourceDescriptions = { sourceDescription },
                Agents = { contributor, repository },
                Documents = { analysis }
            };

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
                Addresses = { new() { City = "Salt Lake City", StateOrProvince = "Utah" } }
            };

            //The attribution for this research.
            Attribution researchAttribution = new()
            {
                Modified = DateTime.Parse("2014-04-25"),
                Contributor = janeDoe
            };

            //The parish register.
            SourceDescription recordDescription = new()
            {
                Id = "S-1",
                Titles = { "Marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England" },
                Descriptions = { "Marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837." },
                Citations = { new() { Value = "Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Repository = fhl
            };

            //The transcription of the grave stone.
            Document transcription = new()
            {
                Id = "D-1",
                Text = "Samuel Ham of the parish of Honiton and Elizabeth Spiller\n" +
                      "were married this 3rd day of November 1828 by David Smith\n" +
                      "Stone, Pl Curate,\n" +
                      "In the Presence of\n" +
                      "Jno Pain.\n" +
                      "R.G. Halls.  Peggy Hammet.\n" +
                      "No. 86.",
                Sources = { recordDescription },
                Lang = "en"
            };

            //The transcription described as a source.
            SourceDescription transcriptionDescription = new()
            {
                Id = "S-2",
                About = "#" + transcription.Id,
                Titles = { "Transcription of marriage entry for Samuel Ham and Elizabeth Spiller, Parish Register, Wilton, Somerset, England" },
                Descriptions = { "Transcription of marriage entry for Samuel Ham and Elizabeth in a copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837." },
                Citations = { new() { Value = "Joseph Houghton Spencer, transcriber, Church of England, Parish Church of Wilton (Somerset). <cite>A copy of the registers of the baptisms, marriages, and burials at the church of St. George in the parish of Wilton : adjoining Taunton, in the county of Somerset from A.D. 1558 to A.D. 1837</cite>; Marriage entry for Samuel Ham and Elizabeth Spiller (3 November 1828), (Taunton: Barnicott, 1890), p. 224, No. 86." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { recordDescription }
            };

            //the marriage fact.
            Fact marriage = new()
            {
                KnownType = FactType.Marriage,
                Date = new() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new() { Original = "Wilton St George, Wilton, Somerset, England" }
            };

            //the groom's residence.
            Fact samsResidence = new()
            {
                KnownType = FactType.Residence,
                Date = new() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new() { Original = "parish of Honiton, Devon, England" }
            };

            //the groom's residence.
            Fact lizsResidence = new()
            {
                KnownType = FactType.Residence,
                Date = new() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new() { Original = "parish of Wilton, Somerset, England" }
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
            Event marriageEvent = new()
            {
                Id = "E-1",
                Date = new() { Original = "3 November 1828", Formal = "+1828-11-03" },
                Place = new() { Original = "Wilton St George, Wilton, Somerset, England" },
                Roles =
                {
                    new() { Person = sam, KnownType = EventRoleType.Principal },
                    new() { Person = liz, KnownType = EventRoleType.Principal },
                    new() { Person = witness1, KnownType = EventRoleType.Witness },
                    new() { Person = witness2, KnownType = EventRoleType.Witness },
                    new() { Person = witness3, KnownType = EventRoleType.Witness },
                    new() { Person = officiator, KnownType = EventRoleType.Official }
                },
                Extracted = true
            };

            //Jane Doe's analysis.
            Document analysis = new() { Id = "D-2", Text = "...Jane Doe's analysis document..." };

            //Jane Doe's conclusions about a person.
            Person samConclusion = new()
            {
                Id = "C-1",
                Evidence = { sam },
                Analysis = analysis,
            };

            Gx.Gedcomx gx = new()
            {
                Attribution = researchAttribution,
                Persons = { sam, liz, witness1, witness2, witness3, officiator, samConclusion },
                Relationships = { marriageRelationship },
                SourceDescriptions = { recordDescription, transcriptionDescription },
                Agents = { janeDoe, fhl },
                Events = { marriageEvent },
                Documents = { transcription, analysis },
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void GeorgeMarthaWashingtonExampleTest()
        {
            var popesCreek = CreatePopesCreek();
            var mountVernon = CreateMountVernon();
            var chestnutGrove = CreateChestnutGrove();
            var george = CreateGeorge(popesCreek, mountVernon);
            var martha = CreateMartha(chestnutGrove, mountVernon);
            var marriage = CreateMarriage(george, martha);
            var sources = CiteGeorgeMarthaAndMarriage(george, martha, marriage);
            var contributor = CreateContributor();
            Gx.Gedcomx gx = new()
            {
                Attribution = new() { Contributor = new() { Resource = "#" + contributor.Id } },
                Persons = new() { george, martha },
                Relationships = { marriage },
                SourceDescriptions = sources,
                Agents = { contributor },
                Places = { popesCreek, mountVernon, chestnutGrove }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void CensusAndResidenceLikeFactsTest()
        {
            Person person = new()
            {
                Facts =
                {
                    new(FactType.Census, "...", "..."),
                    new(FactType.Census, "...", "..."),
                    new(FactType.Emigration, "...", "..."),
                    new(FactType.Immigration, "...", "..."),
                    new(FactType.LandTransaction, "...", "..."),
                    new(FactType.MoveTo, "...", "..."),
                    new(FactType.MoveFrom, "...", "..."),
                    new(FactType.Residence, "...", "...")
                }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MilitaryServiceFactsTest()
        {
            Person person = new()
            {
                Facts =
                {
                    new(FactType.MilitaryAward, "...", "..."),
                    new(FactType.MilitaryDischarge, "...", "..."),
                    new(FactType.MilitaryDraftRegistration, "...", "..."),
                    new(FactType.MilitaryInduction, "...", "..."),
                    new(FactType.MilitaryService, "...", "...")
                }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void EducationAndOccupationFactsTest()
        {
            Person person = new()
            {
                Facts =
                {
                    new(FactType.Apprenticeship, "...", "..."),
                    new(FactType.Education, "...", "..."),
                    new(FactType.Occupation, "...", "..."),
                    new(FactType.Retirement, "...", "...")
                }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void ReligiousOrCulturalFactsTest()
        {
            Person person = new()
            {
                Facts =
                {
                    new(FactType.AdultChristening, "...", "..."),
                    new(FactType.Baptism, "...", "..."),
                    new(FactType.BarMitzvah, "...", "..."),
                    new(FactType.BatMitzvah, "...", "..."),
                    new(FactType.Caste, "...", "..."),
                    new(FactType.Christening, "...", "..."),
                    new(FactType.Circumcision, "...", "..."),
                    new(FactType.Clan, "...", "..."),
                    new(FactType.Confirmation, "...", "..."),
                    new(FactType.Excommunication, "...", "..."),
                    new(FactType.FirstCommunion, "...", "..."),
                    new(FactType.Nationality, "...", "..."),
                    new(FactType.Ordination, "...", "..."),
                    new(FactType.Religion, "...", "..."),
                    new(FactType.Yahrzeit, "...", "...")
                }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void CustomFactTest()
        {
            Person person = new()
            {
                Facts = { new() { Type = "data:,Eagle%20Scout", Place = new() { Original = "..." }, Date = new() { Original = "..." } } }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

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
                    new(FactType.CivilUnion, "...", "..."),
                    new(FactType.DomesticPartnership, "...", "..."),
                    new(FactType.Divorce, "...", "..."),
                    new(FactType.Marriage, "...", "..."),
                    new(FactType.MarriageBanns, "...", "..."),
                    new(FactType.MarriageContract, "...", "..."),
                    new(FactType.MarriageLicense, "...", "...")
                }
            };

            Relationship parentChild = new()
            {
                KnownType = RelationshipType.ParentChild,
                Facts = {
                    new(FactType.AdoptiveParent, "...", "..."),
                    new(FactType.BiologicalParent, "...", "..."),
                    new(FactType.FosterParent, "...", "..."),
                    new(FactType.GuardianParent, "...", "..."),
                    new(FactType.StepParent, "...", "...")
                }
            };

            Gx.Gedcomx gx = new()
            {
                Relationships = { couple, parentChild },
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void BasicWesternNameTest()
        {
            NameForm nameForm = new("John Fitzgerald Kennedy")
            {
                Lang = "en",
                Parts = {
                    new(NamePartType.Given, "John"),
                    new(NamePartType.Given, "Fitzgerald"),
                    new(NamePartType.Surname, "Kennedy")
                }
            };
            Name name = new() { NameForms = { nameForm } };

            Gx.Gedcomx gx = new()
            {
                Persons = { new() { Names = { name } } }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleJapaneseFormsTest()
        {
            NameForm kanji = new("山田太郎")
            {
                Lang = "ja-Hani",
                Parts = {
                    new(NamePartType.Surname, "山田"),
                    new(NamePartType.Given, "太郎")
                }
            };
            NameForm katakana = new("ヤマダタロー")
            {
                Lang = "ja-Kana",
                Parts = {
                    new(NamePartType.Surname, "ヤマダ"),
                    new(NamePartType.Given, "タロー")
                }
            };
            NameForm romanized = new("Yamada Tarō")
            {
                Lang = "ja-Kana",
                Parts = {
                    new(NamePartType.Surname, "Tarō"),
                    new(NamePartType.Given, "Yamada")
                }
            };
            Name name = new() { NameForms = { kanji, katakana, romanized } };

            Gx.Gedcomx gx = new()
            {
                Persons = { new() { Names = { name } } }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleNamePartsOnePartPerTypeTest()
        {
            NameForm nameForm = new("José Eduardo Santos Tavares Melo Silva")
            {
                Lang = "pt-BR",
                Parts = {
                    new(NamePartType.Given, "José Eduardo"),
                    new(NamePartType.Surname, "Santos Tavares Melo Silva")
                }
            };
            Name name = new() { NameForms = { nameForm } };

            Gx.Gedcomx gx = new()
            {
                Persons = { new() { Names = { name } } }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void MultipleNamePartsMultiplePartsPerTypeTest()
        {
            NameForm nameForm = new("José Eduardo Santos Tavares Melo Silva")
            {
                Lang = "pt-BR",
                Parts = {
                    new(NamePartType.Given, "José"),
                    new(NamePartType.Given, "Eduardo"),
                    new(NamePartType.Surname, "Santos"),
                    new(NamePartType.Surname, "Tavares"),
                    new(NamePartType.Surname, "Melo"),
                    new(NamePartType.Surname, "Silva")
                }
            };
            Name name = new() { NameForms = { nameForm } };

            Gx.Gedcomx gx = new()
            {
                Persons = { new() { Names = { name } } }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void PatronymicTest()
        {
            NameForm nameForm = new("Björk Guðmundsdóttir")
            {
                Lang = "is",
                Parts = { new() { Value = "Guðmundsdóttir", Qualifiers = { new(NamePartQualifierType.Patronymic) } } }
            };
            Name name = new() { NameForms = { nameForm } };

            Gx.Gedcomx gx = new()
            {
                Persons = { new() { Names = { name } } }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void FactQualifiersTest()
        {
            Person person = new()
            {
                Facts =
                {
                    new(FactType.Christening, "...", "...") { Qualifiers = { new(FactQualifierType.Religion, "Catholic") } },
                    new(FactType.Census, "...", "...") { Qualifiers = { new(FactQualifierType.Age, "44") } },
                    new(FactType.Death, "...", "...") { Qualifiers = { new(FactQualifierType.Cause, "Heart failure") } }
                }
            };
            Gx.Gedcomx gx = new() { Persons = { person } };

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
                Addresses = { new() { City = "Honolulu", StateOrProvince = "Hawaii" } }
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
                Modified = DateTime.Parse("2014-03-27"),
                Contributor = janeDoe
            };

            //The attribution for the translation.
            Attribution translationAttribution = new()
            {
                Modified = DateTime.Parse("2014-03-27"),
                Contributor = hanyuPinyin
            };

            //The grave stone.
            SourceDescription gravestoneDescription = new()
            {
                Id = "S-1",
                Titles = { "Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new() { Value = "WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.PhysicalArtifact,
                Repository = cemetery
            };

            //The image of the grave stone.
            SourceDescription gravestoneImageDescription = new()
            {
                Id = "S-2",
                Titles = { "Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new() { Value = "WONG Aloiau gravestone (digital photograph), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { gravestoneDescription }
            };

            //The transcription of the grave stone.
            Document transcription = new()
            {
                Id = "D-1",
                Text = "WONG ALOIAU\n" +
                      "NOV. 22, 1848 – AUG. 3, 1920\n" +
                      "中山  大字都  泮沙鄉\n" +
                      "生  於  前  清 戊申 年 十一 月 廿二（日）子   時\n" +
                      "終  於  民國  庚申 年     七月    十二 (日)    午    時\n" +
                      "先考  諱 羅有  字 容康 王 府 君 之 墓",
                Sources = { gravestoneImageDescription },
                Lang = "zh",
            };

            //The transcription described as a source.
            SourceDescription transcriptionDescription = new()
            {
                Id = "S-3",
                About = "#" + transcription.Id,
                Titles = { "Transcription of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new() { Value = "WONG Aloiau gravestone (transcription), Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe." } },
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { gravestoneImageDescription }
            };

            //The translation of the grave stone.
            Document translation = new()
            {
                Id = "D-2",
                Text = "WONG ALOIAU\n" +
                      "NOV. 22, 1848 – AUG. 3, 1920 [lunar dates]\n" +
                      "[Birthplace] [China, Guandong, ]Chung Shan, See Dai Doo, Pun Sha village\n" +
                      "[Date of birth] Born at former Qing 1848 year 11th month 22nd day 23-1 hour.\n" +
                      "[Life] ended at Republic of China year 1920 year 7th mo. 12th day 11-13 hour.\n" +
                      "Deceased father avoid [mention of] Lo Yau also known as Young Hong Wong [noble]residence ruler’s grave.",
                Sources = { transcriptionDescription }
            };

            //The translation described as a source.
            SourceDescription translationDescription = new()
            {
                Id = "S-4",
                About = "#" + translation.Id,
                Titles = { "Translation of Grave Marker of WONG Aloiau, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" },
                Citations = { new() { Value = "WONG Aloiau gravestone, Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii; visited May 1975 by Jane Doe. Translation by HANYU Pinyin 王大年." } },
                Attribution = translationAttribution,
                KnownResourceType = ResourceType.DigitalArtifact,
                Sources = { transcriptionDescription }
            };

            //the birth.
            Fact birth = new()
            {
                KnownType = FactType.Birth,
                Date = new() { Original = "former Qing 1848 year 11th month 22nd day 23-1 hour", Formal = "+1848-11-22" },
                Place = new() { Original = "Pun Sha Village, See Dai Doo, Chung Shan, Guangdong, China" }
            };

            //the death.
            Fact death = new()
            {
                KnownType = FactType.Death,
                Date = new() { Original = "Republic of China year 1920 year 7th mo. 12th day 11-13 hour", Formal = "+1920-08-03" }
            };

            //the burial.
            Fact burial = new()
            {
                KnownType = FactType.Burial,
                Place = new() { Original = "Lin Yee Chung Cemetery, Honolulu, Oahu, Hawaii" }
            };

            //the principal person
            Person aloiau = new()
            {
                Id = "P-1",
                Names = { "WONG Aloiau" },
                Gender = new(GenderType.Male),
                Facts = { birth, death, burial },
                Extracted = true,
                Sources = { translationDescription }
            };

            //the father of the principal (with an aka name).
            Person father = new()
            {
                Id = "P-2",
                Names = { "Lo Yau", new Name() { KnownType = NameType.AlsoKnownAs, NameForms = { new("Young Hong Wong") } } },
                Extracted = true,
                Sources = { translationDescription }
            };

            //the relationship.
            Relationship fatherRelationship = new() { KnownType = RelationshipType.ParentChild, Person1 = father, Person2 = aloiau };

            //Jane Doe's analysis.
            Document analysis = new() { Id = "D-3", Text = "...Jane Doe's analysis document..." };

            //Jane Doe's conclusions about a person.
            Person aloiauConclusion = new()
            {
                Id = "C-1",
                Evidence = { aloiau },
                Analysis = analysis
            };

            Gx.Gedcomx gx = new()
            {
                Attribution = researchAttribution,
                Persons = { aloiau, father, aloiauConclusion },
                Relationships = { fatherRelationship },
                SourceDescriptions = { gravestoneDescription, gravestoneImageDescription, transcriptionDescription, translationDescription },
                Agents = { janeDoe, cemetery, hanyuPinyin },
                Documents = { transcription, translation, analysis }
            };

            VerifyXmlSerialization(gx);
            VerifyJsonSerialization(gx);
        }

        [Test]
        public void GedcomxWithRelationships()
        {
            Gx.Gedcomx sut = new()
            {
                Persons = { new() },
                Relationships = { new() }
            };

            VerifyXmlSerialization(sut);
            VerifyJsonSerialization(sut);
        }

        private static void VerifyXmlSerialization(Gx.Gedcomx sut)
        {
            XmlSerializer serializer = new(typeof(Gx.Gedcomx));
            using MemoryStream stream = new();
            serializer.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream).ReadToEnd();
            result.ShouldContain(sut);
        }

        private static void VerifyJsonSerialization(Gx.Gedcomx sut)
        {
            JsonSerializerSettings jsonSettings = new(); ;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Gx.Gedcomx>(JsonConvert.SerializeObject(sut, jsonSettings), jsonSettings));
        }

        private static PlaceDescription CreatePopesCreek()
        {
            return new()
            {
                Id = "888",
                Latitude = 38.192353,
                Longitude = -76.904069,
                Names = { "Pope's Creek, Westmoreland, Virginia, United States" }
            };
        }

        private static PlaceDescription CreateMountVernon()
        {
            return new()
            {
                Id = "999",
                Latitude = 38.721144,
                Longitude = -77.109461,
                Names = { "Mount Vernon, Fairfax County, Virginia, United States" }
            };
        }

        private static PlaceDescription CreateChestnutGrove()
        {
            return new()
            {
                Id = "KKK",
                Latitude = 37.518304,
                Longitude = -76.984148,
                Names = { "Chestnut Grove, New Kent, Virginia, United States" }
            };
        }

        private static Agent CreateContributor()
        {
            return new()
            {
                Id = "GGG-GGGG",
                Names = { "Ryan Heaton" }
            };
        }

        private static Person CreateGeorge(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            return new()
            {
                Id = "BBB-BBBB",
                Gender = new(GenderType.Male),
                Facts =
                {
                    new()
                    {
                        Id = "123",
                        KnownType = FactType.Birth,
                        Date = new() { Original = "February 22, 1732", Formal = "+1732-02-22" },
                        Place = new(birthPlace) { Original = birthPlace.Names[0].Value.ToLower() }
                    },
                    new()
                    {
                        Id = "456",
                        KnownType = FactType.Death,
                        Date = new() { Original = "December 14, 1799", Formal = "+1799-12-14T22:00:00" },
                        Place = new(deathPlace) { Original = deathPlace.Names[0].Value.ToLower() }
                    }
                },
                Names =
                {
                    new Name("George Washington", new(NamePartType.Given, "George"), new(NamePartType.Surname, "Washington") )
                    {
                        Id = "789",
                    }
                }
            };
        }

        private static Person CreateMartha(PlaceDescription birthPlace, PlaceDescription deathPlace)
        {
            return new()
            {
                Id = "CCC-CCCC",
                Gender = new(GenderType.Female),
                Facts =
                {
                    new()
                    {
                        Id = "321",
                        KnownType = FactType.Birth,
                        Date = new() { Original = "June 2, 1731", Formal = "+1731-06-02" },
                        Place = new(birthPlace) { Original = birthPlace.Names[0].Value.ToLower() }
                    },
                    new()
                    {
                        Id = "654",
                        KnownType = FactType.Death,
                        Date = new() { Original = "May 22, 1802", Formal = "+1802-05-22" },
                        Place = new(deathPlace) { Original = deathPlace.Names[0].Value.ToLower() }
                    }
                },
                Names =
                {
                    new Name("Martha Dandridge Custis", new(NamePartType.Given, "Martha Dandridge"), new(NamePartType.Surname, "Custis") )
                    {
                        Id = "987",
                    }
                }
            };
        }

        private static Relationship CreateMarriage(Person george, Person martha)
        {
            Fact marriage = new()
            {
                Date = new() { Original = "January 6, 1759", Formal = "+01-06-1759" },
                Place = new() { Original = "White House Plantation" }
            };
            return new()
            {
                Id = "DDD-DDDD",
                Person1 = new("#" + george.Id),
                Person2 = new("#" + martha.Id),
                Facts = { marriage }
            };
        }

        private static List<SourceDescription> CiteGeorgeMarthaAndMarriage(Person george, Person martha, Relationship relationship)
        {
            SourceCitation georgeCitation = new()
            {
                Value = "\"George Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012.",
            };
            SourceDescription georgeSource = new()
            {
                Id = "EEE-EEEE",
                About = "http://en.wikipedia.org/wiki/George_washington",
                Citations = { georgeCitation }
            };

            SourceCitation marthaCitation = new()
            {
                Value = "\"Martha Washington.\" Wikipedia, The Free Encyclopedia. Wikimedia Foundation, Inc. 24 October 2012."
            };
            SourceDescription marthaSource = new()
            {
                Id = "FFF-FFFF",
                About = "http://en.wikipedia.org/wiki/Martha_washington",
                Citations = { marthaCitation }
            };

            george.SetSource(georgeSource);

            SourceReference reference = marthaSource;
            martha.SetSource(reference);

            relationship.SetSource(reference);

            return new() { georgeSource, marthaSource };
        }
    }
}
