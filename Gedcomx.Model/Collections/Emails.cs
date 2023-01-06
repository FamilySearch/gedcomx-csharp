using System.Collections.Generic;

using Gx.Common;

namespace Gx.Model.Collections
{
    /// <summary>
    ///  A list of reference to Emails resources.
    /// </summary>
    public class Emails : List<ResourceReference>
    {
        /// <summary>
        /// Add an email.
        /// </summary>
        /// <param name="email">The address to add.</param>
        public void Add(string email) => this.Add(new ResourceReference("mailto:" + email));
    }
}
