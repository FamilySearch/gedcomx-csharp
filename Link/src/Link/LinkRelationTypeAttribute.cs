using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tavis
{

    /// <summary>
    /// Attribute that enables us to identify a class as representing a specific link relation type and can be extracted with instantiating the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LinkRelationTypeAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>
        /// Create a new attribute.  For IANA registered link relations name will be simple string, otherwise it should be a URI 
        /// </summary>
        /// <param name="name"></param>
        public LinkRelationTypeAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
