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
        /// Remap number within a new domain, assuming the source domain is 0 to 1.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="targetDomain"></param>
        /// <returns></returns>
        public static double Remap(this double num, Interval targetDomain)
        {
            return Remap(num, new Interval(0, 1), targetDomain);
        }

        /// <summary>
        /// Remap number within a new domain.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="sourceDomain"></param>
        /// <param name="targetDomain"></param>
        /// <returns></returns>
        public static double Remap(this double num, Interval sourceDomain, Interval targetDomain)
        {
            var range = sourceDomain.Max - sourceDomain.Min;
            var pct = (num - sourceDomain.Min) / range;

            var targetRange = targetDomain.Max - targetDomain.Min;
            var targetStep = targetRange * pct;

            return targetDomain.Min + targetStep;
        }

        /// <summary>
        /// Remap a list of numbers, assuming the source domain is the maximum and minimum values within the list.
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="targetDomain"></param>
        /// <returns></returns>
        public static List<double> Remap(this List<double> nums, Interval targetDomain)
        {
            var sourceDomain = new Interval(nums.Min(), nums.Max());

            return nums.Select(x => x.Remap(sourceDomain, targetDomain)).ToList();
        }

        /// <summary>
        /// Remap a list of numbers.
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="sourceDomain"></param>
        /// <param name="targetDomain"></param>
        /// <returns></returns>
        public static List<double> Remap(this List<double> nums, Interval sourceDomain, Interval targetDomain)
        {
            return nums.Select(x => x.Remap(sourceDomain, targetDomain)).ToList();
        }
    }
}