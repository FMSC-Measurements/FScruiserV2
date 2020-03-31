using System;

using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public static class FixCNTTallyFields
    {
        public const string DBH = "DBH";
        public const string TOTALHEIGHT = "TotalHeight";
        public const string DRC = "DRC";
    }

    public interface IFixCNTTallyClass
    {
        long? FixCNTTallyClass_CN { get; set; }

        string Field { get; set; }

        long? Stratum_CN { get; set; }

        FixCNTStratum Stratum { get; set; }

        void SetTreeFieldValue(Tree tree, IFixCNTTallyBucket tallyBucket);

        double GetTreeFieldValue(Tree tree);
    }

    [Table("FixCNTTallyClass")]
    public class FixCNTTallyClass : IFixCNTTallyClass
    {
        [PrimaryKeyField(Name = "FixCNTTallyClass_CN")]
        public long? FixCNTTallyClass_CN { get; set; }

        // type for FieldName is Integer but value stored is a string
        // cast value to text to tell System.Data.Sqlite to retrive value as string
        [Field(Alias = "FieldName", SQLExpression = "CAST (FieldName as Text)")]
        public string Field { get; set; }

        [Field(Name = "Stratum_CN")]
        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }

        public void SetTreeFieldValue(Tree tree, IFixCNTTallyBucket tallyBucket)
        {
            var field = Field;

            if (field.Equals(FixCNTTallyFields.DBH, StringComparison.OrdinalIgnoreCase))
            {
                tree.DBH = (float)tallyBucket.MidpointValue;
            }
            else if (field.Equals(FixCNTTallyFields.TOTALHEIGHT, StringComparison.OrdinalIgnoreCase))
            {
                tree.TotalHeight = (float)tallyBucket.MidpointValue;
            }
            else if (field.Equals(FixCNTTallyFields.DRC, StringComparison.OrdinalIgnoreCase))
            {
                tree.DRC = (float)tallyBucket.MidpointValue;
            }
            else
            {
                throw new InvalidOperationException("Invalid Field Value:" + field);
            }
        }

        public double GetTreeFieldValue(Tree tree)
        {
            var field = Field;

            if (field.Equals(FixCNTTallyFields.DBH, StringComparison.OrdinalIgnoreCase))
            {
                return tree.DBH;
            }
            else if (field.Equals(FixCNTTallyFields.TOTALHEIGHT, StringComparison.OrdinalIgnoreCase))
            {
                return tree.TotalHeight;
            }
            else if (field.Equals(FixCNTTallyFields.DRC, StringComparison.OrdinalIgnoreCase))
            {
                return tree.DRC;
            }
            else
            {
                throw new InvalidOperationException("Invalid Field Value:" + field);
            }
        }
    }
}