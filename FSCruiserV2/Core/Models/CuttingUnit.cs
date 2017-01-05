using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class CuttingUnit : CuttingUnitDO, ITreeFieldProvider
    {
        protected const int TREE_SAVE_INTERVAL = 10;
        private int _treesAddedSinceLastSave = 0;

        [IgnoreField]
        public IList<PlotStratum> PlotStrata { get; set; }

        [IgnoreField]
        public IList<Stratum> TreeStrata { get; set; }

        [IgnoreField]
        public IEnumerable<SampleGroup> TreeSampleGroups
        {
            get
            {
                foreach (var st in TreeStrata)
                {
                    foreach (var sg in st.SampleGroups)
                    {
                        yield return sg;
                    }
                }
            }
        }

        [IgnoreField]
        public Stratum DefaultStratum { get; set; }

        [IgnoreField]
        public IList<Tree> NonPlotTrees { get; set; }

        [IgnoreField]
        public TallyHistoryCollection TallyHistoryBuffer { get; set; }

        Sale _sale;

        [IgnoreField]
        public Sale Sale
        {
            get
            {
                if (_sale == null)
                {
                    this._sale = DAL.From<Sale>().Limit(1, 0).Query().FirstOrDefault();
                }
                return _sale;
            }
        }

        public CuttingUnit()
            : base()
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

        public Tree UserAddTree(IViewController viewController)
        {
            Tree templateTree = null;
            Stratum stratum = null;
            SampleGroup samplegroup = null;
            TreeDefaultValueDO tdv = null;
            if (NonPlotTrees.Count > 0)
            {
                templateTree = NonPlotTrees[NonPlotTrees.Count - 1];
                if (templateTree != null)
                {
                    stratum = templateTree.Stratum;
                    samplegroup = templateTree.SampleGroup;
                    tdv = templateTree.TreeDefaultValue;
                }
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

            var newTree = this.CreateNewTreeEntry(stratum
                , samplegroup, tdv, true);
            newTree.TreeCount = 0; //user added trees need a tree count of zero because users seem to be adding counts through tally settings

            viewController.ShowCruiserSelection(newTree);

            newTree.TrySave();
            this.AddNonPlotTree(newTree);

            return newTree;
        }

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
            Tree newTree = new Tree(this.DAL);
            newTree.TreeCount = 0;
            newTree.CountOrMeasure = (isMeasure) ? "M" : "C";
            newTree.CuttingUnit = this;

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

        public void AddNonPlotTree(Tree tree)
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

        public void DeleteTree(Tree tree)
        {
            tree.Delete();
            this.NonPlotTrees.Remove(tree);
        }

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

        #endregion validate trees

        #endregion Tree stuff

        public void InitializeStrata()
        {
            this.TreeStrata = this.ReadTreeBasedStrata().ToList();
            this.PlotStrata = this.ReadPlotStrata().ToList();

            this.DefaultStratum = null;
            foreach (Stratum stratum in this.TreeStrata)
            {
                if (stratum.Method == CruiseDAL.Schema.CruiseMethods.H_PCT)
                {
                    this.DefaultStratum = stratum;
                    break;
                }
            }

            if (this.DefaultStratum == null && this.TreeStrata.Count > 0)
            {
                this.DefaultStratum = this.TreeStrata[0];
            }
        }

        public IEnumerable<PlotStratum> ReadPlotStrata()
        {
            Debug.Assert(DAL != null);

            foreach (var st in
                 DAL.From<PlotStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method IN ( 'FIX', 'FCM', 'F3P', 'PNT', 'PCM', 'P3P', '3PPNT')")
                .Query(CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(this);
                st.PopulateHotKeyLookup();
                yield return st;
            }

            foreach (var st in DAL.From<FixCNTStratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                .Where("CUST.CuttingUnit_CN = ? "
                + "AND Stratum.Method = '" + CruiseDAL.Schema.CruiseMethods.FIXCNT + "'")
                .Query(CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(this);
                st.PopulateHotKeyLookup();
                yield return st;
            }
        }

        public IEnumerable<Stratum> ReadTreeBasedStrata()
        {
            Debug.Assert(DAL != null);

            foreach (var st in
                DAL.From<Stratum>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Where("CuttingUnitStratum.CuttingUnit_CN = ?" +
                        "AND Method IN ( '100', 'STR', '3P', 'S3P')")
                .Read(CuttingUnit_CN))
            {
                st.LoadSampleGroups();
                st.LoadCounts(this);
                st.PopulateHotKeyLookup();
                yield return st;
            }
        }

        public void ReleaseData()
        {
            this.TallyHistoryBuffer = null;
            this.NonPlotTrees = null;
            TreeStrata = null;
            PlotStrata = null;
            DefaultStratum = null;
        }

        #region save methods

        public bool SaveFieldData()
        {
            try
            {
                //this.DataStore.BeginTransaction();//not doing transactions right now, need to do http://fmsc-projects.herokuapp.com/issues/526 first

                this.TrySaveTrees();

                this.TallyHistoryBuffer.Save();
                this.SaveSampleGroups(); // save sampler states

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveCounts()
        {
            foreach (Stratum stratum in TreeStrata)
            {
                stratum.SaveCounts();
            }
            foreach (Stratum stratum in PlotStrata)
            {
                stratum.SaveCounts();
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

        #endregion save methods

        #region ITreeFieldProvider

        object _treeFieldsReadLock = new object();
        IEnumerable<TreeFieldSetupDO> _treeFields;

        public IEnumerable<TreeFieldSetupDO> TreeFields
        {
            get
            {
                lock (_treeFieldsReadLock)
                {
                    if (_treeFields == null && DAL != null)
                    { _treeFields = ReadTreeFields().ToList(); }
                    return _treeFields;
                }
            }
            set { _treeFields = value; }
        }

        public IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        {
            var fields = DAL.From<TreeFieldSetupDO>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Join("Stratum", "USING (Stratum_CN)")
                .Where(String.Format("CuttingUnit_CN = ? AND Stratum.Method NOT IN ({0})"
                , string.Join(",", CruiseDAL.Schema.CruiseMethods.PLOT_METHODS.Select(s => "'" + s + "'").ToArray())))
                .GroupBy("Field")
                .OrderBy("FieldOrder")
                .Query(CuttingUnit_CN).ToList();

            if (fields.Count == 0)
            {
                fields.AddRange(Constants.DEFAULT_TREE_FIELDS);
            }

            //if unit has multiple tree strata
            //but stratum column is missing
            if (this.TreeStrata.Count > 1
                && fields.FindIndex(x => x.Field == "Stratum") == -1)
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

        public override string ToString()
        {
            return string.Format("{0}: {1} Area: {2}"
                , base.Code
                , base.Description
                , base.Area);
        }
    }
}