using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public interface DataSource
    {
        Byte[] InputBytes { get; }

        String ContentType { get; }

        String Name { get; }
    }
}
