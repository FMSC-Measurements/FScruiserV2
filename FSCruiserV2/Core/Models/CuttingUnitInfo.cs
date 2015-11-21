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
        

        public List<CountTreeDO> Counts { get; set; }

        public StratumVM DefaultStratum { get; set; }
        public List<SampleGroupVM> SampleGroups { get; set; }
        public List<TreeVM> TreeList { get; set; }
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

        #region validate trees
        public bool ValidateTrees()
        {
            var worker = new TreeValidationWorker(this.TreeList);
            return worker.ValidateTrees();
        }

        public void ValidateTreesAsync()
        {
            var worker = new TreeValidationWorker(this.TreeList);
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
