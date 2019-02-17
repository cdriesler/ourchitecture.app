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
    public static class Volatility
    {
        public static double EvaluatePathLinearVolatility(Curve path)
        {
            var baseCurve = new LineCurve(path.PointAtStart, path.PointAtEnd);

            path.DivideByLength(5, true, false, out var pts);

            var distances = pts.Select(x =>
            {
                baseCurve.ClosestPoint(x, out var t);
                return x.DistanceTo(baseCurve.PointAt(t));
            }).ToList().StandardDeviation();

            return distances;
        }

        public static double EvaluatePathSegmentSlopeVolatility(Curve path)
        {
            var spanCount = path.SpanCount;

            var spans = new List<Curve>();       
            for (int i = 0; i < spanCount; i++)
            {
                spans.Add(path.DuplicateCurve().Trim(path.SpanDomain(i)));
            }

            var segments = new List<Curve>();
            foreach (var span in spans)
            {
                if (span.Degree != 1)
                {
                    //If span is not linear, divide into 3 segments.
                    span.DivideByCount(3, true, out var pts);

                    for (int i = 0; i < pts.Count() - 1; i++)
                    {
                        segments.Add(new LineCurve(pts[i], pts[i + 1]).ToNurbsCurve());
                    }

                    continue;
                }

                segments.Add(span.DuplicateCurve());
            }

            return segments.Select(x => x.PlanarSlope()).ToList().StandardDeviation();
        }

        public static double EvaluateMarketCellSegmentLengthVolatility(Curve cell)
        {
            var spanCount = cell.SpanCount;

            var spans = new List<Curve>();
            for (int i = 0; i < spanCount; i++)
            {
                spans.Add(cell.DuplicateCurve().Trim(cell.SpanDomain(i)));
            }

            return spans.Select(x => x.GetLength()).ToList().StandardDeviation();
        }

        public static double EvaluateMarketCellSegmentSlopeVolatility(Curve cell)
        {
            var spanCount = cell.SpanCount;

            var spans = new List<Curve>();
            for (int i = 0; i < spanCount; i++)
            {
                spans.Add(cell.DuplicateCurve().Trim(cell.SpanDomain(i)));
            }

            var segments = new List<Curve>();
            foreach (var span in spans)
            {
                if (span.Degree != 1)
                {
                    //If span is not linear, divide into 3 segments.
                    span.DivideByCount(3, true, out var pts);

                    for (int i = 0; i < pts.Count() - 1; i++)
                    {
                        segments.Add(new LineCurve(pts[i], pts[i + 1]).ToNurbsCurve());
                    }

                    continue;
                }

                segments.Add(span.DuplicateCurve());
            }

            return segments.Select(x => x.PlanarSlope()).ToList().StandardDeviation();
        }

        public static double EvaluateMarketCellAreaVolatility(Curve cell)
        {
            var box = cell.GetBoundingBox(Plane.WorldXY);
            var rectArea = new Rectangle3d(Plane.WorldXY, new Point3d(box.Min.X, box.Min.Y, 0), new Point3d(box.Max.X, box.Max.Y, 0)).Area;
            var cellArea = AreaMassProperties.Compute(cell).Area;

            return rectArea - cellArea;
        }
    }
}