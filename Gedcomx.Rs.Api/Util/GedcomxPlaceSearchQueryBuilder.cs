using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This is a helper utility for building syntactically correct place search query strings.
    /// </summary>
    public class GedcomxPlaceSearchQueryBuilder : GedcomxBaseSearchQueryBuilder
    {
        /// <summary>
        /// The name parameter in place search queries.
        /// </summary>
        public static readonly String NAME = "name";
        /// <summary>
        /// The date parameter in place search queries.
        /// </summary>
        public static readonly String DATE = "date";
        /// <summary>
        /// The parent parameter in place search queries.
        /// </summary>
        public static readonly String PARENT_ID = "parentId";
        /// <summary>
        /// The type parameter in place search queries.
        /// </summary>
        public static readonly String TYPE_ID = "typeId";
        /// <summary>
        /// The type group parameter in place search queries.
        /// </summary>
        public static readonly String TYPE_GROUP_ID = "typeGroupId";
        /// <summary>
        /// The latitude parameter in place search queries.
        /// </summary>
        public static readonly String LATITUDE = "latitude";
        /// <summary>
        /// The longitude parameter in place search queries.
        /// </summary>
        public static readonly String LONGITUDE = "longitude";
        /// <summary>
        /// The distance parameter in place search queries.
        /// </summary>
        public static readonly String DISTANCE = "distance";

        /// <summary>
        /// Creates a generic search parameter with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the search parameter.</param>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>A <see cref="GedcomxPlaceSearchQueryBuilder"/> containing the new parameter.</returns>
        public GedcomxPlaceSearchQueryBuilder Param(String name, String value)
        {
            return Param(name, value, true);
        }

        /// <summary>
        /// Creates a generic search parameter with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the search parameter.</param>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Param(String name, String value, bool exact)
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
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        /// <remarks>
        /// The prefix parameter can take on three forms:
        ///     "+": The parameter search value should be found in the search results
        ///     null: The parameter search filter is optional
        ///     "-": The parameter search value should not found in the search results (i.e., perform a NOT seaarch)
        /// </remarks>
        public GedcomxPlaceSearchQueryBuilder Param(String prefix, String name, String value, bool exact)
        {
            base.parameters.Add(new SearchParameter(null, name, value, exact));
            return this;
        }

        /// <summary>
        /// Creates a name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Name(String value)
        {
            return Name(value, true);
        }

        /// <summary>
        /// Creates a name search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Name(String value, bool exact)
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
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Name(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, NAME, value, exact);
        }

        /// <summary>
        /// Creates a "not name" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder NameNot(String value)
        {
            return Param("-", NAME, value, false);
        }

        /// <summary>
        /// Creates a date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Date(String value)
        {
            return Date(value, true);
        }

        /// <summary>
        /// Creates a date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Date(String value, bool exact)
        {
            return Date(value, exact, false);
        }

        /// <summary>
        /// Creates a date search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Date(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DATE, value, exact);
        }

        /// <summary>
        /// Creates a "not date" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder DateNot(String value)
        {
            return Param("-", DATE, value, false);
        }

        /// <summary>
        /// Creates a parent ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder ParentId(String value)
        {
            return ParentId(value, true);
        }

        /// <summary>
        /// Creates a parent ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder ParentId(String value, bool exact)
        {
            return ParentId(value, exact, false);
        }

        /// <summary>
        /// Creates a parent ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder ParentId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_ID, value, exact);
        }

        /// <summary>
        /// Creates a "not parent ID" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder ParentIdNot(String value)
        {
            return Param("-", PARENT_ID, value, false);
        }

        /// <summary>
        /// Creates a type ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeId(String value)
        {
            return TypeId(value, true);
        }

        /// <summary>
        /// Creates a type ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeId(String value, bool exact)
        {
            return TypeId(value, exact, false);
        }

        /// <summary>
        /// Creates a type ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, TYPE_ID, value, exact);
        }

        /// <summary>
        /// Creates a "not type ID" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeIdNot(String value)
        {
            return Param("-", TYPE_ID, value, false);
        }

        /// <summary>
        /// Creates a type group ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value)
        {
            return TypeGroupId(value, true);
        }

        /// <summary>
        /// Creates a type group ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value, bool exact)
        {
            return TypeGroupId(value, exact, false);
        }

        /// <summary>
        /// Creates a type group ID search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, TYPE_GROUP_ID, value, exact);
        }

        /// <summary>
        /// Creates a "not type group ID" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder TypeGroupIdNot(String value)
        {
            return Param("-", TYPE_GROUP_ID, value, false);
        }

        /// <summary>
        /// Creates a latitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Latitude(String value)
        {
            return Latitude(value, true);
        }

        /// <summary>
        /// Creates a latitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Latitude(String value, bool exact)
        {
            return Latitude(value, exact, false);
        }

        /// <summary>
        /// Creates a latitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Latitude(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, LATITUDE, value, exact);
        }

        /// <summary>
        /// Creates a "not latitude" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder LatitudeNot(String value)
        {
            return Param("-", LATITUDE, value, false);
        }

        /// <summary>
        /// Creates a longitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Longitude(String value)
        {
            return Longitude(value, true);
        }

        /// <summary>
        /// Creates a longitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Longitude(String value, bool exact)
        {
            return Longitude(value, exact, false);
        }

        /// <summary>
        /// Creates a longitude search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Longitude(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, LONGITUDE, value, exact);
        }

        /// <summary>
        /// Creates a "not longitude" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder LongitudeNot(String value)
        {
            return Param("-", LONGITUDE, value, false);
        }

        /// <summary>
        /// Creates a distance search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Distance(String value)
        {
            return Distance(value, true);
        }

        /// <summary>
        /// Creates a distance search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Distance(String value, bool exact)
        {
            return Distance(value, exact, false);
        }

        /// <summary>
        /// Creates a distance search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
        /// <param name="required">If set to <c>true</c> then search results must satisfy this search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder Distance(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DISTANCE, value, exact);
        }

        /// <summary>
        /// Creates a "not distance" search parameter with the search value.
        /// </summary>
        /// <param name="value">The value of the search parameter.</param>
        /// <returns>
        /// A <see cref="GedcomxPlaceSearchQueryBuilder" /> containing the new parameter.
        /// </returns>
        public GedcomxPlaceSearchQueryBuilder DistanceNot(String value)
        {
            return Param("-", DISTANCE, value, false);
        }
    }
}
