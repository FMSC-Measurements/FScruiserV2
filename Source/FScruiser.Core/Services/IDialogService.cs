﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.Models;

namespace FScruiser.Core.Services
{
    public interface IDialogService
    {
        bool AskCancel(String message, String caption, bool defaultCancel);

        void AskCruiser(Tree tree);

        int? AskKPI(int min, int max);

        bool AskYesNo(String message, String caption);

        bool AskYesNo(string message, String caption, bool defaultNo);

        void ShowMessage(string message);

        void ShowMessage(string message, string caption);
    }
}