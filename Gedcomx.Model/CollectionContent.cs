﻿// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

using Gedcomx.Model.Util;

using Gx.Types;

namespace Gx.Records
{

    /// <remarks>
    ///  A description of the content of a collection by resource type.
    /// </remarks>
    /// <summary>
    ///  A description of the content of a collection by resource type.
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gedcomx.org/v1/", TypeName = "CollectionContent")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://gedcomx.org/v1/", ElementName = "collectionContent")]
    public partial class CollectionContent : Gx.Links.HypermediaEnabledData
    {

        private float? _completeness;
        private bool _completenessSpecified;
        private int? _count;
        private bool _countSpecified;
        private string _resourceType;
        /// <summary>
        ///  A completeness factor for this content aspect, a value between 0 and 1.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "completeness", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("completeness")]
        public float Completeness
        {
            get
            {
                return this._completeness.GetValueOrDefault();
            }
            set
            {
                this._completeness = value;
                this._completenessSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Completeness" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool CompletenessSpecified
        {
            get
            {
                return this._completenessSpecified;
            }
            set
            {
                this._completenessSpecified = value;
            }
        }

        /// <summary>
        ///  The count of the items applicable to this content aspect.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "count", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("count")]
        public int Count
        {
            get
            {
                return this._count.GetValueOrDefault();
            }
            set
            {
                this._count = value;
                this._countSpecified = true;
            }
        }

        /// <summary>
        ///  Property for the XML serializer indicating whether the "Count" property should be included in the output.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Newtonsoft.Json.JsonIgnore]
        public bool CountSpecified
        {
            get
            {
                return this._countSpecified;
            }
            set
            {
                this._countSpecified = value;
            }
        }

        /// <summary>
        ///  The type of resource being covered in this collection.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(ElementName = "resourceType", Namespace = "http://gedcomx.org/v1/")]
        [Newtonsoft.Json.JsonProperty("resourceType")]
        public string ResourceType
        {
            get
            {
                return this._resourceType;
            }
            set
            {
                this._resourceType = value;
            }
        }

        /**
         * Build out the content with a resource type.
         * @param resourceType The resource type.
         * @return this.
         */

        [System.Xml.Serialization.XmlIgnoreAttribute]
        [Newtonsoft.Json.JsonIgnore]
        public Gx.Types.ResourceType KnownResourceType
        {
            get
            {
                return XmlQNameEnumUtil.GetEnumValue<ResourceType>(this._resourceType);
            }
            set
            {
                this._resourceType = XmlQNameEnumUtil.GetNameValue(value);
            }
        }

        public CollectionContent SetResourceType(String resourceType)
        {
            ResourceType = resourceType;
            return this;
        }

        /**
         * Build out the content with a resource type.
         * @param resourceType The resource type.
         * @return this.
         */
        public CollectionContent SetResourceType(ResourceType resourceType)
        {
            KnownResourceType = resourceType;
            return this;
        }

        /**
         * Build out this collection content with a count.
         * @param count The count.
         * @return this.
         */
        public CollectionContent SetCount(Int32 count)
        {
            Count = count;
            return this;
        }

        /**
         * Build out this content with completeness.
         * @param completeness The completeness.
         * @return this.
         */
        public CollectionContent SetCompleteness(float completeness)
        {
            Completeness = completeness;
            return this;
        }
    }
}
