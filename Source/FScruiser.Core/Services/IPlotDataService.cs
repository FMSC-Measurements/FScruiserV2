﻿using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;

namespace FScruiser.Core.Services
{
    public interface IPlotDataService
    {
        DAL DataStore { get; }

        int Region { get; }

        CuttingUnit CuttingUnit { get; }

        bool EnableLogGrading { get; set; }

        IEnumerable<PlotStratum> PlotStrata { get; }

        IEnumerable<string> UnitLevelCruisersInitials { get; }

        IEnumerable<TreeDefaultValueDO> GetTreeDefaultValuesAll();

        long GetNextPlotTreeNumber(long plotNumber);

        bool CrossStrataIsTreeNumberAvalible(Plot plot, long treeNumber);

        Tree UserAddTree(Plot plot, Tree templateTree);

        Tree CreateNewTreeEntry(Plot plot, SubPop subPop);

        Tree CreateNewTreeEntry(Plot plot, CountTree count, bool isMeasure);

        Tree CreateNewTreeEntry(Plot plot, SampleGroup sg, TreeDefaultValueDO tdv, bool isMeasure);

        void SaveTrees(Plot plot);

        bool TrySaveTrees(Plot plot);

        //void TrySaveTreesAsync(Plot plot);

        Exception SavePlotData();

    }
}
