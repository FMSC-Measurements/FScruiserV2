using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using FScruiser.Core.Services;

namespace FSCruiser.Core.Models
{
    public class Plot : PlotDO
    {
        private IList<Tree> _trees;

        public Plot()
            : base()
        { }

        public Plot(DAL dal)
            : base(dal)
        { }

        public Plot(PlotDO obj)
            : base(obj)
        { }

        [IgnoreField]
        public new Stratum Stratum
        {
            get
            {
                return (Stratum)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        [IgnoreField]
        public new CuttingUnit CuttingUnit
        {
            get
            {
                return (CuttingUnit)base.CuttingUnit;
            }
            set
            {
                base.CuttingUnit = value;
            }
        }

        [IgnoreField]
        public IList<Tree> Trees
        {
            get
            {
                return _trees;
            }
            set
            {
                _trees = value;
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
            return DAL.ReadSingleRow<Stratum>(this.Stratum_CN);
        }

        public override CuttingUnitDO GetCuttingUnit()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<CuttingUnit>(this.CuttingUnit_CN);
        }

        public override void Delete()
        {
            Debug.Assert(this.CuttingUnit != null);

            lock (DAL.TransactionSyncLock)
            {
                this.DAL.BeginTransaction();
                try
                {
                    DAL.Execute("DELETE FROM LogStock;");
                    DAL.Execute("DELETE FROM TreeCalculatedValues;");
                    DAL.Execute("DELETE FROM Log WHERE Tree_CN in (SELECT Tree_CN FROM Tree WHERE Plot_CN = ?);", Plot_CN);
                    DAL.Execute("DELETE FROM Tree WHERE Plot_CN = ?;", Plot_CN);
                    DAL.Execute("DELETE FROM Plot WHERE Plot_CN = ?;", Plot_CN);
                    this.DAL.CommitTransaction();
                    OnDeleted();
                }
                catch
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
                List<Tree> tList = base.DAL.From<Tree>()
                    .Where("Stratum_CN = ? AND CuttingUnit_CN = ? AND Plot_CN = ?")
                    .OrderBy("TreeNumber")
                    .Read(base.Stratum.Stratum_CN
                    , base.CuttingUnit.CuttingUnit_CN
                    , base.Plot_CN).ToList();
                this._trees = new BindingList<Tree>(tList);
                //this._trees = tList;
            }
        }

        public void ResequenceTreeNumbers()
        {
            if (Trees != null)
            {
                int curTreeNumber = 1;
                foreach (var tree in Trees)
                {
                    tree.TreeNumber = curTreeNumber;
                    curTreeNumber++;
                }
            }
        }

        public bool IsTreeNumberAvalible(long treeNumber)
        {
            foreach (Tree tree in this.Trees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }

            return true;
        }

        //public bool CrossStrataIsTreeNumberAvalible(long treeNumber)
        //{
        //    foreach (var st in CuttingUnit.PlotStrata.Where(x => x != this.Stratum))
        //    {
        //        var plot = st.Plots.Where(x => x.PlotNumber == this.PlotNumber).FirstOrDefault();
        //        if (plot.Trees.Any(x => x.TreeNumber == treeNumber))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public Tree UserAddTree(IDataEntryDataService dataService, Tree templateTree, IViewController viewController)
        //{
        //    Tree newTree;
        //    SampleGroup assumedSG = null;
        //    TreeDefaultValueDO assumedTDV = null;

        //    if (templateTree != null)
        //    {
        //        assumedSG = templateTree.SampleGroup; ;
        //        assumedTDV = templateTree.TreeDefaultValue;
        //    }

        //    //extrapolate sample group
        //    if (assumedSG == null)//if we have a stratum but no sample group, pick the first one
        //    {
        //        List<SampleGroup> samplegroups = this.DAL.From<SampleGroup>()
        //            .Where("Stratum_CN = ?")
        //            .Read(this.Stratum.Stratum_CN).ToList();
        //        if (samplegroups.Count == 1)
        //        {
        //            assumedSG = samplegroups[0];
        //        }
        //    }

        //    newTree = this.CreateNewTreeEntry(dataService, assumedSG, assumedTDV, true);

        //    DialogService.AskCruiser(newTree);

        //    //if a 3P plot method set Count Measure to empty.
        //    if (Array.IndexOf(CruiseDAL.Schema.CruiseMethods.THREE_P_METHODS,
        //        this.Stratum.Method) >= 0)
        //    {
        //        newTree.CountOrMeasure = string.Empty;
        //    }

        //    newTree.TreeCount = 1; //user added trees need a tree count of one because they aren't being tallied
        //    newTree.TrySave();
        //    this.AddTree(newTree);

        //    return newTree;
        //}

        //public Tree CreateNewTreeEntry(IDataEntryDataService dataService, SubPop subPop)
        //{
        //    var tree = CreateNewTreeEntry(dataService, subPop.SG, subPop.TDV, true);
        //    return tree;
        //}

        //public Tree CreateNewTreeEntry(IDataEntryDataService dataService, CountTree count, bool isMeasure)
        //{
        //    return this.CreateNewTreeEntry(dataService, count.SampleGroup, count.TreeDefaultValue, isMeasure);
        //}

        //protected virtual Tree CreateNewTreeEntry(IDataEntryDataService dataService, SampleGroup sg, TreeDefaultValueDO tdv, bool isMeasure)
        //{
        //    Debug.Assert(dataService.CuttingUnit != null);
        //    var newTree = dataService.CreateNewTreeEntryInternal(this.Stratum, sg, tdv, isMeasure);

        //    newTree.Plot = this;
        //    newTree.TreeNumber = dataService.GetNextPlotTreeNumber(this.PlotNumber);
        //    newTree.TreeCount = 1;

        //    return newTree;
        //}

        //public void SaveTrees()
        //{
        //    if (Trees == null) { return; }
        //    var worker = new SaveTreesWorker(this.DAL, this.Trees);
        //    worker.SaveAll();
        //}

        //public void TrySaveTrees()
        //{
        //    if (Trees == null) { return; }
        //    var worker = new SaveTreesWorker(this.DAL, this.Trees);
        //    worker.TrySaveAll();
        //}

        //public void TrySaveTreesAsync()
        //{
        //    if (Trees == null) { return; }
        //    var worker = new SaveTreesWorker(this.DAL, this.Trees);
        //    worker.TrySaveAllAsync();
        //}

        public void AddTree(Tree tree)
        {
            lock (((System.Collections.ICollection)Trees).SyncRoot)
            {
                this.Trees.Add(tree);
            }
        }

        public void DeleteTree(Tree tree)
        {
            tree.Delete();
            this.Trees.Remove(tree);
        }

        public bool ValidatePlot(out string message)
        {
            message = string.Empty;

            if (!ValidateTrees())
            {
                message = "Error(s) found on tree records in current plot";
                return false;
            }
            return true;
        }

        public bool ValidateTrees()
        {
            if (Trees == null) { return true; }
            var worker = new TreeValidationWorker(Trees);
            return worker.ValidateTrees();
        }

        public override string ToString()
        {
            return base.PlotNumber.ToString() + ((IsNull == true) ? "-Null" : string.Empty);
        }
    }
}