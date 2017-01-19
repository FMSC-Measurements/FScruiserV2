using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.Models;

namespace FScruiser.Core.Services
{
    public class DialogService
    {
        protected static IDialogService _instance;

        public static IDialogService Instance 
        {
            get { return _instance; }
            protected set { _instance = value; }
        }

        public static bool AskCancel(String message, String caption, bool defaultCancel)
        {
            return Instance.AskCancel(message, caption, defaultCancel);
        }

        public static void AskCruiser(Tree tree)
        {
            Instance.AskCruiser(tree);
        }

        public static bool AskYesNo(String message, String caption)
        {
            return Instance.AskYesNo(message, caption);
        }

        public static bool AskYesNo(string message, String caption, bool defaultNo)
        {
            return Instance.AskYesNo(message, caption, defaultNo);
        }

        public static void ShowMessage(string message)
        {
            Instance.ShowMessage(message);
        }

        public static void ShowMessage(string message, string caption)
        {
            Instance.ShowMessage(message, caption);
        }

    }
}
