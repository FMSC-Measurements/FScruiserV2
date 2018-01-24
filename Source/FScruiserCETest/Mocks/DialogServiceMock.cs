using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;

namespace FSCruiserV2.Test.Mocks
{
    public class DialogServiceMock : IDialogService
    {
        public int AskCruiserCallCnt = 0;
        public int ShowMessageCallCnt = 0;

        #region IDialogService Members

        public bool AskCancel(string message, string caption, bool defaultCancel)
        {
            return false;
        }

        public void AskCruiser(FSCruiser.Core.Models.Tree tree)
        {
            AskCruiserCallCnt++;
        }

        public bool AskYesNo(string message, string caption)
        {
            return AskYesNo(message, string.Empty, true);
        }

        public bool AskYesNo(string message, string caption, bool defaultNo)
        {
            return false;
        }

        public void ShowMessage(string message)
        {
            ShowMessage(message, string.Empty);
        }

        public void ShowMessage(string message, string caption)
        {
            ShowMessageCallCnt++;
        }

        public int? AskKPI(int min, int max)
        {
            return 1;
        }

        #endregion
    }
}
