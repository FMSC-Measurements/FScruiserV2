using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public class TreeValidationWorker
    {
        private Thread _validateTreesWorkerThread;
        readonly Tree[] _treesLocal;

        public TreeValidationWorker(ICollection<Tree> trees)
        {
            Debug.Assert(trees != null);
            lock (((System.Collections.ICollection)trees).SyncRoot)
            {
                Tree[] copy = new Tree[trees.Count];
                trees.CopyTo(copy, 0);
                this._treesLocal = copy;
            }
        }

        public void ValidateTreesAsync()
        {
            Debug.Assert(_validateTreesWorkerThread == null);

            if (this._validateTreesWorkerThread != null)
            {
                this._validateTreesWorkerThread.Abort();
            }
            this._validateTreesWorkerThread = new Thread(() => ValidateTrees());
            this._validateTreesWorkerThread.IsBackground = true;
            this._validateTreesWorkerThread.Priority = ThreadPriority.BelowNormal;
            this._validateTreesWorkerThread.Start();
        }

        public bool ValidateTrees()
        {
            bool valid = true;

            foreach (Tree tree in _treesLocal)
            {
                try
                {
                    valid = tree.ValidateVisableFields() && valid;
                }
                catch (Exception e)
                {
                    //TODO NBug
                    Debug.WriteLine(e);
                }
                //var visableFields = tree.Stratum.TreeFields;
                //if (visableFields != null)
                //{
                //    valid = tree.Validate(visableFields) && valid;
                //}
                //else
                //{
                //    valid = tree.Validate() && valid;
                //}
                //try
                //{
                //    tree.SaveErrors();
                //}
                //catch
                //{
                //
                //}
            }

            return valid;
        }
    }
}