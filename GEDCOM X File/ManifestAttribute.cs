using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.File
{
    internal class ManifestAttribute
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }
    }
}
