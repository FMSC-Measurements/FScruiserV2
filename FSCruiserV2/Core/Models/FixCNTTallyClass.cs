using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public enum FixCNTTallyClassField { Unknown, DBH, TotalHeight };

    public class FixCNTTallyClass
    {
        public FixCNTTallyClassField FieldName { get; set; }

        public long? Stratum_CN { get; set; }

        public FixCNTStratum Stratum { get; set; }


    }
}
