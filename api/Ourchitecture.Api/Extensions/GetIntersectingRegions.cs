using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crv"></param>
        /// <param name="regions"></param>
        /// <returns>List of regions that intersect the curve in ascending order of distance from the curve's starting point.</returns>
        public static List<Curve> GetIntersectingRegions(this Curve crv, List<Curve> regions)
        {
            return regions
                .Where(x => Rhino.Geometry.Intersect.Intersection.CurveCurve(crv, x, 0.1, 0.1).Any(y => y.IsPoint))
                .OrderBy(x => crv.PointAtStart.DistanceTo(x.GetBoundingBox(Plane.WorldXY).Center)).ToList();
        }
    }
}