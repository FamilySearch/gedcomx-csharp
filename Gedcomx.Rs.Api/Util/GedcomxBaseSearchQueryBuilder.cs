using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    public class GedcomxBaseSearchQueryBuilder
    {

        protected readonly List<SearchParameter> parameters = new List<SearchParameter>();

        public String Build()
        {
            return string.Join(" ", this.parameters);
        }

        public class SearchParameter
        {
            private readonly String prefix;
            private readonly String name;
            private readonly String value;
            private readonly bool exact;

            public SearchParameter(String prefix, String name, String value, bool exact)
            {
                if (name == null)
                {
                    throw new NullReferenceException("parameter name must not be null");
                }
                this.prefix = prefix;
                this.exact = exact;
                this.value = value;
                this.name = name;
            }

            public String Prefix
            {
                get
                {
                    return prefix;
                }
            }

            public String Name
            {
                get
                {
                    return name;
                }
            }

            public String Value
            {
                get
                {
                    return value;
                }
            }

            public bool IsExact
            {
                get
                {
                    return exact;
                }
            }

            public override String ToString()
            {
                StringBuilder builder = new StringBuilder();
                if (this.prefix != null)
                {
                    builder.Append(this.prefix);
                }
                builder.Append(this.name);
                if (this.value != null)
                {
                    builder.Append(':');
                    String escaped = this.value.Replace('\n', ' ').Replace('\t', ' ').Replace('\f', ' ').Replace('\x013', ' ').Replace("\"", "\\\"");
                    bool needsQuote = escaped.IndexOf(' ') != -1;
                    if (needsQuote)
                    {
                        builder.Append('\"');
                    }
                    builder.Append(escaped);
                    if (needsQuote)
                    {
                        builder.Append('\"');
                    }
                    if (!this.exact)
                    {
                        builder.Append('~');
                    }
                }
                return builder.ToString();
            }
        }
    }
}
