using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class LogRule
    {
        public List<string> Species { get; private set; }
        public IList<LogHeightClass> LogHeights { get; private set; }

        public LogRule()
        {
            LogHeights = new List<LogHeightClass>();
        }

        public LogRule(string species) : this()
        {
            var speciesArray = species.Split(' ');
            this.Species = new List<string>(speciesArray);
        }

        public void Add(LogHeightClass logHeightInfo)
        {
            LogHeights.Add(logHeightInfo);
        }

        public uint GetDefaultLogHeight(float height, float dbh)
        {
            foreach (LogHeightClass lhi in LogHeights)
            {
                if (lhi.Range.IsInRange(height))
                {
                    return lhi.GetDefaultLogCount(dbh);
                }
            }

            return 0;
        }
    }
}
