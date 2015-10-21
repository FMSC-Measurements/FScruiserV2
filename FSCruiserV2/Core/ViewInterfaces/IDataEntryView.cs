using System;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public interface IDataEntryView
    {
        IDataEntryPage FocusedLayout { get; }
        List<IDataEntryPage> Layouts { get; }
        FormDataEntryLogic LogicController { get; }
        IList<StratumVM> PlotStrata { get; }

        //public bool HotKeyEnabled { get; }

        void HandleCuttingUnitDataLoaded();
        void HandleEnableLogGradingChanged();
        void HandleCruisersChanged();

        void GotoTreePage();
        void GoToTallyPage();
        void GoToPageIndex(int i);
        void TreeViewMoveLast();
    }
}
