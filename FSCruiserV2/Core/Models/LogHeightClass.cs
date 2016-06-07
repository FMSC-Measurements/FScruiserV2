using System;
using System.Collections.Generic;

namespace FSCruiser.Core.Models
{
    public class LogHeightClass
    {
        public class HeightRange : IComparable<HeightRange>
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

            #region IComparable<HeightRange> Members

            public int CompareTo(HeightRange other)
            {
                return From.CompareTo(other.From);
            }

            #endregion IComparable<HeightRange> Members
        }

        double _num16FtLogs = 0;

        public HeightRange Range { get; set; }

        public List<uint> Breaks { get; set; }

        public LogHeightClass(float rangeFrom, float rangeTo, float num16FtLogs)
        {
            this.Range = new HeightRange(rangeFrom, rangeTo);
            this._num16FtLogs = num16FtLogs;
        }

        public LogHeightClass WithBreaks(params uint[] breaks)
        {
            this.Breaks = new List<uint>(breaks);

            return this;
        }

        public double GetDefaultLogCount(float dbh)
        {
            var logCount = this._num16FtLogs;

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