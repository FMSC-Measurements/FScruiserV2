using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Forms;

namespace FSCruiserV2.Test.Mocks
{
    public class DataEntryViewMock : IDataEntryView
    {
        #region IDataEntryView Members

        public IDataEntryPage FocusedLayout
        {
            get { throw new NotImplementedException(); }
        }

        public List<IDataEntryPage> Layouts
        {
            get { throw new NotImplementedException(); }
        }

        public FSCruiserV2.Logic.FormDataEntryLogic LogicController
        {
            get { throw new NotImplementedException(); }
        }

        public IList<FSCruiserV2.Logic.StratumVM> PlotStrata
        {
            get { throw new NotImplementedException(); }
        }

        public void HandleCuttingUnitDataLoaded()
        {
            throw new NotImplementedException();
        }

        public void HandleEnableLogGradingChanged()
        {
            throw new NotImplementedException();
        }

        public void HandleCruisersChanged()
        {
            throw new NotImplementedException();
        }

        public void GotoTreePage()
        {
            throw new NotImplementedException();
        }

        public void GoToTallyPage()
        {
            throw new NotImplementedException();
        }

        public void GoToPageIndex(int i)
        {
            throw new NotImplementedException();
        }

        public void TreeViewMoveLast()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
