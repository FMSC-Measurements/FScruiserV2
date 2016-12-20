using System;

using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public enum FixCNTTallyField { Unknown, DBH, TotalHeight, DRC };

    public interface IFixCNTTallyClass
    {
        long? FixCNTTallyClass_CN { get; set; }

        FixCNTTallyField Field { get; set; }

        long? Stratum_CN { get; set; }

        FixCNTStratum Stratum { get; set; }

        void SetTreeFieldValue(Tree tree, IFixCNTTallyBucket tallyBucket);

        double GetTreeFieldValue(Tree tree);
    }

    [EntitySource(SourceName = "FixCNTTallyClass")]
    public class FixCNTTallyClass : IFixCNTTallyClass
    {
        [PrimaryKeyField(Name = "FixCNTTallyClass_CN")]
        public long? FixCNTTallyClass_CN { get; set; }

        [Field(Name = "FieldName")]
        public FixCNTTallyField Field { get; set; }

        [Field(Name = "Stratum_CN")]
        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }

        public void SetTreeFieldValue(Tree tree, IFixCNTTallyBucket tallyBucket)
        {
            if (this.Field == FixCNTTallyField.DBH)
            {
                tree.DBH = (float)tallyBucket.MidpointValue;
            }
            else if (Field == FixCNTTallyField.TotalHeight)
            {
                tree.TotalHeight = (float)tallyBucket.MidpointValue;
            }
            else if (Field == FixCNTTallyField.DRC)
            {
                tree.DRC = (float)tallyBucket.MidpointValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public double GetTreeFieldValue(Tree tree)
        {
            if (this.Field == FixCNTTallyField.DBH)
            {
                return tree.DBH;
            }
            else if (Field == FixCNTTallyField.TotalHeight)
            {
                return tree.TotalHeight;
            }
            else if (Field == FixCNTTallyField.DRC)
            {
                return tree.DRC;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}