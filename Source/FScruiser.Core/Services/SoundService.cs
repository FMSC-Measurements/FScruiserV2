using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public class SoundService
    {
        public static ISoundService Instance { get; set; }

        public static void SignalMeasureTree(bool showMessage)
        {
            Instance.SignalMeasureTree(showMessage);
        }

        public static void SignalInvalidAction()
        {
            Instance.SignalInvalidAction();
        }

        public static void SignalInsuranceTree()
        {
            Instance.SignalInsuranceTree();
        }

        public static void SignalTally()
        {
            Instance.SignalTally();
        }

        public static void SignalPageChanged()
        {
            Instance.SignalPageChanged();
        }

    }
}
