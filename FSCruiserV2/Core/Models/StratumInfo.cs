using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class StratumVM : StratumDO, ITreeFieldProvider, ILogFieldProvider
    {
        private Dictionary<char, CountTreeVM> _hotKeyLookup;

        /// <summary>
        /// for 3ppnt
        /// </summary>
        public ThreePSelecter SampleSelecter { get; set; }

        public string[] TreeFieldNames { get; set; }

        //public StratumDO Stratum { get; set; }
        //public List<PlotVM> Plots { get; set; }
        public List<CountTreeVM> Counts { get; set; }

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

        public void PopulateHotKeyLookup()
        {
            _hotKeyLookup = new Dictionary<char, CountTreeVM>();
            foreach (CountTreeVM count in Counts)
            {
                try
                {
                    char hotkey = count.Tally.Hotkey[0];
                    hotkey = char.ToUpper(hotkey);
                    HotKeyLookup.Add(hotkey, count);
                }
                catch
                { }
            }
        }

        public void LoadTreeFieldNames()
        {
            string command = String.Concat("Select group_concat(distinct Field) FROM TreeFieldSetup WHERE Stratum_CN = ", this.Stratum_CN, ";");
            string result = (string)this.DAL.ExecuteScalar(command);
            if (!string.IsNullOrEmpty(result))
            {
                this.TreeFieldNames = result.Split(',');
            }
        }

        public void SaveCounts()
        {
            if (Counts == null) { return; } // if this is a h_pct stratum then counts won't be populated
            foreach (CountTreeVM count in Counts)
            {
                count.Save();
            }
        }

        public bool TrySaveCounts()
        {
            try
            {
                this.SaveCounts();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e, "Exception");
                return false;
            }
        }

        #region ITreeFieldProvider Members

        public virtual List<TreeFieldSetupDO> ReadTreeFields()
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

        public List<LogFieldSetupDO> ReadLogFields()
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