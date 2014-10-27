using Gx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model
{
    public class VocabElement : IComparable<VocabElement>
    {
        private String id;
        private String uri;
        private String subclass;
        private String type;
        private String sortName;
        private List<TextValue> labels = new List<TextValue>();
        private List<TextValue> descriptions = new List<TextValue>();

        // This is only present (OPTIONALLY) when used as an "Entries in a List" object
        private String sublist;
        // This is only present (OPTIONALLY) when used as an "Entries in a List" object
        private Int32? position;

        public String Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public String Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }

        public String Subclass
        {
            get
            {
                return subclass;
            }
            set
            {
                subclass = value;
            }
        }

        public String Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public String SortName
        {
            get
            {
                return sortName;
            }
            set
            {
                sortName = value;
            }
        }

        public List<TextValue> Labels
        {
            get
            {
                return labels;
            }
        }

        public VocabElement AddLabel(String label, String locale)
        {
            this.labels.Add(new TextValue(label).SetLang(locale));
            return this;
        }

        public List<TextValue> Descriptions
        {
            get
            {
                return descriptions;
            }
        }

        public VocabElement AddDescription(String description, String locale)
        {
            this.descriptions.Add(new TextValue(description).SetLang(locale));
            return this;
        }

        public String Sublist
        {
            get
            {
                return sublist;
            }
            set
            {
                sublist = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Int32? Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public int CompareTo(VocabElement o)
        {
            // A position value overrides and trumps sortName
            // Otherwise, compare alphabetically against sortName
            // Then arbitrarily compare on Term Type, Concept, and Id
            int pos = 0;
            Int32? oPosition = o.Position;
            if (position != null)
            {
                pos = (oPosition == null) ? position.Value : position.Value - (oPosition ?? 0);
            }
            else if (oPosition != null)
            {
                pos = oPosition.Value;
            }
            if (pos == 0)
            { // Either positions are the same or null
                pos = sortName.CompareTo(o.SortName);
            }
            if (pos == 0)
            {
                pos = type.CompareTo(o.Type);
            }
            if (pos == 0)
            {
                pos = subclass.CompareTo(o.Subclass);
            }
            if (pos == 0)
            {
                pos = uri.CompareTo(o.Uri);
            }

            return pos;
        }
    }
}
