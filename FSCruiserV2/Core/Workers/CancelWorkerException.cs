using System;

namespace FSCruiser.Core.Workers
{
    public class CancelWorkerException : Exception
    {
        public CancelWorkerException() : base("Worker Canceled") { }

        public CancelWorkerException(Exception innerException)
            : base("Worker Canceled", innerException)
        { }
    }
}