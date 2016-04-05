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
                            _regionalLogRules = MakeRegionFiveLogRules();
                            break;
                        }
                }

            }
            return _regionalLogRules;
        }

        RegionLogInfo MakeRegionFiveLogRules()
        {
            var regionalLogRules = new RegionLogInfo(5);

            var logRule = new LogRule();
            logRule.AddLogHeight(new LogHeightInfo(26, 35, 1));
            logRule.AddLogHeight(new LogHeightInfo(36, 45, 1));
            logRule.AddLogHeight(new LogHeightInfo(46, 55, 3));
            logRule.AddLogHeight(new LogHeightInfo(56, 65, 3));
            logRule.AddLogHeight(new LogHeightInfo(66, 75, 3));
            logRule.AddLogHeight(new LogHeightInfo(76, 85, 3));
            logRule.AddLogHeight(new LogHeightInfo(86, 95, 4));
            logRule.AddLogHeight(new LogHeightInfo(96, 105, 5));
            logRule.AddLogHeight(new LogHeightInfo(106, 115, 5));
            logRule.AddLogHeight(new LogHeightInfo(116, 125, 6));
            logRule.AddLogHeight(new LogHeightInfo(126, 135, 7));
            logRule.AddLogHeight(new LogHeightInfo(136, 145, 8));
            logRule.AddLogHeight(new LogHeightInfo(146, 155, 9));
            logRule.AddLogHeight(new LogHeightInfo(156, 165, 9));
            logRule.AddLogHeight(new LogHeightInfo(166, 175, 10));
            logRule.AddLogHeight(new LogHeightInfo(176, 185, 10));
            logRule.AddLogHeight(new LogHeightInfo(186, 195, 11));
            logRule.AddLogHeight(new LogHeightInfo(196, 205, 12));
            logRule.AddLogHeight(new LogHeightInfo(206, 215, 12));
            logRule.AddLogHeight(new LogHeightInfo(216, 225, 13));

            regionalLogRules.AddRule(logRule);
            return regionalLogRules;
        }

        RegionLogInfo MakeRegionTenLogRules()
        {
            var regionalLogRules = new RegionLogInfo(10);

            var spruce = new LogRule("98");
            spruce.AddLogHeight(new LogHeightInfo(36, 45, 1));
            spruce.AddLogHeight(new LogHeightInfo(46, 55, 1).WithBreaks(13));
            spruce.AddLogHeight(new LogHeightInfo(56, 65, 1).WithBreaks(18));
            spruce.AddLogHeight(new LogHeightInfo(66, 75, 2).WithBreaks(18));
            spruce.AddLogHeight(new LogHeightInfo(76, 85, 2).WithBreaks(13, 21));
            spruce.AddLogHeight(new LogHeightInfo(86, 95, 3).WithBreaks(17, 33));
            spruce.AddLogHeight(new LogHeightInfo(96, 105, 3).WithBreaks(14, 23));
            spruce.AddLogHeight(new LogHeightInfo(106, 115, 4).WithBreaks(18, 34));
            spruce.AddLogHeight(new LogHeightInfo(116, 125, 4).WithBreaks(15, 24, 56));
            spruce.AddLogHeight(new LogHeightInfo(126, 135, 5).WithBreaks(19, 36));
            spruce.AddLogHeight(new LogHeightInfo(136, 145, 6).WithBreaks(25, 56));
            spruce.AddLogHeight(new LogHeightInfo(146, 155, 6).WithBreaks(19, 37));
            spruce.AddLogHeight(new LogHeightInfo(156, 165, 7).WithBreaks(25, 59));
            spruce.AddLogHeight(new LogHeightInfo(166, 175, 8).WithBreaks(36));
            spruce.AddLogHeight(new LogHeightInfo(176, 185, 9).WithBreaks(59));
            spruce.AddLogHeight(new LogHeightInfo(186, 195, 9).WithBreaks(34));
            spruce.AddLogHeight(new LogHeightInfo(196, 205, 10).WithBreaks(55));
            spruce.AddLogHeight(new LogHeightInfo(206, 215, 11));

            regionalLogRules.AddRule(spruce);

            var wrc = new LogRule("242");//western redcedar
            wrc.AddLogHeight(new LogHeightInfo(36, 45, 1));
            wrc.AddLogHeight(new LogHeightInfo(46, 55, 1).WithBreaks(14));
            wrc.AddLogHeight(new LogHeightInfo(56, 65, 1).WithBreaks(11));
            wrc.AddLogHeight(new LogHeightInfo(66, 75, 1).WithBreaks(10, 16));
            wrc.AddLogHeight(new LogHeightInfo(76, 85, 3));
            wrc.AddLogHeight(new LogHeightInfo(86, 95, 3).WithBreaks(18));
            wrc.AddLogHeight(new LogHeightInfo(96, 105, 4).WithBreaks(20));
            wrc.AddLogHeight(new LogHeightInfo(106, 115, 4).WithBreaks(19));
            wrc.AddLogHeight(new LogHeightInfo(116, 125, 5).WithBreaks(29));
            wrc.AddLogHeight(new LogHeightInfo(126, 135, 6).WithBreaks(55));
            wrc.AddLogHeight(new LogHeightInfo(136, 145, 7));
            wrc.AddLogHeight(new LogHeightInfo(146, 155, 7).WithBreaks(46));
            wrc.AddLogHeight(new LogHeightInfo(156, 165, 8));
            wrc.AddLogHeight(new LogHeightInfo(166, 175, 9));

            regionalLogRules.AddRule(wrc);

            var ayc = new LogRule("42"); //Alaska Yellow-cedar
            ayc.AddLogHeight(new LogHeightInfo(36, 45, 1));
            ayc.AddLogHeight(new LogHeightInfo(46, 55, 1).WithBreaks(13));
            ayc.AddLogHeight(new LogHeightInfo(56, 65, 1).WithBreaks(10));
            ayc.AddLogHeight(new LogHeightInfo(66, 75, 2).WithBreaks(15));
            ayc.AddLogHeight(new LogHeightInfo(76, 85, 3).WithBreaks(26));
            ayc.AddLogHeight(new LogHeightInfo(86, 95, 3).WithBreaks(16));
            ayc.AddLogHeight(new LogHeightInfo(96, 105, 4).WithBreaks(25));
            ayc.AddLogHeight(new LogHeightInfo(106, 115, 5).WithBreaks(45));
            ayc.AddLogHeight(new LogHeightInfo(116, 125, 6));
            ayc.AddLogHeight(new LogHeightInfo(126, 135, 6).WithBreaks(37));
            ayc.AddLogHeight(new LogHeightInfo(136, 145, 7));
            ayc.AddLogHeight(new LogHeightInfo(146, 155, 8));
            ayc.AddLogHeight(new LogHeightInfo(156, 165, 8).WithBreaks(56));
            ayc.AddLogHeight(new LogHeightInfo(166, 175, 9));
            ayc.AddLogHeight(new LogHeightInfo(176, 185, 10));

            regionalLogRules.AddRule(ayc);

            var hmlk = new LogRule("263"); //Hemlock
            hmlk.AddLogHeight(new LogHeightInfo(36, 45, 1));
            hmlk.AddLogHeight(new LogHeightInfo(46, 55, 1).WithBreaks(14));
            hmlk.AddLogHeight(new LogHeightInfo(56, 65, 1).WithBreaks(12, 20));
            hmlk.AddLogHeight(new LogHeightInfo(66, 75, 2).WithBreaks(15, 32));
            hmlk.AddLogHeight(new LogHeightInfo(76, 85, 2).WithBreaks(13, 22));
            hmlk.AddLogHeight(new LogHeightInfo(86, 95, 3).WithBreaks(17, 33));
            hmlk.AddLogHeight(new LogHeightInfo(96, 105, 3).WithBreaks(14, 24));
            hmlk.AddLogHeight(new LogHeightInfo(106, 115, 4).WithBreaks(18, 35));
            hmlk.AddLogHeight(new LogHeightInfo(116, 125, 5).WithBreaks(25, 57));
            hmlk.AddLogHeight(new LogHeightInfo(126, 135, 5).WithBreaks(19, 37));
            hmlk.AddLogHeight(new LogHeightInfo(136, 145, 6).WithBreaks(25, 58));
            hmlk.AddLogHeight(new LogHeightInfo(146, 155, 6).WithBreaks(19, 38));
            hmlk.AddLogHeight(new LogHeightInfo(156, 165, 7).WithBreaks(25, 61));
            hmlk.AddLogHeight(new LogHeightInfo(166, 175, 8).WithBreaks(38));
            hmlk.AddLogHeight(new LogHeightInfo(176, 185, 8).WithBreaks(25, 62));
            hmlk.AddLogHeight(new LogHeightInfo(186, 195, 9).WithBreaks(36));
            hmlk.AddLogHeight(new LogHeightInfo(196, 205, 10).WithBreaks(59));

            regionalLogRules.AddRule(hmlk);

            var ra = new LogRule("351");//Red Alder
            ra.AddLogHeight(new LogHeightInfo(36, 45, 1).WithBreaks(19));
            ra.AddLogHeight(new LogHeightInfo(46, 55, 1).WithBreaks(11));
            ra.AddLogHeight(new LogHeightInfo(56, 65, 1).WithBreaks(9, 18));
            ra.AddLogHeight(new LogHeightInfo(66, 75, 1).WithBreaks(8, 12));
            ra.AddLogHeight(new LogHeightInfo(76, 85, 2).WithBreaks(9, 16));
            ra.AddLogHeight(new LogHeightInfo(86, 95, 3).WithBreaks(12));
            ra.AddLogHeight(new LogHeightInfo(96, 105, 4).WithBreaks(16));

            regionalLogRules.AddRule(ra);

            return regionalLogRules;
        }
    }

    
}
