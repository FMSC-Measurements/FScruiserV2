using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    [Flags]
    public enum DataEntryMode { None = 0, Tree = 1, Plot = 2, OneStagePlot = 4, ThreeP = 8, HundredPct = 16, Mixed = 32, Unknown = 64, TallyTree = 128 };
}
