using System;

namespace FSCruiser.Core.Workers
{
    public class WorkerExceptionThrownEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public Exception Exception { get; set; }

        //public bool Retry { get; set; }
    }
}