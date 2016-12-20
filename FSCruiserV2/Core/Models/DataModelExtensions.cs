using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public static class StratumExtensions
    {
        public static string GetDescriptionShort(this StratumDO stratum)
        {
            if (stratum == null) return "-";
            if (stratum.Method.StartsWith("P"))
            {
                return string.Format("{0}-{1}- BAF: {2}", stratum.Code, stratum.Method, stratum.BasalAreaFactor);
            }
            else if (stratum.Method.StartsWith("F"))
            {
                return String.Format("{0}-{1}- Size: 1/{2}", stratum.Code, stratum.Method, stratum.FixedPlotSize);
            }
            return String.Format("{0}-{1} {2}", stratum.Code, stratum.Method, stratum.Description);
        }
    }

    public static class PlotExtensions
    {
        public static string GetDescription(this PlotDO plot)
        {
            //throw new NotImplementedException();
            return "<Plot Stats Place Holder>";
        }

        public static string GetDescriptionShort(this PlotDO plot)
        {
            StringBuilder sb = new StringBuilder();
            if (plot.Stratum.BasalAreaFactor > 0.0)
            {
                sb.AppendFormat(null, "Stratum:{0} {1} BAF:{2}", plot.Stratum.Code, plot.Stratum.Method, plot.Stratum.BasalAreaFactor);
            }
            else
            {
                sb.AppendFormat(null, "Stratum:{0} {1} 1/{2} acre", plot.Stratum.Code, plot.Stratum.Method, plot.Stratum.FixedPlotSize);
            }

            if (plot.Stratum.Method == "3PPNT")
            {
                sb.AppendFormat(null, " KPI:{0}", plot.KPI);
                if (plot.IsEmpty == true.ToString())
                {
                    sb.Append("-Count");
                }
                else
                {
                    sb.Append("-Measure");
                }
            }
            return sb.ToString();
        }
    }

    public static class SampleGroupExtensions
    {
        public static string GetSampleGroupDescription(this SampleGroupDO sampleGroup)
        {
            return "";
        }

        public static string GetDescriptionShort(this SampleGroupDO sampleGroup)
        {
            if (sampleGroup == null) { return "--"; }
            string code = (string.IsNullOrEmpty(sampleGroup.Code)) ? "<blank>" : sampleGroup.Code;
            return sampleGroup.Code + " " + sampleGroup.Description;
        }
    }

    public static class TreeExtensions
    {
        public static CountTree FindCountRecord(this TreeDO tree)
        {
            return tree.DAL.From<CountTree>()
                .Where("SampleGroup_CN = ? AND CuttingUnit_CN = ? AND (TreeDefaultValue_CN = ? OR ifnull(TreeDefaultValue_CN, 0) = 0)")
                .Read(tree.SampleGroup_CN
                , tree.CuttingUnit_CN
                , tree.TreeDefaultValue_CN).FirstOrDefault();
        }

        public static int ReadHighestLogNumber(this TreeDO tree)
        {
            long? value = (long?)tree.DAL.ExecuteScalar(
                String.Format("SELECT MAX(CAST(LogNumber AS NUMERIC)) FROM Log WHERE Tree_CN = {0};"
                , tree.Tree_CN));
            if (value.HasValue)
            {
                return (int)value.Value;
            }
            else
            {
                return 0;
            }
        }

        public static object ReadValidSampleGroups(this Tree tree)
        {
            if (tree == null || tree.Stratum == null)
            {
                return Constants.EMPTY_SG_LIST;
            }

            return tree.DAL.From<SampleGroup>()
                .Where("Stratum_CN = ?")
                .Read(tree.Stratum_CN).ToList();
        }

        public static ICollection<TreeDefaultValueDO> ReadValidTDVs(this Tree tree)
        {
            if (tree == null || tree.Stratum == null)
            { return Constants.EMPTY_SPECIES_LIST; }

            if (tree.SampleGroup == null)
            {
                //if stratum has only one sampleGroup, make it this tree's SG
                if (tree.DAL.GetRowCount("SampleGroup", "WHERE Stratum_CN = ?", tree.Stratum_CN) == 1)
                {
                    tree.SampleGroup = tree.DAL.From<SampleGroup>()
                        .Where("Stratum_CN = ?")
                        .Read(tree.Stratum_CN)
                        .FirstOrDefault();
                }

                if (tree.SampleGroup == null)
                {
                    return Constants.EMPTY_SPECIES_LIST;
                }
            }

            List<TreeDefaultValueDO> tdvs = tree.DAL.From<TreeDefaultValueDO>()
                .Join("SampleGroupTreeDefaultValue", "USING (TreeDefaultValue_CN)")
                .Where("SampleGroup_CN = ?")
                .Read(tree.SampleGroup_CN).ToList();

            return tdvs;
        }
    }
}