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

        public IList<CuttingUnitVM> CuttingUnits { get; set; }

        protected override void WorkerMain()
        {
            this.DataStore = new DAL(Path);
            DataStore.LogMessage(string.Format("Opened By FSCruiser ({0})", Constants.FSCRUISER_VERSION), "I");
            this.UnitsOfWorkCompleated = 1;
            this.NotifyProgressChanged(null);

            this.CuttingUnits = DataStore.From<CuttingUnitVM>().Read().ToList();
            UnitsOfWorkCompleated = 2;
        }

        protected override void OnExceptionThrown(WorkerExceptionThrownEventArgs e)
        {
            if (this.DataStore != null)
            {
                this.DataStore.Dispose();
            }
            this.DataStore = null;
            this.CuttingUnits = null;

            base.OnExceptionThrown(e);
        }

        protected override void OnStarting(WorkerProgressChangedEventArgs e)
        {
            this.UnitsOfWorkExpected = 2;

            base.OnStarting(e);
        }

        #region IDisposable Members

        bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
                CuttingUnits = null;
            }
        }

        #endregion IDisposable Members
    }
}