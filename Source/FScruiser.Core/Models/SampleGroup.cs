using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.Sampling;

namespace FSCruiser.Core.Models
{
    public class SampleGroup : SampleGroupDO
    {
        public SampleGroup()
            : base()
        { }

        [IgnoreField]
        public new Stratum Stratum
        {
            get
            {
                return (Stratum)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        public IEnumerable<CountTree> Counts { get; set; }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<Stratum>(this.Stratum_CN);
        }

        public void LoadCounts(CuttingUnitDO unit)
        {
            var counts = new List<CountTree>();
            var tallySettings = DAL.From<TallySettings>()
                .Where("SampleGroup_CN = ?")
                .GroupBy("CountTree.SampleGroup_CN", "CountTree.TreeDefaultValue_CN", "CountTree.Tally_CN")
                .Read(SampleGroup_CN);

            foreach (TallySettings ts in tallySettings)
            {
                CountTree count = DAL.From<CountTree>()
                    .Where("CuttingUnit_CN = ? AND SampleGroup_CN = ? AND Tally_CN = ?")
                    .Read(unit.CuttingUnit_CN
                    , ts.SampleGroup_CN
                    , ts.Tally_CN).FirstOrDefault();
                if (count == null)
                {
                    count = new CountTree(DAL);
                    count.CuttingUnit = unit;
                    count.SampleGroup_CN = ts.SampleGroup_CN;
                    count.TreeDefaultValue_CN = ts.TreeDefaultValue_CN;
                    count.Tally_CN = ts.Tally_CN;

                    count.Save();
                }

                count.SampleGroup = this;

                counts.Add(count);
            }

            Counts = counts;
        }

        public bool HasTreeDefault(TreeDefaultValueDO tdv)
        {
            return DAL.ExecuteScalar<bool>("SELECT count(1) " +
                    "FROM SampleGroupTreeDefaultValue " +
                    "WHERE TreeDefaultValue_CN = ? AND SampleGroup_CN = ?"
                    , tdv.TreeDefaultValue_CN, SampleGroup_CN);
        }

        public override string ToString()
        {
            return Code;
        }
    }
}