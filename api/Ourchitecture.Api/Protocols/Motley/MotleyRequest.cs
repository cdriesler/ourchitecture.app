using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public class MotleyRequest
    {
        public Curve Boundary { get; }
        public Curve PrimaryPath { get; }
        public Curve Cell { get; }

        public MotleyRequest(Curve boundary, Curve path, Curve cell)
        {
            Boundary = boundary;
            PrimaryPath = path;
            Cell = cell;
        }
    }
}