using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public interface ISoundService : IDisposable
    {
        void SignalMeasureTree(bool showMessage);

        void SignalInvalidAction();

        void SignalInsuranceTree();

        void SignalTally();

        void SignalPageChanged();

    }
}
