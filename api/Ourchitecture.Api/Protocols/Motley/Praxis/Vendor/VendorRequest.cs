using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public class VendorRequest : MotleyRequest
    {
        public override Curve Boundary { get; }
        public override Curve Path { get; }

        public Curve Cell { get; }
        
        public VendorRequest(string req)
        {
            var request = JsonConvert.DeserializeObject<VendorRequest>(req);

            Boundary = request.Boundary;
            Path = request.Path;
        }

        public VendorRequest(Curve boundary, Curve path, Curve cell)
        {
            Boundary = boundary;
            Path = path;
            Cell = cell;
        }
    }
}