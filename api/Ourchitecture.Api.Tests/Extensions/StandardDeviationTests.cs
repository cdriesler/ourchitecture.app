using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Rhino.Geometry;
using Ourchitecture.Api;
using NUnit.Framework;

namespace Ourchitecture.Api.Tests.Extensions
{
    [TestFixture]
    public class StandardDeviationTests
    {
        [Test]
        public void Given_four_equal_values_should_return_zero()
        {
            var vals = new List<double> { 5, 5, 5, 5 };

            var res = vals.StandardDeviation();

            res.Should().Be(0, "because all values are equal");
        }

        [Test]
        public void Given_ascending_series_should_not_return_zero()
        {
            var vals = new List<double> { 1, 2, 3, 4, 5 };

            var res = vals.StandardDeviation();

            res.Should().NotBe(0, "because no values are equal");
        }
    }
}
