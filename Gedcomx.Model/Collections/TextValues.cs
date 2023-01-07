using System.Collections.Generic;

using Gx.Common;

namespace Gx.Model.Collections
{
    /// <summary>
    ///  A list of <see cref="TextValue"/>.
    /// </summary>
    public class TextValues : List<TextValue>
    {
        /// <summary>
        /// Add a name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Add(string name) => Add(new TextValue(name));
    }
}
