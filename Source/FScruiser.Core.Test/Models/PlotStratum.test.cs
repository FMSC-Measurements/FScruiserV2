using FluentAssertions;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Models
{
    public class PlotStratumTest
    {
        [Fact]
        public void TestMakePlot()
        {
            var dal = new CruiseDAL.DAL("something");

            var unit = new CuttingUnit()
            {
                DAL = dal
            };

            var stratum = new PlotStratum()
            {
                DAL = dal
            };

            var plot = stratum.MakePlot(unit);

            plot.Should().BeOfType<Plot>();

            plot.DAL.ShouldBeEquivalentTo(dal);

            plot.CuttingUnit.ShouldBeEquivalentTo(unit);
            plot.Stratum.ShouldBeEquivalentTo(stratum);

            plot.Trees.Should().NotBeNull();
        }

        [Fact]
        public void TestMakePlot_3ppnt()
        {
            var dal = new CruiseDAL.DAL("something");

            var unit = new CuttingUnit()
            {
                DAL = dal
            };

            var stratum = new PlotStratum()
            {
                DAL = dal
            };

            var plot = stratum.MakePlot(unit);

            plot.Should().BeOfType<Plot3PPNT>();

            plot.CuttingUnit.ShouldBeEquivalentTo(unit);
            plot.Stratum.ShouldBeEquivalentTo(stratum);

            plot.Trees.Should().NotBeNull();
        }
    }
}