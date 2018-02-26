using FMSC.ORM.SQLite;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FSCruiser.Core
{
    public class SaveTreesWorker : IDisposable
    {
        private IEnumerable<Tree> _treesLocal;
        private Thread _saveTreesWorkerThread;
        private SQLiteDatastore _datastore;

        public SaveTreesWorker(SQLiteDatastore datastore, IEnumerable<Tree> trees)
        {
            if (datastore == null) { throw new ArgumentNullException("datastore"); }
            if (trees == null) { throw new ArgumentNullException("trees"); }

            _datastore = datastore;
            lock (((System.Collections.ICollection)trees).SyncRoot)
            {
                //create a local copy of tree collection
                _treesLocal = trees.ToArray();
            }
        }

        //private void SaveTreesAsync()
        //{
        //    if (this._saveTreesWorkerThread != null)
        //    {
        //        this._saveTreesWorkerThread.Abort();
        //    }
        //    try
        //    {
        //        this._saveTreesWorkerThread = new Thread(this.SaveTrees);
        //        this._saveTreesWorkerThread.IsBackground = true;
        //        this._saveTreesWorkerThread.Priority = Constants.SAVE_TREES_THREAD_PRIORISTY;
        //        this._saveTreesWorkerThread.Start();
        //    }
        //    catch
        //    {
        //        this._saveTreesWorkerThread = null;
        //    }
        //}

        public void TrySaveAllAsync()
        {
            if (this._saveTreesWorkerThread != null)
            {
                this._saveTreesWorkerThread.Abort();
            }
            try
            {
                this._saveTreesWorkerThread = new Thread(() => TrySaveAll());
                this._saveTreesWorkerThread.IsBackground = true;
                this._saveTreesWorkerThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                this._saveTreesWorkerThread.Start();
            }
            catch
            {
                this._saveTreesWorkerThread = null;
            }
        }

        public bool TrySaveAll()
        {
            bool success = true;

            foreach (Tree t in _treesLocal)
            {
                success = t.TrySave() && success;
            }

            return success;
        }

        public void SaveAll()
        {
            using (var connection = _datastore.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var tree in _treesLocal)
                        {
                            _datastore.Save(connection, tree, transaction);
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._saveTreesWorkerThread = null;
            }
        }
    }
}