using System.Collections.Generic;

namespace FSCruiser.Core.Models
{
    public class LogRule
    {
        public List<string> Species { get; set; }

        public IList<LogHeightClass> LogHeights { get; private set; }

        public LogRule()
        {
            LogHeights = new List<LogHeightClass>();
        }

        public LogRule(string species)
            : this()
        {
            var speciesArray = species.Split(' ');
            Species = new List<string>(speciesArray);
        }

        public void Add(LogHeightClass logHeightInfo)
        {
            LogHeights.Add(logHeightInfo);
        }

        public virtual double GetDefaultLogCount(float height, float dbh, long mrchHgtLL)
        {
            foreach (LogHeightClass lhi in LogHeights)
            {
                if (lhi.IsInRange(height))
                {
                    var logCnt16Ft = lhi.GetDefaultLogCount(dbh);

                    if (mrchHgtLL == 16)
                    {
                        return logCnt16Ft;
                    }
                    if (mrchHgtLL == 32)
                    {
                        return logCnt16Ft / 2.0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return 0;
        }
    }
}