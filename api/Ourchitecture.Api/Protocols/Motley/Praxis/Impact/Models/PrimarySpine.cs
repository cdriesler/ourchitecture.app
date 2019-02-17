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

        public List<Curve> LotProfiles { get; set; } = new List<Curve>();

        public PrimarySpine(Curve center)
        {
            Centerline = center.DuplicateCurve();
            Direction = new Vector3d(Centerline.PointAtEnd - Centerline.PointAtStart);
            Direction.Unitize();
        }

        public int Grow(int target, double step)
        {
            //Generate ideal condition.
            var targetLength = (target / 2) * step;

            PrimarySpineCurve = new LineCurve(Centerline.PointAtStart, Centerline.PointAtLength(1));

            PrimarySpineCurve.Transform(Transform.Scale(PrimarySpineCurve.PointAtStart, targetLength));

            //Check if new line intersects any regions.

            //If not, check if edges intersect any regions.

            //If no intersections, check if path is still in bounds.

            //Return unfulfilled target.

            return target;
        }
    }
}