using System;

using Gx.Conclusion;
using Gx.Links;

namespace Gx.Model
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Combine '#' anf agent Id to be used with <see cref="Gx.Common.ResourceReference"/>
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns>The nubered agent Id.</returns>
        public static string HashId(Agent.Agent agent)
        {
            return HashId(agent, "agent as a contributor");
        }

        /// <summary>
        /// Combine '#' anf agent Id to be used with <see cref="Gx.Common.ResourceReference"/>
        /// </summary>
        /// <param name="analysis">The analysis.</param>
        /// <returns>The nubered analysis Id.</returns>
        public static string HashId(Document analysis)
        {
            return HashId(analysis, "analysis");
        }

        /// <summary>
        /// Combine '#' anf agent Id to be used with <see cref="Gx.Common.ResourceReference"/>
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>The nubered person Id.</returns>
        public static string HashId(Person person)
        {
            return HashId(person, "person");
        }

        /// <summary>
        /// Combine '#' anf agent Id to be used with <see cref="Gx.Common.ResourceReference"/>
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>The nubered person Id.</returns>
        /// <exception cref="ArgumentException">If the person Id is null.</exception>
        public static string HashId(HypermediaEnabledData data, string name)
        {
            if (data.Id == null)
            {
                throw new ArgumentException("Cannot reference {0}: no id.", name);
            }
            return "#" + data.Id;
        }
    }
}
