using System;
using System.Collections.Generic;

namespace FSCruiser.Core.Models
{
    public class LogHeightClass
    {
        public double Num16FtLogs { get; set; }
        public float From { get; set; }
        public float To { get; set; }

        List<uint> _breaks;

        public List<uint> Breaks
        {
            get { return _breaks; }
            set
            {
                if (value != null)
                {
                    value.Sort();
                }
                _breaks = value;
            }
        }

        public LogHeightClass()
        {
        }

        public LogHeightClass(float rangeFrom, float rangeTo, float num16FtLogs)
        {
            From = rangeFrom;
            To = rangeTo;
            Num16FtLogs = num16FtLogs;
        }

        public LogHeightClass WithBreaks(params uint[] breaks)
        {
            Breaks = new List<uint>(breaks);
            return this;
        }

        public bool IsInRange(float value)
        {
            return value.GreaterThanOrEqualsEx(From)
                    && value.LessThanOrEqualsEx(To);
        }

        public double GetDefaultLogCount(float dbh)
        {
            var logCount = Num16FtLogs;

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