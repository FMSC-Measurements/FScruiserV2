using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class Sale : CruiseDAL.DataObjects.SaleDO
    {
        [Field(Name = "Region")]
        public virtual String Region { get; set; }

        public RegionLogInfo GetRegionLogInfo()
        {
            throw new NotImplementedException();
        }
    }
}
