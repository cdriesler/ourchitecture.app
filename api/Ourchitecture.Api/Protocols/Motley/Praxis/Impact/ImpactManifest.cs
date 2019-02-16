using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class ImpactManifest
    {
        //Primitive input
        public Curve PlanarBounds { get; }
        public Curve PrimaryAxisCurve { get; }
        public Curve PrimaryMarketCellCurve { get; }
        public List<Curve> PrimaryRuinRegions { get; }
        public int PrimaryMarketQuota { get; }

        public ImpactManifest(ImpactRequest req)
        {
            PlanarBounds = req.ParentRequest.Boundary;
            PrimaryAxisCurve = req.ParentRequest.PrimaryPath;
            PrimaryMarketCellCurve = req.ParentRequest.Cell;
            PrimaryRuinRegions = req.RuinRegions;
            PrimaryMarketQuota = req.MarketQuota;
        }
    }
}