using System.Collections.Generic;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryView : IView
    {
        CuttingUnit Unit { get; }

        FormDataEntryLogic LogicController { get; }

        IDataEntryPage FocusedLayout { get; }

        List<IDataEntryPage> Layouts { get; }

        bool AskEnterMeasureTreeData();//TODO remove unused method

        void HandleCuttingUnitDataLoaded();

        void HandleEnableLogGradingChanged();

        void HandleCruisersChanged();

        void GotoTreePage();

        void GoToTallyPage();

        void GoToPageIndex(int i);

        void TreeViewMoveLast();//TODO remove unused method
    }
}