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
            logRule.Add(new LogHeightClass(26, 35, 1));
            logRule.Add(new LogHeightClass(36, 45, 1));
            logRule.Add(new LogHeightClass(46, 55, 3));
            logRule.Add(new LogHeightClass(56, 65, 3));
            logRule.Add(new LogHeightClass(66, 75, 3));
            logRule.Add(new LogHeightClass(76, 85, 3));
            logRule.Add(new LogHeightClass(86, 95, 4));
            logRule.Add(new LogHeightClass(96, 105, 5));
            logRule.Add(new LogHeightClass(106, 115, 5));
            logRule.Add(new LogHeightClass(116, 125, 6));
            logRule.Add(new LogHeightClass(126, 135, 7));
            logRule.Add(new LogHeightClass(136, 145, 8));
            logRule.Add(new LogHeightClass(146, 155, 9));
            logRule.Add(new LogHeightClass(156, 165, 9));
            logRule.Add(new LogHeightClass(166, 175, 10));
            logRule.Add(new LogHeightClass(176, 185, 10));
            logRule.Add(new LogHeightClass(186, 195, 11));
            logRule.Add(new LogHeightClass(196, 205, 12));
            logRule.Add(new LogHeightClass(206, 215, 12));
            logRule.Add(new LogHeightClass(216, 225, 13));

            regionalLogRules.AddRule(logRule);
            return regionalLogRules;
        }

        RegionLogInfo MakeRegionTenLogRules()
        {
            var regionalLogRules = new RegionLogInfo(10);

            var spruce = new LogRule("98");
            spruce.Add(new LogHeightClass(36, 45, 1));
            spruce.Add(new LogHeightClass(46, 55, 1).WithBreaks(13));
            spruce.Add(new LogHeightClass(56, 65, 1).WithBreaks(18));
            spruce.Add(new LogHeightClass(66, 75, 2).WithBreaks(18));
            spruce.Add(new LogHeightClass(76, 85, 2).WithBreaks(13, 21));
            spruce.Add(new LogHeightClass(86, 95, 3).WithBreaks(17, 33));
            spruce.Add(new LogHeightClass(96, 105, 3).WithBreaks(14, 23));
            spruce.Add(new LogHeightClass(106, 115, 4).WithBreaks(18, 34));
            spruce.Add(new LogHeightClass(116, 125, 4).WithBreaks(15, 24, 56));
            spruce.Add(new LogHeightClass(126, 135, 5).WithBreaks(19, 36));
            spruce.Add(new LogHeightClass(136, 145, 6).WithBreaks(25, 56));
            spruce.Add(new LogHeightClass(146, 155, 6).WithBreaks(19, 37));
            spruce.Add(new LogHeightClass(156, 165, 7).WithBreaks(25, 59));
            spruce.Add(new LogHeightClass(166, 175, 8).WithBreaks(36));
            spruce.Add(new LogHeightClass(176, 185, 9).WithBreaks(59));
            spruce.Add(new LogHeightClass(186, 195, 9).WithBreaks(34));
            spruce.Add(new LogHeightClass(196, 205, 10).WithBreaks(55));
            spruce.Add(new LogHeightClass(206, 215, 11));

            regionalLogRules.AddRule(spruce);

            var wrc = new LogRule("242");//western redcedar
            wrc.Add(new LogHeightClass(36, 45, 1));
            wrc.Add(new LogHeightClass(46, 55, 1).WithBreaks(14));
            wrc.Add(new LogHeightClass(56, 65, 1).WithBreaks(11));
            wrc.Add(new LogHeightClass(66, 75, 1).WithBreaks(10, 16));
            wrc.Add(new LogHeightClass(76, 85, 3));
            wrc.Add(new LogHeightClass(86, 95, 3).WithBreaks(18));
            wrc.Add(new LogHeightClass(96, 105, 4).WithBreaks(20));
            wrc.Add(new LogHeightClass(106, 115, 4).WithBreaks(19));
            wrc.Add(new LogHeightClass(116, 125, 5).WithBreaks(29));
            wrc.Add(new LogHeightClass(126, 135, 6).WithBreaks(55));
            wrc.Add(new LogHeightClass(136, 145, 7));
            wrc.Add(new LogHeightClass(146, 155, 7).WithBreaks(46));
            wrc.Add(new LogHeightClass(156, 165, 8));
            wrc.Add(new LogHeightClass(166, 175, 9));

            regionalLogRules.AddRule(wrc);

            var ayc = new LogRule("42"); //Alaska Yellow-cedar
            ayc.Add(new LogHeightClass(36, 45, 1));
            ayc.Add(new LogHeightClass(46, 55, 1).WithBreaks(13));
            ayc.Add(new LogHeightClass(56, 65, 1).WithBreaks(10));
            ayc.Add(new LogHeightClass(66, 75, 2).WithBreaks(15));
            ayc.Add(new LogHeightClass(76, 85, 3).WithBreaks(26));
            ayc.Add(new LogHeightClass(86, 95, 3).WithBreaks(16));
            ayc.Add(new LogHeightClass(96, 105, 4).WithBreaks(25));
            ayc.Add(new LogHeightClass(106, 115, 5).WithBreaks(45));
            ayc.Add(new LogHeightClass(116, 125, 6));
            ayc.Add(new LogHeightClass(126, 135, 6).WithBreaks(37));
            ayc.Add(new LogHeightClass(136, 145, 7));
            ayc.Add(new LogHeightClass(146, 155, 8));
            ayc.Add(new LogHeightClass(156, 165, 8).WithBreaks(56));
            ayc.Add(new LogHeightClass(166, 175, 9));
            ayc.Add(new LogHeightClass(176, 185, 10));

            regionalLogRules.AddRule(ayc);

            var hmlk = new LogRule("263"); //Hemlock
            hmlk.Add(new LogHeightClass(36, 45, 1));
            hmlk.Add(new LogHeightClass(46, 55, 1).WithBreaks(14));
            hmlk.Add(new LogHeightClass(56, 65, 1).WithBreaks(12, 20));
            hmlk.Add(new LogHeightClass(66, 75, 2).WithBreaks(15, 32));
            hmlk.Add(new LogHeightClass(76, 85, 2).WithBreaks(13, 22));
            hmlk.Add(new LogHeightClass(86, 95, 3).WithBreaks(17, 33));
            hmlk.Add(new LogHeightClass(96, 105, 3).WithBreaks(14, 24));
            hmlk.Add(new LogHeightClass(106, 115, 4).WithBreaks(18, 35));
            hmlk.Add(new LogHeightClass(116, 125, 5).WithBreaks(25, 57));
            hmlk.Add(new LogHeightClass(126, 135, 5).WithBreaks(19, 37));
            hmlk.Add(new LogHeightClass(136, 145, 6).WithBreaks(25, 58));
            hmlk.Add(new LogHeightClass(146, 155, 6).WithBreaks(19, 38));
            hmlk.Add(new LogHeightClass(156, 165, 7).WithBreaks(25, 61));
            hmlk.Add(new LogHeightClass(166, 175, 8).WithBreaks(38));
            hmlk.Add(new LogHeightClass(176, 185, 8).WithBreaks(25, 62));
            hmlk.Add(new LogHeightClass(186, 195, 9).WithBreaks(36));
            hmlk.Add(new LogHeightClass(196, 205, 10).WithBreaks(59));

            regionalLogRules.AddRule(hmlk);

            var ra = new LogRule("351");//Red Alder
            ra.Add(new LogHeightClass(36, 45, 1).WithBreaks(19));
            ra.Add(new LogHeightClass(46, 55, 1).WithBreaks(11));
            ra.Add(new LogHeightClass(56, 65, 1).WithBreaks(9, 18));
            ra.Add(new LogHeightClass(66, 75, 1).WithBreaks(8, 12));
            ra.Add(new LogHeightClass(76, 85, 2).WithBreaks(9, 16));
            ra.Add(new LogHeightClass(86, 95, 3).WithBreaks(12));
            ra.Add(new LogHeightClass(96, 105, 4).WithBreaks(16));

            regionalLogRules.AddRule(ra);

            return regionalLogRules;
        }
    }

    
}
