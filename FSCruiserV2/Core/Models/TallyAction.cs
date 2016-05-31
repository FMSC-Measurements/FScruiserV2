using System;
using System.Xml.Serialization;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    [Serializable]
    public class TallyAction
    {
        [XmlIgnore]
        public TreeVM TreeRecord { get; set; }

        //[XmlIgnore]
        //public SampleSelecter Sampler { get { return Count.Tag as SampleSelecter; } }
        [XmlIgnore]
        public CountTreeVM Count { get; set; }

        [XmlIgnore]
        public TreeEstimateDO TreeEstimate { get; set; }

        [XmlAttribute]
        public String Time { get; set; }

        [XmlAttribute]
        public int KPI { get; set; }

        private long _countCN = 0L;

        [XmlAttribute]
        public long CountCN
        {
            get { return (Count != null && Count.CountTree_CN != null) ? Count.CountTree_CN.Value : 0L; }
            set
            {
                _countCN = value;
            }
        }

        private long _treeEstCN;

        [XmlAttribute]
        public long TreeEstimateCN
        {
            get { return (this.TreeEstimate != null && this.TreeEstimate.TreeEstimate_CN != null) ? this.TreeEstimate.TreeEstimate_CN.Value : 0L; }
            set
            {
                _treeEstCN = value;
            }
        }

        private long _treeCN;

        [XmlAttribute]
        public long TreeCN
        {
            get { return (TreeRecord != null && TreeRecord.Tree_CN != null) ? TreeRecord.Tree_CN.Value : 0L; }
            set
            {
                _treeCN = value;
            }
        }

        public TallyAction()
        {
        }

        public TallyAction(CountTreeVM count)
        {
            this.Count = count;
        }

        public TallyAction(TreeVM treeRecord, CountTreeVM count)
        {
            this.TreeRecord = treeRecord;
            this.Count = count;
        }

        public void PopulateData(CruiseDAL.DAL dal)
        {
            if (this._countCN != 0L)
            {
                this.Count = dal.ReadSingleRow<CountTreeVM>(this._countCN);
            }
            if (this._treeCN != 0L)
            {
                this.TreeRecord = dal.ReadSingleRow<TreeVM>(this._treeCN);
            }
            if (this._treeEstCN != 0L)
            {
                this.TreeEstimate = dal.ReadSingleRow<TreeEstimateDO>(this._treeEstCN);
            }
        }

        public override string ToString()
        {
            String stCode = "--";
            string sgCode = "----";
            SampleGroupDO sg = (Count != null) ? Count.SampleGroup : null;
            try//TODO remove try catch?
            {
                if (sg != null)
                {
                    stCode = sg.Stratum.Code;
                    if (stCode.Length > 2) { stCode = stCode.Substring(0, 2); }

                    sgCode = sg.Code;
                    if (sgCode.Length > 4) { sgCode = sgCode.Substring(0, 4); }
                }
            }
            catch { }

            String[] a = new String[3];
            a[0] = string.Format("{0} {1}", stCode, sgCode);
            if (KPI != 0)
            {
                a[1] = KPI.ToString("' ['#']'");
            }
            if (TreeRecord != null)
            {
                a[2] = String.Format(" #{0} {1}", TreeRecord.TreeNumber, TreeRecord.CountOrMeasure);
            }
            return string.Concat(a);

            //System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //builder.AppendFormat(null, "{0} {1}", stCode, sgCode);
            //if(KPI != 0)
            //{
            //    builder.AppendFormat(null, " [{0}]", KPI);
            //}
            //if (TreeRecord != null)
            //{
            //    builder.AppendFormat(null, " #{0} {1}", TreeRecord.TreeNumber, TreeRecord.CountOrMeasure);
            //}
            //return builder.ToString();
        }
    }
}