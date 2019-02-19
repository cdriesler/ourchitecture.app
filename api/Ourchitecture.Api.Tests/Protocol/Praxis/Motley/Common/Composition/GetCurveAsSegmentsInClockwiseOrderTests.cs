using System;
using NUnit.Framework;
using FluentAssertions;
using Rhino.Geometry;
using Ourchitecture.Api.Protocols.Motley;

namespace Ourchitecture.Api.Tests
{
    [TestFixture]
    public class GetCurveAsSegmentsInClockwiseOrderTests
    {
        [Test]
        public void Given_square_should_return_four_segments()
        {
            var env = new Rectangle3d(Plane.WorldXY, 5, 5).ToNurbsCurve();

            var res = Composition.GetCurveAsSegmentsInClockwiseOrder(env);

            var val = res.Count;

            val.Should().Be(4, "because a square has four segments");
        }

        [Test]
        public void Given_open_curve_should_throw_exception()
        {
            var env = new LineCurve(Point3d.Origin, new Point3d(0, 1, 0)).ToNurbsCurve();

            Action act = () => { Composition.GetCurveAsSegmentsInClockwiseOrder(env); };

            act.Should().Throw<Exception>("because method cannot complete without a closed curve.");
        }
    }
}
