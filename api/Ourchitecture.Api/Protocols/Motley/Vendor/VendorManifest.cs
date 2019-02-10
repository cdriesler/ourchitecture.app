using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Vendor
{
    public class VendorManifest
    {
        //Constraints
        public Curve PlanarBounds { get; set; }
        public Brep VolumeBounds { get; set; }

        //Cell
        public Curve CellProfile { get; set; }
        public double CellProfileWidth { get; set; }
        public double CellProfileDepth { get; set; }
        public Point3d CellProfileCenter { get; set; }

        public double CellProfileSegmentVolatility { get; set; }
        public double CellProfileCornerAngleVolatility { get; set; }

        //Path
        public Curve Path { get; set; }
        public List<Curve> PathSegments { get; set; }
        public List<Point3d> PathPoints { get; set; }

        public double PathDriftVolatility { get; set; }

        //Noise intervals
        public Interval NoiseFromCellProfileSegments { get; set; }
        public Interval NoiseFromCellProfileCorners { get; set; }
        public Interval NoiseFromPathDrift { get; set; }

        //Intermediate geometry
        public List<Point3d> PathSamplePoints { get; set; }
        public List<double> PathSamplePointDistances { get; set; }
        public List<double> PathSamplePointNormalizedDistances { get; set; }
        public List<Plane> PathSamplePointFrames { get; set; }

        public List<VendorPathFlank> LeftPathFlanks { get; set; }
        public List<VendorPathFlank> RightPathFlanks { get; set; }

        //Placed geometry
        public List<VendorEntrance> Entrances { get; set; }


        public VendorManifest()
        {

        }

        public List<GeometryBase> GetAllGeometry()
        {
            var geo = new List<GeometryBase>();


            //Entrances.ForEach(x => geo.AddRange(x.Entrance));

            return geo;
        }

        public string ToSvg(VendorDrawing type)
        {
            throw new NotImplementedException();
        }
    }

    public enum VendorDrawing
    {
        DefaultPlan,
        Section
    }
}