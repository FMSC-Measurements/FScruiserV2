using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class LogHeightInfo
    {
        public float RangeFrom { get; set; }
        public float RangeTo { get; set; }
        public List<DBHBreak> DBHBreaks { get; private set; }

        public LogHeightInfo(float rangeFrom, float rangeTo)
        {
            this.RangeFrom = rangeFrom;
            this.RangeTo = rangeTo;
            DBHBreaks = new List<DBHBreak>();
        }

        public void AddBreak(DBHBreak dbhBreak)
        {
            DBHBreaks.Add(dbhBreak);
        }

        public bool IsInRange(float height)
        {
            return height >= RangeFrom && height < RangeTo;
        }
    }
}
