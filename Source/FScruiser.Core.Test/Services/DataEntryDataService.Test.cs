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
            using (var ds = CreateDataStrore())
            {
                var unit = ds.From<CuttingUnitDO>().Query().FirstOrDefault();
                unit.Should().NotBeNull();

                var dataServ = new IDataEntryDataService(unit.Code, ds);

                dataServ.CuttingUnit.Should().NotBeNull();
                dataServ.CuttingUnit.TallyHistoryBuffer.Should().NotBeNull();

                dataServ.TreeStrata.Should().NotBeNullOrEmpty();
                dataServ.TreeStrata.Should().HaveCount(1);

                dataServ.NonPlotTrees.Should().NotBeNullOrEmpty();
                dataServ.NonPlotTrees.Should().HaveCount(1);

                dataServ.DefaultStratum.Should().NotBeNull();
            }
        }

        [Fact]
        public void MakeLogDataService_Test()
        {
            using (var ds = CreateDataStrore())
            {
                var unit = ds.From<CuttingUnitDO>().Query().FirstOrDefault();
                unit.Should().NotBeNull();

                var dataServ = new IDataEntryDataService(unit.Code, ds);

                var tree = dataServ.NonPlotTrees.First();

                var logDataServ = dataServ.MakeLogDataService(tree);

                logDataServ.Should().NotBeNull();
            }
        }

        [Fact]
        public void IsTreeNumberAvalible_Test()
        {
            using (var ds = CreateDataStrore())
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
            var dataService = new IDataEntryDataService();
            dataService.NonPlotTrees = new List<Tree>();

            dataService.GetNextNonPlotTreeNumber().ShouldBeEquivalentTo(1);

            dataService.NonPlotTrees.Add(new Tree() { TreeNumber = 1 });

            dataService.GetNextNonPlotTreeNumber().ShouldBeEquivalentTo(2);

            var tree = new Tree() { TreeNumber = 50 };
            dataService.NonPlotTrees.Add(tree);

            dataService.GetNextNonPlotTreeNumber().ShouldBeEquivalentTo(51);

            dataService.NonPlotTrees.Remove(tree);

            dataService.GetNextNonPlotTreeNumber().ShouldBeEquivalentTo(2);

        }

        DAL CreateDataStrore(string salePurpose = null, string saleRegion = "01", IEnumerable<string> methods = null)
        {
            methods = methods ?? new string[] { CruiseMethods.STR };

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

            int counter = 0;
            foreach (var method in methods)
            {
                var stratum = new StratumDO()
                {
                    DAL = ds,
                    Code = counter++.ToString("d2"),
                    Method = CruiseDAL.Schema.CruiseMethods.STR
                };
                stratum.Save();
                stratum.CuttingUnits.Add(cuttingUnit);
                stratum.CuttingUnits.Save();

                var tdv = new TreeDefaultValueDO()
                {
                    DAL = ds,
                    Species = "something",
                    PrimaryProduct = "something",
                    LiveDead = "L"
                };
                tdv.Save();

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

            return ds;
        }
    }
}