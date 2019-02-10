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

        //Noise intervals
        public Interval NoiseFromCellProfileSegments { get; set; }
        public Interval NoiseFromCellProfileCorners { get; set; }

        //Placed geometry
        public List<VendorEntrance> Entrances { get; set; }


        public VendorManifest()
        {

        }

        public List<Brep> GetAllGeometry()
        {
            var geo = new List<Brep>();

            Entrances.ForEach(x => geo.AddRange(x.Entrance));

            return geo;
        }
    }
}