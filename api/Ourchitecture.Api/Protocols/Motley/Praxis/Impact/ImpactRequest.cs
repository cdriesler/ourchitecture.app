using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley
{
    public class ImpactRequest
    {
        public MotleyRequest ParentRequest { get; }
        public List<Curve> RuinRegions { get; }
        public int MarketQuota { get; }

        public ImpactRequest(MotleyRequest parent, List<Curve> regions, int quota)
        {
            ParentRequest = parent;
            RuinRegions = regions;
            MarketQuota = quota;
        }
    }
}