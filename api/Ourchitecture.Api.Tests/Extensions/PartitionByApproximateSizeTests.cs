using NUnit.Framework;
using FluentAssertions;

namespace Ourchitecture.Api.Tests.Extensions
{
    [TestFixture]
    public class PartitionByApproximateSizeTests
    {
        [Test]
        public void Given_value_of_five_and_size_of_two_should_return_two_and_a_half()
        {
            var val = (5.0).PartitionByApproximateSize(2);

            val.Should().Be(2.5, "because that is the closest value to two that will evenly divide five.");
        }
    }
}
