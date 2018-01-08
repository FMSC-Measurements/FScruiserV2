using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScruiser.Core.Services
{
    public interface ICountTreeDataService
    {
        CountTree Count { get; }

        void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue);

        void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue);
    }
}
