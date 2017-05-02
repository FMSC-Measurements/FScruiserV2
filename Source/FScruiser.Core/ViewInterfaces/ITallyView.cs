using System.Collections.Generic;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using System;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface ITallyView : IDataEntryPage
    {
        bool HotKeyEnabled { get; }

        Dictionary<char, CountTree> HotKeyLookup { get; }

        void OnTally(CountTree count);

        Exception TrySaveCounts();
    }
}