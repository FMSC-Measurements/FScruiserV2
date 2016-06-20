using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class StratumModel : StratumDO, ITreeFieldProvider, ILogFieldProvider
    {
        Dictionary<char, CountTreeVM> _hotKeyLookup;

        /// <summary>
        /// for 3ppnt
        /// </summary>
        public ThreePSelecter SampleSelecter { get; set; }

        public bool Is3P
        {
            get
            {
                return Method == CruiseMethods.THREEP
                    || Method == CruiseMethods.P3P
                    || Method == CruiseMethods.F3P
                    || Method == CruiseMethods.S3P;
            }
        }

        public List<SampleGroupModel> SampleGroups { get; set; }

        public IEnumerable<CountTreeVM> Counts
        {
            get
            {
                if (SampleGroups != null)
                {
                    foreach (var sg in SampleGroups)
                    {
                        if (sg.Counts != null)
                        {
                            foreach (var cnt in sg.Counts)
                            {
                                yield return cnt;
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<char, CountTreeVM> HotKeyLookup
        {
            get
            {
                if (_hotKeyLookup == null)
                {
                    _hotKeyLookup = new Dictionary<char, CountTreeVM>();
                }
                return _hotKeyLookup;
            }
        }

        public Control TallyContainer { get; set; }

        public CountTreeVM GetCountByHotKey(char hotKey)
        {
            if (Counts == null) { return null; }
            foreach (var cnt in Counts)
            {
                if (cnt.Tally != null
                    && !string.IsNullOrEmpty(cnt.Tally.Hotkey))
                {
                    char hk = cnt.Tally.Hotkey[0];
                    hk = char.ToUpper(hk);
                    if (hk == hotKey) { return cnt; }
                }
            }
            return null;
        }

        public void PopulateHotKeyLookup()
        {
            _hotKeyLookup = new Dictionary<char, CountTreeVM>();
            foreach (CountTreeVM count in Counts)
            {
                if (count.Tally != null
                    && !string.IsNullOrEmpty(count.Tally.Hotkey))
                {
                    char hotkey = count.Tally.Hotkey[0];
                    hotkey = char.ToUpper(hotkey);
                    HotKeyLookup.Add(hotkey, count);
                }
            }
        }

        public void LoadSampleGroups()
        {
            SampleGroups = ReadSampleGroups().ToList();
        }

        public void LoadCounts(CuttingUnitDO unit)
        {
            foreach (var sg in SampleGroups)
            {
                sg.LoadCounts(unit);
            }
        }

        public void SaveSampleGroups()
        {
            foreach (SampleGroupModel sg in SampleGroups)
            {
                sg.SerializeSamplerState();
                sg.Save();
            }
        }

        public void SaveCounts()
        {
            if (SampleGroups == null) { return; }
            foreach (var sg in SampleGroups)
            {
                sg.SaveCounts();
            }
        }

        public bool TrySaveCounts()
        {
            bool success = true;
            if (SampleGroups == null) { return true; }
            foreach (var sg in SampleGroups)
            {
                success = sg.TrySaveCounts() & success;
            }

            return success;
        }

        IEnumerable<SampleGroupModel> ReadSampleGroups()
        {
            foreach (var sg in DAL.From<SampleGroupModel>()
                        .Where("Stratum_CN = ?")
                        .Read(Stratum_CN))
            {
                sg.Stratum = this;
                yield return sg;
            }
        }

        #region ITreeFieldProvider Members

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

        protected virtual IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        {
            return InternalReadTreeFields();
        }

        protected List<TreeFieldSetupDO> InternalReadTreeFields()
        {
            var fields = DAL.From<TreeFieldSetupDO>()
                .Where("Stratum_CN = ?")
                .OrderBy("FieldOrder")
                .Query(Stratum_CN).ToList();

            if (fields.Count == 0)
            {
                fields.Clear();
                fields.AddRange(Constants.DEFAULT_TREE_FIELDS);
            }

            return fields;
        }

        #endregion ITreeFieldProvider Members

        #region ILogFieldProvider Members

        IEnumerable<LogFieldSetupDO> _logFields;
        object _logFieldsReadLock = new object();

        public IEnumerable<LogFieldSetupDO> LogFields
        {
            get
            {
                lock (_logFieldsReadLock)
                {
                    if (_logFields == null && DAL != null)
                    {
                        _logFields = ReadLogFields();
                    }
                    return _logFields;
                }
            }
        }

        protected IEnumerable<LogFieldSetupDO> ReadLogFields()
        {
            var fields = DAL.From<LogFieldSetupDO>()
                .Where("Stratum_CN = ?")
                .OrderBy("FieldOrder")
                .Query(Stratum_CN).ToList();

            if (fields.Count == 0)
            {
                fields.AddRange(Constants.DEFAULT_LOG_FIELDS);
            }

            return fields;
        }

        #endregion ILogFieldProvider Members
    }
}