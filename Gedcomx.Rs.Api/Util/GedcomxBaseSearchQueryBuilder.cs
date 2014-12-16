using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// This is the base search query builder class and provides helper functions for building syntactically correct search query strings.
    /// </summary>
    public class GedcomxBaseSearchQueryBuilder
    {
        /// <summary>
        /// The list of search parameters this builder will use.
        /// </summary>
        protected readonly List<SearchParameter> parameters = new List<SearchParameter>();

        /// <summary>
        /// Builds the query string to use for searching.
        /// </summary>
        /// <returns>The query string to use for searching.</returns>
        public String Build()
        {
            return string.Join(" ", this.parameters);
        }

        /// <summary>
        /// Represents a generic search parameter.
        /// </summary>
        public class SearchParameter
        {
            private readonly String prefix;
            private readonly String name;
            private readonly String value;
            private readonly bool exact;

            /// <summary>
            /// Initializes a new instance of the <see cref="SearchParameter"/> class.
            /// </summary>
            /// <param name="prefix">The prefix to apply to the search parameter. This is used for controlling whether a parameter is required or not. See remarks.</param>
            /// <param name="name">The name of the search parameter.</param>
            /// <param name="value">The value of the search parameter.</param>
            /// <param name="exact">If set to <c>true</c> search results will only return values that exactly match the search parameter value.</param>
            /// <exception cref="System.NullReferenceException">Thrown if the <c>name</c> parameter is null.</exception>
            /// <remarks>
            /// The prefix parameter can take on three forms:
            ///     "+": The parameter search value should be found in the search results
            ///     null: The parameter search filter is optional
            ///     "-": The parameter search value should not found in the search results (i.e., perform a NOT seaarch)
            /// </remarks>
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

            /// <summary>
            /// Gets the prefix to apply to the search parameter. This is used for controlling whether a parameter is required or not. See remarks.
            /// </summary>
            /// <value>
            /// The prefix to apply to the search parameter. This is used for controlling whether a parameter is required or not. See remarks.
            /// </value>
            /// <remarks>
            /// The prefix can take on three forms:
            ///     "+": The parameter search value should be found in the search results
            ///     null: The parameter search filter is optional
            ///     "-": The parameter search value should not found in the search results (i.e., perform a NOT seaarch)
            /// </remarks>
            public String Prefix
            {
                get
                {
                    return prefix;
                }
            }

            /// <summary>
            /// Gets the name of the current search parameter.
            /// </summary>
            /// <value>
            /// The name of the current search parameter.
            /// </value>
            public String Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Gets the value of the current search parameter.
            /// </summary>
            /// <value>
            /// The value of the current search parameter.
            /// </value>
            public String Value
            {
                get
                {
                    return value;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current search parameter requires exact value match results. See remarks.
            /// </summary>
            /// <value>
            /// A value indicating whether the current search parameter requires exact value match results. See remarks.
            /// </value>
            /// <remarks>
            /// If this value is <c>true</c>, search results will only return values that exactly match the search parameter value.
            /// </remarks>
            public bool IsExact
            {
                get
                {
                    return exact;
                }
            }

            /// <summary>
            /// Returns a string that is a syntactically conformant search query that can be used in REST API search requests.
            /// </summary>
            /// <returns>
            /// A string string that is a syntactically conformant search query that can be used in REST API search requests.
            /// </returns>
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
