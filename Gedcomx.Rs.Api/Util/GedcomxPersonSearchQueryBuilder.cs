using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This is a helper utility for building syntactically correct person search query strings.
    /// </summary>
    public class GedcomxPersonSearchQueryBuilder : GedcomxBaseSearchQueryBuilder
    {
        /// <summary>
        /// The name parameter in person search queries
        /// </summary>
        public static readonly String NAME = "name";
        /// <summary>
        /// The given name parameter in person search queries
        /// </summary>
        public static readonly String GIVEN_NAME = "givenName";
        /// <summary>
        /// The surname parameter in person search queries
        /// </summary>
        public static readonly String SURNAME = "surname";
        /// <summary>
        /// The gender parameter in person search queries
        /// </summary>
        public static readonly String GENDER = "gender";
        /// <summary>
        /// The birth date parameter in person search queries
        /// </summary>
        public static readonly String BIRTH_DATE = "birthDate";
        /// <summary>
        /// The birth place parameter in person search queries
        /// </summary>
        public static readonly String BIRTH_PLACE = "birthPlace";
        /// <summary>
        /// The death date parameter in person search queries
        /// </summary>
        public static readonly String DEATH_DATE = "deathDate";
        /// <summary>
        /// The death place parameter in person search queries
        /// </summary>
        public static readonly String DEATH_PLACE = "deathPlace";
        /// <summary>
        /// The marriage date parameter in person search queries
        /// </summary>
        public static readonly String MARRIAGE_DATE = "marriageDate";
        /// <summary>
        /// The marriage place parameter in person search queries
        /// </summary>
        public static readonly String MARRIAGE_PLACE = "marriagePlace";
        /// <summary>
        /// The father name parameter in person search queries
        /// </summary>
        public static readonly String FATHER_NAME = "fatherName";
        /// <summary>
        /// The father given name parameter in person search queries
        /// </summary>
        public static readonly String FATHER_GIVEN_NAME = "fatherGivenName";
        /// <summary>
        /// The father surname parameter in person search queries
        /// </summary>
        public static readonly String FATHER_SURNAME = "fatherSurname";
        /// <summary>
        /// The father gender parameter in person search queries
        /// </summary>
        public static readonly String FATHER_GENDER = "fatherGender";
        /// <summary>
        /// The father birth date parameter in person search queries
        /// </summary>
        public static readonly String FATHER_BIRTH_DATE = "fatherBirthDate";
        /// <summary>
        /// The father birth place parameter in person search queries
        /// </summary>
        public static readonly String FATHER_BIRTH_PLACE = "fatherBirthPlace";
        /// <summary>
        /// The father death date parameter in person search queries
        /// </summary>
        public static readonly String FATHER_DEATH_DATE = "fatherDeathDate";
        /// <summary>
        /// The father death place parameter in person search queries
        /// </summary>
        public static readonly String FATHER_DEATH_PLACE = "fatherDeathPlace";
        /// <summary>
        /// The father marriage date parameter in person search queries
        /// </summary>
        public static readonly String FATHER_MARRIAGE_DATE = "fatherMarriageDate";
        /// <summary>
        /// The father marriage place parameter in person search queries
        /// </summary>
        public static readonly String FATHER_MARRIAGE_PLACE = "fatherMarriagePlace";
        /// <summary>
        /// The mother name parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_NAME = "motherName";
        /// <summary>
        /// The mother given name parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_GIVEN_NAME = "motherGivenName";
        /// <summary>
        /// The mother surname parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_SURNAME = "motherSurname";
        /// <summary>
        /// The mother gender parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_GENDER = "motherGender";
        /// <summary>
        /// The mother birth date parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_BIRTH_DATE = "motherBirthDate";
        /// <summary>
        /// The mother birth place parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_BIRTH_PLACE = "motherBirthPlace";
        /// <summary>
        /// The mother death date parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_DEATH_DATE = "motherDeathDate";
        /// <summary>
        /// The mother death place parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_DEATH_PLACE = "motherDeathPlace";
        /// <summary>
        /// The mother marriage date parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_MARRIAGE_DATE = "motherMarriageDate";
        /// <summary>
        /// The mother marriage place parameter in person search queries
        /// </summary>
        public static readonly String MOTHER_MARRIAGE_PLACE = "motherMarriagePlace";
        /// <summary>
        /// The spouse name parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_NAME = "spouseName";
        /// <summary>
        /// The spouse given name parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_GIVEN_NAME = "spouseGivenName";
        /// <summary>
        /// The spouse surname parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_SURNAME = "spouseSurname";
        /// <summary>
        /// The spouse gender parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_GENDER = "spouseGender";
        /// <summary>
        /// The spouse birth date parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_BIRTH_DATE = "spouseBirthDate";
        /// <summary>
        /// The spouse birth place parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_BIRTH_PLACE = "spouseBirthPlace";
        /// <summary>
        /// The spouse death date parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_DEATH_DATE = "spouseDeathDate";
        /// <summary>
        /// The spouse death place parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_DEATH_PLACE = "spouseDeathPlace";
        /// <summary>
        /// The spouse marriage date parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_MARRIAGE_DATE = "spouseMarriageDate";
        /// <summary>
        /// The spouse marriage place parameter in person search queries
        /// </summary>
        public static readonly String SPOUSE_MARRIAGE_PLACE = "spouseMarriagePlace";
        /// <summary>
        /// The parent name parameter in person search queries
        /// </summary>
        public static readonly String PARENT_NAME = "parentName";
        /// <summary>
        /// The parent given name parameter in person search queries
        /// </summary>
        public static readonly String PARENT_GIVEN_NAME = "parentGivenName";
        /// <summary>
        /// The parent surname parameter in person search queries
        /// </summary>
        public static readonly String PARENT_SURNAME = "parentSurname";
        /// <summary>
        /// The parent gender parameter in person search queries
        /// </summary>
        public static readonly String PARENT_GENDER = "parentGender";
        /// <summary>
        /// The parent birth date parameter in person search queries
        /// </summary>
        public static readonly String PARENT_BIRTH_DATE = "parentBirthDate";
        /// <summary>
        /// The parent birth place parameter in person search queries
        /// </summary>
        public static readonly String PARENT_BIRTH_PLACE = "parentBirthPlace";
        /// <summary>
        /// The parent death date parameter in person search queries
        /// </summary>
        public static readonly String PARENT_DEATH_DATE = "parentDeathDate";
        /// <summary>
        /// The parent death place parameter in person search queries
        /// </summary>
        public static readonly String PARENT_DEATH_PLACE = "parentDeathPlace";
        /// <summary>
        /// The parent marriage date parameter in person search queries
        /// </summary>
        public static readonly String PARENT_MARRIAGE_DATE = "parentMarriageDate";
        /// <summary>
        /// The parent marriage place parameter in person search queries
        /// </summary>
        public static readonly String PARENT_MARRIAGE_PLACE = "parentMarriagePlace";

        /// <summary>
        /// Creates a generic search parameter with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the search parameter.</param>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>A <see cref="GedcomxPersonSearchQueryBuilder"/> containing the new parameter.</returns>
        public GedcomxPersonSearchQueryBuilder Param(String name, String value)
        {
            return Param(name, value, false);
        }

        /// <summary>
        /// Creates a generic search parameter with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the search parameter.</param>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Param(String name, String value, bool exact)
        {
            return Param(null, name, value, exact);
        }

        /// <summary>
        /// Creates a generic search parameter with the specified name and value.
        /// </summary>
        /// <param name="prefix">The prefix to apply to the search parameter. This is used for controlling whether a parameter is required or not. See remarks.</param>
        /// <param name="name">The name of the search parameter.</param>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        /// <remarks>
        /// The prefix parameter can take on three forms:
        ///     "+": The parameter search value should be found in the search results
        ///     null: The parameter search filter is optional
        ///     "-": The parameter search value should not found in the search results (i.e., perform a NOT seaarch)
        /// </remarks>
        public GedcomxPersonSearchQueryBuilder Param(String prefix, String name, String value, bool exact)
        {
            base.parameters.Add(new SearchParameter(prefix, name, value, exact));
            return this;
        }

        /// <summary>
        /// Creates a name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Name(String value)
        {
            return Name(value, false);
        }

        /// <summary>
        /// Creates a name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Name(String value, bool exact)
        {
            return Name(value, exact, false);
        }

        /// <summary>
        /// Creates a name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Name(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, NAME, value, exact);
        }

        /// <summary>
        /// Creates a given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder GivenName(String value)
        {
            return GivenName(value, false);
        }

        /// <summary>
        /// Creates a given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder GivenName(String value, bool exact)
        {
            return GivenName(value, exact, false);
        }

        /// <summary>
        /// Creates a given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder GivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, GIVEN_NAME, value, exact);
        }

        /// <summary>
        /// Creates a surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Surname(String value)
        {
            return Surname(value, false);
        }

        /// <summary>
        /// Creates a surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Surname(String value, bool exact)
        {
            return Surname(value, exact, false);
        }

        /// <summary>
        /// Creates a surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Surname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SURNAME, value, exact);
        }

        /// <summary>
        /// Creates a gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Gender(String value)
        {
            return Gender(value, false);
        }

        /// <summary>
        /// Creates a gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Gender(String value, bool exact)
        {
            return Gender(value, exact, false);
        }

        /// <summary>
        /// Creates a gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder Gender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, GENDER, value, exact);
        }

        /// <summary>
        /// Creates a birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthDate(String value)
        {
            return BirthDate(value, false);
        }

        /// <summary>
        /// Creates a birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthDate(String value, bool exact)
        {
            return BirthDate(value, exact, false);
        }

        /// <summary>
        /// Creates a birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, BIRTH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthPlace(String value)
        {
            return BirthPlace(value, false);
        }

        /// <summary>
        /// Creates a birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthPlace(String value, bool exact)
        {
            return BirthPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder BirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, BIRTH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathDate(String value)
        {
            return DeathDate(value, false);
        }

        /// <summary>
        /// Creates a death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathDate(String value, bool exact)
        {
            return DeathDate(value, exact, false);
        }

        /// <summary>
        /// Creates a death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DEATH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathPlace(String value)
        {
            return DeathPlace(value, false);
        }

        /// <summary>
        /// Creates a death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathPlace(String value, bool exact)
        {
            return DeathPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder DeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DEATH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriageDate(String value)
        {
            return MarriageDate(value, false);
        }

        /// <summary>
        /// Creates a marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriageDate(String value, bool exact)
        {
            return MarriageDate(value, exact, false);
        }

        /// <summary>
        /// Creates a marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MARRIAGE_DATE, value, exact);
        }

        /// <summary>
        /// Creates a marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value)
        {
            return MarriagePlace(value, false);
        }

        /// <summary>
        /// Creates a marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value, bool exact)
        {
            return MarriagePlace(value, exact, false);
        }

        /// <summary>
        /// Creates a marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MARRIAGE_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a father name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherName(String value)
        {
            return FatherName(value, false);
        }

        /// <summary>
        /// Creates a father name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherName(String value, bool exact)
        {
            return FatherName(value, exact, false);
        }

        /// <summary>
        /// Creates a father name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_NAME, value, exact);
        }

        /// <summary>
        /// Creates a father given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value)
        {
            return FatherGivenName(value, false);
        }

        /// <summary>
        /// Creates a father given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value, bool exact)
        {
            return FatherGivenName(value, exact, false);
        }

        /// <summary>
        /// Creates a father given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_GIVEN_NAME, value, exact);
        }

        /// <summary>
        /// Creates a father surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherSurname(String value)
        {
            return FatherSurname(value, false);
        }

        /// <summary>
        /// Creates a father surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherSurname(String value, bool exact)
        {
            return FatherSurname(value, exact, false);
        }

        /// <summary>
        /// Creates a father surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_SURNAME, value, exact);
        }

        /// <summary>
        /// Creates a father gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGender(String value)
        {
            return FatherGender(value, false);
        }

        /// <summary>
        /// Creates a father gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGender(String value, bool exact)
        {
            return FatherGender(value, exact, false);
        }

        /// <summary>
        /// Creates a father gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_GENDER, value, exact);
        }

        /// <summary>
        /// Creates a father birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value)
        {
            return FatherBirthDate(value, false);
        }

        /// <summary>
        /// Creates a father birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value, bool exact)
        {
            return FatherBirthDate(value, exact, false);
        }

        /// <summary>
        /// Creates a father birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_BIRTH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a father birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value)
        {
            return FatherBirthPlace(value, false);
        }

        /// <summary>
        /// Creates a father birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value, bool exact)
        {
            return FatherBirthPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a father birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_BIRTH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a father death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value)
        {
            return FatherDeathDate(value, false);
        }

        /// <summary>
        /// Creates a father death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value, bool exact)
        {
            return FatherDeathDate(value, exact, false);
        }

        /// <summary>
        /// Creates a father death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_DEATH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a father death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value)
        {
            return FatherDeathPlace(value, false);
        }

        /// <summary>
        /// Creates a father death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value, bool exact)
        {
            return FatherDeathPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a father death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_DEATH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a father marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value)
        {
            return FatherMarriageDate(value, false);
        }

        /// <summary>
        /// Creates a father marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value, bool exact)
        {
            return FatherMarriageDate(value, exact, false);
        }

        /// <summary>
        /// Creates a father marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_MARRIAGE_DATE, value, exact);
        }

        /// <summary>
        /// Creates a father marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value)
        {
            return FatherMarriagePlace(value, false);
        }

        /// <summary>
        /// Creates a father marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value, bool exact)
        {
            return FatherMarriagePlace(value, exact, false);
        }

        /// <summary>
        /// Creates a father marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder FatherMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, FATHER_MARRIAGE_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a mother name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherName(String value)
        {
            return MotherName(value, false);
        }

        /// <summary>
        /// Creates a mother name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherName(String value, bool exact)
        {
            return MotherName(value, exact, false);
        }

        /// <summary>
        /// Creates a mother name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_NAME, value, exact);
        }

        /// <summary>
        /// Creates a mother given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value)
        {
            return MotherGivenName(value, false);
        }

        /// <summary>
        /// Creates a mother given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value, bool exact)
        {
            return MotherGivenName(value, exact, false);
        }

        /// <summary>
        /// Creates a mother given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_GIVEN_NAME, value, exact);
        }

        /// <summary>
        /// Creates a mother surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherSurname(String value)
        {
            return MotherSurname(value, false);
        }

        /// <summary>
        /// Creates a mother surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherSurname(String value, bool exact)
        {
            return MotherSurname(value, exact, false);
        }

        /// <summary>
        /// Creates a mother surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_SURNAME, value, exact);
        }

        /// <summary>
        /// Creates a mother gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGender(String value)
        {
            return MotherGender(value, false);
        }

        /// <summary>
        /// Creates a mother gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGender(String value, bool exact)
        {
            return MotherGender(value, exact, false);
        }

        /// <summary>
        /// Creates a mother gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_GENDER, value, exact);
        }

        /// <summary>
        /// Creates a mother birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value)
        {
            return MotherBirthDate(value, false);
        }

        /// <summary>
        /// Creates a mother birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value, bool exact)
        {
            return MotherBirthDate(value, exact, false);
        }

        /// <summary>
        /// Creates a mother birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_BIRTH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a mother birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value)
        {
            return MotherBirthPlace(value, false);
        }

        /// <summary>
        /// Creates a mother birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value, bool exact)
        {
            return MotherBirthPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a mother birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_BIRTH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a mother death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value)
        {
            return MotherDeathDate(value, false);
        }

        /// <summary>
        /// Creates a mother death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value, bool exact)
        {
            return MotherDeathDate(value, exact, false);
        }

        /// <summary>
        /// Creates a mother death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_DEATH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a mother death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value)
        {
            return MotherDeathPlace(value, false);
        }

        /// <summary>
        /// Creates a mother death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value, bool exact)
        {
            return MotherDeathPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a mother death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_DEATH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a mother marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value)
        {
            return MotherMarriageDate(value, false);
        }

        /// <summary>
        /// Creates a mother marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value, bool exact)
        {
            return MotherMarriageDate(value, exact, false);
        }

        /// <summary>
        /// Creates a mother marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_MARRIAGE_DATE, value, exact);
        }

        /// <summary>
        /// Creates a mother marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value)
        {
            return MotherMarriagePlace(value, false);
        }

        /// <summary>
        /// Creates a mother marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value, bool exact)
        {
            return MotherMarriagePlace(value, exact, false);
        }

        /// <summary>
        /// Creates a mother marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder MotherMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, MOTHER_MARRIAGE_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a spouse name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseName(String value)
        {
            return SpouseName(value, false);
        }

        /// <summary>
        /// Creates a spouse name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseName(String value, bool exact)
        {
            return SpouseName(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_NAME, value, exact);
        }

        /// <summary>
        /// Creates a spouse given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value)
        {
            return SpouseGivenName(value, false);
        }

        /// <summary>
        /// Creates a spouse given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value, bool exact)
        {
            return SpouseGivenName(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_GIVEN_NAME, value, exact);
        }

        /// <summary>
        /// Creates a spouse surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value)
        {
            return SpouseSurname(value, false);
        }

        /// <summary>
        /// Creates a spouse surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value, bool exact)
        {
            return SpouseSurname(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_SURNAME, value, exact);
        }

        /// <summary>
        /// Creates a spouse gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGender(String value)
        {
            return SpouseGender(value, false);
        }

        /// <summary>
        /// Creates a spouse gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGender(String value, bool exact)
        {
            return SpouseGender(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_GENDER, value, exact);
        }

        /// <summary>
        /// Creates a spouse birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value)
        {
            return SpouseBirthDate(value, false);
        }

        /// <summary>
        /// Creates a spouse birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value, bool exact)
        {
            return SpouseBirthDate(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_BIRTH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a spouse birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value)
        {
            return SpouseBirthPlace(value, false);
        }

        /// <summary>
        /// Creates a spouse birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value, bool exact)
        {
            return SpouseBirthPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_BIRTH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a spouse death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value)
        {
            return SpouseDeathDate(value, false);
        }

        /// <summary>
        /// Creates a spouse death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value, bool exact)
        {
            return SpouseDeathDate(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_DEATH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a spouse death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value)
        {
            return SpouseDeathPlace(value, false);
        }

        /// <summary>
        /// Creates a spouse death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value, bool exact)
        {
            return SpouseDeathPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_DEATH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a spouse marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value)
        {
            return SpouseMarriageDate(value, false);
        }

        /// <summary>
        /// Creates a spouse marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value, bool exact)
        {
            return SpouseMarriageDate(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_MARRIAGE_DATE, value, exact);
        }

        /// <summary>
        /// Creates a spouse marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value)
        {
            return SpouseMarriagePlace(value, false);
        }

        /// <summary>
        /// Creates a spouse marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value, bool exact)
        {
            return SpouseMarriagePlace(value, exact, false);
        }

        /// <summary>
        /// Creates a spouse marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder SpouseMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, SPOUSE_MARRIAGE_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a parent name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentName(String value)
        {
            return ParentName(value, false);
        }

        /// <summary>
        /// Creates a parent name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentName(String value, bool exact)
        {
            return ParentName(value, exact, false);
        }

        /// <summary>
        /// Creates a parent name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_NAME, value, exact);
        }

        /// <summary>
        /// Creates a parent given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value)
        {
            return ParentGivenName(value, false);
        }

        /// <summary>
        /// Creates a parent given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value, bool exact)
        {
            return ParentGivenName(value, exact, false);
        }

        /// <summary>
        /// Creates a parent given name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGivenName(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_GIVEN_NAME, value, exact);
        }

        /// <summary>
        /// Creates a parent surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentSurname(String value)
        {
            return ParentSurname(value, false);
        }

        /// <summary>
        /// Creates a parent surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentSurname(String value, bool exact)
        {
            return ParentSurname(value, exact, false);
        }

        /// <summary>
        /// Creates a parent surname search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentSurname(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_SURNAME, value, exact);
        }

        /// <summary>
        /// Creates a parent gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGender(String value)
        {
            return ParentGender(value, false);
        }

        /// <summary>
        /// Creates a parent gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGender(String value, bool exact)
        {
            return ParentGender(value, exact, false);
        }

        /// <summary>
        /// Creates a parent gender search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentGender(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_GENDER, value, exact);
        }

        /// <summary>
        /// Creates a parent birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value)
        {
            return ParentBirthDate(value, false);
        }

        /// <summary>
        /// Creates a parent birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value, bool exact)
        {
            return ParentBirthDate(value, exact, false);
        }

        /// <summary>
        /// Creates a parent birth date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_BIRTH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a parent birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value)
        {
            return ParentBirthPlace(value, false);
        }

        /// <summary>
        /// Creates a parent birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value, bool exact)
        {
            return ParentBirthPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a parent birth place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentBirthPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_BIRTH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a parent death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value)
        {
            return ParentDeathDate(value, false);
        }

        /// <summary>
        /// Creates a parent death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value, bool exact)
        {
            return ParentDeathDate(value, exact, false);
        }

        /// <summary>
        /// Creates a parent death date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_DEATH_DATE, value, exact);
        }

        /// <summary>
        /// Creates a parent death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value)
        {
            return ParentDeathPlace(value, false);
        }

        /// <summary>
        /// Creates a parent death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value, bool exact)
        {
            return ParentDeathPlace(value, exact, false);
        }

        /// <summary>
        /// Creates a parent death place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentDeathPlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_DEATH_PLACE, value, exact);
        }

        /// <summary>
        /// Creates a parent marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value)
        {
            return ParentMarriageDate(value, false);
        }

        /// <summary>
        /// Creates a parent marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value, bool exact)
        {
            return ParentMarriageDate(value, exact, false);
        }

        /// <summary>
        /// Creates a parent marriage date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriageDate(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_MARRIAGE_DATE, value, exact);
        }

        /// <summary>
        /// Creates a parent marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value)
        {
            return ParentMarriagePlace(value, false);
        }

        /// <summary>
        /// Creates a parent marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value, bool exact)
        {
            return ParentMarriagePlace(value, exact, false);
        }

        /// <summary>
        /// Creates a parent marriage place search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPersonSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPersonSearchQueryBuilder ParentMarriagePlace(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_MARRIAGE_PLACE, value, exact);
        }
    }
}
