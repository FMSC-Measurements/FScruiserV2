using FMSC.ORM.Core;
using System;
using System.Text;
using CruiseDAL.DataObjects;
using CruiseDAL;
using FSCruiser.Core.Models;

namespace FScruiser.Core.Services
{
    public class CountTreeDataService : ICountTreeDataService
    {
        public CruiseDAL.DAL Datastore { get; protected set; }

        public CountTree Count { get; protected set; }

        public CountTreeDataService(DatastoreRedux datastore, CountTree count)
        {
            Count = count;
        }

        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            LogMessage(String.Format("Tree Count Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            LogMessage(String.Format("SumKPI Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        public void LogMessage(string message, string level)
        {
            Datastore.LogMessage(message, level);
        }

        public bool TryLogMessage(string message, string level)
        {
            try
            {
                Datastore.LogMessage(message, level);
                return true;
            }
            catch { return false; }
        }
    }
}
