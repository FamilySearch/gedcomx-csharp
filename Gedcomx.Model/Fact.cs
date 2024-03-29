﻿// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;
using System.Linq;

using Gedcomx.Model.Rt;
using Gedcomx.Model.Util;

using Gx.Common;
using Gx.Records;
using Gx.Types;

namespace Gx.Conclusion
{
    /// <remarks>
    ///  A conclusion about a fact applicable to a person or relationship.
    /// </remarks>
    /// <summary>
    ///  A conclusion about a fact applicable to a person or relationship.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Fact")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "fact")]
    public partial class Fact : Gx.Conclusion.Conclusion
    {
        private bool? _primary;
        private bool _primarySpecified;
        private string _type;
        private Gx.Conclusion.DateInfo _date;
        private Gx.Conclusion.PlaceReference _place;
        private string _value;
        private System.Collections.Generic.List<Gx.Common.Qualifier> _qualifiers;
        private System.Collections.Generic.List<Gx.Records.Field> _fields;

        public Fact()
        {
        }

        public Fact(FactType factType, String value)
        {
            SetType(factType);
            SetValue(value);
        }

        public Fact(FactType factType, String date, String place)
            : this(factType, new DateInfo().SetOriginal(date), new PlaceReference().SetOriginal(place), null)
        {
        }

        public Fact(FactType factType, DateInfo date, PlaceReference place)
            : this(factType, date, place, null)
        {
        }

        public Fact(FactType factType, DateInfo date, PlaceReference place, String value)
        {
            SetType(factType);
            SetDate(date);
            SetPlace(place);
            SetValue(value);
        }

        /// <summary>
        ///  Whether this fact is the primary fact of the record from which the subject was extracted.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "primary")]
        [Newtonsoft.Json.JsonProperty("primary")]
        public bool Primary
        {
            get
            {
                return _primary.GetValueOrDefault();
            }
            set
            {
                _primary = value;
                _primarySpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Primary" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool PrimarySpecified
        {
            get
            {
                return _primarySpecified;
            }
            set
            {
                _primarySpecified = value;
            }
        }

        /// <summary>
        ///  The type of the fact.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "type")]
        [Newtonsoft.Json.JsonProperty("type")]
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        ///  Convenience property for treating Type as an enum. See Gx.Types.FactTypeQNameUtil for details on getter/setter functionality.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [Newtonsoft.Json.JsonIgnore]
        public Gx.Types.FactType KnownType
        {
            get
            {
                return XmlQNameEnumUtil.GetEnumValue<FactType>(_type);
            }
            set
            {
                _type = XmlQNameEnumUtil.GetNameValue(value);
            }
        }
        /// <summary>
        ///  The date of applicability of this fact.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "date", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("date")]
        public Gx.Conclusion.DateInfo Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }
        /// <summary>
        ///  The place of applicability of this fact.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "place", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("place")]
        public Gx.Conclusion.PlaceReference Place
        {
            get
            {
                return _place;
            }
            set
            {
                _place = value;
            }
        }
        /// <summary>
        ///  The value as supplied by the user.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "value", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        /// <summary>
        ///  The qualifiers associated with this fact.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "qualifier", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("qualifiers")]
        public System.Collections.Generic.List<Gx.Common.Qualifier> Qualifiers
        {
            get
            {
                return _qualifiers ?? (_qualifiers = new System.Collections.Generic.List<Gx.Common.Qualifier>());
            }
            set
            {
                _qualifiers = value;
            }
        }
        public bool ShouldSerializeQualifiers() => AnyQualifiers();
        public bool AnyQualifiers()
        {
            return _qualifiers?.Any() ?? false;
        }
        /// <summary>
        ///  The references to the record fields being used as evidence.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "field", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("fields")]
        public System.Collections.Generic.List<Gx.Records.Field> Fields
        {
            get
            {
                return _fields ?? (_fields = new System.Collections.Generic.List<Gx.Records.Field>());
            }
            set
            {
                _fields = value;
            }
        }
        public bool ShouldSerializeFields() => AnyFields();
        public bool AnyFields()
        {
            return _fields?.Any() ?? false;
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitFact(this);
        }

        /**
         * Build up this fact with a type.
         *
         * @param type The type.
         * @return this
         */
        public Fact SetType(String type)
        {
            Type = type;
            return this;
        }

        /**
         * Build up this fact with a type.
         *
         * @param type The type.
         * @return this
         */
        public Fact SetType(FactType type)
        {
            KnownType = type;
            return this;
        }

        /**
         * Build up this fact with a 'primary' flag.
         *
         * @param primary The primary flag.
         * @return this.
         */
        public Fact SetPrimary(Boolean primary)
        {
            Primary = primary;
            return this;
        }

        /**
         * Build up this fact with a date.
         *
         * @param date the date.
         * @return this.
         */
        public Fact SetDate(DateInfo date)
        {
            Date = date;
            return this;
        }

        /**
         * Build up this fact with a place.
         *
         * @param place The place.
         * @return this.
         */
        public Fact SetPlace(PlaceReference place)
        {
            Place = place;
            return this;
        }

        /**
         * Build up this fact with a value.
         *
         * @param value The value.
         * @return this.
         */
        public Fact SetValue(String value)
        {
            Value = value;
            return this;
        }

        /**
         * Build up this fact with a qualifier.
         *
         * @param qualifier The qualifier.
         * @return this.
         */
        public Fact SetQualifier(Qualifier qualifier)
        {
            Qualifiers.Add(qualifier);
            return this;
        }

        /**
         * Build up this fact with a field.
         *
         * @param field The field.
         * @return this.
         */
        public Fact SetField(Field field)
        {
            if (field != null)
            {
                Fields.Add(field);
            }
            return this;
        }
    }
}
