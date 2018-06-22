using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FluentAssertions;
using FScruiser.Core.Services;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Services
{
    public class DataEntryDataServiceTest
    {
        [Fact]
        public void Ctor_Test()
        {
            using (var ds = CreateDataStore())
            {
                var unit = ds.From<CuttingUnitDO>().Query().FirstOrDefault();
                unit.Should().NotBeNull();

                var dataServ = new IDataEntryDataService(unit.Code, ds);

                dataServ.CuttingUnit.Should().NotBeNull("cutting unit required");
                dataServ.TallyHistory.Should().NotBeNull("tally history required");

                dataServ.TreeStrata.Should().NotBeNullOrEmpty("tree strata");
                dataServ.TreeStrata.Should().HaveCount(1, "only one tree strata");

                dataServ.NonPlotTrees.Should().NotBeNullOrEmpty("non plot trees");
                dataServ.NonPlotTrees.Should().HaveCount(1, "only one non plot tree");

                dataServ.PlotStrata.Should().NotBeNullOrEmpty("plot strata");

                foreach(var st in dataServ.PlotStrata)
                {
                    st.Plots.Should().NotBeNullOrEmpty();
                    foreach(var plt in st.Plots)
                    {
                        plt.Trees.Should().NotBeNullOrEmpty();
                    }
                }

                dataServ.DefaultStratum.Should().NotBeNull();
            }
        }

        [Fact]
        public void ReadUnitLevelCruisersTest()
        {
            using (var ds = CreateDataStore())
            {
                var unitLevelInitialsEmpty = IDataEntryDataService.ReadUnitLevelCruisers(ds);

                unitLevelInitialsEmpty.Should().NotBeNull();
                unitLevelInitialsEmpty.Should().BeEmpty();

                var initialsRaw = new string[] { " ", "", "A", "AB", " C " };
                var initialsExpected = new string[] { "A", "AB", "C" };

                var cuttingUnit = ds.From<CuttingUnitDO>().Query().First();
                var stratum = ds.From<StratumDO>().Query().First();

                long j = 0;
                foreach (var initial in initialsRaw)
                {
                    var tree = new TreeDO() { DAL = ds,
                        CuttingUnit = cuttingUnit,
                        Stratum = stratum,
                        TreeNumber = ++j,
                        Initials = initial };

                    tree.Save();
                }

                var unitLevelInitials = IDataEntryDataService.ReadUnitLevelCruisers(ds);

                unitLevelInitials.Should().Contain(initialsExpected);
            }
        }

        [Fact]
        public void IsTreeNumberAvalible_Test()
        {
            using (var ds = CreateDataStore())
            {
                var unit = ds.From<CuttingUnitDO>().Query().FirstOrDefault();
                unit.Should().NotBeNull();

                var dataServ = new IDataEntryDataService(unit.Code, ds);

                dataServ.IsTreeNumberAvalible(1).Should().BeFalse();
                dataServ.IsTreeNumberAvalible(2).Should().BeTrue();
            }
        }

        [Fact]
        public void GetNextNonPlotTreeNumber_Test()
        {
            var trees = new List<Tree>();

            IDataEntryDataService.GetNextTreeNumber(trees).Should().Be(1);

            trees.Add(new Tree() { TreeNumber = 1 });

            IDataEntryDataService.GetNextTreeNumber(trees).Should().Be(2);

            var tree = new Tree() { TreeNumber = 50 };
            trees.Add(tree);

            IDataEntryDataService.GetNextTreeNumber(trees).Should().Be(51);

            trees.Remove(tree);

            IDataEntryDataService.GetNextTreeNumber(trees).Should().Be(2);

        }

        DAL CreateDataStore(string salePurpose = null, string saleRegion = "01", IEnumerable<string> methods = null)
        {
            methods = methods ?? new string[] { CruiseMethods.STR, CruiseMethods.FIX };

            var ds = new DAL();

            var sale = new SaleDO()
            {
                DAL = ds,
                SaleNumber = "12345",
                Region = saleRegion,
                Forest = "11",
                District = "something",
                Purpose = salePurpose
            };
            sale.Save();

            var cuttingUnit = new CuttingUnitDO()
            {
                DAL = ds,
                Code = "01"
            };
            cuttingUnit.Save();

            var tdv = new TreeDefaultValueDO()
            {
                DAL = ds,
                Species = "something",
                PrimaryProduct = "something",
                LiveDead = "L"
            };
            tdv.Save();

            int counter = 0;
            foreach (var method in methods)
            {
                var stratum = new StratumDO()
                {
                    DAL = ds,
                    Code = counter++.ToString("d2"),
                    Method = method
                };
                stratum.Save();
                stratum.CuttingUnits.Add(cuttingUnit);
                stratum.CuttingUnits.Save();

                var sg = new SampleGroupDO()
                {
                    DAL = ds,
                    Code = 1.ToString("d2"),
                    Stratum = stratum,
                    CutLeave = "C",
                    UOM = "something",
                    PrimaryProduct = "something"
                };
                sg.Save();
                sg.TreeDefaultValues.Add(tdv);
                sg.TreeDefaultValues.Save();

                if(CruiseMethods.PLOT_METHODS.Contains(method))
                {
                    var plot = new PlotDO() { DAL = ds, Stratum = stratum, CuttingUnit = cuttingUnit, PlotNumber = 1 };
                    plot.Save();

                    var tree = new TreeDO()
                    {
                        DAL = ds,
                        CuttingUnit = cuttingUnit,
                        Stratum = stratum,
                        Plot = plot,
                        SampleGroup = sg,
                        TreeDefaultValue = tdv,
                        TreeNumber = 1
                    };
                    tree.Save();
                }
                else
                {
                    var tree = new TreeDO()
                    {
                        DAL = ds,
                        CuttingUnit = cuttingUnit,
                        Stratum = stratum,
                        SampleGroup = sg,
                        TreeDefaultValue = tdv,
                        TreeNumber = 1
                    };
                    tree.Save();
                }                
            }

            return ds;
        }
    }
}