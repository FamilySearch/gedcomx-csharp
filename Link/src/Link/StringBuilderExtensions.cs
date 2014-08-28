using System.Text;

namespace Tavis
{
    public static class StringBuilderExtensions {

        public static StringBuilder AppendQuotedString(this StringBuilder builder, string value)
        {
            builder.Append('"');
            builder.Append(value);
            builder.Append('"');
            return builder;
        }
        public static StringBuilder AppendKey(this StringBuilder builder, string key)
        {
            builder.Append(key);
            builder.Append("=");
            return builder;
        }

    }
}