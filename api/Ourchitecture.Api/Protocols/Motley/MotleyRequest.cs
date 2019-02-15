using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public abstract class MotleyRequest
    {
        public Curve Boundary { get; }
        public Curve PrimaryPath { get; }
        public List<Curve> Regions { get; } = new List<Curve>();

        public MotleyRequest(Curve boundary, Curve path, List<Curve> regions)
        {
            Boundary = boundary;
            PrimaryPath = path;
            Regions = regions;
        }
    }
}