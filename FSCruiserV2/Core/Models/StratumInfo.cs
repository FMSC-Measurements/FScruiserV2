using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using System.Windows.Forms;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class StratumVM : StratumDO
    {
        private Dictionary<char, CountTreeVM> _hotKeyLookup;

        public StratumVM()
            : base()
        { }


        /// <summary>
        /// for 3ppnt 
        /// </summary>
        public ThreePSelecter SampleSelecter { get; set; }

        public string[] TreeFieldNames { get; set; }

        //public StratumDO Stratum { get; set; }
        public List<PlotVM> Plots { get; set; }
        //public List<CountTreeDO> Counts { get; set; }
        
        public Dictionary<char, CountTreeVM> HotKeyLookup 
        { 
            get
            {
                if(_hotKeyLookup == null)
                {
                    _hotKeyLookup = new Dictionary<char, CountTreeVM>();
                }
                return _hotKeyLookup;
            }
        }
        public Control TallyContainer { get; set; }

        public void LoadTreeFieldNames()
        {
            string command = String.Concat("Select group_concat(distinct Field) FROM TreeFieldSetup WHERE Stratum_CN = ", this.Stratum_CN, ";");
            string result = (string)this.DAL.ExecuteScalar(command);
            if (!string.IsNullOrEmpty(result))
            {
                this.TreeFieldNames = result.Split(',');
            }
        }
    }
}
