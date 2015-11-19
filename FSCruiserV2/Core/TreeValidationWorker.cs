using System;

using System.Collections.Generic;
using System.Text;
using FSCruiser.Core.Models;
using System.Threading;

namespace FSCruiser.Core
{
    public class TreeValidationWorker
    {
        private Thread _validateTreesWorkerThread;


        private void ValidateTreesAsync(ICollection<TreeVM> list)
        {
            if (this._validateTreesWorkerThread != null)
            {
                this._validateTreesWorkerThread.Abort();
            }
            this._validateTreesWorkerThread = new Thread(this.InternalValidateTrees);
            this._validateTreesWorkerThread.IsBackground = true;
            this._validateTreesWorkerThread.Priority = ThreadPriority.BelowNormal;
            this._validateTreesWorkerThread.Start();
        }

        public bool ValidateTrees(ICollection<TreeVM> list)
        {
            this.ViewController.ShowWait();
            try
            {
                //this._cDal.BeginTransaction();//TODO encapsulate in trasaction once trasaction tracking is done
                bool valid = InternalValidateTrees(list);
                return valid;
            }
            finally
            {
                //this._cDal.EndTransaction();
                this.ViewController.HideWait();
            }

        }

        private static bool InternalValidateTrees(ICollection<TreeVM> list)
        {
            bool valid = true;
            TreeVM[] a = new TreeVM[list.Count];
            list.CopyTo(a, 0);
            foreach (TreeVM tree in a)
            {
                if (tree.Stratum != null && tree.Stratum.TreeFieldNames != null)
                {
                    valid = tree.Validate(tree.Stratum.TreeFieldNames) && valid;
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
