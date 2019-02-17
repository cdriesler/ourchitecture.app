using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact.Schema
{
    public static class Composition
    {
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
    }
}