using System.Collections.Generic;

using Gx.Conclusion;

namespace Gx.Model.Collections
{
    /// <summary>
    ///  A list of <see cref="Name"/>.
    /// </summary>
    public class Names : List<Name>
    {
        /// <summary>
        /// Add a name form to the list of name forms.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Add(string value) => Add(new Name(value));

        /// <summary>
        /// Add a name form to the list of name forms.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parts">The name partsto be added.</param>
        public void Add(string value, params NamePart[] parts) => Add(new Name(value, parts));
    }
}
