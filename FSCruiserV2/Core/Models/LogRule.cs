using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class LogRule
    {
        public List<string> Species { get; private set; }
        public List<LogHeightInfo> LogHeights { get; private set; }

        public LogRule()
        {
            LogHeights = new List<LogHeightInfo>();
        }

        public LogRule(string species)
        {
            var speciesArray = species.Split(' ');
            this.Species = new List<string>(speciesArray);
        }

        public void AddLogHeight(LogHeightInfo logHeightInfo)
        {
            LogHeights.Add(logHeightInfo);
        }

        public uint GetDefaultLogHeight(float height, float dbh)
        {
            foreach (LogHeightInfo lhi in LogHeights)
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
