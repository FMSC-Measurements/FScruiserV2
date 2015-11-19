using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public static class CuttingUnitExtensions
    {
        //public static String GetDiscription(this CuttingUnitDO unit)
        //{
        //    return String.Format("{0} Area: {1}", unit.Description, unit.Area);
        //}

        public static DataEntryMode GetDataEntryMode(this CuttingUnitDO unit)
        {
            //"SELECT goupe_concat(Method, ' ') FROM Stratum JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = {0} GROUP BY Stratum.Method;";
            List<StratumVM> strata = unit.ReadStrata<StratumVM>();
            DataEntryMode mode = DataEntryMode.Unknown;
            foreach (StratumVM stratum in strata)
            {
                mode = mode | stratum.GetDataEntryMode();
            }

            return mode;
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
            throw new NotImplementedException();
            //return "<Plot Stats Place Holder>";
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
        public static string GetLogLevelDescription(this TreeVM tree)
        {
            return String.Format("Tree:{0} Sp:{1} DBH:{2} Ht:{3} MrchHt:{4} Logs:{5}",
                tree.TreeNumber,
                tree.Species,
                tree.DBH,
                tree.TotalHeight,
                tree.MerchHeightPrimary,
                tree.LogCount);

        }
    }
}
