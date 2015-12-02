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

        public override void Delete()
        {
            Debug.Assert(this.CuttingUnit != null);

            try
            {
                this.DAL.BeginTransaction();

                foreach (TreeVM tree in this.Trees)
                {
                    tree.Delete();
                    
                    //TreeDO.RecursiveDeleteTree(tree);
                }
                base.Delete();
                this.DAL.CommitTransaction();
            }
            catch (Exception e)
            {
                this.DAL.RollbackTransaction();
                throw;
            }
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

        public TreeVM UserAddTree(TreeVM templateTree, IViewController viewController)
        {
            TreeVM newTree;
            SampleGroupVM assumedSG = templateTree.SampleGroup;;
            TreeDefaultValueDO assumedTDV = templateTree.TreeDefaultValue;

            //extrapolate sample group
            if (assumedSG == null)//if we have a stratum but no sample group, pick the first one
            {
                List<SampleGroupVM> samplegroups = this.DAL.Read<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", 
                    this.Stratum.Stratum_CN);
                if (samplegroups.Count == 1)
                {
                    assumedSG = samplegroups[0];
                }
            }

            newTree = this.CreateNewTreeEntry(assumedSG, assumedTDV, true);

            viewController.ShowCruiserSelection(newTree);

            //if a 3P plot method set Count Measure to empty. 
            if (Array.IndexOf(CruiseDAL.Schema.Constants.CruiseMethods.THREE_P_METHODS, 
                this.Stratum.Method) >= 0)
            {
                newTree.CountOrMeasure = string.Empty;
            }

            newTree.TreeCount = 1; //user added trees need a tree count of one because they aren't being tallied 
            newTree.TrySave();
            this.AddTree(newTree);


            return newTree;

        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count, bool isMeasure)
        {
            return this.CreateNewTreeEntry(count.SampleGroup, count.TreeDefaultValue, isMeasure);
        }

        public TreeVM CreateNewTreeEntry(SampleGroupVM sg, TreeDefaultValueDO tdv, bool isMeasure)
        {
            Debug.Assert(this.CuttingUnit != null);
            var newTree = this.CuttingUnit.CreateNewTreeEntryInternal(this.Stratum, sg, tdv, isMeasure);

            newTree.Plot = this;
            newTree.TreeNumber = this.NextPlotTreeNum + 1;
            newTree.TreeCount = 1;

            return newTree;
        }

        public void AddTree(TreeVM tree)
        {
            lock (((System.Collections.ICollection)Trees).SyncRoot)
            {
                this.Trees.Add(tree);
            }
        }

        public void DeleteTree(TreeVM tree)
        {
            tree.Delete();
            //TreeDO.RecursiveDeleteTree(tree);
            //this.CuttingUnit.TreeList.Remove(tree);
            this.Trees.Remove(tree);
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
