using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface ITreeFieldProvider
    {
        List<TreeFieldSetupDO> ReadTreeFields();
    }
}
