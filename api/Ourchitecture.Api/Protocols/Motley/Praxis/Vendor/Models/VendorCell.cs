using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Vendor
{
    public class VendorCell
    {
        public Curve CellProfile { get; set; }
        public Plane CellPlane { get; set; }
        public Curve FrontEdge { get; set; }
        public Curve BackEdge { get; set; }
        public Curve LeftEdge { get; set; }
        public Curve RightEdge { get; set; }

        public Brep CellVolume { get; set; }
        public Brep SculptedCellMass { get; set; }

        public Curve InteriorProfile { get; set; }
        public Curve PartitionProfile { get; set; }

        public Brep EntranceRemovalMass { get; set; }
        public Brep PrimaryInteriorRemovalMass { get; set; }
        public Brep SecondaryInteriorRemovalMass { get; set; }

        public VendorCell()
        {

        }
    }
}