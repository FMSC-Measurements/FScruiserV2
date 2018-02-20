﻿using System;
using System.Xml.Serialization;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    [Serializable]
    public class TallyAction
    {
        [XmlIgnore]
        public Tree TreeRecord { get; set; }

        //[XmlIgnore]
        //public SampleSelecter Sampler { get { return Count.Tag as SampleSelecter; } }
        [XmlIgnore]
        public CountTree Count { get; set; }

        [XmlIgnore]
        public TreeEstimateDO TreeEstimate { get; set; }

        [XmlAttribute]
        public String Time { get; set; }

        [XmlAttribute]
        public int KPI { get; set; }

        [XmlAttribute]
        public long CountCN { get; set; }

        [XmlAttribute]
        public long TreeEstimateCN { get; set; }

        [XmlAttribute]
        public long TreeCN { get; set; }

        public TallyAction()
        {
            Time = DateTime.Now.ToString("hh:mm");
        }

        public TallyAction(CountTree count)
            : this()
        {
            Count = count;
        }

        public TallyAction(Tree treeRecord, CountTree count)
        {
            TreeRecord = treeRecord;
        }

        

        public override string ToString()
        {
            String stCode = "--";
            string sgCode = "----";

            if (Count != null)
            {
                var sg = Count.SampleGroup;
                if (sg != null && sg.Stratum != null)
                {
                    stCode = sg.Stratum.Code;
                    if (stCode.Length > 2) { stCode = stCode.Substring(0, 2); }

                    sgCode = sg.Code;
                    if (sgCode.Length > 4) { sgCode = sgCode.Substring(0, 4); }
                }
            }

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