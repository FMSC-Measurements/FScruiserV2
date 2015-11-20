using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using System.ComponentModel;
using System.Threading;

namespace FSCruiser.Core.Models
{
    public class CuttingUnitVM : CuttingUnitDO
    {
        private int _talliesSinceLastSave = 0;
        
        private Thread _saveTreesWorkerThread;
        private Thread _loadCuttingUnitDataThread;

        public List<CountTreeDO> Counts { get; set; }

        public StratumVM DefaultStratum { get; protected set; }
        public List<SampleGroupVM> SampleGroups { get; protected set; }
        public List<TreeVM> TreeList { get; set; }
        public IList<TreeVM> NonPlotTrees { get; set; }
        public TallyHistoryCollection TallyHistoryBuffer { get; protected set; }



        public CuttingUnitVM() 
            :base()
        {
        }


        public void AsyncLoadCuttingUnitData()
        {
            if (this._loadCuttingUnitDataThread != null)
            {
                this._loadCuttingUnitDataThread.Abort();
            }
            this._loadCuttingUnitDataThread = new Thread(this.LoadData);
            this._loadCuttingUnitDataThread.IsBackground = true;
            this._loadCuttingUnitDataThread.Priority = Constants.LOAD_CUTTINGUNITDATA_PRIORITY;
            this._loadCuttingUnitDataThread.Start();
        }


        public void LoadData()
        {

            InitializeSampleGroups();
            //InitializeUnitTreeNumIndex();
            TallyHistoryBuffer = new TallyHistoryCollection(this);
            TallyHistoryBuffer.Initialize();


            InitializeUnitTreeList();
            //create a list of just trees in tree based strata
            List<TreeVM> nonPlotTrees = DAL.Read<TreeVM>(@"JOIN Stratum ON Tree.Stratum_CN = Stratum.Stratum_CN WHERE Tree.CuttingUnit_CN = ? AND
                        (Stratum.Method = '100' OR Stratum.Method = 'STR' OR Stratum.Method = '3P' OR Stratum.Method = 'S3P') ORDER BY TreeNumber", 
                        (object)this.CuttingUnit_CN);
            this.NonPlotTrees = new BindingList<TreeVM>(nonPlotTrees);

            if (this.DAL.GetRowCount("CuttingUnitStratum", "WHERE CuttingUnit_CN = ?", this.CuttingUnit_CN) == 1)
            {
                this.DefaultStratum = this.DAL.ReadSingleRow<StratumVM>("JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ?",
                        (object)this.CuttingUnit_CN);
            }
            else
            {
                this.DefaultStratum = this.DAL.ReadSingleRow<StratumVM>(
                        "JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ? AND Method = ?",
                        (object)CruiseDAL.Schema.Constants.CruiseMethods.H_PCT,
                        (object)this.CuttingUnit_CN);
            }



            this.ViewController.HandleCuttingUnitDataLoaded();

        }

        public void InitializeSampleGroups()
        {
            //create a list of all samplegroups in the unit
            this.SampleGroups = DAL.Read<SampleGroupVM>("SampleGroup", @"JOIN Stratum ON SampleGroup.Stratum_CN = Stratum.Stratum_CN 
                JOIN CuttingUnitStratum ON CuttingUnitStratum.Stratum_CN = Stratum.Stratum_CN
                WHERE CuttingUnitStratum.CuttingUnit_CN = ?", this.CuttingUnit_CN);

            //initialize sample selectors for all sampleGroups
            foreach (SampleGroupVM sg in this.SampleGroups)
            {
                //DataEntryMode mode = GetStrataDataEntryMode(sg.Stratum);
                sg.Sampler = sg.MakeSampleSelecter();
            }
        }

        public void InitializeUnitTreeList()
        {
            //create a list of all trees in the unit
            this.TreeList = DAL.Read<TreeVM>("WHERE CuttingUnit_CN = ?", (object)this.CuttingUnit_CN);
            //this.InternalValiateTrees((ICollection<TreeVM>)this.CurrentUnitTreeList);
            this.ValidateTreesAsync();
        }

        #region validate trees
        public bool ValidateTrees()
        {
            return ValidateTrees((ICollection<TreeVM>)TreeList);
        }



        private void InternalValidateTrees()
        {
            this.InternalValidateTrees(this.TreeList);
        }

        private bool InternalValidateTrees(ICollection<TreeVM> list)
        {
            bool valid = true;
            TreeVM[] a = new TreeVM[list.Count];
            list.CopyTo(a, 0);
            foreach (TreeVM tree in a)
            {
                if (tree.Stratum != null && tree.Stratum.TreeFieldNames != null)
                {
                    valid = tree.Validate(tree.Stratum.TreeFieldNames) && valid;
                }
                else
                {
                    valid = tree.Validate() && valid;
                }
                try
                {
                    tree.SaveErrors();
                }
                catch
                {
                    valid = false;
                    //TODO should we do something if tree error unable to save
                }
            }
            return valid;
        }

        //private bool InternalValidateTrees(ICollection<TreeVM> trees, ICollection<string> fields)
        //{
        //    bool valid = true;
        //    TreeVM[] a = new TreeVM[trees.Count];
        //    trees.CopyTo(a, 0);
        //    foreach (TreeDO tree in a)
        //    {
        //        valid = tree.Validate(fields) && valid;
        //        try
        //        {
        //            tree.SaveErrors();
        //        }
        //        catch
        //        {
        //            valid = false;
        //            //TODO should we do something if tree error unable to save
        //        }
        //    }
        //    return valid;
        //}

        private void ValidateTreesAsync(ICollection<TreeVM> list)
        {
            if (this._validateTreesWorkerThread != null)
            {
                this._validateTreesWorkerThread.Abort();
            }
            this._validateTreesWorkerThread = new Thread(this.InternalValidateTrees);
            this._validateTreesWorkerThread.IsBackground = true;
            this._validateTreesWorkerThread.Priority = ThreadPriority.BelowNormal;
            this._validateTreesWorkerThread.Start();
        }
        #endregion

        


        #region Tree stuff
        #region treeNumbering
        private long GetNextNonPlotTreeNumber()
        {
            if (this.NonPlotTrees == null || this.NonPlotTrees.Count == 0)
            { return 1; }
            var lastTree = this.NonPlotTrees[this.NonPlotTrees.Count - 1];
            long lastTreeNum = lastTree.TreeNumber;
            return lastTreeNum + 1;
            //return ++UnitTreeNumIndex;
        }

        public bool IsTreeNumberAvalible(long treeNumber)
        {
            foreach (TreeVM tree in this.NonPlotTrees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        public TreeVM CreateNewTreeEntry(CountTreeVM count)
        {
            return CreateNewTreeEntry(count, null, true);
        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count, PlotVM plot, bool isMeasure)
        {
            return CreateNewTreeEntry(count.SampleGroup.Stratum, count.SampleGroup, count.TreeDefaultValue, plot, isMeasure);
        }

        public TreeVM CreateNewTreeEntry(StratumVM stratum, SampleGroupVM sg, TreeDefaultValueDO tdv, PlotVM plot, bool isMeasure)
        {
            TreeVM newTree = new TreeVM(this.DAL);
            newTree.TreeCount = 0;
            newTree.CountOrMeasure = (isMeasure) ? "M" : "C";
            newTree.CuttingUnit = this; 

            if (sg != null)
            {
                newTree.SampleGroup = sg;
                if (tdv == null)
                {
                    if (sg.TreeDefaultValues.IsPopulated == false) { sg.TreeDefaultValues.Populate(); }
                    if (sg.TreeDefaultValues.Count == 1)
                    {
                        tdv = sg.TreeDefaultValues[0];
                    }
                }
            }
            if (stratum != null) { newTree.Stratum = stratum; }
            if (tdv != null)
            {
                newTree.SetTreeTDV(tdv);
            }

            if (plot != null)
            {
                newTree.Plot = plot;
                newTree.TreeNumber = plot.NextPlotTreeNum + 1;
                newTree.TreeCount = 1;
            }
            else
            {
                newTree.TreeNumber = GetNextNonPlotTreeNumber();
                this.NonPlotTrees.Add(newTree);
            }

            lock (((System.Collections.ICollection)this.TreeList).SyncRoot)
            {
                this.TreeList.Add(newTree);
            }

            newTree.Validate();
            //newTree.Save();

            return newTree;
        }

        public void DeleteTree(TreeVM tree)
        {
            //ReleaseUnitTreeNumber((int)tree.TreeNumber);
            tree.Delete();
            //TreeDO.RecursiveDeleteTree(tree);
            TreeList.Remove(tree);
            this.NonPlotTrees.Remove(tree);
        }
        #endregion

        #region save methods
        public bool SaveFieldData()
        {
            try
            {
                //this._cDal.BeginTransaction();//not doing transactions right now, need to do http://fmsc-projects.herokuapp.com/issues/526 first
                this.TallyHistoryBuffer.Save();
                this.TrySaveTrees();
                //this.SaveCounts();
                this.SaveSampleGroups();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveTrees()
        {
            var worker = new SaveTreesWorker(this.DAL, this.TreeList);
            worker.SaveAll();
        }

        public void TrySaveTrees()
        {
            var worker = new SaveTreesWorker(this.DAL, this.TreeList);
            worker.TrySaveAll();
        }

        public void TrySaveTreesAsync()
        {
            var worker = new SaveTreesWorker(this.DAL, this.TreeList);
            worker.TrySaveAllAsync();
        }


        protected void SaveSampleGroups()
        {
            foreach (SampleGroupVM sg in this.SampleGroups)
            {
                sg.SerializeSamplerState();
                sg.Save();
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.Code, String.Format("{0} Area: {1}", base.Description, base.Area));
        }
    }
}
