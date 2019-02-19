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

        public List<Curve> PrimarySplinterCurves { get; set; } = new List<Curve>();
        public List<Curve> SecondarySplinterCurves { get; set; } = new List<Curve>();

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

            //Rhino.RhinoApp.WriteLine(target.ToString());

            //Generate ideal condition.
            var targetLength = (target / 2) * step;

            PrimarySpineCurve = new LineCurve(Centerline.PointAtStart, Centerline.GetLength() > 1 ? Centerline.PointAtLength(1) : Centerline.PointAtEnd);

            PrimarySpineCurve.Transform(Transform.Scale(PrimarySpineCurve.PointAtStart, targetLength));

            //Rhino.RhinoDoc.ActiveDoc.Objects.Add(PrimarySpineCurve);

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

                var validCxRegions = cxRegions.Where(x => x.CrossesQuadRegion(PrimarySpineCurve)).ToList();

                //Generate truncated spine.
                if (validCxRegions.Any())
                {
                    var endPts = Rhino.Geometry.Intersect.Intersection.CurveCurve(PrimarySpineCurve, validCxRegions[0].QuadRegion, 0.1, 0.1)
                        .Where(x => x.IsPoint)
                        .Select(x => x.PointA)
                        .OrderBy(x => PrimarySpineCurve.PointAtStart.DistanceTo(x));

                    PrimarySpineCurve = new LineCurve(PrimarySpineCurve.PointAtStart, endPts.First());

                    complete += Convert.ToInt32(Math.Floor(PrimarySpineCurve.GetLength() / step) * 2);

                    //Generate primary splinter curve.
                    var splinterCurve = validCxRegions[0].GetSplinterSegment(PrimarySpineCurve);

                    if (splinterCurve != null)
                    {
                        var splinterAxis = splinterCurve.SafeOffsetMove(15, validCxRegions[0].QuadRegion.GetBoundingBox(Plane.WorldXY).Center, false);

                        //Rhino.RhinoDoc.ActiveDoc.Objects.Add(splinterAxis);

                        splinterAxis.Transform(Transform.Scale(splinterAxis.PointAtNormalizedLength(0.5), 1 / splinterAxis.GetLength()));

                        //splinterAxis.Translate(new Vector3d(splinterAxis.PointAtEnd - splinterAxis.PointAtStart) * 15)

                        PrimarySplinterCurves.Add(splinterAxis);
                    }

                    PrimarySplinterCurves.Add(new LineCurve(endPts.Last(), new Point3d(endPts.Last()) + new Vector3d(endPts.First() - PrimarySpineCurve.PointAtStart)));
                }

                //If less than half of quota fulfilled, generate secondary spliter curves.

                if (target - complete > target * 0.5)
                {
                    SecondarySplinterCurves.Add(PrimarySpineCurve.SafeOffsetMove(22.5, Point3d.Origin, false));
                    SecondarySplinterCurves.Add(PrimarySpineCurve.SafeOffsetMove(22.5, Point3d.Origin, true));
                    SecondarySplinterCurves[SecondarySplinterCurves.Count - 1].Reverse();
                }

                return target - complete;

                //Generate tertiary splinter curves.
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

                    if (target - complete > 5)
                    {
                        //Generate secondary paths.
                        SecondarySplinterCurves.Add(PrimarySpineCurve.SafeOffsetMove(22.5, Point3d.Origin, false));
                        SecondarySplinterCurves.Add(PrimarySpineCurve.SafeOffsetMove(22.5, Point3d.Origin, true));
                        SecondarySplinterCurves[SecondarySplinterCurves.Count - 1].Reverse();
                    }

                    return target - complete;
                }
            }

            //Return unfulfilled target.
            complete += Convert.ToInt32(Math.Floor(PrimarySpineCurve.GetLength() / step) * 2);

            return target - complete;
        }
    }
}