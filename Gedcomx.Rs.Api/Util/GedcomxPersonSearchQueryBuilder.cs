using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public class GedcomxPersonSearchQueryBuilder : GedcomxBaseSearchQueryBuilder
    {
        public static readonly String NAME = "name";
        public static readonly String GIVEN_NAME = "givenName";
        public static readonly String SURNAME = "surname";
        public static readonly String GENDER = "gender";
        public static readonly String BIRTH_DATE = "birthDate";
        public static readonly String BIRTH_PLACE = "birthPlace";
        public static readonly String DEATH_DATE = "deathDate";
        public static readonly String DEATH_PLACE = "deathPlace";
        public static readonly String MARRIAGE_DATE = "marriageDate";
        public static readonly String MARRIAGE_PLACE = "marriagePlace";
        public static readonly String FATHER_NAME = "fatherName";
        public static readonly String FATHER_GIVEN_NAME = "fatherGivenName";
        public static readonly String FATHER_SURNAME = "fatherSurname";
        public static readonly String FATHER_GENDER = "fatherGender";
        public static readonly String FATHER_BIRTH_DATE = "fatherBirthDate";
        public static readonly String FATHER_BIRTH_PLACE = "fatherBirthPlace";
        public static readonly String FATHER_DEATH_DATE = "fatherDeathDate";
        public static readonly String FATHER_DEATH_PLACE = "fatherDeathPlace";
        public static readonly String FATHER_MARRIAGE_DATE = "fatherMarriageDate";
        public static readonly String FATHER_MARRIAGE_PLACE = "fatherMarriagePlace";
        public static readonly String MOTHER_NAME = "motherName";
        public static readonly String MOTHER_GIVEN_NAME = "motherGivenName";
        public static readonly String MOTHER_SURNAME = "motherSurname";
        public static readonly String MOTHER_GENDER = "motherGender";
        public static readonly String MOTHER_BIRTH_DATE = "motherBirthDate";
        public static readonly String MOTHER_BIRTH_PLACE = "motherBirthPlace";
        public static readonly String MOTHER_DEATH_DATE = "motherDeathDate";
        public static readonly String MOTHER_DEATH_PLACE = "motherDeathPlace";
        public static readonly String MOTHER_MARRIAGE_DATE = "motherMarriageDate";
        public static readonly String MOTHER_MARRIAGE_PLACE = "motherMarriagePlace";
        public static readonly String SPOUSE_NAME = "spouseName";
        public static readonly String SPOUSE_GIVEN_NAME = "spouseGivenName";
        public static readonly String SPOUSE_SURNAME = "spouseSurname";
        public static readonly String SPOUSE_GENDER = "spouseGender";
        public static readonly String SPOUSE_BIRTH_DATE = "spouseBirthDate";
        public static readonly String SPOUSE_BIRTH_PLACE = "spouseBirthPlace";
        public static readonly String SPOUSE_DEATH_DATE = "spouseDeathDate";
        public static readonly String SPOUSE_DEATH_PLACE = "spouseDeathPlace";
        public static readonly String SPOUSE_MARRIAGE_DATE = "spouseMarriageDate";
        public static readonly String SPOUSE_MARRIAGE_PLACE = "spouseMarriagePlace";
        public static readonly String PARENT_NAME = "parentName";
        public static readonly String PARENT_GIVEN_NAME = "parentGivenName";
        public static readonly String PARENT_SURNAME = "parentSurname";
        public static readonly String PARENT_GENDER = "parentGender";
        public static readonly String PARENT_BIRTH_DATE = "parentBirthDate";
        public static readonly String PARENT_BIRTH_PLACE = "parentBirthPlace";
        public static readonly String PARENT_DEATH_DATE = "parentDeathDate";
        public static readonly String PARENT_DEATH_PLACE = "parentDeathPlace";
        public static readonly String PARENT_MARRIAGE_DATE = "parentMarriageDate";
        public static readonly String PARENT_MARRIAGE_PLACE = "parentMarriagePlace";

        public GedcomxPersonSearchQueryBuilder Param(String name, String value)
        {
            return Param(name, value, false);
        }

        public GedcomxPersonSearchQueryBuilder Param(String name, String value, bool exact)
        {
            return Param(null, name, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder Param(String prefix, String name, String value, bool exact)
        {
            base.parameters.Add(new SearchParameter(prefix, name, value, exact));
            return this;
        }

        public GedcomxPersonSearchQueryBuilder Name(String value)
        {
            return Name(value, false);
        }

        public GedcomxPersonSearchQueryBuilder Name(String value, bool exact)
        {
            return Name(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder Name(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder GivenName(String value)
        {
            return GivenName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder GivenName(String value, bool exact)
        {
            return GivenName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder GivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, GIVEN_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder Surname(String value)
        {
            return Surname(value, false);
        }

        public GedcomxPersonSearchQueryBuilder Surname(String value, bool exact)
        {
            return Surname(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder Surname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SURNAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder Gender(String value)
        {
            return Gender(value, false);
        }

        public GedcomxPersonSearchQueryBuilder Gender(String value, bool exact)
        {
            return Gender(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder Gender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, GENDER, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder BirthDate(String value)
        {
            return BirthDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder BirthDate(String value, bool exact)
        {
            return BirthDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder BirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, BIRTH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder BirthPlace(String value)
        {
            return BirthPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder BirthPlace(String value, bool exact)
        {
            return BirthPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder BirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, BIRTH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder DeathDate(String value)
        {
            return DeathDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder DeathDate(String value, bool exact)
        {
            return DeathDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder DeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DEATH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder DeathPlace(String value)
        {
            return DeathPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder DeathPlace(String value, bool exact)
        {
            return DeathPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder DeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DEATH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MarriageDate(String value)
        {
            return MarriageDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MarriageDate(String value, bool exact)
        {
            return MarriageDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MARRIAGE_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value)
        {
            return MarriagePlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value, bool exact)
        {
            return MarriagePlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MARRIAGE_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherName(String value)
        {
            return FatherName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherName(String value, bool exact)
        {
            return FatherName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value)
        {
            return FatherGivenName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value, bool exact)
        {
            return FatherGivenName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_GIVEN_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherSurname(String value)
        {
            return FatherSurname(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherSurname(String value, bool exact)
        {
            return FatherSurname(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_SURNAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherGender(String value)
        {
            return FatherGender(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherGender(String value, bool exact)
        {
            return FatherGender(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_GENDER, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value)
        {
            return FatherBirthDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value, bool exact)
        {
            return FatherBirthDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_BIRTH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value)
        {
            return FatherBirthPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value, bool exact)
        {
            return FatherBirthPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_BIRTH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value)
        {
            return FatherDeathDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value, bool exact)
        {
            return FatherDeathDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_DEATH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value)
        {
            return FatherDeathPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value, bool exact)
        {
            return FatherDeathPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_DEATH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value)
        {
            return FatherMarriageDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value, bool exact)
        {
            return FatherMarriageDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_MARRIAGE_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value)
        {
            return FatherMarriagePlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value, bool exact)
        {
            return FatherMarriagePlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_MARRIAGE_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherName(String value)
        {
            return MotherName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherName(String value, bool exact)
        {
            return MotherName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value)
        {
            return MotherGivenName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value, bool exact)
        {
            return MotherGivenName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_GIVEN_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherSurname(String value)
        {
            return MotherSurname(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherSurname(String value, bool exact)
        {
            return MotherSurname(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_SURNAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherGender(String value)
        {
            return MotherGender(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherGender(String value, bool exact)
        {
            return MotherGender(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_GENDER, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value)
        {
            return MotherBirthDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value, bool exact)
        {
            return MotherBirthDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_BIRTH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value)
        {
            return MotherBirthPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value, bool exact)
        {
            return MotherBirthPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_BIRTH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value)
        {
            return MotherDeathDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value, bool exact)
        {
            return MotherDeathDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_DEATH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value)
        {
            return MotherDeathPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value, bool exact)
        {
            return MotherDeathPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_DEATH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value)
        {
            return MotherMarriageDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value, bool exact)
        {
            return MotherMarriageDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_MARRIAGE_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value)
        {
            return MotherMarriagePlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value, bool exact)
        {
            return MotherMarriagePlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_MARRIAGE_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseName(String value)
        {
            return SpouseName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseName(String value, bool exact)
        {
            return SpouseName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value)
        {
            return SpouseGivenName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value, bool exact)
        {
            return SpouseGivenName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_GIVEN_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value)
        {
            return SpouseSurname(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value, bool exact)
        {
            return SpouseSurname(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_SURNAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGender(String value)
        {
            return SpouseGender(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGender(String value, bool exact)
        {
            return SpouseGender(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_GENDER, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value)
        {
            return SpouseBirthDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value, bool exact)
        {
            return SpouseBirthDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_BIRTH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value)
        {
            return SpouseBirthPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value, bool exact)
        {
            return SpouseBirthPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_BIRTH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value)
        {
            return SpouseDeathDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value, bool exact)
        {
            return SpouseDeathDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_DEATH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value)
        {
            return SpouseDeathPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value, bool exact)
        {
            return SpouseDeathPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_DEATH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value)
        {
            return SpouseMarriageDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value, bool exact)
        {
            return SpouseMarriageDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_MARRIAGE_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value)
        {
            return SpouseMarriagePlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value, bool exact)
        {
            return SpouseMarriagePlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_MARRIAGE_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentName(String value)
        {
            return ParentName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentName(String value, bool exact)
        {
            return ParentName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value)
        {
            return ParentGivenName(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value, bool exact)
        {
            return ParentGivenName(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_GIVEN_NAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentSurname(String value)
        {
            return ParentSurname(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentSurname(String value, bool exact)
        {
            return ParentSurname(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_SURNAME, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentGender(String value)
        {
            return ParentGender(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentGender(String value, bool exact)
        {
            return ParentGender(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_GENDER, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value)
        {
            return ParentBirthDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value, bool exact)
        {
            return ParentBirthDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_BIRTH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value)
        {
            return ParentBirthPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value, bool exact)
        {
            return ParentBirthPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_BIRTH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value)
        {
            return ParentDeathDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value, bool exact)
        {
            return ParentDeathDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_DEATH_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value)
        {
            return ParentDeathPlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value, bool exact)
        {
            return ParentDeathPlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_DEATH_PLACE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value)
        {
            return ParentMarriageDate(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value, bool exact)
        {
            return ParentMarriageDate(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_MARRIAGE_DATE, value, exact);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value)
        {
            return ParentMarriagePlace(value, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value, bool exact)
        {
            return ParentMarriagePlace(value, exact, false);
        }

        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_MARRIAGE_PLACE, value, exact);
        }
    }
}
