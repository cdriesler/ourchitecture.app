using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public static class Composition
    {
        public static List<Curve> GetCurveAsSegments(Curve crv)
        {
            var segments = new List<Curve>();

            for (int i = 0; i < crv.SpanCount; i++)
            {
                segments.Add(crv.DuplicateCurve().Trim(crv.SpanDomain(i)));
            }

            return segments;
        }

        public static List<Curve> GetCurveAsLinearSegments(Curve crv)
        {
            var spanCount = crv.SpanCount;

            var spans = new List<Curve>();
            for (int i = 0; i < spanCount; i++)
            {
                spans.Add(crv.DuplicateCurve().Trim(crv.SpanDomain(i)));
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

            return segments;
        }

        /// <summary>
        /// Given a polycurve, return a list of segments in clockwise order, starting with the highest piece. (Noon)
        /// </summary>
        /// <param name="crv"></param>
        /// <returns></returns>
        public static List<Curve> GetCurveAsSegmentsInClockwiseOrder(Curve crv)
        {
            if (!crv.IsClosed) throw new Exception("Method requires input curve to be closed.");

            var segments = GetCurveAsSegments(crv);

            var midpointHeights = segments.Select(x => x.PointAtNormalizedLength(0.5).Y).ToList();

            var noonCurveIndex = midpointHeights.IndexOf(midpointHeights.Max());
            var noonCurve = segments[noonCurveIndex];

            var orderedSegments = new List<Curve>();

            orderedSegments.Add(noonCurve);

            var isIncreasing = noonCurve.PointAtEnd.X > noonCurve.PointAtStart.X;

            if (isIncreasing)
            {
                for (int i = 0; i < segments.Count - 1; i++)
                {
                    orderedSegments.Add(segments.Find(x => x.PointAtStart.DistanceTo(orderedSegments[i].PointAtEnd) < 0.1));
                }
            }
            else
            {
                for (int i = 0; i < segments.Count - 1; i++)
                {
                    orderedSegments.Add(segments.Find(x => x.PointAtEnd.DistanceTo(orderedSegments[i].PointAtStart) < 0.1));
                }
            }

            return orderedSegments;
        }
    }
}