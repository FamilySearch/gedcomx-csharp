using Gx.Common;
using Gx.Conclusion;
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

        public static SourceReference GetPersonSourceReference(string id = null)
        {
            return new SourceReference()
            {
                Id = id,
                Attribution = new Attribution()
                {
                    ChangeMessage = "Family is at the same address found in other sources associated with this family.  Names are a good match.  Estimated births are reasonable.",
                },
                DescriptionRef = "https://sandbox.familysearch.org/platform/sources/descriptions/MMH1-PNF",
            };
        }

        public static SourceReference GetPersonSourceReference2()
        {
            return new SourceReference()
            {
                Attribution = new Attribution()
                {
                    ChangeMessage = "Family is at the same address found in other sources associated with this family.  Names are a good match.  Estimated births are reasonable.",
                },
                DescriptionRef = "https://sandbox.familysearch.org/platform/sources/descriptions/MMH1-PP4",
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

        public static Fact GetNewFact()
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
    }
}
