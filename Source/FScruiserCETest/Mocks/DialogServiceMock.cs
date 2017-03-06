using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;

namespace FSCruiserV2.Test.Mocks
{
    public class DialogServiceMock : IDialogService
    {
        #region IDialogService Members

        public bool AskCancel(string message, string caption, bool defaultCancel)
        {
            throw new NotImplementedException();
        }

        public void AskCruiser(FSCruiser.Core.Models.Tree tree)
        {
            throw new NotImplementedException();
        }

        public bool AskYesNo(string message, string caption)
        {
            throw new NotImplementedException();
        }

        public bool AskYesNo(string message, string caption, bool defaultNo)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, string caption)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
