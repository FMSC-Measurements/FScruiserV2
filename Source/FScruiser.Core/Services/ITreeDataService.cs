using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public interface ITreeDataService
    {
        //IList<Tree> NonPlotTrees { get; }
        CuttingUnit CuttingUnit { get; }

        ICollection<Tree> NonPlotTrees { get; }

        Tree CreateNewTreeEntry(CountTree count);

        Tree CreateNewTreeEntry(CountTree count, bool isMeasure);

        Tree CreateNewTreeEntry(Stratum stratum
            , SampleGroup sg
            , TreeDefaultValueDO tdv
            , bool isMeasure);


        void AddNonPlotTree(Tree tree);

        void DeleteTree(Tree tree);

        bool IsTreeNumberAvalible(long treeNumber);

        TreeEstimateDO LogTreeEstimate(CountTree count, int kpi);

        void SaveTrees();

        bool TrySaveTrees();

        void TrySaveTreesAsync();
    }
}
