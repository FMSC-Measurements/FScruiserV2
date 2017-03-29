using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public class SoundService 
    {
        public static ISoundService Instance { get; set; }

        public static void SignalMeasureTree()
        {
            Instance.SignalMeasureTree();
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

        public static void SignalTally(bool force)
        {
            Instance.SignalTally(force);
        }

        public static void SignalPageChanged()
        {
            Instance.SignalPageChanged();
        }

        public static void SignalPageChanged(bool force)
        {
            Instance.SignalPageChanged(force);
        }

    }
}
