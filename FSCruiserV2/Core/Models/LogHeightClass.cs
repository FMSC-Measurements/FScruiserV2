using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class LogHeightClass
    {
        public class HeightRange
        {
            public HeightRange(float from, float to)
            {
                From = from;
                To = to;
            }

            public float From = float.MinValue;
            public float To = float.MaxValue;

            public bool IsInRange(float value)
            {
                return value.GreaterThanOrEqualsEx(From) 
                    && value.LessThanOrEqualsEx(To);
            }
        }

        uint _numLogs = 0;

        public HeightRange Range { get; set; }

        public List<uint> Breaks { get; set; }

        public LogHeightClass(float rangeFrom, float rangeTo, uint logs)
        {
            this.Range = new HeightRange(rangeFrom, rangeTo);
            this._numLogs = logs;
        }

        public LogHeightClass WithBreaks(params uint[] breaks)
        {
            this.Breaks = new List<uint>(breaks);

            return this;
        }

        public uint GetDefaultLogCount(float dbh)
        {
            uint logCount = this._numLogs;

            if (Breaks != null)
            {
                foreach (uint brk in Breaks)
                {
                    if (dbh < brk) { break; }
                    else { logCount++; } //for each break where dbh >= break, increment logCount
                }
            }

            return logCount;
        }
    }
}
