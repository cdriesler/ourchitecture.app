using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static Brep ExtrudeAndCap(this Curve crv, Vector3d direction)
        {
            var capA = Brep.CreatePlanarBreps(crv, 0.1)[0];

            var extrusion = Extrusion.CreateExtrusion(crv, direction).ToBrep();

            var capBCrv = crv.DuplicateCurve();
            capBCrv.Translate(direction);

            var capB = Brep.CreatePlanarBreps(capBCrv, 0.1)[0];

            var edges = new List<Brep>
            {
                extrusion,
                capA,
                capB
            };

            return Brep.JoinBreps(edges, 0.1)[0];
        }
    }
}