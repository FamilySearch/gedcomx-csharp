using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class FileDataSource : DataSource, IDisposable
    {
        public FileDataSource()
        {
        }

        public FileDataSource(String file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {
                Byte[] bytes = new Byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                InputStream = new MemoryStream(bytes);
            }

            InputStream.Seek(0, SeekOrigin.Begin);
            Name = Path.GetFileName(file);
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
