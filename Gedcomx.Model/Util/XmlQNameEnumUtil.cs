using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gedcomx.Model.Util
{
    public static class XmlQNameEnumUtil
    {
        public static String GetNameValue(Enum value)
        {
            var attribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(XmlEnumAttribute));
            String result;

            if (attribute != null)
            {
                result = attribute.Name;
            }
            else
            {
                result = value.ToString();
            }

            return result;
        }

        public static T GetEnumValue<T>(String value)
        {
            var found = typeof(T).GetEnumValues().Cast<Enum>().FirstOrDefault(x => GetNameValue(x) == value);
            T result = default(T);

            if (found is T)
            {
                result = (T)(object)found;
            }

            return result;
        }
    }
}
