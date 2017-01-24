using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public delegate Control MakeCountTallyRowHadler(Control container, CountTree count);

    public interface ITallyView : IDataEntryPage
    {
        FormDataEntryLogic DataEntryController { get; }

        bool HotKeyEnabled { get; }

        Dictionary<char, CountTree> HotKeyLookup { get; }

        void HandleStratumLoaded(Control container);

        void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container);

        Control MakeTallyRow(Control container, CountTree count);

        Control MakeTallyRow(Control container, SubPop subPop);

        void OnTally(CountTree count);

        void SaveCounts();

        bool TrySaveCounts();
    }
}