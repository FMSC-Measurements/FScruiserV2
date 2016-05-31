using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class PlotVM : PlotDO
    {
        private bool _isTreeDataPopulated = false;
        private IList<TreeVM> _trees;

        public PlotVM()
            : base()
        { }

        public PlotVM(DAL dal)
            : base(dal)
        { }

        public PlotVM(PlotDO obj)
            : base(obj)
        { }

        [IgnoreField]
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

        [IgnoreField]
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

        [IgnoreField]
        public IList<TreeVM> Trees
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

        [IgnoreField]
        public long HighestTreeNum
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

        [IgnoreField]
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
            return DAL.ReadSingleRow<StratumVM>(this.Stratum_CN);
        }

        public override CuttingUnitDO GetCuttingUnit()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<CuttingUnitVM>(this.CuttingUnit_CN);
        }

        public override void Delete()
        {
            Debug.Assert(this.CuttingUnit != null);

            lock (DAL.TransactionSyncLock)
            {
                this.DAL.BeginTransaction();
                try
                {
                    foreach (TreeVM tree in this.Trees)
                    {
                        tree.Delete();

                        //TreeDO.RecursiveDeleteTree(tree);
                    }
                    base.Delete();
                    this.DAL.CommitTransaction();
                }
                catch (Exception)
                {
                    this.DAL.RollbackTransaction();
                    throw;
                }
            }
        }

        public virtual void PopulateTrees()
        {
            if (this._trees == null)
            {
                List<TreeVM> tList = base.DAL.From<TreeVM>()
                    .Where("Stratum_CN = ? AND CuttingUnit_CN = ? AND Plot_CN = ?")
                    .OrderBy("TreeNumber")
                    .Read(base.Stratum.Stratum_CN
                    , base.CuttingUnit.CuttingUnit_CN
                    , base.Plot_CN).ToList();
                this._trees = new BindingList<TreeVM>(tList);
                //this._trees = tList;
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
            SampleGroupVM assumedSG = null;
            TreeDefaultValueDO assumedTDV = null;

            if (templateTree != null)
            {
                assumedSG = templateTree.SampleGroup; ;
                assumedTDV = templateTree.TreeDefaultValue;
            }

            //extrapolate sample group
            if (assumedSG == null)//if we have a stratum but no sample group, pick the first one
            {
                List<SampleGroupVM> samplegroups = this.DAL.From<SampleGroupVM>()
                    .Where("Stratum_CN = ?")
                    .Read(this.Stratum.Stratum_CN).ToList();
                if (samplegroups.Count == 1)
                {
                    assumedSG = samplegroups[0];
                }
            }

            newTree = this.CreateNewTreeEntry(assumedSG, assumedTDV, true);

            viewController.ShowCruiserSelection(newTree);

            //if a 3P plot method set Count Measure to empty.
            if (Array.IndexOf(CruiseDAL.Schema.CruiseMethods.THREE_P_METHODS,
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
            newTree.TreeNumber = this.HighestTreeNum + 1;
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

        public bool ValidateTrees()
        {
            var worker = new TreeValidationWorker(Trees);
            return worker.ValidateTrees();
        }

        public override string ToString()
        {
            return base.PlotNumber.ToString() + ((IsNull == true) ? "-Null" : string.Empty);
        }
    }
}