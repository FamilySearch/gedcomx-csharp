using Gedcomx.Model.Rt;
using Gedcomx.Model.Util;
using Gx.Types;
// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Gx.Conclusion
{

    /// <remarks>
    ///  A name conclusion.
    /// </remarks>
    /// <summary>
    ///  A name conclusion.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://gedcomx.org/v1/", TypeName = "Name")]
    [XmlRoot(Namespace = "http://gedcomx.org/v1/", ElementName = "name")]
    public partial class Name : Gx.Conclusion.Conclusion
    {
        private string _type;
        private bool? _preferred;
        private bool _preferredSpecified;
        private Gx.Conclusion.DateInfo _date;
        private List<Gx.Conclusion.NameForm> _nameForms;

        public Name()
        {
        }

        public Name(String fullText, params NamePart[] parts)
        {
            AddNameForm(new NameForm(fullText, parts));
        }

        /// <summary>
        ///  The type of the name.
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        [JsonProperty("type")]
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
        ///  Convenience property for treating Type as an enum. See Gx.Types.NameTypeQNameUtil for details on getter/setter functionality.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Gx.Types.NameType KnownType
        {
            get
            {
                return XmlQNameEnumUtil.GetEnumValue<NameType>(this._type);
            }
            set
            {
                this._type = XmlQNameEnumUtil.GetNameValue(value);
            }
        }
        /// <summary>
        ///  Whether the conclusion is preferred above other conclusions of the same type. Useful, for example, for display purposes.
        /// </summary>
        [XmlElement(ElementName = "preferred", Namespace = "http://gedcomx.org/v1/")]
        [JsonProperty("preferred")]
        public bool Preferred
        {
            get
            {
                return this._preferred.GetValueOrDefault();
            }
            set
            {
                this._preferred = value;
                this._preferredSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Preferred" property should be included in the output.
        /// </summary>
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public bool PreferredSpecified
        {
            get
            {
                return this._preferredSpecified;
            }
            set
            {
                this._preferredSpecified = value;
            }
        }

        /// <summary>
        ///  The date the name was first applied or adopted.
        /// </summary>
        [XmlElement(ElementName = "date", Namespace = "http://gedcomx.org/v1/")]
        [JsonProperty("date")]
        public Gx.Conclusion.DateInfo Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }
        /// <summary>
        ///  Alternate forms of the name, such as the romanized form of a non-latin name.
        /// </summary>
        [XmlElement(ElementName = "nameForm", Namespace = "http://gedcomx.org/v1/")]
        [JsonProperty("nameForms")]
        public List<Gx.Conclusion.NameForm> NameForms
        {
            get
            {
                return this._nameForms;
            }
            set
            {
                this._nameForms = value;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public NameForm NameForm
        {
            get
            {
                return this._nameForms != null ? this._nameForms.FirstOrDefault() : null;
            }
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitName(this);
        }

        /**
         * Build up this name with a type.
         * @param type The type.
         * @return this.
         */
        public Name SetType(String type)
        {
            Type = type;
            return this;
        }

        /**
         * Build up this name with a type.
         * @param type The type.
         * @return this.
         */
        public Name SetType(NameType type)
        {
            KnownType = type;
            return this;
        }

        /**
         * Build up this name with a date.
         * @param date The date.
         * @return this.
         */
        public Name SetDate(DateInfo date)
        {
            Date = date;
            return this;
        }

        /**
         * Build up this name with a name form.
         *
         * @param nameForm The name form.
         * @return this.
         */
        public Name SetNameForm(NameForm nameForm)
        {
            AddNameForm(nameForm);
            return this;
        }

        /**
         * Build up this name with a preferred flag.
         * @param preferred The preferred flag.
         * @return this.
         */
        public Name SetPreferred(Boolean preferred)
        {
            Preferred = preferred;
            return this;
        }

        /**
         * Add a name form to the list of name forms.
         *
         * @param nameForm The name form to be added.
         */
        public void AddNameForm(NameForm nameForm)
        {
            if (nameForm != null)
            {
                if (_nameForms == null)
                {
                    _nameForms = new List<NameForm>();
                }

                _nameForms.Add(nameForm);
            }
        }
    }
}
