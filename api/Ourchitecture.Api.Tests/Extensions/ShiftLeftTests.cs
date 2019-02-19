using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Ourchitecture.Api.Tests.Extensions
{
    [TestFixture]
    public class ShiftLeftTests
    {
        [Test]
        public void Given_a_shift_of_two_should_return_accurate_shift()
        {
            var env = new List<string> { "C", "D", "A", "B" };

            var res = env.ShiftLeft(2);

            var val = res[0];

            val.Should().Be("A", "because the list should have shifted two positions left.");
        }

        [Test]
        public void Given_a_shift_of_one_should_return_accurate_shift()
        {
            var env = new List<string> { "D", "A", "B", "C" };

            var res = env.ShiftLeft(1);

            var val = res[0];

            val.Should().Be("A", "because the list should have shifted one position left.");
        }

        [Test]
        public void Given_a_shift_larger_than_list_size_should_roll_over_and_return_accurate_shift()
        {
            var env = new List<string> { "D", "A", "B", "C" };

            var res = env.ShiftLeft(5);

            var val = res[0];

            val.Should().Be("A", "because the list should have shifted five positions (one net position) left.");
        }
    }
}
