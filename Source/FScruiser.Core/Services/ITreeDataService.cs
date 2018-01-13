using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FScruiser.Core.Services
{
    public interface ITreeDataService : ITreeFieldProvider
    {
        //IList<Tree> NonPlotTrees { get; }

        DAL DataStore { get; }

        int Region { get; }

        bool EnableLogGrading { get; set; }

        CuttingUnit CuttingUnit { get; }

        ICollection<Tree> NonPlotTrees { get; }

        IEnumerable<Stratum> TreeStrata { get; }

        IEnumerable<string> UnitLevelCruisersInitials { get; }

        IEnumerable<TreeDefaultValueDO> GetTreeDefaultValuesAll();

        Tree CreateNewTreeEntry(CountTree count);

        Tree CreateNewTreeEntry(CountTree count, bool isMeasure);

        Tree CreateNewTreeEntry(Stratum stratum
            , SampleGroup sg
            , TreeDefaultValueDO tdv
            , bool isMeasure);


        void AddNonPlotTree(Tree tree);

        void DeleteTree(Tree tree);

        Tree UserAddTree();

        bool IsTreeNumberAvalible(long treeNumber);

        TreeEstimateDO LogTreeEstimate(CountTree count, int kpi);

        void SaveTrees();

        bool TrySaveTrees();

        void TrySaveTreesAsync();
    }
}
