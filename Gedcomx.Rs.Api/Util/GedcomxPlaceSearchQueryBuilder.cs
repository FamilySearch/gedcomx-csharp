using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public class GedcomxPlaceSearchQueryBuilder : GedcomxBaseSearchQueryBuilder
    {
        public static readonly String NAME = "name";
        public static readonly String DATE = "date";
        public static readonly String PARENT_ID = "parentId";
        public static readonly String TYPE_ID = "typeId";
        public static readonly String TYPE_GROUP_ID = "typeGroupId";
        public static readonly String LATITUDE = "latitude";
        public static readonly String LONGITUDE = "longitude";
        public static readonly String DISTANCE = "distance";

        public GedcomxPlaceSearchQueryBuilder Param(String name, String value)
        {
            return Param(name, value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Param(String name, String value, bool exact)
        {
            return Param(null, name, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder Param(String prefix, String name, String value, bool exact)
        {
            base.parameters.Add(new SearchParameter(null, name, value, exact));
            return this;
        }

        public GedcomxPlaceSearchQueryBuilder Name(String value)
        {
            return Name(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Name(String value, bool exact)
        {
            return Name(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder Name(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, NAME, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder NameNot(String value)
        {
            return Param("-", NAME, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder Date(String value)
        {
            return Date(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Date(String value, bool exact)
        {
            return Date(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder Date(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DATE, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder DateNot(String value)
        {
            return Param("-", DATE, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder ParentId(String value)
        {
            return ParentId(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder ParentId(String value, bool exact)
        {
            return ParentId(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder ParentId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, PARENT_ID, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder ParentIdNot(String value)
        {
            return Param("-", PARENT_ID, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder TypeId(String value)
        {
            return TypeId(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder TypeId(String value, bool exact)
        {
            return TypeId(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder TypeId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, TYPE_ID, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder TypeIdNot(String value)
        {
            return Param("-", TYPE_ID, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value)
        {
            return TypeGroupId(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value, bool exact)
        {
            return TypeGroupId(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder TypeGroupId(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, TYPE_GROUP_ID, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder TypeGroupIdNot(String value)
        {
            return Param("-", TYPE_GROUP_ID, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder Latitude(String value)
        {
            return Latitude(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Latitude(String value, bool exact)
        {
            return Latitude(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder Latitude(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, LATITUDE, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder LatitudeNot(String value)
        {
            return Param("-", LATITUDE, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder Longitude(String value)
        {
            return Longitude(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Longitude(String value, bool exact)
        {
            return Longitude(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder Longitude(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, LONGITUDE, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder LongitudeNot(String value)
        {
            return Param("-", LONGITUDE, value, false);
        }

        public GedcomxPlaceSearchQueryBuilder Distance(String value)
        {
            return Distance(value, true);
        }

        public GedcomxPlaceSearchQueryBuilder Distance(String value, bool exact)
        {
            return Distance(value, exact, false);
        }

        public GedcomxPlaceSearchQueryBuilder Distance(String value, bool exact, bool required)
        {
            return Param(required ? "+" : null, DISTANCE, value, exact);
        }

        public GedcomxPlaceSearchQueryBuilder DistanceNot(String value)
        {
            return Param("-", DISTANCE, value, false);
        }
    }
}
