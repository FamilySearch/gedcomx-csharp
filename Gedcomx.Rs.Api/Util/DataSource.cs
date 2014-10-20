using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public interface DataSource : IDisposable
    {
        Stream InputStream { get; }

        String ContentType { get; }

        String Name { get; }
    }
}
