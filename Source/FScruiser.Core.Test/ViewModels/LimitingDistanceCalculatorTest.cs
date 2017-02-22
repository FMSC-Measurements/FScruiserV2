using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.DataEntry;
using Xunit;
using FluentAssertions;

namespace FScruiser.Core.Test.ViewModels
{
    public class LimitingDistanceCalculatorTest
    {
        public LimitingDistanceCalculatorTest()
        {
        }

        [Fact]
        public void TestDeterminTreeInOrOut()
        {
            LimitingDistanceCalculator.DeterminTreeInOrOut(.01, .01).Should().BeTrue();

            LimitingDistanceCalculator.DeterminTreeInOrOut(.012, .011).Should().BeTrue();

            LimitingDistanceCalculator.DeterminTreeInOrOut(.011, .012).Should().BeTrue();

            LimitingDistanceCalculator.DeterminTreeInOrOut(.02, .015).Should().BeTrue();

            LimitingDistanceCalculator.DeterminTreeInOrOut(.02, .014).Should().BeFalse();
        }
    }
}