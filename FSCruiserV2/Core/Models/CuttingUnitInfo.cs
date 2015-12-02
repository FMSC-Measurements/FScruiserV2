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
        protected const int TREE_SAVE_INTERVAL = 10;
        private int _treesAddedSinceLastSave = 0;
        
        private Thread _saveTreesWorkerThread;
        

        public List<CountTreeDO> Counts { get; set; }

        public StratumVM DefaultStratum { get; set; }
        public List<SampleGroupVM> SampleGroups { get; set; }
        //public List<TreeVM> TreeList { get; set; }
        public IList<TreeVM> NonPlotTrees { get; set; }
        public TallyHistoryCollection TallyHistoryBuffer { get; set; }



        public CuttingUnitVM() 
            :base()
        {
        }


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

        public TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, IViewController viewController)
        {
            TreeVM newTree;
            SampleGroupVM assumedSG = null;
            TreeDefaultValueDO assumedTDV = null;

            //extrapolate stratum 
            if (knownStratum == null && this.DefaultStratum != null)//default stratum is going to be our first choice
            {
                knownStratum = this.DefaultStratum;
            }
            else if (knownStratum == null && templateTree != null)//if no default stratum try to use stratum/samplegroup from previous tree in view
            {
                assumedSG = templateTree.SampleGroup;
                assumedTDV = templateTree.TreeDefaultValue;
                if (assumedSG != null)
                {
                    knownStratum = assumedSG.Stratum;
                }
                else
                {
                    knownStratum = templateTree.Stratum;
                }
            }

            //extrapolate sample group
            if (knownStratum != null && assumedSG == null)//if we have a stratum but no sample group, pick the first one
            {
                List<SampleGroupVM> samplegroups = DAL.Read<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", knownStratum.Stratum_CN);
                if (samplegroups.Count == 1)
                {
                    assumedSG = samplegroups[0];
                }
            }

            newTree = this.CreateNewTreeEntry(knownStratum, assumedSG, assumedTDV, true);
            newTree.TreeCount = 1; //user added trees need a tree count of one because they aren't being tallied 

            viewController.ShowCruiserSelection(newTree);

            newTree.TrySave();
            this.AddNonPlotTree(newTree);

            return newTree;
        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count)
        {
            return CreateNewTreeEntry(count, true);
        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count, bool isMeasure)
        {
            return CreateNewTreeEntry(count.SampleGroup.Stratum, count.SampleGroup, count.TreeDefaultValue, isMeasure);
        }

        public TreeVM CreateNewTreeEntry(StratumVM stratum, SampleGroupVM sg, TreeDefaultValueDO tdv, bool isMeasure)
        {
            var tree = CreateNewTreeEntryInternal(stratum, sg, tdv, isMeasure);
            tree.TreeNumber = GetNextNonPlotTreeNumber();
            return tree;
        }

        internal TreeVM CreateNewTreeEntryInternal(StratumVM stratum, SampleGroupVM sg, TreeDefaultValueDO tdv, bool isMeasure)
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

            newTree.Validate();
            //newTree.Save();

            return newTree;
        }

        public void AddNonPlotTree(TreeVM tree)
        {
            lock (((System.Collections.ICollection)this.NonPlotTrees).SyncRoot)
            {
                this.NonPlotTrees.Add(tree);
            }
            _treesAddedSinceLastSave++;
            if (_treesAddedSinceLastSave >= TREE_SAVE_INTERVAL)
            {
                this.TrySaveTreesAsync();
            }
        }


        public void DeleteTree(TreeVM tree)
        {
            //ReleaseUnitTreeNumber((int)tree.TreeNumber);
            tree.Delete();
            //TreeDO.RecursiveDeleteTree(tree);
            //TreeList.Remove(tree);
            this.NonPlotTrees.Remove(tree);
        }
        #endregion

        #region validate trees
        public bool ValidateTrees()
        {
            var worker = new TreeValidationWorker(this.NonPlotTrees);
            return worker.ValidateTrees();
        }

        public void ValidateTreesAsync()
        {
            var worker = new TreeValidationWorker(this.NonPlotTrees);
            worker.ValidateTreesAsync();
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

        protected void SaveSampleGroups()
        {
            foreach (SampleGroupVM sg in this.SampleGroups)
            {
                sg.SerializeSamplerState();
                sg.Save();
            }
        }

        public void SaveTrees()
        {
            
            var worker = new SaveTreesWorker(this.DAL, this.NonPlotTrees);
            worker.SaveAll();
            _treesAddedSinceLastSave = 0;
        }

        public void TrySaveTrees()
        {
            var worker = new SaveTreesWorker(this.DAL, this.NonPlotTrees);
            worker.TrySaveAll();
            _treesAddedSinceLastSave = 0;
        }

        public void TrySaveTreesAsync()
        {
            var worker = new SaveTreesWorker(this.DAL, this.NonPlotTrees);
            worker.TrySaveAllAsync();
            _treesAddedSinceLastSave = 0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.Code, String.Format("{0} Area: {1}", base.Description, base.Area));
        }
    }
}
