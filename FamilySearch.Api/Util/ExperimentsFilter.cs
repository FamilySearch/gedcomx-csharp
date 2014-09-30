using Gx.Rs.Api.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    public class ExperimentsFilter : IFilter
    {
        private readonly String experiments;

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

        public void Handle(IRestClient client, IRestRequest request)
        {
            request.AddHeader("X-FS-Feature-Tag", this.experiments);
        }
    }
}
