using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static double PartitionByApproximateSize(this double num, double size)
        {
            var min = Convert.ToInt32(Math.Floor(num / size));
            var dif = num - (min * size);

            return size + (dif / min);
        }
    }
}