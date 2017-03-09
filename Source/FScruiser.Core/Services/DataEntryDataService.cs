using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.SQLite;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FScruiser.Core.Services
{
    public class IDataEntryDataService : ITreeFieldProvider
    {
        #region Events
        public event EventHandler EnableLogGradingChanged;

        #endregion

        public bool IsReconCruise { get; set; }

        public DAL DataStore { get; protected set; }

        public CuttingUnit CuttingUnit { get; set; }

        object _nonPlotTreesSyncLock = new object();
        ICollection<Tree> _nonPlotTrees;

        public ICollection<Tree> NonPlotTrees
        {
            get
            {
                lock (_nonPlotTreesSyncLock)
                {
                    if (_nonPlotTrees == null)
                    {
                        var trees = DataStore.From<Tree>()
                            .Join("Stratum", "USING (Stratum_CN)")
                            .Where("Tree.CuttingUnit_CN = ? AND " +
                                    "Stratum.Method IN ('100','STR','3P','S3P')")
                            .OrderBy("TreeNumber")
                            .Read(CuttingUnit.CuttingUnit_CN).ToList();
                        _nonPlotTrees = new BindingList<Tree>(trees);
                    }
                    return _nonPlotTrees;
                }
            }
        }

        public Stratum DefaultStratum { get; protected set; }

        public IEnumerable<Stratum> TreeStrata { get; protected set; }

        public IEnumerable<PlotStratum> PlotStrata { get; protected set; }

        public IEnumerable<Stratum> Strata
        {
            get
            {
                foreach (var st in TreeStrata)
                {
                    yield return st;
                }

                foreach (var st in PlotStrata)
                {
                    yield return st;
                }
            }
        }

        bool _enableLogGrading;

        public bool EnableLogGrading
        {
            get { return _enableLogGrading; }
            set
            {
                _enableLogGrading = value;
                OnEnableLogGradingChanged();
            }
        }

        private void OnEnableLogGradingChanged()
        {
        }

        public IDataEntryDataService(string unitCode, DAL dataStore)
        {
            DataStore = dataStore;

            CuttingUnit = DataStore.From<CuttingUnit>()
                .Where("Code = ?").Read(unitCode).FirstOrDefault();

            EnableLogGrading = DataStore.ExecuteScalar<bool>("SELECT LogGradingEnabled FROM Sale Limit 1;");
            IsReconCruise = DataStore.ExecuteScalar<bool>("SELECT [Purpose] == 'Recon' FROM Sale LIMIT 1;");

            var tallyBuffer = new TallyHistoryCollection(this, Constants.MAX_TALLY_HISTORY_SIZE);
            tallyBuffer.Initialize();

            CuttingUnit.TallyHistoryBuffer = tallyBuffer;

            TreeStrata = ReadTreeBasedStrata().ToList();
            PlotStrata = ReadPlotStrata().ToList();

            LoadNonPlotTrees();

            DefaultStratum = null;
            foreach (Stratum stratum in this.TreeStrata)
            {
                if (stratum.Method == CruiseDAL.Schema.CruiseMethods.H_PCT)
                {
                    this.DefaultStratum = stratum;
                    break;
                }
            }

            //if no 100% stratum found
            if (this.DefaultStratum == null)
            {
                this.DefaultStratum = this.TreeStrata.FirstOrDefault();
            }
        }

        private void LoadNonPlotTrees()
        {
            lock (_nonPlotTreesSyncLock)
            {
                if (_nonPlotTrees == null)
                {
                    var trees = DataStore.From<Tree>()
                        .Join("Stratum", "USING (Stratum_CN)")
                        .Where("Tree.CuttingUnit_CN = ? AND " +
                                "Stratum.Method IN ('100','STR','3P','S3P')")
                        .OrderBy("TreeNumber")
                        .Read(CuttingUnit.CuttingUnit_CN).ToList();

                    foreach (var tree in trees)
                    {
                        tree.ValidateVisableFields();
                    }

                    _nonPlotTrees = new BindingList<Tree>(trees);
                }
            }
        }

        #region Tree

        #region treeNumbering

        private long GetNextNonPlotTreeNumber()
        {
            if (this.NonPlotTrees == null || this.NonPlotTrees.Count == 0)
            { return 1; }
            var lastTree = this.NonPlotTrees.LastOrDefault();
            long lastTreeNum = lastTree.TreeNumber;
            return lastTreeNum + 1;
            //return ++UnitTreeNumIndex;
        }

        public long GetNextPlotTreeNumber(long plotNumber)
        {
            var topTreeNum = 0L;

            foreach (var st in PlotStrata)
            {
                var plot = st.Plots.Where(x => x.PlotNumber == plotNumber).FirstOrDefault();
                if (plot != null
                    && plot.Trees != null
                    && plot.Trees.Count > 0)
                {
                    topTreeNum = Math.Max(topTreeNum, plot.Trees.Max(x => x.TreeNumber));
                }
            }
            return topTreeNum + 1;
        }

        public bool IsTreeNumberAvalible(long treeNumber)
        {
            foreach (Tree tree in this.NonPlotTrees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion treeNumbering

        #region plot variations

        public bool CrossStrataIsTreeNumberAvalible(Plot plot, long treeNumber)
        {
            foreach (var st in PlotStrata.Where(x => x != plot.Stratum))
            {
                var p = st.Plots.Where(x => x.PlotNumber == plot.PlotNumber).FirstOrDefault();
                if (p != null && p.Trees.Any(x => x.TreeNumber == treeNumber))
                {
                    return false;
                }
            }

            return true;
        }

        public Tree UserAddTree(Plot plot, Tree templateTree, IViewController viewController)
        {
            Tree newTree;
            SampleGroup assumedSG = null;
            TreeDefaultValueDO assumedTDV = null;

            if (templateTree != null)
            {
                assumedSG = templateTree.SampleGroup; ;
                assumedTDV = templateTree.TreeDefaultValue;
            }

            //extrapolate sample group
            if (assumedSG == null)//if we have a stratum but no sample group, pick the first one
            {
                List<SampleGroup> samplegroups = DataStore.From<SampleGroup>()
                    .Where("Stratum_CN = ?")
                    .Read(plot.Stratum.Stratum_CN).ToList();
                if (samplegroups.Count == 1)
                {
                    assumedSG = samplegroups[0];
                }
            }

            newTree = this.CreateNewTreeEntry(plot, assumedSG, assumedTDV, true);

            DialogService.AskCruiser(newTree);

            //if a 3P plot method set Count Measure to empty.
            if (Array.IndexOf(CruiseDAL.Schema.CruiseMethods.THREE_P_METHODS,
                plot.Stratum.Method) >= 0)
            {
                newTree.CountOrMeasure = string.Empty;
            }

            newTree.TreeCount = 1; //user added trees need a tree count of one because they aren't being tallied
            newTree.TrySave();
            plot.AddTree(newTree);

            return newTree;
        }

        public Tree CreateNewTreeEntry(Plot plot, SubPop subPop)
        {
            var tree = CreateNewTreeEntry(plot, subPop.SG, subPop.TDV, true);
            return tree;
        }

        public Tree CreateNewTreeEntry(Plot plot, CountTree count, bool isMeasure)
        {
            return this.CreateNewTreeEntry(plot, count.SampleGroup, count.TreeDefaultValue, isMeasure);
        }

        public virtual Tree CreateNewTreeEntry(Plot plot, SampleGroup sg, TreeDefaultValueDO tdv, bool isMeasure)
        {
            Debug.Assert(plot.CuttingUnit != null);

            if (plot is FixCNTPlot)
            { isMeasure = false; }

            var newTree = CreateNewTreeEntryInternal(plot.Stratum, sg, tdv, isMeasure);

            newTree.Plot = plot;
            if (IsReconCruise)
            {
                newTree.TreeNumber = plot.GetNextTreeNumber();
            }
            else
            {
                newTree.TreeNumber = GetNextPlotTreeNumber(plot.PlotNumber);
            }
            newTree.TreeCount = 1;

            return newTree;
        }

        public void SaveTrees(Plot plot)
        {
            if (plot.Trees == null) { return; }
            var worker = new SaveTreesWorker(DataStore, plot.Trees);
            worker.SaveAll();
        }

        public void TrySaveTrees(Plot plot)
        {
            if (plot.Trees == null) { return; }
            var worker = new SaveTreesWorker(DataStore, plot.Trees);
            worker.TrySaveAll();
        }

        public void TrySaveTreesAsync(Plot plot)
        {
            if (plot.Trees == null) { return; }
            var worker = new SaveTreesWorker(DataStore, plot.Trees);
            worker.TrySaveAllAsync();
        }

        #endregion plot variations

        int _treesAddedSinceLastSave;
        int TREE_SAVE_INTERVAL = 10;

        public Tree CreateNewTreeEntry(CountTree count)
        {
            return CreateNewTreeEntry(count, true);
        }

        public Tree CreateNewTreeEntry(CountTree count, bool isMeasure)
        {
            return CreateNewTreeEntry(count.SampleGroup.Stratum, count.SampleGroup, count.TreeDefaultValue, isMeasure);
        }

        public Tree CreateNewTreeEntry(Stratum stratum
            , SampleGroup sg
            , TreeDefaultValueDO tdv
            , bool isMeasure)
        {
            var tree = CreateNewTreeEntryInternal(stratum, sg, tdv, isMeasure);
            tree.TreeNumber = GetNextNonPlotTreeNumber();
            return tree;
        }

        internal Tree CreateNewTreeEntryInternal(Stratum stratum
            , SampleGroup sg
            , TreeDefaultValueDO tdv
            , bool isMeasure)
        {
            Tree newTree = new Tree(DataStore);
            newTree.TreeCount = 0;
            newTree.CountOrMeasure = (isMeasure) ? "M" : "C";
            newTree.CuttingUnit = CuttingUnit;

            if (sg != null)
            {
                newTree.SampleGroup = sg;
                if (tdv == null)
                {
                    if (sg.TreeDefaultValues.IsPopulated == false)
                    {
                        sg.TreeDefaultValues.Populate();
                    }
                    if (sg.TreeDefaultValues.Count == 1)
                    {
                        tdv = sg.TreeDefaultValues.FirstOrDefault();//TODO should be SingleOrDefault
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

        public void AddNonPlotTree(Tree tree)
        {
            lock (_nonPlotTreesSyncLock)
            {
                this.NonPlotTrees.Add(tree);
            }
            _treesAddedSinceLastSave++;
            if (_treesAddedSinceLastSave >= TREE_SAVE_INTERVAL)
            {
                this.TrySaveTreesAsync();
            }
        }

        public void DeleteTree(Tree tree)
        {
            lock (_nonPlotTreesSyncLock)
            {
                tree.Delete();
                this.NonPlotTrees.Remove(tree);
            }
        }

        public Tree UserAddTree()
        {
            Tree templateTree = NonPlotTrees.LastOrDefault(); ;
            Stratum stratum = null;
            SampleGroup samplegroup = null;
            TreeDefaultValueDO tdv = null;

            if (templateTree != null)
            {
                stratum = templateTree.Stratum;
                samplegroup = templateTree.SampleGroup;
                tdv = templateTree.TreeDefaultValue;
            }
            else if (DefaultStratum != null)
            {
                stratum = this.DefaultStratum;

                //var samplegroups = DAL.From<SampleGroupModel>()
                //    .Where("Stratum_CN = ?")
                //    .Read(stratum.Stratum_CN).ToList();
                if (stratum.SampleGroups != null
                    && stratum.SampleGroups.Count == 1)
                {
                    samplegroup = stratum.SampleGroups[0];
                }
            }

            var newTree = CreateNewTreeEntry(stratum
                , samplegroup, tdv, true);
            newTree.TreeCount = 0; //user added trees need a tree count of zero because users seem to be adding counts through tally settings

            DialogService.AskCruiser(newTree);

            newTree.TrySave();
            this.AddNonPlotTree(newTree);

            return newTree;
        }

        #endregion Tree

        #region save methods

        public bool SaveFieldData()
        {
            try
            {
                //this.DataStore.BeginTransaction();//not doing transactions right now, need to do http://fmsc-projects.herokuapp.com/issues/526 first

                TrySaveTrees();

                CuttingUnit.TallyHistoryBuffer.Save();
                SaveSampleGroups(); // save sampler states

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void SaveSampleGroups()
        {
            foreach (var st in TreeStrata)
            {
                st.SaveSampleGroups();
            }

            foreach (var st in PlotStrata)
            {
                st.SaveSampleGroups();
            }
        }

        public bool TrySaveCounts()
        {
            bool success = true;
            foreach (Stratum stratum in TreeStrata)
            {
                success = stratum.TrySaveCounts() && success;
            }
            foreach (Stratum stratum in PlotStrata)
            {
                success = stratum.TrySaveCounts() && success;
            }
            return success;
        }

        public void TrySaveTreesAsync()
        {
            var worker = new SaveTreesWorker(DataStore, NonPlotTrees);
            worker.TrySaveAllAsync();
            _treesAddedSinceLastSave = 0;
        }

        public void TrySaveTrees()
        {
            var worker = new SaveTreesWorker(DataStore, NonPlotTrees);
            worker.TrySaveAll();
        }

        #endregion save methods

        #region private members

        IEnumerable<PlotStratum> ReadPlotStrata()
        {
            Debug.Assert(DataStore != null);

            foreach (var st in
                 DataStore.From<PlotStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method IN ( 'FIX', 'FCM', 'F3P', 'PNT', 'PCM', 'P3P', '3PPNT')")
                .Query(CuttingUnit.CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(CuttingUnit);
                st.PopulateHotKeyLookup();
                yield return st;
            }

            foreach (var st in DataStore.From<FixCNTStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method = '" + CruiseDAL.Schema.CruiseMethods.FIXCNT + "'")
                .Query(CuttingUnit.CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(CuttingUnit);
                st.PopulateHotKeyLookup();
                yield return st;
            }
        }

        IEnumerable<Stratum> ReadTreeBasedStrata()
        {
            Debug.Assert(DataStore != null);

            foreach (var st in
                DataStore.From<Stratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Where("CuttingUnitStratum.CuttingUnit_CN = ?" +
                        "AND Method IN ( '100', 'STR', '3P', 'S3P')")
                .Read(CuttingUnit.CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(CuttingUnit);
                st.PopulateHotKeyLookup();
                yield return st;
            }
        }

        #endregion private members

        #region ITreeFieldProvider

        object _treeFieldsReadLock = new object();
        IEnumerable<TreeFieldSetupDO> _treeFields;

        public IEnumerable<TreeFieldSetupDO> TreeFields
        {
            get
            {
                lock (_treeFieldsReadLock)
                {
                    if (_treeFields == null && DataStore != null)
                    { _treeFields = ReadTreeFields().ToList(); }
                    return _treeFields;
                }
            }
            set { _treeFields = value; }
        }

        public IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        {
            var fields = DataStore.From<TreeFieldSetupDO>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Join("Stratum", "USING (Stratum_CN)")
                .Where(String.Format("CuttingUnit_CN = ? AND Stratum.Method NOT IN ({0})"
                , string.Join(",", CruiseDAL.Schema.CruiseMethods.PLOT_METHODS.Select(s => "'" + s + "'").ToArray())))
                .GroupBy("Field")
                .OrderBy("FieldOrder")
                .Query(CuttingUnit.CuttingUnit_CN).ToList();

            if (fields.Count == 0)
            {
                fields.AddRange(Constants.DEFAULT_TREE_FIELDS);
            }

            //if unit has multiple tree strata
            //but stratum column is missing
            if (TreeStrata.Count() > 1
                && !fields.Any(x => x.Field == "Stratum"))
            {
                //find the location of the tree number field
                int indexOfTreeNum = fields.FindIndex(x => x.Field == CruiseDAL.Schema.TREE.TREENUMBER);
                //if user doesn't have a tree number field, fall back to the last field index
                if (indexOfTreeNum == -1) { indexOfTreeNum = fields.Count - 1; }//last item index
                //add the stratum field to the filed list
                TreeFieldSetupDO tfs = new TreeFieldSetupDO() { Field = "Stratum", Heading = "St", Format = "[Code]" };
                fields.Insert(indexOfTreeNum + 1, tfs);
            }

            return fields;
        }

        #endregion ITreeFieldProvider

        #region utility

        public void LogMessage(string message, string level)
        {
            DataStore.LogMessage(message, level);
        }

        public bool TryLogMessage(string message, string level)
        {
            try
            {
                DataStore.LogMessage(message, level);
                return true;
            }
            catch { return false; }
        }

        #endregion utility
    }
}