using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiser.Core.Models;
using CruiseDAL;

namespace FScruiser.Core.Services
{
    public class ILogDataService
    {
        DAL DataStore { get; set; }

        public Tree Tree { get; protected set; }

        public Stratum Stratum { get; protected set; }

        RegionLogInfo RegionalLogRule { get; set; }

        public ICollection<Log> Logs { get; protected set; }

        public double LogCountDesired
        {
            get
            {
                return GetDefaultLogCount();
            }
        }

        public ILogDataService(Tree tree, Stratum stratum, RegionLogInfo logRule, DAL dataStore)
        {
            DataStore = dataStore;
            Stratum = stratum;
            RegionalLogRule = logRule;
            Tree = tree;
            Logs = LoadLogs();
        }

        IList<Log> LoadLogs()
        {
            var logs = DataStore.From<Log>()
                .Where("Tree_CN = ?")
                .OrderBy("CAST (LogNumber AS NUMERIC)")
                .Read(Tree.Tree_CN).ToList();

            if (logs.Count == 0)
            {
                var defaultLogCnt = Math.Ceiling(LogCountDesired);
                for (int i = 0; i < LogCountDesired; i++)
                {
                    logs.Add(
                        new Log()
                        {
                            DAL = DataStore,
                            LogNumber = i + 1,
                            Tree_CN = Tree.Tree_CN
                        });
                }
            }

            return logs;
        }

        double GetDefaultLogCount()
        {
            if (Tree.TreeDefaultValue == null) { return 0.0; }

            var mrchHtLL = Tree.TreeDefaultValue.MerchHeightLogLength;
            var species = Tree.TreeDefaultValue.Species;

            if (RegionalLogRule != null)
            {
                var logRule = RegionalLogRule.GetLogRule(species);
                if (logRule != null)
                {
                    return logRule.GetDefaultLogCount(Tree.TotalHeight, Tree.DBH, mrchHtLL);
                }
            }
            return 0;
        }

        int GetNextLogNum()
        {
            int highest = 0;
            foreach (var log in Logs)
            {
                highest = Math.Max(highest, log.LogNumber);
            }
            return highest + 1;
        }

        public bool IsLogNumAvalible(int newLogNum)
        {
            if (Logs == null || Logs.Count == 0) { return true; }
            return !Logs.Any(x => x.LogNumber == newLogNum);
        }

        public Log AddLogRec()
        {
            var newLog = new Log();
            newLog.DAL = DataStore;
            newLog.Tree_CN = Tree.Tree_CN;
            newLog.LogNumber = GetNextLogNum();

            Logs.Add(newLog);
            return newLog;
        }

        public bool DeleteLog(Log log)
        {
            if (log.IsPersisted)
            {
                Tree.LogCountDirty = true;
                log.Delete();
            }
            return Logs.Remove(log);
        }

        public void Save()
        {
            Tree.LogCountDirty = true;
            foreach (var log in Logs)
            {
                DataStore.Save(log);
            }
        }
    }
}