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
            //for tree to be in slope distance (first value) must be less than or equal to limiting distance (second value)
            //both values are round to three decimal places
            //here we test edge cases where both values should be determined to be equal
            LimitingDistanceCalculator.DeterminTreeInOrOut(.001, .001).Should().BeTrue();
            LimitingDistanceCalculator.DeterminTreeInOrOut(.0012, .0011).Should().BeTrue();
            LimitingDistanceCalculator.DeterminTreeInOrOut(.0011, .0012).Should().BeTrue();
            LimitingDistanceCalculator.DeterminTreeInOrOut(.002, .0015).Should().BeTrue();
            LimitingDistanceCalculator.DeterminTreeInOrOut(.002, .0014).Should().BeFalse();
        }

        [Theory]
        //variable radius
        [InlineData(20.0, 10.0, 0, true, true, 19.028)]//to face
        [InlineData(20.0, 10.0, 50, true, true, 21.274)]//to face, w/ 50% slope
        [InlineData(20.0, 10.0, 0, true, false, 19.445)]//to center
        //fixed
        [InlineData(50.0, 10.0, 0, false, true, 16.236)]//to face
        [InlineData(50.0, 10.0, 50, false, true, 18.152)]//to face, w/ 50% slope
        [InlineData(50.0, 10.0, 0, false, false, 16.653)]//to center
        public void TestCalculateLimitingDistance(double BAForFPS, double dbh, int slopePCT, bool isVar, bool isFace, double expected)
        {
            int sigDec = 3;

            string measureTo = (isFace) ? LimitingDistanceCalculator.MEASURE_TO_FACE : LimitingDistanceCalculator.MEASURE_TO_CENTER;
            var ld = LimitingDistanceCalculator.CalculateLimitingDistance(BAForFPS, dbh, slopePCT, isVar, measureTo);
            ld = Math.Round(ld, sigDec);
            expected = Math.Round(expected, 3);
            ld.ShouldBeEquivalentTo(expected);
        }
    }
}