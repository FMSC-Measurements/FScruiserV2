using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public enum FixCNTTallyField { Unknown, DBH, TotalHeight };

    public interface IFixCNTTallyClass
    {
        FixCNTTallyField Field { get; set; }

        long? Stratum_CN { get; set; }

        FixCNTStratum Stratum { get; set; }

        void SetTreeFieldValue(TreeVM tree, FixCNTTallyBucket tallyBucket);
    }

    public class FixCNTTallyClass
    {
        public FixCNTTallyField FieldName { get; set; }

        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }

        public void SetTreeFieldValue(TreeVM tree, FixCNTTallyBucket tallyBucket)
        {
            if (this.FieldName == FixCNTTallyField.DBH)
            {
                tree.DBH = (float)tallyBucket.IntervalValue;
            }
            else if (FieldName == FixCNTTallyField.TotalHeight)
            {
                tree.TotalHeight = (float)tallyBucket.IntervalValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }        
    }
}
