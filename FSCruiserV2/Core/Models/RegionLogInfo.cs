using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class RegionLogInfo
    {
        public int RegionCode { get; private set; }

        private Dictionary<String, Guid> SpeciesToRuleCodes;
        private Dictionary<Guid, LogRule> Rules;

        public RegionLogInfo(int regionCode)
        {
            this.RegionCode = regionCode;
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
                    throw new Exception("Rule not found");
            }
            else
                throw new Exception("Tree species not found");
        }

        public void AddRule(String speciesCode, LogRule rule)
        {
            if (rule.ID == null)
                rule.ID = Guid.NewGuid();

            if (SpeciesToRuleCodes.ContainsKey(speciesCode))
            {
                SpeciesToRuleCodes[speciesCode] = rule.ID;
            }
            else
            {
                SpeciesToRuleCodes.Add(speciesCode, rule.ID);
            }

            if (Rules.ContainsKey(rule.ID))
            {
                Rules[rule.ID] = rule;
            }
            else
            {
                Rules.Add(rule.ID, rule);
            }
        }
    }
}
