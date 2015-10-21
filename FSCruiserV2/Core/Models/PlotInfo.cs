using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using System.ComponentModel;
using CruiseDAL;
using CruiseDAL.Schema;

namespace FSCruiser.Core.Models
{
    public class PlotVM : PlotDO
    {
        private bool _isDataLoaded = false;
        private BindingList<TreeVM> _trees;

        public PlotVM() 
            : base()
        { }

        public PlotVM(DAL dal) 
            : base(dal)
        { }

        public PlotVM(PlotDO obj)
            : base(obj)
        { }

        public new StratumVM Stratum
        {
            get
            {
                return (StratumVM)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }


        public BindingList<TreeVM> Trees
        {
            get
            {
                if (this._trees == null)
                {
                    this._trees = new BindingList<TreeVM>();
                }
                return this._trees;
            }
        }

        public long NextPlotTreeNum
        {
            get
            {
                if (this._trees == null || this.Trees.Count == 0)
                {
                    return 0L;
                }
                return this.Trees[this.Trees.Count - 1].TreeNumber;
            }
        }
        public bool IsNull
        {
            get
            {
                return base.IsEmpty == "True";
            }
            set
            {
                base.IsEmpty = (value) ? "True" : "False";
            }
        }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<StratumVM>(STRATUM._NAME, this.Stratum_CN);
        }


        public void LoadData()
        {
            if (!_isDataLoaded)
            {
                List<TreeVM> tList = base.DAL.Read<TreeVM>("Tree", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? AND Plot_CN = ? ORDER BY TreeNumber", base.Stratum.Stratum_CN, base.CuttingUnit.CuttingUnit_CN, base.Plot_CN);
                this._trees = new BindingList<TreeVM>(tList);

                //long? value = base.DAL.ExecuteScalar(String.Format("Select MAX(TreeNumber) FROM Tree WHERE Plot_CN = {0}", base.Plot_CN)) as long?;
                //this.NextPlotTreeNum = (value.HasValue) ? (int)value.Value : 0;
                _isDataLoaded = true;
            }
        }

        public void CheckDataState()//TODO perhaps there needs to be a better way to control the _isDataLoaded state
        {
            if (this.Trees.Count > 0)
            {
                this._isDataLoaded = true;
            }
        }

        public override string ToString()
        {
            return base.PlotNumber.ToString() + ((IsNull == true) ? "-Null" : string.Empty);
        }
    }
}
