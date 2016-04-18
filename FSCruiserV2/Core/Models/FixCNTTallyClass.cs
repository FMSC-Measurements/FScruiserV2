using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public enum FixCNTTallyClassField { Unknown, DBH, TotalHeight };

    public class FixCNTTallyClass
    {
        public FixCNTTallyClassField FieldName { get; set; }

        public long? SampleGroup_CN { get; set; }


    }
}
