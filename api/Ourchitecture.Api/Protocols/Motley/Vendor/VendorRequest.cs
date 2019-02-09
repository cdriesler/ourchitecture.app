using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Vendor
{
    public class VendorRequest : MotleyRequest
    {
        public override Curve Boundary { get; }
        public override Curve Path { get; }
        
        public VendorRequest(string req)
        {
            var request = JsonConvert.DeserializeObject<VendorRequest>(req);

            Boundary = request.Boundary;
            Path = request.Path;
        }
    }
}