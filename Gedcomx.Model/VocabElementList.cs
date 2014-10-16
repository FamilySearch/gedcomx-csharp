using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model
{
    public class VocabElementList
    {
        private String title;
        private String description;
        private String uri;
        private String id;
        private List<VocabElement> elements = new List<VocabElement>();

        public String Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
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

        public List<VocabElement> Elements
        {
            get
            {
                return elements;
            }
        }

        public VocabElementList AddElement(VocabElement element)
        {
            this.elements.Add(element);
            return this;
        }
    }
}
