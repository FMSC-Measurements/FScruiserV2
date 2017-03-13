using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public interface ISoundService : IDisposable
    {
        void SignalMeasureTree();

        void SignalInsuranceTree();

        void SignalTally(bool force);

        void SignalPageChanged(bool force);

        void SignalInvalidAction();

    }

    public static class SoundServiceExtentions
    {
        public static void SignalTally(this ISoundService srvc)
        {
            srvc.SignalTally(false);
        }

        public static void SignalPageChanged(this ISoundService srvc)
        {
            srvc.SignalPageChanged(false);
        }
    }
    
}
