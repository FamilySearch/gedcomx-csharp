﻿// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;
using System.Linq;

using Gedcomx.Model.Rt;

using Gx.Common;
using Gx.Records;
using Gx.Types;

namespace Gx.Conclusion
{

    /// <remarks>
    ///  A person.
    /// </remarks>
    /// <summary>
    ///  A person.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Person")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "person")]
    public partial class Person : Gx.Conclusion.Subject
    {
        private bool? _principal;
        private bool _principalSpecified;
        private bool? _private;
        private bool _privateSpecified;
        private bool? _living;
        private bool _livingSpecified;
        private Gx.Conclusion.Gender _gender;
        private Gx.Model.Collections.Names _names;
        private System.Collections.Generic.List<Gx.Conclusion.Fact> _facts;
        private System.Collections.Generic.List<Gx.Records.Field> _fields;
        private Gx.Conclusion.DisplayProperties _displayExtension;
        private System.Collections.Generic.List<Gx.Source.DiscussionReference> _discussionreference;

        /// <summary>
        ///  Whether this person is the "principal" person extracted from the record.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "principal")]
        [Newtonsoft.Json.JsonProperty("principal")]
        public bool Principal
        {
            get
            {
                return this._principal.GetValueOrDefault();
            }
            set
            {
                this._principal = value;
                this._principalSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Principal" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool PrincipalSpecified
        {
            get
            {
                return this._principalSpecified;
            }
            set
            {
                this._principalSpecified = value;
            }
        }

        /// <summary>
        ///  Whether this person has been designated for limited distribution or display.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "private")]
        [Newtonsoft.Json.JsonProperty("private")]
        public bool Private
        {
            get
            {
                return this._private.GetValueOrDefault();
            }
            set
            {
                this._private = value;
                this._privateSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Private" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool PrivateSpecified
        {
            get
            {
                return this._privateSpecified;
            }
            set
            {
                this._privateSpecified = value;
            }
        }

        /// <summary>
        ///  Living status of the person as treated by the system.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "living", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("living")]
        public bool Living
        {
            get
            {
                return this._living.GetValueOrDefault();
            }
            set
            {
                this._living = value;
                this._livingSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Living" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool LivingSpecified
        {
            get
            {
                return this._livingSpecified;
            }
            set
            {
                this._livingSpecified = value;
            }
        }

        /// <summary>
        ///  The gender conclusion for the person.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "gender", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("gender")]
        public Gx.Conclusion.Gender Gender
        {
            get
            {
                return this._gender;
            }
            set
            {
                this._gender = value;
            }
        }
        /// <summary>
        ///  The name conclusions for the person.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "name", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("names")]
        public Gx.Model.Collections.Names Names
        {
            get
            {
                return this._names ?? (_names = new Gx.Model.Collections.Names());
            }
            set
            {
                this._names = value;
            }
        }
        public bool ShouldSerializeNames() => AnyNames();
        public bool AnyNames()
        {
            return _names?.Any() ?? false;
        }
        /// <summary>
        ///  The fact conclusions for the person.
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
        /// <summary>
        ///  Display properties for the person. Display properties are not specified by GEDCOM X core, but as extension elements by GEDCOM X RS.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "display", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("display")]
        public Gx.Conclusion.DisplayProperties DisplayExtension
        {
            get
            {
                return this._displayExtension;
            }
            set
            {
                this._displayExtension = value;
            }
        }
        /// <summary>
        ///  Discussion References properties for the person.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "discussion-references", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("discussion-references")]
        public System.Collections.Generic.List<Gx.Source.DiscussionReference> DiscussionReferences
        {
            get
            {
                return this._discussionreference ?? (_discussionreference = new System.Collections.Generic.List<Gx.Source.DiscussionReference>());
            }
            set
            {
                this._discussionreference = value;
            }
        }
        public bool ShouldSerializeDiscussionReferences() => AnyDiscussionReferences();
        public bool AnyDiscussionReferences()
        {
            return _discussionreference?.Any() ?? false;
        }

        /// <summary>
        /// Embed the specified person into this one.
        /// </summary>
        /// <param name="person">The person to embed.</param>
        public void Embed(Person person)
        {
            this._private = this._private == null ? person._private : this._private;
            this._living = this._living == null ? person._living : this._living;
            this._principal = this._principal == null ? person._principal : this._principal;
            this._gender = this._gender == null ? person._gender : this._gender;
            if (this._displayExtension != null && person._displayExtension != null)
            {
                this._displayExtension.Embed(person._displayExtension);
            }
            else if (person._displayExtension != null)
            {
                this._displayExtension = person._displayExtension;
            }
            if (person._names != null)
            {
                this.Names.AddRange(person._names);
            }
            if (person._facts != null)
            {
                this.Facts.AddRange(person._facts);
            }
            if (person._fields != null)
            {
                this.Fields.AddRange(person._fields);
            }
            base.Embed(person);
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitPerson(this);
        }

        /**
         * Build up this subject with an persona reference.
         *
         * @param persona The persona reference.
         * @return this.
         */
        public Subject SetPersonaReference(EvidenceReference persona)
        {
            SetEvidence(persona);
            return this;
        }

        /**
         * Build out this person with a living flag.
         * @param living The flag.
         * @return this.
         */
        public Person SetLiving(Boolean living)
        {
            Living = living;
            return this;
        }

        /**
         * Build out this person with a principal flag.
         *
         * @param principal The principal flag.
         * @return this
         */
        public Person SetPrincipal(Boolean principal)
        {
            Principal = principal;
            return this;
        }

        /**
         * Build out this person with a gender.
         * @param gender The gender.
         * @return this.
         */
        public Person SetGender(Gender gender)
        {
            Gender = gender;
            return this;
        }

        /**
         * Build out this person with a gender.
         * @param gender The gender.
         * @return this.
         */
        public Person SetGender(GenderType gender)
        {
            Gender = new Gender(gender);
            return this;
        }

        /**
         * Build out this person with a name.
         * @param name The name.
         * @return this.
         */
        public Person SetName(Name name)
        {
            if (name != null)
            {
                Names.Add(name);
            }
            return this;
        }

        /**
         * Build out this person with a name.
         * @param name The name.
         * @return this.
         */
        public Person SetName(String name)
        {
            return SetName(new Name(name));
        }

        /**
         * Build out this person with a fact.
         *
         * @param fact The fact.
         * @return this.
         */
        public Person SetFact(Fact fact)
        {
            if (fact != null)
            {
                Facts.Add(fact);
            }
            return this;
        }

        /**
         * Build out this person with a display exension.
         *
         * @param display the display.
         * @return this
         */
        public Person SetDisplayExtension(DisplayProperties display)
        {
            DisplayExtension = display;
            return this;
        }

        /**
         * Build out this person with a field.
         * @param field The field.
         * @return this.
         */
        public Person SetField(Field field)
        {
            if (field != null)
            {
                Fields.Add(field);
            }
            return this;
        }
    }
}
