using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;

namespace FSCruiserV2.Test.Mocks
{
    public class SoundServiceMock : ISoundService
    {
        #region ISoundService Members

        public void SignalMeasureTree(bool showMessage)
        {
            throw new NotImplementedException();
        }

        public void SignalInvalidAction()
        {
            throw new NotImplementedException();
        }

        public void SignalInsuranceTree()
        {
            throw new NotImplementedException();
        }

        public void SignalTally()
        {
            throw new NotImplementedException();
        }

        public void SignalPageChanged()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
