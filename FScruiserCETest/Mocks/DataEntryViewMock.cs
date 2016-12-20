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

        public FSCruiser.Core.Models.CuttingUnit Unit
        {
            get { throw new NotImplementedException(); }
        }

        public FSCruiser.Core.DataEntry.FormDataEntryLogic LogicController
        {
            get { throw new NotImplementedException(); }
        }

        public IDataEntryPage FocusedLayout
        {
            get { throw new NotImplementedException(); }
        }

        public List<IDataEntryPage> Layouts
        {
            get { throw new NotImplementedException(); }
        }

        public bool AskEnterMeasureTreeData()
        {
            throw new NotImplementedException();
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

        #endregion IDataEntryView Members
    }
}