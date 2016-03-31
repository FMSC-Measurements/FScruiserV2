using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class RegionLogInfo
    {
        //public int RegionCode { get; private set; }

        private Dictionary<String, Guid> SpeciesToRuleCodes;
        private Dictionary<Guid, LogRule> Rules;

        public RegionLogInfo(int regionCode)
        {
            //this.RegionCode = regionCode;
            SpeciesToRuleCodes = new Dictionary<String, Guid>();
            Rules = new Dictionary<Guid, LogRule>();
        }

        public LogRule GetLogRule(String species)
        {
            if (SpeciesToRuleCodes.ContainsKey(species))
            {
                Guid ruleCode = SpeciesToRuleCodes[species];

                if (Rules.ContainsKey(ruleCode))
                {
                    return Rules[ruleCode];
                }
                else 
                { return null; }

            }
            else 
            { return null; }
        }

        public void AddRule(LogRule rule)
        {
            var ruleID = Guid.NewGuid();

            Rules.Add(ruleID, rule);

            foreach (string sp in rule.Species)
            {
                SpeciesToRuleCodes.Add(sp, ruleID);
            }
        }

        public void AddRule(IEnumerable<LogRule> rules)
        {
            foreach (LogRule rule in rules)
            {
                AddRule(rule);
            }
        }
    }
}
