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
        public int MarketQuota { get; }

        public ImpactRequest(MotleyRequest parent, int quota)
        {
            ParentRequest = parent;
            MarketQuota = quota;
        }
    }
}