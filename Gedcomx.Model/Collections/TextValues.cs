using System.Collections.Generic;
using System.Linq;

using Gx.Common;

namespace Gx.Model.Collections
{
    /// <summary>
    ///  A list of text values.
    /// </summary>
    public class TextValues : List<TextValue>
    {
        /// <summary>
        /// Add a name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Add(string name) => Add(new TextValue(name));

        /// <summary>
        /// Check if List<TextValue> is filled:
        /// </summary>
        public bool Safe { get { return !(this?.Any() ?? false); } }
    }
}
