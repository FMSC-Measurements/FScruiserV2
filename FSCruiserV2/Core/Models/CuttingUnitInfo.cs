using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class CuttingUnitVM : CuttingUnitDO, ITreeFieldProvider
    {
        protected const int TREE_SAVE_INTERVAL = 10;
        private int _treesAddedSinceLastSave = 0;

        [IgnoreField]
        public IList<PlotStratum> PlotStrata { get; set; }

        [IgnoreField]
        public IList<StratumModel> TreeStrata { get; set; }

        [IgnoreField]
        public IEnumerable<SampleGroupModel> TreeSampleGroups
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
        public StratumModel DefaultStratum { get; set; }

        [IgnoreField]
        public IList<TreeVM> NonPlotTrees { get; set; }

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

        public CuttingUnitVM()
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
            foreach (TreeVM tree in this.NonPlotTrees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion treeNumbering

        public TreeVM UserAddTree(TreeVM templateTree
            , StratumModel knownStratum
            , IViewController viewController)
        {
            TreeVM newTree;
            SampleGroupModel assumedSG = null;
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
                List<SampleGroupModel> samplegroups = DAL.From<SampleGroupModel>().Where("Stratum_CN = ?").Read(knownStratum.Stratum_CN).ToList();
                if (samplegroups.Count == 1)
                {
                    assumedSG = samplegroups[0];
                }
            }

            newTree = this.CreateNewTreeEntry(knownStratum
                , assumedSG, assumedTDV, true);
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

        public TreeVM CreateNewTreeEntry(StratumModel stratum
            , SampleGroupModel sg
            , TreeDefaultValueDO tdv
            , bool isMeasure)
        {
            var tree = CreateNewTreeEntryInternal(stratum, sg, tdv, isMeasure);
            tree.TreeNumber = GetNextNonPlotTreeNumber();
            return tree;
        }

        internal TreeVM CreateNewTreeEntryInternal(StratumModel stratum
            , SampleGroupModel sg
            , TreeDefaultValueDO tdv
            , bool isMeasure)
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
            foreach (StratumModel stratum in this.TreeStrata)
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

        public IEnumerable<StratumModel> ReadTreeBasedStrata()
        {
            Debug.Assert(DAL != null);

            foreach (var st in
                DAL.From<StratumModel>()
                .Join("CuttingUnitStratum", "USING (Stratum_CN)")
                .Where("CuttingUnitStratum.CuttingUnit_CN = ?" +
                        "AND Method IN ( '100', 'STR', '3P', 'S3P')")
                .Query(CuttingUnit_CN))
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

        public void SaveCounts()
        {
            foreach (StratumModel stratum in TreeStrata)
            {
                stratum.SaveCounts();
            }
            foreach (StratumModel stratum in PlotStrata)
            {
                stratum.SaveCounts();
            }
        }

        public bool TrySaveCounts()
        {
            bool success = true;
            foreach (StratumModel stratum in TreeStrata)
            {
                success = stratum.TrySaveCounts() && success;
            }
            foreach (StratumModel stratum in PlotStrata)
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
                .Where("CuttingUnit_CN = ?")
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