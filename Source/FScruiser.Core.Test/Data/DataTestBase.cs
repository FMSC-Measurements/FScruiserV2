using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FScruiser.Core.Test.Data
{
    public class DataTestBase
    {
        protected DAL CreateDataStore(string salePurpose = null, string saleRegion = "01", IEnumerable<string> methods = null)
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

                if (CruiseMethods.PLOT_METHODS.Contains(method))
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

                var countTree = new CountTree()
                {
                    CuttingUnit_CN = cuttingUnit.CuttingUnit_CN,
                    SampleGroup_CN = sg.SampleGroup_CN,
                    TreeDefaultValue_CN = tdv.TreeDefaultValue_CN,
                };

                ds.Save(countTree);
            }

            return ds;
        }
    }
}
