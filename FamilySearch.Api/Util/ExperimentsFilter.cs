using Gx.Rs.Api.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    /// <summary>
    /// This filter enables SDK consumers to enable specific FamilySearch features that are not yet enabled by default.
    /// </summary>
    public class ExperimentsFilter : IFilter
    {
        private readonly String experiments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentsFilter"/> class.
        /// </summary>
        /// <param name="experiments">The array of features to enable. The list of features can always be determined by calling the /platform/pending-modifications path on the specific environment in use.</param>
        public ExperimentsFilter(params String[] experiments)
        {
            StringBuilder experimentsList = new StringBuilder();
            for (int i = 0; i < experiments.Length; i++)
            {
                String experiment = experiments[i];
                experimentsList.Append(experiment);
                if (i + 1 < experiments.Length)
                {
                    experimentsList.Append(',');
                }
            }
            this.experiments = experimentsList.ToString();
        }

        /// <summary>
        /// This method applies the list of features to the specified REST API request.
        /// </summary>
        /// <param name="client">This parameter is currently unused.</param>
        /// <param name="request">The REST API request that will have the features applied.</param>
        /// <remarks>
        /// The specific features will be added as a special header to the REST API request.
        /// </remarks>
        public void Handle(IRestClient client, IRestRequest request)
        {
            request.AddHeader("X-FS-Feature-Tag", this.experiments);
        }
    }
}
