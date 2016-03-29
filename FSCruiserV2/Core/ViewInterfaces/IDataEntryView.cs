using System;
using System.Collections.Generic;
using System.Text;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryView : IView
    {
        IDataEntryPage FocusedLayout { get; }
        List<IDataEntryPage> Layouts { get; }
        FormDataEntryLogic LogicController { get; }
        //IList<StratumVM> PlotStrata { get; }

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
