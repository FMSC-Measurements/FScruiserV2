using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CruiseDAL;
using FMSC.ORM.EntityModel.Attributes;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class PlotStratum : StratumVM
    {
        [IgnoreField]
        public bool Is3PPNT
        {
            get
            {
                return Method == CruiseDAL.Schema.CruiseMethods.THREEPPNT;
            }
        }

        [IgnoreField]
        public bool IsSingleStage
        {
            get
            {
                return this.Method == CruiseDAL.Schema.CruiseMethods.PNT
                    || this.Method == CruiseDAL.Schema.CruiseMethods.FIX;
            }
        }

        [IgnoreField]
        public IList<PlotVM> Plots { get; protected set; }


        public virtual PlotVM MakePlot(CuttingUnitVM cuttingUnit)
        {
            if (this.Is3PPNT)
            {
                return new Plot3PPNT(this.DAL)
                {
                    CuttingUnit = cuttingUnit,
                    Stratum = this,
                    PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value)
                };
            }
            else
            {
                return new PlotVM(this.DAL)
                {
                    CuttingUnit = cuttingUnit,
                    Stratum = this,
                    PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value)
                };
            }
        }

        public virtual void PopulatePlots(long cuttingUnit_CN)
        {
            this.Plots = this.DAL.From<PlotVM>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Query(this.Stratum_CN, cuttingUnit_CN)
                .ToList();

            //this.Plots = this.DAL.Read<PlotVM>("WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber"
            //            , this.Stratum_CN
            //            , cuttingUnit_CN);
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
            foreach (PlotVM pi in Plots)
            {
                if (pi.PlotNumber == plotNumber)
                {
                    return false;
                }
            }
            return true;
        }


        public override List<TreeFieldSetupDO> ReadTreeFields()
        {
            var fields = InternalReadTreeFields();

            //if not a single stage plot strata 
            //and count measure field is missing 
            //automaticly add it
            if (!IsSingleStage
                && fields.FindIndex(((tfs) => tfs.Field == CruiseDAL.Schema.TREE.COUNTORMEASURE)) < 0)
            {
                fields.Insert(5
                    , new TreeFieldSetupDO() { 
                        Field = CruiseDAL.Schema.TREE.COUNTORMEASURE
                        , Heading = "C/M" });
            }

            return fields;
        }
    }
}
