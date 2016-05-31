using System.Collections.Generic;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ITreeFieldProvider
    {
        List<TreeFieldSetupDO> ReadTreeFields();
    }
}