using Gx.Common;
using Gx.Conclusion;
using Gx.Fs.Tree;
using Gx.Links;
using Gx.Rs.Api;
using Gx.Source;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Rs.Api.Test
{
    public static class TestBacking
    {
        public static Person GetCreateMalePerson()
        {
            return new Person()
            {
                Living = false,
                Gender = new Gender()
                {
                    KnownType = Gx.Types.GenderType.Male
                },
                Names = new List<Name>()
                {
                    new Name()
                    {
                        KnownType = Gx.Types.NameType.BirthName,
                        NameForms = new List<NameForm>()
                        {
                            new NameForm()
                            {
                                FullText = "GedcomX User",
                                Parts = new List<NamePart>()
                                {
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Given,
                                        Value = "GedcomX",
                                    },
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Surname,
                                        Value = "User",
                                    }
                                }
                            },
                            new NameForm()
                            {
                                FullText = "GedcomX2 User",
                                Parts = new List<NamePart>()
                                {
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Given,
                                        Value = "GedcomX2",
                                    },
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Surname,
                                        Value = "User",
                                    }
                                }
                            },
                        },
                        Preferred = true,
                    }
                },
                Facts = new List<Fact>()
                {
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Birth,
                        Date = new DateInfo()
                        {
                            Original = "June 1800",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Christening,
                        Date = new DateInfo()
                        {
                            Original = "1802",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "American Fork, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Residence,
                        Date = new DateInfo()
                        {
                            Original = "4 Jan 1896",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Death,
                        Date = new DateInfo()
                        {
                            Original = "July 14, 1900",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Burial,
                        Date = new DateInfo()
                        {
                            Original = "1900",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Sandy, Salt Lake, Utah, United States",
                        },
                    }
                }
            };
        }

        public static Person GetCreateFemalePerson()
        {
            return new Person()
            {
                Living = false,
                Gender = new Gender()
                {
                    KnownType = Gx.Types.GenderType.Female,
                },
                Names = new List<Name>()
                {
                    new Name()
                    {
                        KnownType = Gx.Types.NameType.BirthName,
                        NameForms = new List<NameForm>()
                        {
                            new NameForm()
                            {
                                FullText = "GedcomX User",
                                Parts = new List<NamePart>()
                                {
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Given,
                                        Value = "GedcomX",
                                    },
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Surname,
                                        Value = "User",
                                    }
                                }
                            },
                            new NameForm()
                            {
                                FullText = "GedcomX2 User",
                                Parts = new List<NamePart>()
                                {
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Given,
                                        Value = "GedcomX2",
                                    },
                                    new NamePart()
                                    {
                                        KnownType = Gx.Types.NamePartType.Surname,
                                        Value = "User",
                                    }
                                }
                            },
                        },
                        Preferred = true,
                    }
                },
                Facts = new List<Fact>()
                {
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Birth,
                        Date = new DateInfo()
                        {
                            Original = "June 1800",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Christening,
                        Date = new DateInfo()
                        {
                            Original = "1802",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "American Fork, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Residence,
                        Date = new DateInfo()
                        {
                            Original = "4 Jan 1896",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Death,
                        Date = new DateInfo()
                        {
                            Original = "July 14, 1900",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Provo, Utah, Utah, United States",
                        },
                    },
                    new Fact()
                    {
                        KnownType = Gx.Types.FactType.Burial,
                        Date = new DateInfo()
                        {
                            Original = "1900",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Sandy, Salt Lake, Utah, United States",
                        },
                    }
                }
            };
        }

        public static SourceReference GetPersonSourceReference()
        {
            return new SourceReference()
            {
                Attribution = new Attribution()
                {
                    ChangeMessage = "Family is at the same address found in other sources associated with this family.  Names are a good match.  Estimated births are reasonable.",
                },
                DescriptionRef = "https://sandbox.familysearch.org/platform/sources/descriptions/MMH1-PNF",
            };
        }

        public static Person GetCreatePersonConclusion(string personId)
        {
            return new Person()
            {
                Id = personId,
                Facts = new List<Fact>()
                {
                    new Fact()
                    {
                        Attribution = new Attribution()
                        {
                            ChangeMessage = "Change message",
                        },
                        KnownType = Gx.Types.FactType.Birth,
                        Date = new DateInfo()
                        {
                            Original = "3 Apr 1836",
                            Formal = "+1836",
                        },
                        Place = new PlaceReference()
                        {
                            Original = "Moscow, Russia",
                        },
                    }
                }
            };
        }

        public static Person GetCreateDiscussionReference(string personId)
        {
            return new Person()
            {
                Id = personId,
                Attribution = new Attribution()
                {
                    ChangeMessage = "Change message",
                },
                // TODO: discussion-references
            };
        }

        public static Note GetCreateNote(string contributorResourceId)
        {
            return new Note()
            {
                Subject = "Sample",
                Text = "Sample note text",
                Attribution = new Attribution()
                {
                    Contributor = new ResourceReference()
                    {
                        Resource = "https://familysearch.org/platform/users/agents/" + contributorResourceId,
                        ResourceId = contributorResourceId,
                    },
                },
            };
        }

        public static Fact GetBirthFact()
        {
            return new Fact()
            {
                Attribution = new Attribution()
                {
                    ChangeMessage = "Change message",
                },
                KnownType = Gx.Types.FactType.Birth,
                Date = new DateInfo()
                {
                    Original = "3 Apr 1836",
                    Formal = "+1836",
                },
                Place = new PlaceReference()
                {
                    Original = "Moscow, Russia",
                },
            };
        }

        public static Fact GetUpdateFact()
        {
            return new Fact()
            {
                Id = "N.777-7774",
                Attribution = new Attribution()
                {
                    ChangeMessage = "Change message",
                },
                Type = "data:,Eagle%20Scout",
                Qualifiers = new List<Qualifier>()
                {
                    new Qualifier()
                    {
                        Name = "http://familysearch.org/v1/Event",
                        Value = bool.FalseString.ToLower(),
                    },
                },
            };
        }

        public static Gx.Gedcomx GetCreatePersonLifeSketch(string personId)
        {
            return new Gx.Gedcomx()
            {
                Persons = new List<Person>()
                {
                    new Person()
                    {
                        Id = personId,
                        Facts = new List<Fact>()
                        {
                            new Fact()
                            {
                                Attribution = new Attribution()
                                {
                                    ChangeMessage = "...change message...",
                                },
                                Type = "http://familysearch.org/v1/LifeSketch",
                                Value = "What a long and colorful life this person had!\nDetails are numerous and humorous.",
                            },
                        },
                    },
                },
            };
        }

        public static Gx.Gedcomx GetUpdatePersonLifeSketch(string personId, string factId)
        {
            var result = GetCreatePersonLifeSketch(personId);

            result.Persons[0].Facts[0].Id = factId;

            return result;
        }

        public static string GetFactId(Person person, string factType)
        {
            string result = null;

            if (person != null && person.Facts != null)
            {
                result = person.Facts.Where(x => x.Type == factType).Select(x => x.Id).FirstOrDefault();
            }

            return result;
        }

        public static Fact GetMarriageFact()
        {
            return new Fact()
            {
                Attribution = new Attribution()
                {
                    ChangeMessage = "Change message",
                },
                KnownType = Gx.Types.FactType.Marriage,
                Date = new DateInfo()
                {
                    Original = "3 Apr 1930",
                    Formal = "+1930",
                },
                Place = new PlaceReference()
                {
                    Original = "Moscow, Russia",
                },
            };
        }

        public static Relationship GetCreateInvalidRelationship()
        {
            return new Relationship()
            {
                Links = new List<Link>()
                {
                    new Link()
                    {
                        Rel = "relationship",
                        Href = "https://sandbox.familysearch.org/platform/tree/couple-relationships/INVALID",
                    },
                },
            };
        }

        public static ChildAndParentsRelationship GetCreateChildAndParentsRelationship(PersonState father = null, PersonState mother = null, PersonState child = null)
        {
            var result = new ChildAndParentsRelationship();

            if (father != null)
            {
                result.Father = new ResourceReference(father.GetSelfUri());
            }

            if (mother != null)
            {
                result.Mother = new ResourceReference(mother.GetSelfUri());
            }

            if (child != null)
            {
                result.Child = new ResourceReference(child.GetSelfUri());
            }

            return result;
        }

        public static Fact GetBiologicalParentFact()
        {
            return new Fact()
            {
                Attribution = new Attribution()
                {
                    ChangeMessage = "Change message",
                },
                KnownType = Gx.Types.FactType.BiologicalParent,
            };
        }
    }
}
