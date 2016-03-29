using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core.Workers
{
    public class WorkerExceptionThrownEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public Exception Exception { get; set; }

        //public bool Retry { get; set; }
    }
}
