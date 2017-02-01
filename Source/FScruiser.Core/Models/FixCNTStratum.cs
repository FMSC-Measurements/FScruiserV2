﻿using System.Collections.Generic;
using System.Linq;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulationProvider
    {
        IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations();
    }

    public class FixCNTStratum : PlotStratum, IFixCNTTallyPopulationProvider
    {
        IFixCNTTallyClass _tallyClass;

        IFixCNTTallyClass TallyClass
        {
            get
            {
                if (_tallyClass == null)
                {
                    _tallyClass = DAL.From<FixCNTTallyClass>()
                        .Where("Stratum_CN = ?")
                        .Query(Stratum_CN).FirstOrDefault();
                }
                return _tallyClass;
            }
        }

        public override Plot MakePlot(CuttingUnit cuttingUnit)
        {
            return new FixCNTPlot(this.DAL)
            {
                CuttingUnit = cuttingUnit,
                Stratum = this,
                PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value)
            };
        }

        public IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations()
        {
            var tallyClass = TallyClass;

            var tallyPopulations = DAL.From<FixCNTTallyPopulation>()
                .Where("FixCNTTallyClass_CN = ?")
                .Query(tallyClass.FixCNTTallyClass_CN);

            foreach (var tallyPop in tallyPopulations)
            {
                tallyPop.SampleGroup = DAL.From<SampleGroup>()
                    .Where("SampleGroup_CN = ?")
                    .Read(tallyPop.SampleGroup_CN).FirstOrDefault();

                tallyPop.TreeDefaultValue = DAL.From<TreeDefaultValueDO>()
                    .Where("TreeDefaultValue_CN = ?")
                    .Read(tallyPop.TreeDefaultValue_CN).FirstOrDefault();

                tallyPop.TallyClass = tallyClass;
                yield return tallyPop;
            }
        }

        protected override IEnumerable<Plot> ReadPlots(long cuttingUnit_CN)
        {
            foreach (var plot in this.DAL.From<FixCNTPlot>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Query(this.Stratum_CN, cuttingUnit_CN))
            {
                plot.Stratum = this;
                yield return plot;
            }
        }

        protected override IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        {
            return InternalReadTreeFields();
        }
    }
}