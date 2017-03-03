using CruiseDAL.Schema;
using FluentAssertions;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Fact]
        public void TestReadPlots()
        {
            var tempPath = "temp.cruise";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            using (var ds = new CruiseDAL.DAL(tempPath, true))
            {
                var unit = new CuttingUnit()
                {
                    Code = "1"
                };

                var st = new PlotStratum()
                {
                    Code = "1",
                    Method = CruiseMethods.THREEPPNT
                };

                ds.Insert(unit);

                ds.Insert(st);

                ds.Insert(new Plot()
                {
                    CuttingUnit = unit,
                    Stratum = st,
                    PlotNumber = 1
                });

                st.DAL = ds;
                st.PopulatePlots(unit.CuttingUnit_CN.Value);

                st.Plots.Should().NotBeNullOrEmpty();
                st.Plots.Should().OnlyContain(x => x is Plot3PPNT);
            }
        }
    }
}