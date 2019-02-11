using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api
{
    public static partial class Extensions
    {
        /// <summary>
        /// Boolean difference that guarantees a non-null result. If the difference fails at any point, the original mass is returned.
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="removals"></param>
        /// <returns></returns>
        public static Brep SafeBooleanDifference(this Brep volume, List<Brep> removals)
        {
            var mass = volume.DuplicateBrep();

            foreach(var removal in removals)
            {
                var diff = Brep.CreateBooleanDifference(mass, removal, 0.1);

                mass = diff == null || diff.Count() == 0 ? mass : diff[0];
            }

            return mass;
        }
    }
}