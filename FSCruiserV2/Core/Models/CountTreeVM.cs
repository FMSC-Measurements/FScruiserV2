using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using CruiseDAL;
using FMSC.ORM.Core.EntityAttributes;

namespace FSCruiser.Core.Models
{
    public class CountTreeVM : CountTreeDO
    {
        public CountTreeVM()
            : base()
        { }

        public CountTreeVM(DAL ds)
            : base(ds)
        { }

        [IgnoreField]
        public new SampleGroupVM SampleGroup
        {
            get
            {
                return (SampleGroupVM)base.SampleGroup;
            }
            set
            {
                base.SampleGroup = value;
            }
        }

        public override SampleGroupDO GetSampleGroup()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<SampleGroupVM>(this.SampleGroup_CN);
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


    }
}
