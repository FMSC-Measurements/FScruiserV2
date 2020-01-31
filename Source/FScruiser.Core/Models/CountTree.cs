using System;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.ORM.Core;

namespace FSCruiser.Core.Models
{
    public class CountTree : CountTreeDO
    {
        public CountTree()
            : base()
        { }

        public CountTree(Datastore ds)
            : base(ds)
        { }

        [IgnoreField]
        public new SampleGroup SampleGroup
        {
            get
            {
                return (SampleGroup)base.SampleGroup;
            }
            set
            {
                base.SampleGroup = value;
            }
        }

        public override SampleGroupDO GetSampleGroup()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<SampleGroup>(this.SampleGroup_CN);
        }

        public long GetCountsFromTrees()
        {
            object value;
            if (this.TreeDefaultValue_CN != null && this.TreeDefaultValue_CN != 0)
            {
                value = this.DAL.ExecuteScalar("SELECT sum(TreeCount) FROM Tree WHERE CuttingUnit_CN = ? AND SampleGroup_CN = ? AND TreeDefaultValue_CN = ?;", this.CuttingUnit_CN, this.SampleGroup_CN, this.TreeDefaultValue_CN);
            }
            else
            {
                value = this.DAL.ExecuteScalar("SELECT sum(TreeCount) FROM Tree WHERE CuttingUnit_CN = ? AND SampleGroup_CN = ?;", this.CuttingUnit_CN, this.SampleGroup_CN);
            }
            if(value == null || value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt64(value);
        }

        public long GetMeasureTreeCount()
        {
            object value;
            if (this.TreeDefaultValue_CN != null && this.TreeDefaultValue_CN != 0)
            {
                value = this.DAL.GetRowCount("Tree", "WHERE CuttingUnit_CN = ? AND SampleGroup_CN = ? AND TreeDefaultValue_CN = ? AND CountOrMeasure = 'M'", this.CuttingUnit_CN, this.SampleGroup_CN, this.TreeDefaultValue_CN);
            }
            else
            {
                value = this.DAL.GetRowCount("Tree", "WHERE CuttingUnit_CN = ? AND SampleGroup_CN = ? AND CountOrMeasure = 'M'", this.CuttingUnit_CN, this.SampleGroup_CN);
            }

            return Convert.ToInt64(value);
        }

        public long GetTotalTreeCount()
        {
            return GetCountsFromTrees() + this.TreeCount;
        }

        //public TreeEstimateDO LogTreeEstimate(int kpi)
        //{
        //    var te = new TreeEstimateDO(DAL);
        //    te.KPI = kpi;
        //    te.CountTree = this;
        //    te.Save();

        //    return te;
        //}
    }
}