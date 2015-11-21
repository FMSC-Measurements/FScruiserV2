using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using System.ComponentModel;
using CruiseDAL;
using CruiseDAL.Schema;
using System.Diagnostics;

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

        public new CuttingUnitVM CuttingUnit
        {
            get
            {
                return (CuttingUnitVM)base.CuttingUnit;
            }
            set
            {
                base.CuttingUnit = value;
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

        public override CuttingUnitDO GetCuttingUnit()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<CuttingUnitVM>(CUTTINGUNIT._NAME, this.CuttingUnit_CN);
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

        public bool IsTreeNumberAvalible(long treeNumber)
        {
            foreach (TreeVM tree in this.Trees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public TreeVM CreateNewTreeEntry(SampleGroupVM sg, TreeDefaultValueDO tdv, bool isMeasure)
        {
            Debug.Assert(this.CuttingUnit != null);
            return this.CuttingUnit.CreateNewTreeEntry(this.Stratum, sg, tdv, this, isMeasure);
        }

        public void SaveTrees()
        {
            var worker = new SaveTreesWorker(this.DAL, this.Trees);
            worker.SaveAll();
        }

        public void TrySaveTrees()
        {
            var worker = new SaveTreesWorker(this.DAL, this.Trees);
            worker.TrySaveAll();
        }

        public void TrySaveTreesAsync()
        {
            var worker = new SaveTreesWorker(this.DAL, this.Trees);
            worker.TrySaveAllAsync();
        }

        public override string ToString()
        {
            return base.PlotNumber.ToString() + ((IsNull == true) ? "-Null" : string.Empty);
        }
    }
}
