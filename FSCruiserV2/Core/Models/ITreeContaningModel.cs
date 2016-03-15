using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ITreeContaningModel
    {
        //ITreeContaningModel Parent { get; set; }
        //IList<ITreeContaningModel> Children { get; }

        IList<TreeVM> Trees { get; }

        //TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, IViewController viewController)

        TreeVM CreateNewTreeEntry(CountTreeVM count);

        TreeVM CreateNewTreeEntry(StratumVM stratum, SampleGroupVM sg, TreeDefaultValueDO tdv, bool isMeasure);

        long GetNextTreeNumber();

        bool IsTreeNumberAvalible(long treeNumber);

        void DeleteTree(TreeVM tree);

        bool ValidateTrees();

        void ValidateTreesAsync();

        void SaveTrees();

        void TrySaveTrees();

        void TrySaveTreesAsync();

    }
}
