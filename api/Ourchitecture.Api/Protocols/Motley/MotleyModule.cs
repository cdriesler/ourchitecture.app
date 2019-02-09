using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Rhino.Geometry;
using Newtonsoft.Json;

namespace Ourchitecture.Api.Protocols
{
    public class MotleyModule : NancyModule
    {
        private Dictionary<string, Action> MotleyRoutes { get; } = new Dictionary<string, Action>()
        {

        };

        public MotleyModule()
        {
            Get["/motley"] = _ => MotleyManifest();
            Post["/motley/{dialect}"] = parameters => MotleyRoutes[parameters.dialect];
        }

        private string MotleyManifest()
        {
            var crv = new LineCurve(Point3d.Origin, new Point3d(1, 1, 1));
            return JsonConvert.SerializeObject(crv);
            
            //return JsonConvert.SerializeObject(MotleyRoutes.Keys.Select(x => x));
        }
    }
}