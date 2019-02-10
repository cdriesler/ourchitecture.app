using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static double NoiseBasedValue(this Interval target, Random random, Interval noise)
        {
            return ((random.NextDouble() * noise.Max) * (target.Max - target.Min)) + target.Min;
        }
    }
}