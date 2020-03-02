using System;
using System.Collections.Generic;
using System.Linq;
using CruiseDAL;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.Workers
{
    public class FileLoadWorker : Worker, IDisposable
    {
        public FileLoadWorker(string path, ApplicationController appController)
        {
            this.Path = path;
        }

        public ApplicationController AppController { get; set; }

        public String Path { get; protected set; }

        public DAL DataStore { get; set; }

        public IList<CuttingUnit> CuttingUnits { get; set; }

        protected override void WorkerMain()
        {
            this.DataStore = new DAL(Path);
            DataStore.LogMessage(string.Format("Opened By FSCruiser ({0})", FSCruiser.Core.Constants.FSCRUISER_VERSION), "I");
            this.UnitsOfWorkCompleated = 1;
            this.NotifyProgressChanged(null);
        }

        protected override void OnExceptionThrown(WorkerExceptionThrownEventArgs e)
        {
            if (this.DataStore != null)
            {
                this.DataStore.Dispose();
            }
            this.DataStore = null;

            base.OnExceptionThrown(e);
        }

        protected override void OnStarting(WorkerProgressChangedEventArgs e)
        {
            this.UnitsOfWorkExpected = 2;

            base.OnStarting(e);
        }

        #region IDisposable Members

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (DataStore != null)
                {
                    DataStore.Dispose();
                    DataStore = null;
                }
            }
        }

        #endregion IDisposable Members
    }
}