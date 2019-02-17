using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ourchitecture.Api.Protocols.Motley.Impact.Schema;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public static class ImpactSchema
    {
        /// <summary>
        /// Top-level map of logical flow.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static ImpactManifest Solve(ImpactRequest req)
        {
            var res = new ImpactManifest(req);

            EvaluateVolatility(res);

            return res;
        }

        private static void EvaluateVolatility(ImpactManifest res)
        {
            res.PrimaryPathLinearVolatility = Volatility.EvaluatePathLinearVolatility(res.PrimaryPathCurve);
            res.PrimaryPathSegmentSlopeVolatility = Volatility.EvaluatePathSegmentSlopeVolatility(res.PrimaryPathCurve);
            res.PrimaryMarketCellSegmentLengthVolatility = Volatility.EvaluateMarketCellSegmentLengthVolatility(res.PrimaryMarketCellCurve);
            res.PrimaryMarketCellSegmentSlopeVolatility = Volatility.EvaluateMarketCellSegmentSlopeVolatility(res.PrimaryMarketCellCurve);
            res.PrimaryMarketCellAreaVolatility = Volatility.EvaluateMarketCellAreaVolatility(res.PrimaryMarketCellCurve);
        }
    }
}