using CruiseDAL;
using CruiseDAL.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScruiser.Data
{
    public class Dataservice_Base_V2
    {
        protected readonly string PLOT_METHODS = String.Join(", ",
            new[] {
                CruiseMethods.FIX,
                CruiseMethods.FCM,
                CruiseMethods.F3P,
                CruiseMethods.PNT,
                CruiseMethods.PCM,
                CruiseMethods.P3P,
                CruiseMethods.THREEPPNT,
                CruiseMethods.FIXCNT,
            }
            .Select(x => "'" + x + "'").ToArray());

        public Dataservice_Base_V2(DAL database)
        {
            Database = database;
        }

        protected DAL Database { get; }


    }
}
