using System.Collections.Generic;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ITreeFieldProvider
    {
        IEnumerable<TreeFieldSetupDO> TreeFields { get; }

        //IEnumerable<TreeFieldSetupDO> ReadTreeFields();
    }
}