using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public static class ImpactSchema
    {
        public static ImpactManifest Solve(ImpactRequest req)
        {
            var res = new ImpactManifest(req);


            return res;
        }
    }
}