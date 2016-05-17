using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using System.Diagnostics;

namespace FSCruiser.Core.Models
{
    public static class CuttingUnitExtensions
    {
        //public static String GetDiscription(this CuttingUnitDO unit)
        //{
        //    return String.Format("{0} Area: {1}", unit.Description, unit.Area);
        //}

        //public static DataEntryMode GetDataEntryMode(this CuttingUnitDO unit)
        //{
        //    //"SELECT goupe_concat(Method, ' ') FROM Stratum JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = {0} GROUP BY Stratum.Method;";
        //    List<StratumVM> strata = unit.ReadStrata<StratumVM>();
        //    DataEntryMode mode = DataEntryMode.Unknown;
        //    foreach (StratumVM stratum in strata)
        //    {
        //        mode = mode | stratum.GetDataEntryMode();
        //    }

        //    return mode;
        //}

        public static IList<StratumVM> GetTreeBasedStrata(this CuttingUnitDO unit)
        {
            Debug.Assert(unit != null);
            Debug.Assert(unit.DAL != null);

            IList<StratumVM> list = unit.DAL.From<StratumVM>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Where("CuttingUnitStratum.CuttingUnit_CN = ?" +
                        "AND Method IN ( '100', 'STR', '3P', 'S3P')")
                .Read(unit.CuttingUnit_CN).ToList();

            foreach (StratumVM s in list)
            {
                s.LoadTreeFieldNames();
            }
            return list;
        }

        public static IList<PlotStratum> GetPlotStrata(this CuttingUnitDO unit)
        {
            var list = unit.DAL.From<PlotStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method IN ( 'FIX', 'FCM', 'F3P', 'PNT', 'PCM', 'P3P', '3PPNT')")
                .Query(unit.CuttingUnit_CN).ToList();

            var fixCNT = unit.DAL.From<FixCNTStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method = '" + CruiseDAL.Schema.CruiseMethods.FIXCNT + "'")
                .Query(unit.CuttingUnit_CN);

            foreach (var st in fixCNT)
            {
                list.Add(st);
            }

            return list;
        }
    }

    public static class StratumExtensions
    {
        //public string GetDescription(this StratumDO stratum)
        //{
        //    throw new NotImplementedException();
        //    //return "";
        //}

        //public string GetDescription(this StratumDO stratum, CuttingUnitDO unit)
        //{
        //    throw new NotImplementedException();
        //    //return "";
        //}

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

        public static DataEntryMode GetDataEntryMode(this StratumDO stratum)
        {
            switch (stratum.Method)
            {
                case "100":
                    {
                        return DataEntryMode.Tree | DataEntryMode.HundredPct;
                    }
                case "STR":
                    {
                        return DataEntryMode.Tree | DataEntryMode.TallyTree;
                    }
                case "3P":
                    {
                        return DataEntryMode.Tree | DataEntryMode.ThreeP | DataEntryMode.TallyTree;
                    }
                case "PNT":
                case "FIX":
                    {
                        return DataEntryMode.Plot | DataEntryMode.OneStagePlot;
                    }
                case "FCM":
                case "PCM":
                    {
                        return DataEntryMode.Plot;
                    }

                case "F3P":
                case "P3P":
                case "3PPNT":
                    {
                        return DataEntryMode.Plot | DataEntryMode.ThreeP;
                    }
                case "S3P":
                default:
                    {
                        return DataEntryMode.HundredPct;//fall back on 100pct
                    }
            }

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
        public static CountTreeVM FindCountRecord(this TreeDO tree)
        {
            return tree.DAL.From<CountTreeVM>()
                .Where("SampleGroup_CN = ? AND CuttingUnit_CN = ? AND (TreeDefaultValue_CN = ? OR ifnull(TreeDefaultValue_CN, 0) = 0)")
                .Read(tree.SampleGroup_CN
                ,tree.CuttingUnit_CN
                ,tree.TreeDefaultValue_CN).FirstOrDefault();
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



        public static object ReadValidSampleGroups(this TreeVM tree)
        {
            if (tree == null || tree.Stratum == null)
            {
                return Constants.EMPTY_SG_LIST;
            }

            return tree.DAL.From<SampleGroupVM>()
                .Where("Stratum_CN = ?")
                .Read(tree.Stratum_CN).ToList();
        }

        public static ICollection<TreeDefaultValueDO> ReadValidTDVs(this TreeVM tree)
        {
            if (tree == null || tree.Stratum == null) 
            { return Constants.EMPTY_SPECIES_LIST; }


            if (tree.SampleGroup == null)
            {
                //if stratum has only one sampleGroup, make it this tree's SG
                if (tree.DAL.GetRowCount("SampleGroup", "WHERE Stratum_CN = ?", tree.Stratum_CN) == 1)
                {
                    tree.SampleGroup = tree.DAL.From<SampleGroupVM>()
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

            //if (Constants.NEW_SPECIES_OPTION)
            //{
            //    throw new NotImplementedException();
            //    //tdvs.Add(_newPopPlaceHolder);
            //    //return tdvs;
            //}
            //else
            //{
            //    return tdvs;
            //    //return tree.SampleGroup.TreeDefaultValues;
            //}


        }

    }
}
