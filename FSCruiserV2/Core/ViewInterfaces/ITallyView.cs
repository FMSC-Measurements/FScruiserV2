using System;
using System.Collections.Generic;
using System.Text;
using FMSC.Sampling;
using CruiseDAL.DataObjects;
using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public delegate Control MakeCountTallyRowHadler(Control container, CountTreeVM count); 

    public interface ITallyView : IDataEntryPage
    {
        FormDataEntryLogic DataEntryController { get; }
        Dictionary<char, CountTreeVM> HotKeyLookup { get;}
        bool HotKeyEnabled { get; }


        //IList<StratumInfo> Strata { get; }
        //StratumInfo SelectedStratum { get; }

        bool HandleHotKeyFirst(char key);
        //bool HandleKeyDown(char key);
        //bool HandleKeyUp(char key);
        void MakeSGList(List<SampleGroupVM> list, Panel container);
        //SampleSelecter MakeSampleSelecter(CountTreeDO count, DataEntryMode mode);
        Control MakeTallyRow(Control container, CountTreeVM count);
        Control MakeTallyRow(Control container, SubPop subPop);
        void OnTally(CountTreeVM count);
        void HandleStratumLoaded(Control container);
        
    }
}
