using System.Collections.Generic;
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

        public IFixCNTTallyClass TallyClass
        {
            get
            {
                if (_tallyClass == null)
                {
                    _tallyClass = DAL.From<FixCNTTallyClass>()
                        .Where("Stratum_CN = @p1")
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
                PlotNumber = GetNextPlotNumber(cuttingUnit.CuttingUnit_CN.Value),
                Trees = new System.ComponentModel.BindingList<Tree>(),
        };
        }

        public IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations()
        {
            var tallyClass = TallyClass;

            var tallyPopulations = DAL.From<FixCNTTallyPopulation>()
                .Where("FixCNTTallyClass_CN = @p1")
                .Query(tallyClass.FixCNTTallyClass_CN);

            foreach (var tallyPop in tallyPopulations)
            {
                tallyPop.SampleGroup = DAL.From<SampleGroup>()
                    .Where("SampleGroup_CN = @p1")
                    .Read(tallyPop.SampleGroup_CN).FirstOrDefault();

                tallyPop.TreeDefaultValue = DAL.From<TreeDefaultValueDO>()
                    .Where("TreeDefaultValue_CN = @p1")
                    .Read(tallyPop.TreeDefaultValue_CN).FirstOrDefault();

                tallyPop.TallyClass = tallyClass;
                yield return tallyPop;
            }
        }

        protected override IEnumerable<Plot> ReadPlots(long cuttingUnit_CN)
        {
            foreach (var plot in this.DAL.From<FixCNTPlot>().Where("Stratum_CN = @p1 AND CuttingUnit_CN = @p2")
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