using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            //Measure input
            EvaluateInputComposition(res);
            EvaluateInputMeasurements(res);
            EvaluateAverages(res);
            EvaluateVariance(res);
            EvaluateNoise(res);

            //Begin path construction
            GenerateMemorialRegions(res);
            GeneratePrimarySpine(res);

            return res;
        }

        private static void EvaluateInputComposition(ImpactManifest res)
        {
            res.PathSegments = Composition.GetCurveAsLinearSegments(res.PrimaryPathCurve);
            res.MarketCellSegments = Composition.GetCurveAsLinearSegments(res.PrimaryMarketCellCurve);

            res.PrimaryRuinRegions.ForEach(x => res.MemorialRegions.Add(new MemorialRegion(x)));
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

            res.PrecastArchWidth = res.MarketCellWidth.PartitionByApproximateSize(5);
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
            var rng = new Interval(0, 1);
            var pathDeflectionExtents = new Interval(0, 200);
            var pathSegmentLengthExtents = new Interval(0, 150);
            var pathSegmentSlopeExtents = new Interval(0, 1);
            var cellSegmentLengthExtents = new Interval(0, 5);
            var cellSegmentSlopeExtents = new Interval(0, 2.5);
            var cellAreaExtents = new Interval(0, 100);

            res.NoiseFromPrimaryPathDeflectionVariance = res.VarianceFromPrimaryPathDeflection.Remap(pathDeflectionExtents, rng).Contain(rng);
            res.NoiseFromPrimaryPathSegmentLengthVariance = res.VarianceFromPrimaryPathSegmentLengths.Remap(pathSegmentLengthExtents, rng).Contain(rng);
            res.NoiseFromPrimaryPathSegmentSlopeVariance = res.VarianceFromPrimaryPathSegmentSlopes.Remap(pathSegmentSlopeExtents, rng).Contain(rng);
            res.NoiseFromPrimaryMarketCellSegmentLengthVariance = res.VarianceFromPrimaryMarketCellSegmentLengths.Remap(cellSegmentLengthExtents, rng).Contain(rng);
            res.NoiseFromPrimaryMarketCellSegmentSlopeVariance = (res.VarianceFromPrimaryMarketCellSegmentSlopes.Remap(cellSegmentSlopeExtents, rng) - 0.1).Contain(rng);
            res.NoiseFromPrimaryMarketCellAreaVariance = res.VarianceFromPrimaryMarketCellArea.Remap(cellAreaExtents, rng).Contain(rng);
        }

        private static void GenerateMemorialRegions(ImpactManifest res)
        {
            res.PrimaryRuinRegions.ForEach(x => res.MemorialRegions.Add(new MemorialRegion(x)));
        }

        private static void GeneratePrimarySpine(ImpactManifest res)
        {
            res.MarketCellsRemaining = res.PrimaryMarketQuota;

            //First pass maximal extensions and reactions.
            foreach(var segment in res.PathSegments)
            {
                var spine = new PrimarySpine(segment);
                res.PrimarySpines.Add(spine);
                res.MarketCellsRemaining = spine.Grow(res.MarketCellsRemaining, res.PrecastArchWidth, res.MemorialRegions, res.PlanarBounds);

                if (spine.PrimarySplinterCurves.Any() || spine.SecondarySplinterCurves.Any())
                {
                    var splinterCurves = new List<Curve>(spine.PrimarySplinterCurves);
                    var secondaryCurves = new List<Curve>(spine.SecondarySplinterCurves);

                    while(res.MarketCellsRemaining > 0 && (splinterCurves.Any() || secondaryCurves.Any()))
                    {
                        
                        var splinterCache = new List<Curve>();
                        var secondaryCache = new List<Curve>();

                        //Generate primary splinters
                        foreach (var splinter in splinterCurves)
                        {
                            //Rhino.RhinoDoc.ActiveDoc.Objects.Add(splinter);

                            if (res.NoiseFromPrimaryPathDeflectionVariance > 0.1)
                            {
                                splinter.Translate(new Vector3d(splinter.PointAtStart - splinter.PointAtEnd) * (res.NoiseFromPrimaryPathDeflectionVariance * 35));
                            }

                            var splinterSpine = new PrimarySpine(splinter);
                            res.PrimarySpines.Add(splinterSpine);
                            res.MarketCellsRemaining = splinterSpine.Grow(res.MarketCellsRemaining, res.PrecastArchWidth, res.MemorialRegions, res.PlanarBounds);

                            if (splinterSpine.PrimarySplinterCurves.Any())
                            {
                                splinterCache.AddRange(splinterSpine.PrimarySplinterCurves);
                            }

                            if (splinterSpine.SecondarySplinterCurves.Any())
                            {
                                secondaryCache.AddRange(splinterSpine.PrimarySplinterCurves);
                            }
                        }

                        //Commit any primary splinters.
                        if (splinterCache.Any())
                        {
                            splinterCurves = splinterCache;
                        }
                        else
                        {
                            splinterCurves.Clear();
                        }

                        var r = new Random(9);

                        //Generate secondary splinters
                        foreach (var secondary in secondaryCurves)
                        {
                            secondary.Transform(Transform.Scale(secondary.PointAtNormalizedLength(0.5), (Math.Round(r.NextDouble() * res.MarketCellsRemaining) * res.PrecastArchWidth) / secondary.GetLength()));
                            var delta = Convert.ToInt32(Math.Floor(secondary.GetLength() / res.PrecastArchWidth));

                            if (delta == 0)
                            {
                                secondaryCache.Clear();
                                break;
                            }

                            res.MarketCellsRemaining -= delta;
                            res.SecondarySpines.Add(new SecondarySpine(secondary));
                        }

                        //Commit any secondary splinters
                        if (secondaryCache.Any())
                        {
                            secondaryCurves = secondaryCache;
                        }
                        else
                        {
                            secondaryCache.Clear();
                        }

                        //Safe exit condition
                        if (!splinterCache.Any() && !secondaryCache.Any())
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}