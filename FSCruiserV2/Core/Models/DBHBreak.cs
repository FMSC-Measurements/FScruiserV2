using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class DBHBreak
    {
        public int NumberOfLogs { get; set; }
        public float DBHValue { get; set; }

        public DBHBreak(int numberOfLogs, float dbhValue)
        {
            this.NumberOfLogs = numberOfLogs;
            this.DBHValue = dbhValue;
        }
    }
}
