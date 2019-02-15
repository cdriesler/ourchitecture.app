using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static double StandardDeviation(this List<double> vals)
        {
            var avg = vals.Average();
            var dev = vals.Select(x => Math.Pow(x - avg, 2)).Sum() / vals.Count;

            return dev;
        }
    }
}