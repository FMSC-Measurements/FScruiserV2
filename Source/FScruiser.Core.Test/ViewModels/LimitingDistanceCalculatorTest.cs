﻿using System;
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

        [Theory]
        [InlineData(.01,.01,true)]
        [InlineData(.012, .011, true)]
        [InlineData(.011,.012, true)]
        [InlineData(.02,.015, true)]
        [InlineData(.02,.014, false)]
        //for tree to be in slope distance (first value) must be less than or equal to limiting distance (second value)
        //both values are round to two decimal places
        //here we test edge cases where both values should be determined to be equal
        public void TestDeterminTreeInOrOut(double slopeDistance, double limitingDistance, bool expectedResult)
        {
            LimitingDistanceCalculator.DeterminTreeInOrOut(slopeDistance, limitingDistance).Should().Be(expectedResult);
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
            ld.Should().Be(expected);
        }

        [Fact]
        public void GenerateReportTest()
        {
            var calculator = new LimitingDistanceCalculator();

            calculator.GenerateReport().Should().BeNullOrEmpty("Because calculator with default values should generate a empty report");

            calculator.SlopeDistance = 1;
            calculator.DBH = 1;
            calculator.BAForFPSize = 1;
            calculator.MeasureTo = LimitingDistanceCalculator.MEASURE_TO_FACE;

            calculator.Recalculate();
            calculator.LimitingDistance.Should().BeGreaterThan(0, "Because we need to confirm that the calculator is setup to generate a positive limiting distance");

            var report = calculator.GenerateReport();
            report.Should().NotBeNullOrWhiteSpace();
            report.Should().NotContain("Azimuth", "Because azimuth should not be included if not greater than 0");

            calculator.Azimuth = 1;
            report = calculator.GenerateReport();
            report.Should().Contain("Azimuth");
        }
    }
}