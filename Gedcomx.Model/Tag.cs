using Gedcomx.Model.Util;
using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Source
{
    /// <remarks>
    ///  A tag in the FamilySearch system.
    /// </remarks>
    /// <summary>
    ///  A tag in the FamilySearch system.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://familysearch.org/v1/", TypeName = "Tag")]
    [XmlRoot(Namespace = "http://familysearch.org/v1/", ElementName = "tag")]
    public partial class Tag
    {
        private string _resource;

        public Tag()
        {
        }

        public Tag(Enum value)
        {
            this._resource = XmlQNameEnumUtil.GetNameValue(value);
        }

        /// <summary>
        ///  A reference to the value of the tag.
        /// </summary>
        [XmlAttribute(AttributeName = "resource")]
        [JsonProperty("resource")]
        public string Resource
        {
            get
            {
                return this._resource;
            }
            set
            {
                this._resource = value;
            }
        }
    }
}
