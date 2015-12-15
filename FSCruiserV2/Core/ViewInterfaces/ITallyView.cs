using System;
using System.Collections.Generic;
using System.Text;
using FMSC.Sampling;
using CruiseDAL.DataObjects;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public delegate Control MakeCountTallyRowHadler(Control container, CountTreeVM count); 

    public interface ITallyView : IDataEntryPage
    {
        FormDataEntryLogic DataEntryController { get; }
        Dictionary<char, CountTreeVM> HotKeyLookup { get;}
        bool HotKeyEnabled { get; }


        bool HandleHotKeyFirst(char key);
        void MakeSGList(List<SampleGroupVM> list, Panel container);
        //SampleSelecter MakeSampleSelecter(CountTreeDO count, DataEntryMode mode);
        Control MakeTallyRow(Control container, CountTreeVM count);
        Control MakeTallyRow(Control container, SubPop subPop);
        void OnTally(CountTreeVM count);
        void HandleStratumLoaded(Control container);
        
    }
}
