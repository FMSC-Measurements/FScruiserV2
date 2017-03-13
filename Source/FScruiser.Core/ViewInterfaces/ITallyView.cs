using System.Collections.Generic;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface ITallyView : IDataEntryPage
    {
        //FormDataEntryLogic DataEntryController { get; }

        bool HotKeyEnabled { get; }

        Dictionary<char, CountTree> HotKeyLookup { get; }

        void OnTally(CountTree count);

        void SaveCounts();

        bool TrySaveCounts();
    }
}