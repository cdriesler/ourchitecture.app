using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Vendor
{
    public class VendorPathFlank
    {
        public List<Point3d> FlankPoints { get; set; } = new List<Point3d>();
        public Curve FlankCurve { get; set; }

        public VendorPathFlank()
        {

        }

        public VendorPathFlank(List<Point3d> pts)
        {
            FlankPoints = pts;
            FlankCurve = new Polyline(pts).ToNurbsCurve();
        }

    }
}