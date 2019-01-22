using Nancy;
using Rhino.Geometry;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ourchitecture.Api.Routes
{
    public class BaseModule : NancyModule
    {
        public BaseModule()
        {
            Get["/"] = _ => "Hello World!";
            Get["/health"] = _ => "Healthy!";
            Get["/test"] = _ =>
            {
                var testLineA = new LineCurve(new Point3d(0, -1, 0), new Point3d(0, 1, 0));
                var testLineB = new LineCurve(new Point3d(-1, 0, 0), new Point3d(1, 0, 0));

                var ccx = Rhino.Geometry.Intersect.Intersection.CurveCurve(testLineA, testLineB, 0.1, 0.1);

                return ccx.Count.ToString();
            };
        }
    }

    public class GrasshopperInput
    {
        [JsonProperty(PropertyName = "algo")]
        public string Algo { get; set; }

        [JsonProperty(PropertyName = "values")]
        public Dictionary<string, object> Values { get; set; }
    }

    public class GrasshopperOutput
    {
        public GrasshopperOutput()
        {
            this.Items = new List<GrasshopperOutputItem>();
        }

        [JsonProperty(PropertyName = "items")]
        public List<GrasshopperOutputItem> Items { get; set; }
    }

    public class GrasshopperOutputItem
    {
        [JsonProperty(PropertyName = "type")]
        public string TypeHint { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}