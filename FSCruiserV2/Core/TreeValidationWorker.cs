using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public class TreeValidationWorker
    {
        private Thread _validateTreesWorkerThread;
        readonly TreeVM[] _treesLocal;

        public TreeValidationWorker(ICollection<TreeVM> trees)
        {
            Debug.Assert(trees != null);
            lock (((System.Collections.ICollection)trees).SyncRoot)
            {
                TreeVM[] copy = new TreeVM[trees.Count];
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

            foreach (TreeVM tree in _treesLocal)
            {
                var visableFields = tree.Stratum.TreeFields;
                if (visableFields != null)
                {
                    valid = tree.Validate(visableFields) && valid;
                }
                else
                {
                    valid = tree.Validate() && valid;
                }
                try
                {
                    tree.SaveErrors();
                }
                catch
                {
                    valid = false;
                    //TODO should we do something if tree error unable to save
                }
            }
            return valid;
        }
    }
}