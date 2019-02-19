using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public static class Measure
    {
        public static double CurveSegmentVolatility(Curve crv)
        {
            var crvSegments = new List<Curve>();

            for (int i = 0; i < crv.SpanCount; i++)
            {
                var domain = crv.SpanDomain(i);
                var crvDup = crv.Duplicate() as Curve;

                crvSegments.Add(crvDup.Trim(domain));
            }

            return crvSegments.Select(x => x.GetLength()).ToList().StandardDeviation();
        }

        public static double CurveCornerAngleVolatility(Curve crv)
        {
            var crvSegments = new List<Curve>();

            for (int i = 0; i < crv.SpanCount; i++)
            {
                var domain = crv.SpanDomain(i);
                var crvDup = crv.Duplicate() as Curve;

                crvSegments.Add(crvDup.Trim(domain));
            }

            var angles = new List<double>();

            for (int i = 0; i < crvSegments.Count; i++)
            {
                var firstCrv = crvSegments[i];
                var secondCrv = crvSegments[(i + 1) % crvSegments.Count];

                angles.Add(Vector3d.VectorAngle(new Vector3d(firstCrv.PointAtStart - firstCrv.PointAtEnd), new Vector3d(secondCrv.PointAtEnd - secondCrv.PointAtStart)));
            }

            return angles.StandardDeviation();
        }
    }
}