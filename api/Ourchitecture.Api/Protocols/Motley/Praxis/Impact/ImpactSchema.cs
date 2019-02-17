using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ourchitecture.Api.Protocols.Motley.Impact.Schema;
using Rhino.Geometry;

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

            EvaluateInputComposition(res);
            EvaluateInputMeasurements(res);
            EvaluateAverages(res);
            EvaluateVariance(res);
            EvaluateNoise(res);

            return res;
        }

        private static void EvaluateInputComposition(ImpactManifest res)
        {
            res.PathSegments = Composition.GetCurveAsLinearSegments(res.PrimaryPathCurve);
            res.MarketCellSegments = Composition.GetCurveAsLinearSegments(res.PrimaryMarketCellCurve);
        }

        private static void EvaluateInputMeasurements(ImpactManifest res)
        {
            res.PrimaryPathSegmentLengths = res.PathSegments.Select(x => x.GetLength()).ToList();
            res.PrimaryPathSegmentSlopes = res.PathSegments.Select(x => x.PlanarSlope()).ToList();
            res.MarketCellSegmentLengths = res.MarketCellSegments.Select(x => x.GetLength()).ToList();
            res.MarketCellSegmentSlopes = res.MarketCellSegments.Select(x => x.PlanarSlope()).ToList();

            var box = res.PrimaryMarketCellCurve.GetBoundingBox(Plane.WorldXY);

            res.MarketCellWidth = Math.Abs(box.Max.X - box.Min.X);
            res.MarketCellDepth = Math.Abs(box.Max.Y - box.Min.Y);
            res.MarketCellBoundingBoxArea = new Rectangle3d(Plane.WorldXY, new Point3d(box.Min.X, box.Min.Y, 0), new Point3d(box.Max.X, box.Max.Y, 0)).Area;
            res.MarketCellArea = AreaMassProperties.Compute(res.PrimaryMarketCellCurve).Area;
        }

        private static void EvaluateAverages(ImpactManifest res)
        {
            res.AverageFromPrimaryPathSegmentLengths = res.PrimaryPathSegmentLengths.Average();
            res.AverageFromPrimaryPathSegmentSlopes = res.PrimaryPathSegmentSlopes.Average();
            res.AverageFromMarketCellSegmentLengths = res.MarketCellSegmentLengths.Average();
            res.AverageFromMarketCellSegmentSlopes = res.MarketCellSegmentSlopes.Average();
        }

        private static void EvaluateVariance(ImpactManifest res)
        {
            res.VarianceFromPrimaryPathDeflection = Variance.EvaluateCurveLinearVariance(res.PrimaryPathCurve);
            res.VarianceFromPrimaryPathSegmentLengths = res.PrimaryPathSegmentLengths.StandardDeviation();
            res.VarianceFromPrimaryPathSegmentSlopes = res.PrimaryPathSegmentSlopes.StandardDeviation();
            res.VarianceFromPrimaryMarketCellSegmentLengths = res.MarketCellSegmentLengths.StandardDeviation();
            res.VarianceFromPrimaryMarketCellSegmentSlopes = res.MarketCellSegmentSlopes.StandardDeviation();
            res.VarianceFromPrimaryMarketCellArea = res.MarketCellBoundingBoxArea - res.MarketCellArea;
        }

        private static void EvaluateNoise(ImpactManifest res)
        {

        }
    }
}