using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FSCruiser.Core.Models
{
    public class RegionLogInfo
    {
        protected class RuleCollection : ICollection<LogRule>
        {
            Dictionary<string, int> SpeciesToRuleCodes = new Dictionary<string, int>();
            List<LogRule> Rules = new List<LogRule>();

            public int Count { get { return Rules.Count; } }

            public bool IsReadOnly { get { return false; } }

            public void Add(LogRule rule)
            {
                var index = Rules.Count;
                Rules.Add(rule);

                var species = rule.Species;
                if (species == null
                    || species.Count == 0)
                {
                    System.Diagnostics.Debug.Assert(SpeciesToRuleCodes.ContainsKey("") == false);
                    SpeciesToRuleCodes.Add("", index);
                }
                else
                {
                    foreach (string sp in species)
                    {
                        SpeciesToRuleCodes.Add(sp, index);
                    }
                }
            }

            public LogRule GetLogRule(String species)
            {
                species = species ?? String.Empty;

                if (SpeciesToRuleCodes.ContainsKey(species))
                {
                    var index = SpeciesToRuleCodes[species];
                    return Rules[index];
                }
                else
                { return null; }
            }

            public void Clear()
            {
                Rules.Clear();
            }

            public bool Contains(LogRule item)
            {
                return Rules.Contains(item);
            }

            public void CopyTo(LogRule[] array, int arrayIndex)
            {
                Rules.CopyTo(array, arrayIndex);
            }

            public bool Remove(LogRule item)
            {
                return Rules.Remove(item);
            }

            public IEnumerator<LogRule> GetEnumerator()
            {
                return Rules.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Rules.GetEnumerator();
            }
        }

        public int RegionCode { get; private set; }

        RuleCollection _rules = new RuleCollection();
        public ICollection<LogRule> Rules { get { return _rules; } }

        public RegionLogInfo()
        {
        }

        public RegionLogInfo(int regionCode)
        {
            RegionCode = regionCode;
        }

        public virtual LogRule GetLogRule(String species)
        {
            return _rules.GetLogRule(species);
        }

        public void AddRule(LogRule rule)
        {
            _rules.Add(rule);
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