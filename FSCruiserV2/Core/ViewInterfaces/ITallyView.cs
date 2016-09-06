using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public delegate Control MakeCountTallyRowHadler(Control container, CountTreeVM count);

    public interface ITallyView : IDataEntryPage
    {
        FormDataEntryLogic DataEntryController { get; }

        bool HotKeyEnabled { get; }

        Dictionary<char, CountTreeVM> HotKeyLookup { get; }

        void HandleStratumLoaded(Control container);

        void MakeSGList(IEnumerable<SampleGroupModel> sampleGroups, Panel container);

        Control MakeTallyRow(Control container, CountTreeVM count);

        Control MakeTallyRow(Control container, SubPop subPop);

        void OnTally(CountTreeVM count);

        void SaveCounts();

        bool TrySaveCounts();
    }
}