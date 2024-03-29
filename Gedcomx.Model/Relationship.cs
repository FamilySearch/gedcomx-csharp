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
    ///  A relationship between two or more persons.
    /// </remarks>
    /// <summary>
    ///  A relationship between two or more persons.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Relationship")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "relationship")]
    public partial class Relationship : Gx.Conclusion.Subject
    {

        private string _type;
        private Gx.Common.ResourceReference _person1;
        private Gx.Common.ResourceReference _person2;
        private System.Collections.Generic.List<Gx.Conclusion.Fact> _facts;
        private System.Collections.Generic.List<Gx.Records.Field> _fields;
        /// <summary>
        ///  The type of this relationship.
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
        ///  Convenience property for treating Type as an enum. See Gx.Types.RelationshipTypeQNameUtil for details on getter/setter functionality.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [Newtonsoft.Json.JsonIgnore]
        public Gx.Types.RelationshipType KnownType
        {
            get
            {
                return XmlQNameEnumUtil.GetEnumValue<RelationshipType>(this._type);
            }
            set
            {
                this._type = XmlQNameEnumUtil.GetNameValue(value);
            }
        }
        /// <summary>
        ///  A reference to a person in the relationship. The name "person1" is used only to distinguish it from
        ///  the other person in this relationship and implies neither order nor role. When the relationship type
        ///  implies direction, it goes from "person1" to "person2".
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "person1", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("person1")]
        public Gx.Common.ResourceReference Person1
        {
            get
            {
                return this._person1;
            }
            set
            {
                this._person1 = value;
            }
        }
        /// <summary>
        ///  A reference to a person in the relationship. The name "person2" is used only to distinguish it from
        ///  the other person in this relationship and implies neither order nor role. When the relationship type
        ///  implies direction, it goes from "person1" to "person2".
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "person2", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("person2")]
        public Gx.Common.ResourceReference Person2
        {
            get
            {
                return this._person2;
            }
            set
            {
                this._person2 = value;
            }
        }
        /// <summary>
        ///  The fact conclusions for the relationship.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "fact", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("facts")]
        public System.Collections.Generic.List<Gx.Conclusion.Fact> Facts
        {
            get
            {
                return this._facts ?? (_facts = new System.Collections.Generic.List<Gx.Conclusion.Fact>());
            }
            set
            {
                this._facts = value;
            }
        }
        public bool ShouldSerializeFacts() => AnyFacts();
        public bool AnyFacts()
        {
            return _facts?.Any() ?? false;
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

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitRelationship(this);
        }

        /**
         * Build out this relationship with a type.
         * @param type The type.
         * @return this.
         */
        public Relationship SetType(String type)
        {
            Type = type;
            return this;
        }

        /**
         * Build out this relationship with a type.
         * @param type The type.
         * @return this.
         */
        public Relationship SetType(RelationshipType type)
        {
            KnownType = type;
            return this;
        }

        /**
         * Build out this relationship with a reference to person 1.
         * 
         * @param person1 person 1.
         * @return this.
         */
        public Relationship SetPerson1(ResourceReference person1)
        {
            Person1 = person1;
            return this;
        }

        /**
         * Build out this relationship with a reference to person 2.
         *
         * @param person2 person 2.
         * @return this.
         */
        public Relationship SetPerson2(ResourceReference person2)
        {
            Person2 = person2;
            return this;
        }

        /**
         * Build out this relationship with a fact.
         * @param fact The fact.
         * @return this
         */
        public Relationship SetFact(Fact fact)
        {
            if (fact != null)
            {
                Facts.Add(fact);
            }
            return this;
        }

        /**
         * Build out this relationship with a field.
         *
         * @param field The field.
         * @return this.
         */
        public Relationship SetField(Field field)
        {
            if (field != null)
            {
                Fields.Add(field);
            }
            return this;
        }
    }
}
