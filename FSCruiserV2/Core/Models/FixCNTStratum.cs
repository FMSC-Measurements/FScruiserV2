using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulationProvider 
    {
        IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations();
    }

    public class FixCNTStratum : PlotStratum , IFixCNTTallyPopulationProvider
    {
        public IEnumerable<IFixCNTTallyPopulation> GetFixCNTTallyPopulations()
        {
            return (IEnumerable<IFixCNTTallyPopulation>)DAL.From<FixCNTTallyPopulation>()
                .Join("SampleGroup", "USING (SampleGroup_CN)")
                .Where("Stratum_CN = ?").Query(this.Stratum_CN);
        }

        public new IList<FixCNTPlot> Plots { get; set; }

        public override void PopulatePlots(long cuttingUnit_CN)
        {
            this.Plots = this.DAL.From<FixCNTPlot>().Where("Stratum_CN = ? AND CuttingUnit_CN = ?")
                .OrderBy("PlotNumber")
                .Query(this.Stratum_CN, cuttingUnit_CN)
                .ToList();
        }

    }
}
