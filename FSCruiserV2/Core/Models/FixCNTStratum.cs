using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulationProvider 
    {
        IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations();
    }

    public class FixCNTStratum : PlotStratum , IFixCNTTallyPopulationProvider
    {
        IFixCNTTallyClass _tallyClass;
        IEnumerable<IFixCNTTallyPopulation> _tallyPopulations;

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

        public IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations()
        {
            var tallyClass = TallyClass;

            var tallyPopulations = DAL.From<FixCNTTallyPopulation>()
                .Where("FixCNTTallyClass_CN = ?")
                .Query(this.Stratum_CN);

            foreach (var tallyPop in tallyPopulations)
            {
                tallyPop.SampleGroup = DAL.From<SampleGroupVM>()
                    .Where("SampleGroup_CN = ?")
                    .Read(tallyPop.SampleGroup_CN).FirstOrDefault();

                tallyPop.TreeDefaultValue = DAL.From<TreeDefaultValueDO>()
                    .Where("TreeDefaultValue_CN = ?")
                    .Read(tallyPop.TreeDefaultValue_CN).FirstOrDefault();

                tallyPop.TallyClass = tallyClass;
                yield return tallyPop;
            }
        }


        public override void PopulatePlots(long cuttingUnit_CN)
        {
            this.Plots = new List<PlotVM>();

            foreach (var plot in this.DAL.From<FixCNTPlot>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Query(this.Stratum_CN, cuttingUnit_CN))
            {
                Plots.Add(plot);
            }
        }

    }
}
