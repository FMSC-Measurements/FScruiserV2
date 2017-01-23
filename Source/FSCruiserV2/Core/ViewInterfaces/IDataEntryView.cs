using System.Collections.Generic;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryView
    {
        CuttingUnit Unit { get; }

        FormDataEntryLogic LogicController { get; }

        IDataEntryPage FocusedLayout { get; }

        List<IDataEntryPage> Layouts { get; }


        void HandleCuttingUnitDataLoaded();

        //void HandleEnableLogGradingChanged();

        void GotoTreePage();

        void GoToTallyPage();

        void GoToPageIndex(int i);
    }
}