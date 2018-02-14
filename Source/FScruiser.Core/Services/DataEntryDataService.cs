using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Collections;

namespace FScruiser.Core.Services
{
    public class IDataEntryDataService : ITreeDataService, IPlotDataService
    {
        public DAL DataStore { get; protected set; }

        public CuttingUnit CuttingUnit { get; protected set; }

        public TallyHistoryCollection TallyHistory { get; protected set; }

        #region NonPlotTrees

        private object _nonPlotTreesSyncLock = new object();
        //ICollection<Tree> _nonPlotTrees;

        public ICollection<Tree> NonPlotTrees { get; protected set; }

        #endregion NonPlotTrees

        public Stratum DefaultStratum { get; protected set; }

        #region TreeStrata

        private IEnumerable<Stratum> _treeStrata;

        public IEnumerable<Stratum> TreeStrata
        {
            get { return _treeStrata; }
            protected set
            {
                _treeStrata = value;
                OnTreeStrataChanged();
            }
        }

        private void OnTreeStrataChanged()
        {
            var treeStrata = TreeStrata;
            if (treeStrata != null)
            {
                DefaultStratum = null;
                foreach (Stratum stratum in treeStrata)
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
                    this.DefaultStratum = treeStrata.FirstOrDefault();
                }
            }
        }

        #endregion TreeStrata

        public IEnumerable<PlotStratum> PlotStrata { get; protected set; }

        //private IEnumerable<Stratum> AllStrata
        //{
        //    get
        //    {
        //        foreach (var st in TreeStrata)
        //        {
        //            yield return st;
        //        }

        //        foreach (var st in PlotStrata)
        //        {
        //            yield return st;
        //        }
        //    }
        //}

        //private IEnumerable<CountTree> AllCounts
        //{
        //    get
        //    {
        //        foreach (var st in AllStrata)
        //        {
        //            foreach (var cnt in st.Counts)
        //            {
        //                yield return cnt;
        //            }
        //        }
        //    }
        //}

        #region sale level props

        #region EnableLogGrading

        private bool _enableLogGrading;

        public bool EnableLogGrading
        {
            get { return _enableLogGrading; }
            set
            {
                _enableLogGrading = value;
            }
        }

        #endregion EnableLogGrading

        public int Region { get; protected set; }

        public bool IsReconCruise { get; protected set; }

        #endregion sale level props

        public IDataEntryDataService()
        {
        }

        public IDataEntryDataService(string unitCode, DAL dataStore)
        {
            DataStore = dataStore;

            ReadSaleLevelData();

            ReadCruiseData(unitCode);

            var tallyBuffer = new TallyHistoryCollection(CuttingUnit, this, Constants.MAX_TALLY_HISTORY_SIZE);
            tallyBuffer.Initialize(DataStore);
            TallyHistory = tallyBuffer;
        }

        #region load data methods

        private void ReadCruiseData(string unitCode)
        {
            CuttingUnit = DataStore.From<CuttingUnit>()
                            .Where("Code = ?").Read(unitCode).FirstOrDefault();

            TreeStrata = ReadTreeBasedStrata().ToList();
            PlotStrata = ReadPlotStrata().ToList();

            foreach (var st in PlotStrata)
            {
                st.PopulatePlots(CuttingUnit.CuttingUnit_CN.GetValueOrDefault());

                foreach (var plot in st.Plots)
                {
                    plot.PopulateTrees();
                }
            }

            LoadNonPlotTrees();

            UnitLevelCruisersInitials = ReadUnitLevelCruisers(DataStore).ToArray();
        }

        public static IEnumerable<string> ReadUnitLevelCruisers(FMSC.ORM.Core.DatastoreRedux datastore)
        {
            var initialsStr = datastore.ExecuteScalar<string>("SELECT group_concat(Initials, ',') FROM (SELECT Trim(Initials) AS Initials FROM Tree WHERE Initials IS NOT NULL AND trim(Initials) != '' GROUP BY Initials);");
            if (!string.IsNullOrEmpty(initialsStr))
            {
                var initialsArray = initialsStr.Split(',');
                return initialsArray;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        private void ReadSaleLevelData()
        {
            EnableLogGrading = DataStore.ExecuteScalar<bool>("SELECT LogGradingEnabled FROM Sale Limit 1;");
            IsReconCruise = DataStore.ExecuteScalar<bool>("SELECT [Purpose] == 'Recon' FROM Sale LIMIT 1;");
            Region = DataStore.ExecuteScalar<int>("SELECT Region FROM Sale LIMIT 1;");
        }

        #endregion load data methods

        #region Tree

        #region treeNumbering

        public long GetNextNonPlotTreeNumber()
        {
            if (this.NonPlotTrees == null || this.NonPlotTrees.Count == 0)
            { return 1; }

            var highestTreeNum = NonPlotTrees.Max(x => x.TreeNumber);
            return highestTreeNum + 1;
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
                if (p != null && p.Trees.OrEmpty().Any(x => x.TreeNumber == treeNumber))
                {
                    return false;
                }
            }

            return true;
        }

        public Tree UserAddTree(Plot plot, Tree templateTree)
        {
            Tree newTree;
            SampleGroup assumedSG = null;
            TreeDefaultValueDO assumedTDV = null;

            if (templateTree != null)
            {
                assumedSG = templateTree.SampleGroup;
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
            SaveTrees(plot.Trees);
        }

        public bool TrySaveTrees(Plot plot)
        {
            return TrySaveTrees(plot.Trees);
        }

        //public void TrySaveTreesAsync(Plot plot)
        //{
        //    TrySaveTreesAsync(plot.Trees);
        //}

        #endregion plot variations

        private int _treesAddedSinceLastSave;
        private int TREE_SAVE_INTERVAL = 10;

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
            Tree newTree = new Tree(DataStore)
            {
                TreeCount = 0,
                CountOrMeasure = (isMeasure) ? "M" : "C",
                CuttingUnit = CuttingUnit
            };

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
                _treesAddedSinceLastSave = 0;
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
            Tree templateTree = NonPlotTrees.LastOrDefault();
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
            AddNonPlotTree(newTree);

            return newTree;
        }

        #endregion Tree

        public TreeEstimateDO LogTreeEstimate(CountTree count, int kpi)
        {
            if (count == null) { throw new ArgumentNullException("count"); }

            var te = new TreeEstimateDO(DataStore)
            {
                KPI = kpi,
                CountTree = count
            };
            te.Save();

            return te;
        }

        #region save methods

        public void SaveCounts()
        {
            SaveCounts(DataStore, TreeStrata);
        }

        //HACK using IEnumerable because no covariance in net20
        public static void SaveCounts(DAL datastore, IEnumerable strata)
        {
            if (strata == null) { throw new ArgumentNullException("strata"); }

            int changesSaved = 0;

            using (var connection = datastore.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    Exception error = null;

                    foreach (Stratum stratum in strata.OfType<Stratum>())
                    {
                        if (stratum.SampleGroups == null) { continue; }

                        foreach (var sg in stratum.SampleGroups)
                        {
                            if (sg.Counts == null) { continue; }

                            foreach (var count in sg.Counts)
                            {
                                if (count.IsChanged)
                                {
                                    try
                                    {
                                        changesSaved++;
                                        datastore.Save(connection, count, transaction);
                                    }
                                    catch (Exception e)
                                    {
                                        System.Diagnostics.Debug.WriteLine(e, "Exception");
                                        error = e;
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    if (error != null) { throw error; }
                }
            }

            Debug.Assert(changesSaved == 0, "counts saved " + changesSaved);//counts saved should be zero because we are now saving counts as they are modified
        }

        public Exception TrySaveCounts(PlotStratum stratum)
        {
            if (stratum == null) { throw new ArgumentNullException("stratum"); }
            Exception error = null;

            using (var connection = DataStore.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var sg in stratum.SampleGroups.OrEmpty())
                    {
                        if (sg.Counts == null) { continue; }
                        foreach (var count in sg.Counts)
                        {
                            try
                            {
                                DataStore.Save(connection, count, transaction);
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e, "Exception");
                                error = e;
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return error;
        }

        //HACK using IEnumerable because no covariance in net20
        public static void SaveSampleGroups(DAL datastore, IEnumerable strata)
        {
            using (var connection = datastore.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    Exception error = null;

                    foreach (var stratum in strata.OfType<Stratum>())
                    {
                        if (stratum.SampleGroups == null) { continue; }

                        foreach (SampleGroup sg in stratum.SampleGroups)
                        {
                            try
                            {
                                sg.SerializeSamplerState();
                                datastore.Save(connection, sg, transaction);
                            }
                            catch (Exception ex)
                            {
                                error = ex;
                            }
                        }
                    }

                    transaction.Commit();
                    if (error != null) { throw error; }
                }
            }
        }

        public Exception TrySaveSampleGroups(Stratum stratum)
        {
            Exception ex = null;
            foreach (var sg in stratum.SampleGroups.OrEmpty())
            {
                try
                {
                    sg.SerializeSamplerState();
                    sg.Save();
                }
                catch (Exception e)
                {
                    if (ex == null)
                    {
                        ex = e;
                    }
                }
            }
            return ex;
        }

        public bool TrySaveTrees()
        {
            return TrySaveTrees(NonPlotTrees);
        }

        protected void TrySaveTreesAsync()
        {
            TrySaveTreesAsync(NonPlotTrees);
        }

        public void SaveTrees(IEnumerable<Tree> trees)
        {
            var worker = new SaveTreesWorker(DataStore, trees);
            worker.SaveAll();
        }

        public bool TrySaveTrees(IEnumerable<Tree> trees)
        {
            bool success = true;

            foreach (Tree t in trees)
            {
                success = t.TrySave() && success;
            }

            return success;
        }

        public void TrySaveTreesAsync(IEnumerable<Tree> trees)
        {
            var worker = new SaveTreesWorker(DataStore, trees);
            worker.TrySaveAllAsync();
        }

        #endregion save methods

        #region read methods

        public IEnumerable<TreeDefaultValueDO> GetTreeDefaultValuesAll()
        {
            return DataStore.From<TreeDefaultValueDO>().Read();
        }

        private IEnumerable<PlotStratum> ReadPlotStrata()
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

        private IEnumerable<Stratum> ReadTreeBasedStrata()
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

        private void LoadNonPlotTrees()
        {
            lock (_nonPlotTreesSyncLock)
            {
                if (NonPlotTrees == null && DataStore != null)
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

                    NonPlotTrees = new BindingList<Tree>(trees);
                }
            }
        }

        #endregion read methods

        private IEnumerable<string> _unitLevelCruisersInitials;

        public IEnumerable<string> UnitLevelCruisersInitials
        {
            get { return _unitLevelCruisersInitials; }
            protected set { _unitLevelCruisersInitials = value; }
        }

        #region ITreeFieldProvider

        private object _treeFieldsReadLock = new object();
        private IEnumerable<TreeFieldSetupDO> _treeFields;

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

            if (TreeStrata.Any(st => st.Is3P)
                && !fields.Any(f => f.Field == "STM"))
            {
                fields.Add(new TreeFieldSetupDO() { Field = "STM", Heading = "STM" });
            }

            return fields;
        }

        #endregion ITreeFieldProvider

        public Exception SavePlotData()
        {
            Exception ex = null;

            try
            {
                SaveCounts(DataStore, PlotStrata);
            }
            catch (Exception e)
            {
                ex = e;
            }

            try
            {
                SaveSampleGroups(DataStore, PlotStrata);
            }
            catch (Exception e)
            {
                ex = e;
            }

            if (PlotStrata != null)
            {
                foreach (var st in PlotStrata)
                {
                    if (st.Plots == null) { continue; }
                    foreach (var plot in st.Plots)
                    {
                        try
                        {
                            plot.Save();
                            if (plot.Trees != null)
                            {
                                SaveTrees(plot);
                            }
                        }
                        catch (Exception e)
                        {
                            ex = e;
                        }
                    }
                }
            }
            return ex;
        }

        public Exception SaveNonPlotData()
        {
            Exception ex = null;

            try
            {
                SaveCounts(DataStore, TreeStrata);
            }
            catch (Exception e)
            {
                ex = e;
            }

            try
            {
                SaveSampleGroups(DataStore, TreeStrata);
            }
            catch (Exception e)
            {
                ex = e;
            }

            try
            {
                SaveTrees(NonPlotTrees);

                TallyHistory.Save();
            }
            catch (Exception e)
            {
                ex = e;
            }

            return ex;
        }

        //public void Dump(string path)
        //{
        //    using (var writer = new System.IO.StreamWriter(path))
        //    {
        //        //DumpCounts(writer);
        //        DumpNonPlotTrees(writer);
        //        DumpPlotStrata(writer);
        //    }
        //}

        //public void DumpNonPlotTrees(System.IO.TextWriter writer)
        //{
        //    var nonPlotTrees = NonPlotTrees;
        //    if (nonPlotTrees != null)
        //    {
        //        var treeSerializer = new XmlSerializer(typeof(Tree));
        //        treeSerializer.Serialize(writer, nonPlotTrees);
        //    }
        //}

        //public void DumpPlotStrata(System.IO.TextWriter writer)
        //{
        //    if (PlotStrata != null)
        //    {
        //        var plotStrataSerializer = new XmlSerializer(typeof(PlotStratum), new Type[] { typeof(Plot), typeof(Tree) });
        //        plotStrataSerializer.Serialize(writer, PlotStrata);
        //    }
        //}

        //public void DumpCounts(System.IO.TextWriter writer)
        //{
        //    var countTreeSerializer = new XmlSerializer(typeof(CountTree));
        //    countTreeSerializer.Serialize(writer, AllCounts.ToList());
        //}
    }
}