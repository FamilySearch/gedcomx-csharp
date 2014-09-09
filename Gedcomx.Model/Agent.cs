using Gedcomx.Model.Util;
using Newtonsoft.Json;
// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Agent
{

    /// <remarks>
    ///  An agent, e.g. person, organization, or group. In genealogical research, an agent often
    ///  takes the role of a contributor.
    /// </remarks>
    /// <summary>
    ///  An agent, e.g. person, organization, or group. In genealogical research, an agent often
    ///  takes the role of a contributor.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Agent")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "Agent")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "agent")]
    public partial class Agent : Gx.Links.HypermediaEnabledData
    {

        private System.Collections.Generic.List<Gx.Agent.OnlineAccount> _accounts;
        private System.Collections.Generic.List<Gx.Agent.Address> _addresses;
        private System.Collections.Generic.List<Gx.Common.ResourceReference> _emails;
        private Gx.Common.ResourceReference _homepage;
        private System.Collections.Generic.List<Gx.Conclusion.Identifier> _identifiers;
        private System.Collections.Generic.List<Gx.Common.TextValue> _names;
        private Gx.Common.ResourceReference _openid;
        private System.Collections.Generic.List<Gx.Common.ResourceReference> _phones;
        /// <summary>
        ///  The accounts that belong to this person or organization.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "account", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "account")]
        public System.Collections.Generic.List<Gx.Agent.OnlineAccount> Accounts
        {
            get
            {
                return this._accounts;
            }
            set
            {
                this._accounts = value;
            }
        }
        /// <summary>
        ///  The addresses that belong to this person or organization.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "address", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "address")]
        public System.Collections.Generic.List<Gx.Agent.Address> Addresses
        {
            get
            {
                return this._addresses;
            }
            set
            {
                this._addresses = value;
            }
        }
        /// <summary>
        ///  The emails that belong to this person or organization.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "email", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "email")]
        public System.Collections.Generic.List<Gx.Common.ResourceReference> Emails
        {
            get
            {
                return this._emails;
            }
            set
            {
                this._emails = value;
            }
        }
        /// <summary>
        ///  The homepage.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "homepage", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "homepage")]
        public Gx.Common.ResourceReference Homepage
        {
            get
            {
                return this._homepage;
            }
            set
            {
                this._homepage = value;
            }
        }
        /// <summary>
        ///  The list of identifiers for the agent.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "identifier", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "identifier")]
        [JsonConverter(typeof(JsonIdentifiersConverter))]
        public System.Collections.Generic.List<Gx.Conclusion.Identifier> Identifiers
        {
            get
            {
                return this._identifiers;
            }
            set
            {
                this._identifiers = value;
            }
        }
        /// <summary>
        ///  The list of names for the agent.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "name", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "name")]
        public System.Collections.Generic.List<Gx.Common.TextValue> Names
        {
            get
            {
                return this._names;
            }
            set
            {
                this._names = value;
            }
        }
        /// <summary>
        ///  The &lt;a href=&quot;http://openid.net/&quot;&gt;openid&lt;/a&gt; of the person or organization.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "openid", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "openid")]
        public Gx.Common.ResourceReference Openid
        {
            get
            {
                return this._openid;
            }
            set
            {
                this._openid = value;
            }
        }
        /// <summary>
        ///  The phones that belong to this person or organization.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "phone", Namespace = "http://gedcomx.org/v1/")]
        [System.Xml.Serialization.SoapElementAttribute(ElementName = "phone")]
        public System.Collections.Generic.List<Gx.Common.ResourceReference> Phones
        {
            get
            {
                return this._phones;
            }
            set
            {
                this._phones = value;
            }
        }
    }
}
