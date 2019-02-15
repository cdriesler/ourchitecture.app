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
        public abstract Curve Boundary { get; }
        public abstract Curve Path { get; }
    }
}