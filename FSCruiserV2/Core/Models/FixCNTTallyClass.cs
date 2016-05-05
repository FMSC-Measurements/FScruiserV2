using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public enum FixCNTTallyField { Unknown, DBH, TotalHeight };

    public interface IFixCNTTallyClass
    {
        FixCNTTallyField Field { get; set; }

        long? Stratum_CN { get; set; }

        FixCNTStratum Stratum { get; set; }

        void SetTreeFieldValue(TreeVM tree, IFixCNTTallyBucket tallyBucket);

        double GetTreeFieldValue(TreeVM tree);
    }


    [EntitySource(SourceName="FixCNTTallyClass")]
    public class FixCNTTallyClass : IFixCNTTallyClass
    {
        [Field(Name="FieldName")]
        public FixCNTTallyField Field { get; set; }

        [Field(Name="Stratum_CN")]
        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }

        public void SetTreeFieldValue(TreeVM tree, IFixCNTTallyBucket tallyBucket)
        {
            if (this.Field == FixCNTTallyField.DBH)
            {
                tree.DBH = (float)tallyBucket.IntervalValue;
            }
            else if (Field == FixCNTTallyField.TotalHeight)
            {
                tree.TotalHeight = (float)tallyBucket.IntervalValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public double GetTreeFieldValue(TreeVM tree)
        {
            if (this.Field == FixCNTTallyField.DBH)
            {
                return tree.DBH;
            }
            else if (Field == FixCNTTallyField.TotalHeight)
            {
                return tree.TotalHeight;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
