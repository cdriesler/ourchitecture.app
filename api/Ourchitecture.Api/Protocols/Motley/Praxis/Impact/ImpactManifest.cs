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
        public Curve PrimaryPathCurve { get; }
        public Curve PrimaryMarketCellCurve { get; }
        public List<Curve> PrimaryRuinRegions { get; }
        public int PrimaryMarketQuota { get; }

        //Volatility values
        /// <summary>
        /// Standard deviation along path from the line that connects its endpoints.
        /// </summary>
        public double PrimaryPathLinearVolatility { get; set; }
        /// <summary>
        /// Standard deviation of each path segment's slope.
        /// </summary>
        public double PrimaryPathSegmentSlopeVolatility { get; set; }
        /// <summary>
        /// Standard deviation of each cell segment's length.
        /// </summary>
        public double PrimaryMarketCellSegmentLengthVolatility { get; set; }
        /// <summary>
        /// Standard deviation of each cell segment's slope.
        /// </summary>
        public double PrimaryMarketCellSegmentSlopeVolatility { get; set; }
        /// <summary>
        /// Difference between cell's world coordinates bounding box area and cell's area.
        /// </summary>
        public double PrimaryMarketCellAreaVolatility { get; set; }

        //Noise values (volatility values normalized between 0 and 1 based on praxis qualities)
        public double NoiseFromPrimaryPathLinearVolatility { get; set; }
        public double NoiseFromPrimaryPathSegmentVolatility { get; set; }
        public double NoiseFromPrimaryMarketCellSegmentLengthVolatility { get; set; }
        public double NoiseFromPrimaryMarketCellSegmentSlopeVolatility { get; set; }
        public double NoiseFromPrimaryMarketCellAreaVolatility { get; set; }

        public ImpactManifest(ImpactRequest req)
        {
            PlanarBounds = req.ParentRequest.Boundary;
            PrimaryPathCurve = req.ParentRequest.PrimaryPath;
            PrimaryMarketCellCurve = req.ParentRequest.Cell;
            PrimaryRuinRegions = req.RuinRegions;
            PrimaryMarketQuota = req.MarketQuota;
        }
    }
}