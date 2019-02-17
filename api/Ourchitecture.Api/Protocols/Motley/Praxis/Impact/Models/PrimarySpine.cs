using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class PrimarySpine
    {
        public Curve Centerline { get; set; }
        public Vector3d Direction { get; set; }
        public Curve PrimarySpineCurve { get; set; }
        public Curve LeftEdge { get; set; }
        public Curve LeftAxis { get; set; }
        public Curve RightEdge { get; set; }
        public Curve RightAxis { get; set; }

        public Curve PrimarySplinterCurves { get; set; }
        public Curve SecondarySplinterCurves { get; set; }

        public List<Curve> LotProfiles { get; set; } = new List<Curve>();

        public PrimarySpine(Curve center)
        {
            Centerline = center.DuplicateCurve();
            Direction = new Vector3d(Centerline.PointAtEnd - Centerline.PointAtStart);
            Direction.Unitize();
        }

        public int Grow(int target, double step, List<MemorialRegion> regions, Curve bounds)
        {
            var complete = 0;

            Rhino.RhinoApp.WriteLine(target.ToString());

            //Generate ideal condition.
            var targetLength = (target / 2) * step;

            PrimarySpineCurve = new LineCurve(Centerline.PointAtStart, Centerline.PointAtLength(1));

            PrimarySpineCurve.Transform(Transform.Scale(PrimarySpineCurve.PointAtStart, targetLength));

            //Check if new line intersects any regions.
            var cxRegions = regions
                .Where(x => x.IntersectsWithQuadRegion(PrimarySpineCurve))
                .OrderBy(x => PrimarySpineCurve.PointAtStart.DistanceTo(x.QuadRegion.GetBoundingBox(Plane.WorldXY).Center))
                .ToList();

            if (cxRegions.Any())
            {
                //If it starts inside a region, ignore first intersection.
                var isStartsInRegion = cxRegions[0].QuadRegion.Contains(PrimarySpineCurve.PointAtStart, Plane.WorldXY, 0.1) == PointContainment.Inside;

                if (isStartsInRegion)
                {
                    cxRegions.RemoveAt(0);
                }

                //Generate truncated spine.
                if (cxRegions.Any())
                {
                    var endPt = Rhino.Geometry.Intersect.Intersection.CurveCurve(PrimarySpineCurve, cxRegions[0].QuadRegion, 0.1, 0.1)
                        .Where(x => x.IsPoint)
                        .Select(x => x.PointA)
                        .OrderBy(x => PrimarySpineCurve.PointAtStart.DistanceTo(x))
                        .First();

                    PrimarySpineCurve = new LineCurve(PrimarySpineCurve.PointAtStart, endPt);

                    complete += Convert.ToInt32(Math.Floor(PrimarySpineCurve.GetLength() / step) * 2);
                }

                //Generate primary splinter curve.

                //If less than half of quota fulfilled, generate secondary spliter curves.

                //Generate tertiary splinter curves.

                return target - complete;
            }

            //If not, check if edges intersect any regions.


            //If no intersections, trim path to bounds.
            var cxBounds = Rhino.Geometry.Intersect.Intersection.CurveCurve(PrimarySpineCurve, bounds, 0.1, 0.1);

            if (cxBounds.Any(x => x.IsPoint))
            {
                var cxPts = cxBounds
                    .Where(x => x.IsPoint)
                    .Select(x => x.PointA)
                    .OrderBy(x => PrimarySpineCurve.PointAtStart.DistanceTo(x))
                    .ToList();

                if (cxPts.First().DistanceTo(PrimarySpineCurve.PointAtStart) < 0.01)
                {
                    cxPts.RemoveAt(0);
                }

                if (cxPts.Any())
                {
                    PrimarySpineCurve = new LineCurve(PrimarySpineCurve.PointAtStart, cxPts[0]).ToNurbsCurve();

                    complete += Convert.ToInt32(Math.Floor(PrimarySpineCurve.GetLength() / step) * 2);

                    return target - complete;
                }
            }

            //Return unfulfilled target.
            complete += Convert.ToInt32(Math.Floor(PrimarySpineCurve.GetLength() / step) * 2);

            return target - complete;
        }
    }
}