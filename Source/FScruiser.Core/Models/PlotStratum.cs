using System;
using System.Collections.Generic;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.Sampling;
using System.Xml;
using System.Xml.Serialization;

namespace FSCruiser.Core.Models
{
    public class PlotStratum : Stratum
    {
        [XmlIgnore]
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
        [XmlIgnore]
        public ThreePSelecter SampleSelecter { get; set; }

        [XmlIgnore]
        [IgnoreField]
        public bool IsSingleStage
        {
            get
            {
                return this.Method == CruiseMethods.PNT
                    || this.Method == CruiseMethods.FIX;
            }
        }

        [XmlArray]
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
            //HACK covariance wasn't added until C# 4.0 so we need to do some inefficient coding here
            if (Is3PPNT)
            {
                var source = DAL.From<Plot3PPNT>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Read(this.Stratum_CN, cuttingUnit_CN);

                foreach (var plot in source)
                {
                    plot.Stratum = this;
                    yield return plot;
                }
            }
            else
            {
                var source = DAL.From<Plot>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Read(this.Stratum_CN, cuttingUnit_CN);

                foreach (var plot in source)
                {
                    plot.Stratum = this;
                    yield return plot;
                }
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
                highestInUnit = DAL.ExecuteScalar<int?>(query1) ?? 0;

                string query2 = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0} AND Stratum_CN = {1}", cuttingUnit_CN, Stratum_CN);
                highestInStratum = DAL.ExecuteScalar<int?>(query2) ?? 0;

                if (highestInUnit > highestInStratum && highestInUnit > 0)
                {
                    return highestInUnit;
                }
                return highestInUnit + 1;
            }
            catch (Exception e)
            {
                Logger.Log.E("Unable to establish next plot number", e);
                return 1;
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
                    Field = CruiseDAL.Schema.TREE.COUNTORMEASURE,
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