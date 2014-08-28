using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public interface StateTransitionOption
    {
        void Apply(IRestRequest request);
    }
}
