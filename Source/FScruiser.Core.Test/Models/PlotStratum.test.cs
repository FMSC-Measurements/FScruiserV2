using CruiseDAL;
using CruiseDAL.DataObjects;
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
            using (var dal = new CruiseDAL.DAL(":memory:", true))
            {
                var unit = new CuttingUnit()
                {
                    Code = "01",
                    DAL = dal
                };

                dal.Insert(unit);

                var stratum = new PlotStratum()
                {
                    Code = "01",
                    Method = "something",
                    DAL = dal
                };

                dal.Insert(stratum);

                var plot = stratum.MakePlot(unit);

                plot.Should().BeOfType<Plot>();

                plot.PlotNumber.ShouldBeEquivalentTo(1L);

                plot.DAL.ShouldBeEquivalentTo(dal);

                plot.CuttingUnit.ShouldBeEquivalentTo(unit);
                plot.Stratum.ShouldBeEquivalentTo(stratum);

                plot.Trees.Should().NotBeNull();

                plot.Save();

                plot = stratum.MakePlot(unit);
                plot.PlotNumber.ShouldBeEquivalentTo(2L);
            }
        }

        [Fact]
        public void TestMakePlot_3ppnt()
        {
            using (var dal = new CruiseDAL.DAL(":memory:", true))
            {
                var unit = new CuttingUnit()
                {
                    Code = "01",
                    DAL = dal
                };

                dal.Insert(unit);

                var stratum = new PlotStratum()
                {
                    Code = "01",
                    Method = CruiseMethods.THREEPPNT,
                    DAL = dal
                };

                dal.Insert(stratum);

                var plot = stratum.MakePlot(unit);

                plot.Should().BeOfType<Plot3PPNT>();

                plot.PlotNumber.ShouldBeEquivalentTo(1L);

                plot.CuttingUnit.ShouldBeEquivalentTo(unit);
                plot.Stratum.ShouldBeEquivalentTo(stratum);

                plot.Trees.Should().NotBeNull();
            }
        }

        [Fact]
        public void TestReadPlots()
        {
            using (var ds = new CruiseDAL.DAL(":memory:", true))
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

        [Fact]
        public void TreeFieldsTest()
        {
            foreach (var method in CruiseMethods.PLOT_METHODS)
            {
                using (var dataStore = CreateDataStore(method))
                {
                    var stratum = dataStore.From<PlotStratum>().Query().FirstOrDefault();

                    stratum.TreeFields.Should().NotBeNullOrEmpty();
                    if (!stratum.IsSingleStage)
                    {
                        stratum.TreeFields.Should().Contain(f => f.Field == "CountOrMeasure");
                    }
                    if (stratum.Is3P)
                    {
                        stratum.TreeFields.Should().Contain(f => f.Field == "STM");
                    }
                }
            }
        }

        DAL CreateDataStore(string method)
        {
            var dataStore = new DAL();

            var stratum = new StratumDO()
            {
                DAL = dataStore,
                Code = "01",
                Method = method
            };

            stratum.Save();

            return dataStore;
        }
    }
}