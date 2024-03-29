// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;
using Gx.Fs;
using Gx.Fs.Rt;
using Gx.Common;
using System.Collections.Generic;
using Gedcomx.Model.Util;

namespace Gx.Fs.Discussions
{

    /// <remarks>
    ///  A discussion.
    /// </remarks>
    /// <summary>
    ///  A discussion.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://familysearch.org/v1/", TypeName = "Discussion")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://familysearch.org/v1/", ElementName = "discussion")]
    public partial class Discussion : Gx.Links.HypermediaEnabledData
    {

        private string _title;
        private string _details;
        private DateTime? _created;
        private bool _createdSpecified;
        private Gx.Common.ResourceReference _contributor;
        private DateTime? _modified;
        private bool _modifiedSpecified;
        private int? _numberOfComments;
        private bool _numberOfCommentsSpecified;
        private System.Collections.Generic.List<Gx.Fs.Discussions.Comment> _comments;
        /// <summary>
        ///  the one-line summary text
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "title", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("title")]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        /// <summary>
        ///  The text or "message body" of the discussion
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "details", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("details")]
        public string Details
        {
            get
            {
                return this._details;
            }
            set
            {
                this._details = value;
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
        ///  contributor of discussion
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
        /// <summary>
        ///  Date of last modification
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "modified", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("modified")]
        [Newtonsoft.Json.JsonConverter(typeof(JsonUnixTimestampConverter))]
        public DateTime Modified
        {
            get
            {
                return this._modified.GetValueOrDefault();
            }
            set
            {
                this._modified = value;
                this._modifiedSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Modified" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool ModifiedSpecified
        {
            get
            {
                return this._modifiedSpecified;
            }
            set
            {
                this._modifiedSpecified = value;
            }
        }

        /// <summary>
        ///  Number of comments
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "numberOfComments", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("numberOfComments")]
        public int NumberOfComments
        {
            get
            {
                return this._numberOfComments.GetValueOrDefault();
            }
            set
            {
                this._numberOfComments = value;
                this._numberOfCommentsSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "NumberOfComments" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool NumberOfCommentsSpecified
        {
            get
            {
                return this._numberOfCommentsSpecified;
            }
            set
            {
                this._numberOfCommentsSpecified = value;
            }
        }

        /// <summary>
        ///  The comments on this discussion.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "comment", Namespace = "http://familysearch.org/v1/")]
        [Newtonsoft.Json.JsonProperty("comments")]
        public System.Collections.Generic.List<Gx.Fs.Discussions.Comment> Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        /**
         * Accept a visitor.
         *
         * @param visitor The visitor to accept.
         */
        public void Accept(IFamilySearchPlatformModelVisitor visitor)
        {
            visitor.VisitDiscussion(this);
        }

        protected override void Embed(ExtensibleData value)
        {
            Discussion discussion = value as Discussion;
            List<Comment> comments = discussion.Comments;
            if (comments != null)
            {
                foreach (Comment comment in comments)
                {
                    bool found = false;
                    if (comment.Id != null)
                    {
                        if (Comments != null)
                        {
                            foreach (Comment target in Comments)
                            {
                                if (comment.Id.Equals(target.Id))
                                {
                                    target.EmbedInt(comment);
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!found)
                    {
                        AddComment(comment);
                    }
                }
            }

            base.Embed(discussion);
        }

        internal void EmbedInt(ExtensibleData value)
        {
            this.Embed(value);
        }

        /**
         * Add a comment.
         *
         * @param comment The comment to add.
         */
        public void AddComment(Comment comment)
        {
            if (_comments == null)
            {
                _comments = new List<Comment>();
            }
            _comments.Add(comment);
        }

        /**
         * Build out this discussion by applying a title.
         * 
         * @param title The title.
         * @return this.
         */
        public Discussion SetTitle(String title)
        {
            Title = title;
            return this;
        }

        /**
         * Build out this discussion by applying details.
         *
         * @param details The details.
         * @return this.
         */
        public Discussion SetDetails(String details)
        {
            Details = details;
            return this;
        }

        /**
         * Build out this discussion by applying a contributor.
         *
         * @param contributor The contributor.
         * @return this.
         */
        public Discussion SetContributor(ResourceReference contributor)
        {
            Contributor = contributor;
            return this;
        }

        /**
         * Build out this discussion by applying a created date.
         *
         * @param created The created date.
         * @return this.
         */
        public Discussion SetCreated(DateTime created)
        {
            Created = created;
            return this;
        }

        /**
         * Build out this discussion by applying a modified date.
         *
         * @param modified The modified date.
         * @return this.
         */
        public Discussion SetModified(DateTime modified)
        {
            Modified = modified;
            return this;
        }

        /**
         * Build our this discussion by setting a number of comments.
         *
         * @param numberOfComments The number of comments.
         * @return this.
         */
        public Discussion SetNumberOfComments(Int32 numberOfComments)
        {
            NumberOfComments = numberOfComments;
            return this;
        }

        /**
         * Build out this discussion by adding a comment.
         *
         * @param comment The comment to add.
         * @return this.
         */
        public Discussion SetComment(Comment comment)
        {
            AddComment(comment);
            return this;
        }
    }
}
