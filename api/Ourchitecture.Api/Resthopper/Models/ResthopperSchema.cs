using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Resthopper.Models
{
    public class ResthopperSchema
    {
        [JsonProperty(PropertyName = "algo")]
        public string Algo { get; set; }

        [JsonProperty(PropertyName = "pointer")]
        public string Pointer { get; set; }

        [JsonProperty(PropertyName = "values")]
        public List<DataTree<ResthopperObject>> Values { get; set; }

        public ResthopperSchema()
        {
            Values = new List<DataTree<ResthopperObject>>();
        }
    }

    public class IoQuerySchema
    {
        [JsonProperty(PropertyName = "requestedFile")]
        public string RequestedFile { get; set; }
    }

    public class IoResponseSchema
    {
        public List<string> InputNames { get; set; }
        public List<string> OutputNames { get; set; }
    }
}