using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// Provides a simple implementation of the <see cref="DataSource"/> interface.
    /// </summary>
    public class BasicDataSource : DataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicDataSource"/> class.
        /// </summary>
        public BasicDataSource()
            : this(null, null, new byte[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicDataSource"/> class.
        /// </summary>
        /// <param name="name">The name of the data source.</param>
        /// <param name="contentType">The content-type of the data source.</param>
        /// <param name="bytes">The byte array of data for this data source.</param>
        public BasicDataSource(String name, String contentType, Byte[] bytes)
            : this(name, contentType, new MemoryStream(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicDataSource"/> class.
        /// </summary>
        /// <param name="name">The name of the data source.</param>
        /// <param name="contentType">The content-type of the data source.</param>
        /// <param name="stream">The stream for this data source.</param>
        public BasicDataSource(String name, String contentType, Stream stream)
        {
            Name = name;
            ContentType = contentType;
            InputStream = stream;
        }

        /// <summary>
        /// Gets the input stream of the target data source.
        /// </summary>
        /// <value>
        /// The input stream of the target data source.
        /// </value>
        public Stream InputStream
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content type of the target data source.
        /// </summary>
        /// <value>
        /// The content type of the target data source.
        /// </value>
        public String ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the data source.
        /// </summary>
        /// <value>
        /// The name of the data source.
        /// </value>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (InputStream != null)
            {
                InputStream.Dispose();
            }
        }
    }
}
