using System;
using System.Collections.Generic;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class PlotStratum : Stratum
    {
        [IgnoreField]
        public bool Is3PPNT
        {
            get
            {
                return Method == CruiseMethods.THREEPPNT;
            }
        }

        /// <summary>
        /// for 3ppnt
        /// </summary>
        public ThreePSelecter SampleSelecter { get; set; }

        [IgnoreField]
        public bool IsSingleStage
        {
            get
            {
                return this.Method == CruiseMethods.PNT
                    || this.Method == CruiseMethods.FIX;
            }
        }

        [IgnoreField]
        public IList<Plot> Plots { get; protected set; }

        public virtual Plot MakePlot(CuttingUnit cuttingUnit)
        {
            Plot newPlot;
            if (this.Is3PPNT)
            {
                newPlot = new Plot3PPNT(this.DAL)
                {
                    CuttingUnit = cuttingUnit,
                    Stratum = this,
                    PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value)
                };
            }
            else
            {
                newPlot = new Plot(this.DAL)
                {
                    CuttingUnit = cuttingUnit,
                    Stratum = this,
                    PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value)
                };
            }
            newPlot.Trees = new System.ComponentModel.BindingList<Tree>();
            return newPlot;
        }

        protected virtual IEnumerable<Plot> ReadPlots(long cuttingUnit_CN)
        {
            foreach (var plot in DAL.From<Plot>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Read(this.Stratum_CN, cuttingUnit_CN))
            {
                plot.Stratum = this;
                yield return plot;
            }
        }

        public virtual void PopulatePlots(long cuttingUnit_CN)
        {
            this.Plots = ReadPlots(cuttingUnit_CN).ToList();
        }

        protected int GetNextPlotNumber(long cuttingUnit_CN)
        {
            try
            {
                int highestInUnit = 0;
                int highestInStratum = 0;

                var query1 = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0}", cuttingUnit_CN);
                int? result1 = DAL.ExecuteScalar<int?>(query1);
                highestInUnit = result1.GetValueOrDefault(0);

                string query2 = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0} AND Stratum_CN = {1}", cuttingUnit_CN, Stratum_CN);
                int? result2 = DAL.ExecuteScalar<int?>(query2);
                highestInStratum = result2.GetValueOrDefault(0);

                if (highestInUnit > highestInStratum && highestInUnit > 0)
                {
                    return highestInUnit;
                }
                return highestInUnit + 1;
            }
            catch (Exception e)
            {
                Logger.Log.E("Unable to establish next plot number", e);
                return 0;
            }
        }

        public bool IsPlotNumberAvailable(long plotNumber)
        {
            System.Diagnostics.Debug.Assert(this.Plots != null);
            foreach (Plot pi in Plots)
            {
                if (pi.PlotNumber == plotNumber)
                {
                    return false;
                }
            }
            return true;
        }

        protected override IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        {
            var fields = InternalReadTreeFields();

            //if not a single stage plot strata
            //and count measure field is missing
            //automaticly add it
            if (!IsSingleStage
                && fields.FindIndex(((tfs) => tfs.Field == CruiseDAL.Schema.TREE.COUNTORMEASURE)) < 0)
            {
                var cmField = new TreeFieldSetupDO()
                {
                    Field = CruiseDAL.Schema.TREE.COUNTORMEASURE
                        ,
                    Heading = "C/M"
                };
                if (fields.Count > 5)
                {
                    fields.Insert(5, cmField);
                }
                else
                {
                    fields.Add(cmField);
                }
            }

            return fields;
        }
    }
}