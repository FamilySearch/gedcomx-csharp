using Gedcomx.Model.Util;
using Gx.Common;
using Gx.Fs.Rt;
// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Discussions
{

    /// <remarks>
    ///  
    /// </remarks>
    /// <summary>
    ///  
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://familysearch.org/v1/", TypeName = "Comment")]
    public partial class Comment : Gx.Links.HypermediaEnabledData
    {

        private string _text;
        private DateTime? _created;
        private bool _createdSpecified;
        private Gx.Common.ResourceReference _contributor;
        /// <summary>
        ///  The text or "message body" of the comment
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "text", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("text")]
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
        /// <summary>
        ///  date of creation
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "created", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("created")]
        [Newtonsoft.Json.JsonConverter(typeof(JsonUnixTimestampConverter))]
        public DateTime Created
        {
            get
            {
                return this._created.GetValueOrDefault();
            }
            set
            {
                this._created = value;
                this._createdSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Created" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool CreatedSpecified
        {
            get
            {
                return this._createdSpecified;
            }
            set
            {
                this._createdSpecified = value;
            }
        }

        /// <summary>
        ///  contributor of comment
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "contributor", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("contributor")]
        public Gx.Common.ResourceReference Contributor
        {
            get
            {
                return this._contributor;
            }
            set
            {
                this._contributor = value;
            }
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor to accept.
         */
        public void Accept(IFamilySearchPlatformModelVisitor visitor)
        {
            visitor.VisitComment(this);
        }

        internal void EmbedInt(ExtensibleData value)
        {
            this.Embed(value);
        }

        /**
         * Update this comment by applying text.
         *
         * @param text The text to apply.
         * @return The comment.
         */
        public Comment SetText(String text)
        {
            Text = text;
            return this;
        }

        /**
         * Build this comment by applying a contributor.
         *
         * @param contributor The contributor.
         * @return this.
         */
        public Comment SetContributor(ResourceReference contributor)
        {
            Contributor = contributor;
            return this;
        }

        /**
         * Build this comment by applying a created date.
         *
         * @param created The created date.
         * @return The comment.
         */
        public Comment SetCreated(DateTime created)
        {
            Created = created;
            return this;
        }
    }
}
