using System;
using System.Collections.Generic;

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
            else if (SpeciesToRuleCodes.ContainsKey(String.Empty))
            {
                Guid ruleCode = SpeciesToRuleCodes[String.Empty];

                if (Rules.ContainsKey(ruleCode))
                {
                    return Rules[ruleCode];
                }
                else
                { return null; }
            }
            { return null; }
        }

        public void AddRule(LogRule rule)
        {
            var ruleID = Guid.NewGuid();

            Rules.Add(ruleID, rule);

            var species = rule.Species;

            if (species == null
                || species.Count == 0)
            {
                SpeciesToRuleCodes.Add("", ruleID);
            }
            else
            {
                foreach (string sp in species)
                {
                    SpeciesToRuleCodes.Add(sp, ruleID);
                }
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