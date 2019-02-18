using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static Curve SafeOffsetMove(this Curve crv, double distance, Point3d reference, bool closer) {
            crv.FrameAt(0, out var plane);

            var dirA = plane.YAxis * 1;
            var dirB = plane.YAxis * 1;

            var crvA = crv.DuplicateCurve();
            crvA.Translate(dirA * distance);
            var crvB = crv.DuplicateCurve();
            crvB.Translate(dirB * -distance);

            var crvs = new List<Curve> { crvA, crvB };
            crvs.OrderBy(x => x.PointAtNormalizedLength(0.5).DistanceTo(reference));

            return closer ? crvs.First() : crvs.Last();
        }
    }
}