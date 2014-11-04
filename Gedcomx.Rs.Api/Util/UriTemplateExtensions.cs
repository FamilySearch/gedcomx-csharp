using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis.UriTemplates;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// A <see cref="UriTemplate"/> extensions helper.
    /// </summary>
    public static class UriTemplateExtensions
    {
        /// <summary>
        /// Adds a parameter name value pair to the specified URI template.
        /// </summary>
        /// <param name="this">The <see cref="UriTemplate"/> to modify.</param>
        /// <param name="name">The name of the parameter to add.</param>
        /// <param name="value">The value of the parameter to add.</param>
        /// <returns>The specified <see cref="UriTemplate"/> with the newly added parameter.</returns>
        public static UriTemplate AddParameter(this UriTemplate @this, string name, object value)
        {
            @this.SetParameter(name, value);

            return @this;
        }
    }
}
