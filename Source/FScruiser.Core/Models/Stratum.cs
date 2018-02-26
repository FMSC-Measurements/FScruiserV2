using System;
using System.Collections.Generic;
using System.Linq;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using System.Xml.Serialization;

namespace FSCruiser.Core.Models
{
    public class Stratum : StratumDO, ITreeFieldProvider, ILogFieldProvider
    {
        Dictionary<char, CountTree> _hotKeyLookup;

        [XmlIgnore]
        public bool Is3P
        {
            get
            {
                return CruiseMethods.THREE_P_METHODS.Contains(Method);
            }
        }

        [XmlIgnore]
        public List<SampleGroup> SampleGroups { get; set; }

        [XmlIgnore]
        public IEnumerable<CountTree> Counts
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

        [XmlIgnore]
        public Dictionary<char, CountTree> HotKeyLookup
        {
            get
            {
                if (_hotKeyLookup == null)
                {
                    _hotKeyLookup = new Dictionary<char, CountTree>();
                }
                return _hotKeyLookup;
            }
        }

        public CountTree GetCountByHotKey(char hotKey)
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
            _hotKeyLookup = new Dictionary<char, CountTree>();
            foreach (CountTree count in Counts)
            {
                if (count.Tally != null
                    && !string.IsNullOrEmpty(count.Tally.Hotkey))
                {
                    char hotkey = count.Tally.Hotkey[0];
                    hotkey = char.ToUpper(hotkey);
                    try
                    {
                        HotKeyLookup.Add(hotkey, count);
                    }
                    catch (ArgumentException e)
                    {
                        //MessageBox.Show("Duplicate Tally, File My Have Bad Setup Data");
                        throw new CruiseConfigurationException("Duplicate Tally Setup Information, Please Contact FMSC", e);
                    }
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

        //public Exception TrySaveSampleGroups()
        //{
        //    Exception ex = null;
        //    foreach (var sg in SampleGroups)
        //    {
        //        try
        //        {
        //            sg.SerializeSamplerState();
        //            sg.Save();
        //        }
        //        catch (Exception e)
        //        {
        //            if (ex == null)
        //            {
        //                ex = e;
        //            }
        //        }
        //    }
        //    return ex;
        //}

        //public void SaveSampleGroups()
        //{
        //    foreach (SampleGroup sg in SampleGroups)
        //    {
        //        sg.SerializeSamplerState();
        //        sg.Save();
        //    }
        //}

        //public void SaveCounts()
        //{
        //    if (SampleGroups == null) { return; }
        //    foreach (var sg in SampleGroups)
        //    {
        //        sg.SaveCounts();
        //    }
        //}

        //public Exception TrySaveCounts()
        //{
        //    Exception ex = null;

        //    if (SampleGroups == null) { return null; }
        //    foreach (var sg in SampleGroups)
        //    {
        //        Exception ex1;
        //        if (!sg.TrySaveCounts(out ex1))
        //        {
        //            ex = ex1;
        //        }
        //    }

        //    return ex;
        //}

        new IEnumerable<SampleGroup> ReadSampleGroups()
        {
            foreach (var sg in DAL.From<SampleGroup>()
                        .Where("Stratum_CN = ?")
                        .Read(Stratum_CN))
            {
                sg.Stratum = this;
                yield return sg;
            }
        }

        public override string ToString()
        {
            return Code;
        }

        #region ITreeFieldProvider Members

        object _treeFieldsReadLock = new object();
        IEnumerable<TreeFieldSetupDO> _treeFields;

        [XmlIgnore]
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

            if (Is3P && !fields.Any(f => f.Field == "STM"))
            {
                fields.Add(new TreeFieldSetupDO() { Field = "STM", Heading = "STM" });
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