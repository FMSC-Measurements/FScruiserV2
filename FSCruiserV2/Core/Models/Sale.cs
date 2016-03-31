using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class Sale 
    {
        RegionLogInfo _regionalLogRules;

        [Field(Name = "Region")]
        public new uint Region { get; set; }


        public RegionLogInfo GetRegionLogInfo()
        {
            if (_regionalLogRules == null)
            {
                switch (this.Region)
                {
                    case 10:
                        {
                            _regionalLogRules = MakeRegionTenLogRules();
                            break;
                        }
                    case 5:
                        {

                            break;
                        }
                }

            }
            return _regionalLogRules;
        }

        RegionLogInfo MakeRegionFiveLogRules()
        {
            throw new NotImplementedException();
        }

        RegionLogInfo MakeRegionTenLogRules()
        {
            var regionalLogRules = new RegionLogInfo(10);

            var sAndH = new LogRule("98 263");
            sAndH.AddLogHeight(new LogHeightInfo(0, 46, 1));


            throw new NotImplementedException();            
        }
    }

    
}
