using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class PrimarySpine
    {
        public Curve Centerline { get; set; }
        public Curve PrimarySpineCurve { get; set; }
        public Curve LeftEdge { get; set; }
        public Curve LeftAxis { get; set; }
        public Curve RightEdge { get; set; }
        public Curve RightAxis { get; set; }

        public List<Curve> LotProfiles { get; set; } = new List<Curve>();

        public PrimarySpine(Curve center)
        {
            Centerline = center.DuplicateCurve();
        }
    }
}