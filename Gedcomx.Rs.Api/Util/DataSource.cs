using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// An interface for using various data sources with the artifacts and memories API.
    /// </summary>
    public interface DataSource : IDisposable
    {
        /// <summary>
        /// When overridden this gets the input stream of the target data source.
        /// </summary>
        /// <value>
        /// When overridden this is the input stream of the target data source.
        /// </value>
        Stream InputStream { get; }

        /// <summary>
        /// When overridden this gets the content type of the target data source.
        /// </summary>
        /// <value>
        /// When overridden this is the content type of the target data source.
        /// </value>
        String ContentType { get; }

        /// <summary>
        /// When overridden this gets the name of the data source.
        /// </summary>
        /// <value>
        /// When overridden this is the name of the data source.
        /// </value>
        String Name { get; }
    }
}
