﻿// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;
using System.Linq;

using Gedcomx.Model.Rt;

using Gx.Types;

namespace Gx.Conclusion
{

    /// <remarks>
    ///  A historical event.
    /// </remarks>
    /// <summary>
    ///  A historical event.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Event")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "event")]
    [Newtonsoft.Json.JsonObject("events")]
    public partial class Event : Gx.Conclusion.Subject
    {
        private string _type;
        private Gx.Conclusion.DateInfo _date;
        private Gx.Conclusion.PlaceReference _place;
        private System.Collections.Generic.List<Gx.Conclusion.EventRole> _roles;

        /**
         * Create an event.
         */
        public Event()
        {
        }

        /**
         * Create an event with the passed in type and values.
         *
         * @param EventType the event type.
         */
        public Event(EventType EventType)
        {
            SetType(EventType);
        }

        /**
         * Create a date/place event with the passed in type and values.
         *
         * @param EventType the event type.
         * @param date The date of applicability of this event.
         * @param place The place of applicability of this event.
         */
        public Event(EventType EventType, DateInfo date, PlaceReference place)
        {
            SetType(EventType);
            SetDate(date);
            SetPlace(place);
        }

        /// <summary>
        ///  The type of the event.
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
        ///  Convenience property for treating Type as an enum. See Gx.Types.EventTypeQNameUtil for details on getter/setter functionality.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [Newtonsoft.Json.JsonIgnore]
        public Gx.Types.EventType KnownType
        {
            get
            {
                return Gx.Types.EventTypeQNameUtil.ConvertFromKnownQName(this._type);
            }
            set
            {
                this._type = Gx.Types.EventTypeQNameUtil.ConvertToKnownQName(value);
            }
        }
        /// <summary>
        ///  The date of this event.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "date", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("date")]
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
        ///  The place of this event.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "place", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("place")]
        public Gx.Conclusion.PlaceReference Place
        {
            get
            {
                return this._place;
            }
            set
            {
                this._place = value;
            }
        }
        /// <summary>
        ///  The roles played in this event.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "role", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("roles")]
        public System.Collections.Generic.List<Gx.Conclusion.EventRole> Roles
        {
            get
            {
                return this._roles ?? (_roles = new System.Collections.Generic.List<Gx.Conclusion.EventRole>());
            }
            set
            {
                this._roles = value;
            }
        }
        public bool ShouldSerializeRoles() => AnyRoles();
        public bool AnyRoles()
        {
            return _roles?.Any() ?? false;
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor.
         */
        public void Accept(IGedcomxModelVisitor visitor)
        {
            visitor.VisitEvent(this);
        }

        /**
         * Build up this event with a type.
         *
         * @param type The type of the event.
         * @return this.
         */
        public Event SetType(String type)
        {
            Type = type;
            return this;
        }

        /**
         * Build up this event with a type.
         *
         * @param type The type of the event.
         * @return this.
         */
        public Event SetType(EventType type)
        {
            KnownType = type;
            return this;
        }

        /**
         * Build up this event with a date.
         *
         * @param date The date.
         * @return this.
         */
        public Event SetDate(DateInfo date)
        {
            Date = date;
            return this;
        }

        /**
         * Build up this event with a place.
         *
         * @param place The place.
         * @return this.
         */
        public Event SetPlace(PlaceReference place)
        {
            Place = place;
            return this;
        }

        /**
         * Build up this event with a role.
         *
         * @param role The role.
         * @return this.
         */
        public Event SetRole(EventRole role)
        {
            if (role != null)
            {
                Roles.Add(role);
            }
            return this;
        }
    }
}
