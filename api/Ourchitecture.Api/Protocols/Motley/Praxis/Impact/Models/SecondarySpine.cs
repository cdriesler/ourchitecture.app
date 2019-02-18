using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class SecondarySpine
    {
        public Curve Centerline { get; set; }

        public SecondarySpine(Curve curve)
        {
            Centerline = curve.DuplicateCurve();
        }
    }
}