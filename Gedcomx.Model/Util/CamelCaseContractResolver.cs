using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomx.Model.Util
{
    public class CamelCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            var result = propertyName;

            if (!string.IsNullOrEmpty(propertyName))
            {
                result = char.ToLower(propertyName[0], CultureInfo.InvariantCulture).ToString() + propertyName.Substring(1);
            }

            return result;
        }
    }
}
