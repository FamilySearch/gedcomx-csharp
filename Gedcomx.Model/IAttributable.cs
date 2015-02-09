using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Common
{
    public interface IAttributable
    {
        /// <summary>
        /// Attribution metadata for a genealogical resource. Attribution data is necessary to support
        /// a sound <a href="https://wiki.familysearch.org/en/Genealogical_Proof_Standard">genealogical proof statement</a>.
        /// </summary>
        /// <value>
        /// Attribution metadata for a genealogical resource. Attribution data is necessary to support
        /// a sound <a href="https://wiki.familysearch.org/en/Genealogical_Proof_Standard">genealogical proof statement</a>.
        /// </value>
        Attribution Attribution { get; set; }
    }
}
