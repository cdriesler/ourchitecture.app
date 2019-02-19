using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static Curve TrimLineWithinRegion(this Curve crv, Curve region)
        {
            var ccx = Intersection.CurveCurve(crv, region, 0.1, 0.1).Where(x => x.IsPoint).Select(x => x.PointA).ToList();

            if (ccx.Count == 0)
            {
                return crv;
            }
            else if (ccx.Count == 1)
            {
                return new LineCurve(crv.PointAtStart, ccx[0]).ToNurbsCurve();
            }
            else if (ccx.Count >= 2)
            {
                return new LineCurve(ccx[0], ccx[1]).ToNurbsCurve();
            }

            return crv;
        }
    }
}