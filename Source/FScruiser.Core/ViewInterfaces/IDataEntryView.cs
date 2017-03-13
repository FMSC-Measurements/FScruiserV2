using System.Collections.Generic;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IDataEntryView
    {
        //FormDataEntryLogic LogicController { get; }

        IDataEntryPage FocusedLayout { get; }

        List<IDataEntryPage> Layouts { get; }

        void GotoTreePage();

        void GoToTallyPage();

        void GoToPageIndex(int i);
    }
}