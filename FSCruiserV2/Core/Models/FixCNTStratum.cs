using System;

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

    }
}
