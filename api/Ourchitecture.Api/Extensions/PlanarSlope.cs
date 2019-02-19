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
        /// Returns slope of curve projected to ground plane.
        /// </summary>
        /// <param name="crv"></param>
        /// <returns></returns>
        public static double PlanarSlope(this Curve crv)
        {
            var isIncreasing = crv.PointAtEnd.X > crv.PointAtStart.X;

            var startPt = isIncreasing ? crv.PointAtStart : crv.PointAtEnd;
            var endPt = isIncreasing ? crv.PointAtEnd : crv.PointAtStart;

            var rise = endPt.Y - startPt.Y;
            var run = endPt.X - startPt.X;

            if (run < 0.1) return 1;

            return rise / run;
        }
    }
}