using System.Collections.Generic;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ITreeContaningModel
    {
        //ITreeContaningModel Parent { get; set; }
        //IList<ITreeContaningModel> Children { get; }

        IList<Tree> Trees { get; }

        //TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, IViewController viewController)

        Tree CreateNewTreeEntry(CountTree count);

        Tree CreateNewTreeEntry(Stratum stratum, SampleGroup sg, TreeDefaultValueDO tdv, bool isMeasure);

        long GetNextTreeNumber();

        bool IsTreeNumberAvalible(long treeNumber);

        void DeleteTree(Tree tree);

        bool ValidateTrees();

        void ValidateTreesAsync();

        void SaveTrees();

        void TrySaveTrees();

        void TrySaveTreesAsync();
    }
}