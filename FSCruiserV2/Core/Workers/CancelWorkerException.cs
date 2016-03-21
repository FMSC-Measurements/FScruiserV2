using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
