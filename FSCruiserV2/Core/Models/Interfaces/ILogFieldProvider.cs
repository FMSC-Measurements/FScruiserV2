using System.Collections.Generic;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ILogFieldProvider
    {
        IEnumerable<LogFieldSetupDO> LogFields { get; }

        //List<CruiseDAL.DataObjects.LogFieldSetupDO> ReadLogFields();
    }
}