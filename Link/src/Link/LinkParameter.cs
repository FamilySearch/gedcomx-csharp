using System;

namespace Tavis
{
    /// <summary>
    /// Parameter used to fill in URI templates
    /// </summary>
    public class LinkParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Uri Identifier { get; set; }
    }
}