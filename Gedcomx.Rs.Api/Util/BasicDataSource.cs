using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class BasicDataSource : DataSource
    {
        public BasicDataSource()
            : this(null, null, new byte[] { })
        {
        }

        public BasicDataSource(String name, String contentType, Byte[] bytes)
            : this(name, contentType, new MemoryStream(bytes))
        {
        }

        public BasicDataSource(String name, String contentType, Stream stream)
        {
            Name = name;
            ContentType = contentType;
            InputStream = stream;
        }

        public Stream InputStream
        {
            get;
            private set;
        }

        public String ContentType
        {
            get;
            private set;
        }

        public String Name
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (InputStream != null)
            {
                InputStream.Dispose();
            }
        }
    }
}
