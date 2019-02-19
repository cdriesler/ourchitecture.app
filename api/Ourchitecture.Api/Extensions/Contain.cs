using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        public static double Contain(this double num, Interval extents)
        {
            if (num < extents.Min)
            {
                return extents.Min;
            }

            if (num > extents.Max)
            {
                return extents.Max;
            }

            return num;
        }
    }
}