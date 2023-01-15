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
        /// Add a value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Add(string value) => Add(new TextValue(value));
    }
}
