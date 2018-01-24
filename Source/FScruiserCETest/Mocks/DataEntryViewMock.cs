using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiserV2.Test.Mocks
{
    public class DataEntryViewMock : IDataEntryView
    {

        #region IDataEntryView Members

        public IDataEntryPage FocusedLayout
        {
            get;
            set;
        }

        public List<IDataEntryPage> Layouts
        {
            get;
            set;
        }

        public void GotoTreePage()
        {
            return;
        }

        public void GoToTallyPage()
        {
            return;
        }

        public void GoToPage(IDataEntryPage page)
        {
            return;
        }

        #endregion
    }
}