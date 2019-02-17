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
            var testMemorialRegion = manifest.MemorialRegions[0];

            var geo = new List<GeometryBase>
            {
                testMemorialRegion.QuadSegmentA,
                testMemorialRegion.QuadSegmentB,
                testMemorialRegion.QuadSegmentC,
                testMemorialRegion.QuadSegmentD,
            };

            return geo;
        }
    }
}
