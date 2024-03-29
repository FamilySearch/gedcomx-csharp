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
    ///  A part of a name.
    /// </remarks>
    /// <summary>
    ///  A part of a name.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "NamePart")]
    public sealed partial class NamePart : Gx.Common.ExtensibleData
    {

        private string _value;
        private string _type;
        private System.Collections.Generic.List<Gx.Records.Field> _fields;
        private System.Collections.Generic.List<Gx.Common.Qualifier> _qualifiers;

        public NamePart()
            : this(default, null)
        {
        }

        public NamePart(NamePartType type, String text)
        {
            if (type != NamePartType.NULL)
            {
                KnownType = type;
            }
            Value = text;
        }

        /// <summary>
        ///  The value of the name part.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "value")]
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        /// <summary>
        ///  The type of the name part.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "type")]
        [Newtonsoft.Json.JsonProperty("type")]
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        ///  Convenience property for treating Type as an enum. See Gx.Types.NamePartTypeQNameUtil for details on getter/setter functionality.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [Newtonsoft.Json.JsonIgnore]
        public Gx.Types.NamePartType KnownType
        {
            get
            {
                return XmlQNameEnumUtil.GetEnumValue<NamePartType>(this._type);
            }
            set
            {
                this._type = XmlQNameEnumUtil.GetNameValue(value);
            }
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
                return this._fields ?? (_fields = new System.Collections.Generic.List<Gx.Records.Field>());
            }
            set
            {
                this._fields = value;
            }
        }
        public bool ShouldSerializeFields() => AnyFields();
        public bool AnyFields()
        {
            return _fields?.Any() ?? false;
        }
        /// <summary>
        ///  The qualifiers associated with this name part.
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
                this._qualifiers = value;
            }
        }
        public bool ShouldSerializeQualifiers() => AnyQualifiers();
        public bool AnyQualifiers()
        {
            return _qualifiers?.Any() ?? false;
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitNamePart(this);
        }

        /**
         * Build out this name part with a type.
         *
         * @param type The type.
         * @return this.
         */
        public NamePart SetType(String type)
        {
            Type = type;
            return this;
        }

        /**
         * Build out this name part with a type.
         *
         * @param type The type.
         * @return this.
         */
        public NamePart SetType(NamePartType type)
        {
            KnownType = type;
            return this;
        }

        /**
         * Build out this name part with a value.
         *
         * @param value The value.
         * @return this.
         */
        public NamePart SetValue(String value)
        {
            Value = value;
            return this;
        }

        /**
         * Build out this name part with a qualifier.
         *
         * @param qualifier The qualifier.
         * @return this.
         */
        public NamePart SetQualifier(Qualifier qualifier)
        {
            if (qualifier != null)
            {
                Qualifiers.Add(qualifier);
            }
            return this;
        }

        /**
         * Build out this name part with a field.
         * @param field The field.
         * @return this.
         */
        public NamePart SetField(Field field)
        {
            if (field != null)
            {
                Fields.Add(field);
            }
            return this;
        }
    }
}
