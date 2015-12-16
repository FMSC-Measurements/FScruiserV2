using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class PlotStratum : StratumVM
    {

        public int GetNextPlotNumber(long cuttingUnit_CN)
        {
            try
            {
                int highestInUnit = 0;
                int highestInStratum = 0;

                var query1 = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0}", cuttingUnit_CN);
                int? result1 = DAL.ExecuteScalar<int?>(query1);
                highestInUnit = result1.GetValueOrDefault(0);

                string query2 = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0} AND Stratum_CN = {1}", cuttingUnit_CN, Stratum_CN);
                int? result2 = DAL.ExecuteScalar<int?>(query2);
                highestInStratum = result2.GetValueOrDefault(0);


                if (highestInUnit > highestInStratum && highestInUnit > 0)
                {
                    return highestInUnit;
                }
                return highestInUnit + 1;
            }
            catch (Exception e)
            {
                Logger.Log.E("Unable to establish next plot number", e);
                return 0;
            }
        }

    }
}
