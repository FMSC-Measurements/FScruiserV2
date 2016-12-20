using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.Core.Workers
{
    public class SaveTreeWorker2 : Worker
    {
        public ITreeView TreeView { get; set; }

        protected override void WorkerMain()
        {
            var trees = TreeView.Trees.ToArray();
            UnitsOfWorkExpected = trees.Length;
            SaveAllTrees(trees);
        }

        protected void SaveAllTrees(IEnumerable<Tree> trees)
        {
            foreach (var tree in trees)
            {
                try
                {
                    tree.Save();
                    UnitsOfWorkCompleated = UnitsOfWorkCompleated + 1;
                    NotifyProgressChanged(String.Empty);
                }
                catch (Exception e)
                {
                    if (!NotifyExceptionThrown(e))
                    {
                        throw;
                    }
                }
            }
        }
    }
}