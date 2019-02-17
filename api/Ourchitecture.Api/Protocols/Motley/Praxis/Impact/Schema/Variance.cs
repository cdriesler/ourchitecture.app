using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact.Schema
{
    /// <summary>
    /// Suite of evaluation methods for how "irregular" a given input is in the context of the current praxis.
    /// </summary>
    public static class Variance
    {
        public static double EvaluateCurveLinearVariance(Curve crv)
        {
            var baseCurve = new LineCurve(crv.PointAtStart, crv.PointAtEnd);

            crv.DivideByLength(5, true, false, out var pts);

            var distances = pts.Select(x =>
            {
                baseCurve.ClosestPoint(x, out var t);
                return x.DistanceTo(baseCurve.PointAt(t));
            }).ToList().StandardDeviation();

            return distances;
        }
    }
}