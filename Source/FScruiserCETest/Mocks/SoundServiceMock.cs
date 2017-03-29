using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;

namespace FSCruiserV2.Test.Mocks
{
    public class SoundServiceMock : ISoundService
    {
        public int SignalMeasureTreeCallCnt = 0;
        public int SignalInvalidActionCallCnt = 0;
        public int SignalInsuranceTreeCallCnt = 0;
        public int SignalTallyCallCnt = 0;
        public int SignalPageChangedCallCnt = 0;
        public int DisposedCallCnt = 0;

        #region ISoundService Members

        public void SignalMeasureTree()
        {
            SignalMeasureTreeCallCnt++;
        }

        public void SignalInvalidAction()
        {
            SignalInvalidActionCallCnt++;
        }

        public void SignalInsuranceTree()
        {
            SignalInsuranceTreeCallCnt++;
        }

        public void SignalTally()
        {
            SignalTally(false);
        }

        public void SignalPageChanged()
        {
            SignalPageChanged(false);
        }

        public void SignalTally(bool force)
        {
            SignalTallyCallCnt++;
        }

        public void SignalPageChanged(bool force)
        {
            SignalPageChangedCallCnt++;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            DisposedCallCnt++;
        }

        #endregion
    }
}
