using System;
using System.Threading;

namespace FSCruiser.Core.Workers
{
    public abstract class Worker : IDisposable
    {
        static int runCount = 0; //for diagnostics

        bool _isDone;
        bool _isCanceled;

        protected Thread _thread;
        protected int _defaultTimeout = Timeout.Infinite;
        object _threadLock = new object();

        #region events

        public event EventHandler<WorkerExceptionThrownEventArgs> ExceptionThrown;

        public event EventHandler<WorkerProgressChangedEventArgs> ProgressChanged;

        public event EventHandler<WorkerProgressChangedEventArgs> Ended;

        public event EventHandler<WorkerProgressChangedEventArgs> Starting;

        #endregion events

        protected int UnitsOfWorkExpected { get; set; }

        protected int UnitsOfWorkCompleated { get; set; }

        public virtual string Name { get; protected set; }

        public bool IsDone
        {
            get
            {
                lock (_threadLock)
                {
                    return _isDone;
                }
            }
            protected set
            {
                lock (_threadLock)
                {
                    _isDone = value;
                }
            }
        }

        public virtual bool IsWorking
        {
            get
            {
                if (_thread == null) { return false; }
#if NetCF
                try
                {
                    return !_thread.Join(0);
                }
                catch (ThreadStateException) { return false; }
#else
                return (_thread.ThreadState & ThreadState.Running) == ThreadState.Running;
#endif
            }
        }

        public bool IsCanceled
        {
            get
            {
                lock (_threadLock)
                {
                    return _isCanceled;
                }
            }
            protected set
            {
                lock (_threadLock)
                {
                    _isCanceled = value;
                }
            }
        }

        public object ThreadLock { get { return _threadLock; } }

        public void Start()
        {
            if (IsWorking)
            {
                throw new InvalidOperationException("Cancel or wait for current job to finish before starting again");
            }

            if (_thread == null)
            {
                ThreadStart ts = new ThreadStart(this.Run);
                this._thread = new Thread(ts)
                {
                    IsBackground = true
                };
            }

            System.Threading.Interlocked.Increment(ref Worker.runCount);
            _thread.Name = this.Name + Worker.runCount.ToString();
            this._thread.Start();
        }

        public void Cancel()
        {
            this.IsCanceled = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>true if worker ended successfuly</returns>
        public bool Wait(int millSecTimeout)
        {
            if (this._thread != null)
            {
                return this._thread.Join(millSecTimeout);
            }
            return true;
        }

        public bool Wait()
        {
            return this.Wait(_defaultTimeout);
        }

        public void Kill()
        {
            if (this._thread != null)
            {
                this._thread.Abort();
            }
        }

        protected bool NotifyExceptionThrown(Exception ex)
        {
            var arg = new WorkerExceptionThrownEventArgs()
            {
                Exception = ex
            };

            OnExceptionThrown(arg);
            return arg.Handled;
        }

        protected void NotifyProgressChanged(string message)
        {
            var percentDone = CalcPercentDone(UnitsOfWorkExpected, UnitsOfWorkCompleated);

            var eArg = new WorkerProgressChangedEventArgs(percentDone)
            {
                Message = message
            };
            this.OnProgressChanged(eArg);
        }

        protected void NotifyWorkStarting()
        {
            var eArg = new WorkerProgressChangedEventArgs(0)
            {
                Message = "Start " + Name
            };
            OnProgressChanged(eArg);
            OnStarting(eArg);
        }

        protected void NotifyWorkEnded(string message)
        {
            var percentDone = CalcPercentDone(UnitsOfWorkExpected, UnitsOfWorkCompleated);

            var eArg = new WorkerProgressChangedEventArgs(percentDone)
            {
                Message = message
            };
            OnProgressChanged(eArg);
            OnEnded(eArg);
        }

        #region virtual methods

        protected virtual void Run()
        {
            try
            {
                NotifyWorkStarting();
                WorkerMain();
                IsDone = true;
                NotifyWorkEnded("Done");
            }
            catch (ThreadAbortException)
            {
                NotifyWorkEnded("Aborted");
            }
            catch (CancelWorkerException)
            {
                NotifyWorkEnded("Canceled");
            }
            catch (Exception e)
            {
                if (!NotifyExceptionThrown(e))
                {
                    throw;
                }
            }
        }

        protected abstract void WorkerMain();

        protected virtual void OnExceptionThrown(WorkerExceptionThrownEventArgs e)
        {
            if (ExceptionThrown != null)
            {
                ExceptionThrown(this, e);
            }
        }

        protected virtual void OnProgressChanged(WorkerProgressChangedEventArgs e)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, e);
            }
        }

        protected virtual void OnEnded(WorkerProgressChangedEventArgs e)
        {
            if (this.Ended != null)
            {
                this.Ended(this, e);
            }
        }

        protected virtual void OnStarting(WorkerProgressChangedEventArgs e)
        {
            this.IsCanceled = false;
            this.IsDone = false;
            this.UnitsOfWorkCompleated = 0;

            if (this.Starting != null)
            {
                this.Starting(this, e);
            }
        }

        #endregion virtual methods

        protected void CheckCanceled()
        {
            if (this.IsCanceled)
            {
                throw new CancelWorkerException();
            }
        }

        private static int CalcPercentDone(int workExpected, int workDone)
        {
            if (workDone <= 0) { return 0; }
            if (workExpected <= 0) { return 0; }

            float frac = (float)workDone / workExpected;
            return (int)(100 * frac);
        }

        #region IDisposable Members

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsWorking)
            {
                this.Cancel();
                if (this.Wait(1000))
                {
                    this.Kill();
                }
            }
            if (disposing)
            {
                ExceptionThrown = null;
                ProgressChanged = null;
                Ended = null;
                Starting = null;
            }
            Disposed = true;
        }

        #endregion IDisposable Members
    }
}