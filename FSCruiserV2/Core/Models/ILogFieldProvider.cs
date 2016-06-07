using System.Collections.Generic;

namespace FSCruiser.Core.Models
{
    public interface ILogFieldProvider
    {
        List<CruiseDAL.DataObjects.LogFieldSetupDO> ReadLogFields();
    }
}