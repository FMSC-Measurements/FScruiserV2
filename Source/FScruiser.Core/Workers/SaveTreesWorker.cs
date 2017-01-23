using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CruiseDAL;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public class SaveTreesWorker
    {
        Tree[] _trees;
        Thread _saveTreesWorkerThread;
        DAL _datastore;

        public SaveTreesWorker(DAL datastore, ICollection<Tree> trees)
        {
            Debug.Assert(datastore != null);
            Debug.Assert(trees != null);

            _datastore = datastore;
            lock (((System.Collections.ICollection)trees).SyncRoot)
            {
                //create a local copy of tree collection
                Tree[] a = new Tree[trees.Count];
                trees.CopyTo(a, 0);
                _trees = a;
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
                this._saveTreesWorkerThread = new Thread(this.TrySaveAll);
                this._saveTreesWorkerThread.IsBackground = true;
                this._saveTreesWorkerThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                this._saveTreesWorkerThread.Start();
            }
            catch
            {
                this._saveTreesWorkerThread = null;
            }
        }

        public void TrySaveAll()
        {
            bool success = true;

            foreach (Tree t in _trees)
            {
                success = t.TrySave() && success;
            }

            if (!success)
            {
                throw new FMSC.ORM.SQLException("not all trees were able to be saved", null);
            }
        }

        public void SaveAll()
        {
            lock (_datastore.TransactionSyncLock)
            {
                _datastore.BeginTransaction();
                try
                {
                    foreach (Tree tree in _trees)
                    {
                        tree.Save();
                    }
                    _datastore.CommitTransaction();
                }
                catch
                {
                    _datastore.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}