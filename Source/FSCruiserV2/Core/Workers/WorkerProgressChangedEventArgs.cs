using System;

namespace FSCruiser.Core.Workers
{
    public class WorkerProgressChangedEventArgs : EventArgs
    {
        public WorkerProgressChangedEventArgs()
            : this(0)
        { }

        public WorkerProgressChangedEventArgs(int progress)
        {
            Progress = progress;
        }

        public int Progress { get; protected set; }

        public string Message { get; set; }
    }
}