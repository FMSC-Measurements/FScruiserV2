using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class LogRule
    {
        public Guid ID { get; internal set; }
        public List<LogHeightInfo> LogHeights { get; private set; }

        public LogRule()
        {
            LogHeights = new List<LogHeightInfo>();
        }

        public void AddLogHeight(LogHeightInfo logHeightInfo)
        {
            LogHeights.Add(logHeightInfo);
        }

        public int GetDefaultLogHeight(float height, float dbh)
        {
            foreach (LogHeightInfo lhi in LogHeights)
            {
                if (lhi.IsInRange(height))
                {
                    foreach (DBHBreak dbhBreak in lhi.DBHBreaks)
                    {
                        if (dbhBreak.DBHValue < dbh)
                        {
                            return dbhBreak.NumberOfLogs;
                        }
                    }
                }
            }

            return 0;
        }
    }
}
