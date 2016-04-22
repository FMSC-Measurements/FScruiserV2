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
    }

    public class FixCNTTallyClass
    {
        public FixCNTTallyField FieldName { get; set; }

        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }


    }
}
