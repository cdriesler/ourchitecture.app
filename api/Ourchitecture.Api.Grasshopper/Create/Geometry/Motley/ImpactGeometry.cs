using System;
using Ourchitecture.Api.Protocols.Motley.Impact;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Create.Geometry
{
    public static class ImpactGeometry
    {
        public static List<GeometryBase> GetGeometry(ImpactManifest manifest, GeometryRequestType type)
        {
            if (type == GeometryRequestType.Test) return TestGeometry(manifest);

            return null;
        }

        private static List<GeometryBase> TestGeometry(ImpactManifest manifest)
        {
            var geo = new List<GeometryBase>();

            manifest.PrimarySpines.ForEach(x => geo.Add(x.PrimarySpineCurve));
            manifest.SecondarySpines.ForEach(x => geo.Add(x.Centerline));

            return geo;
        }
    }
}
