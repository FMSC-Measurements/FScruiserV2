using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiser.Core.Models;
using CruiseDAL;
using CruiseDAL.DataObjects;

namespace FScruiser.Core.Services
{
    public class ILogDataService
    {
        DAL DataStore { get; set; }

        public Tree Tree { get; protected set; }

        public Stratum Stratum { get; protected set; }

        RegionLogInfo RegionalLogRule { get; set; }

        public ICollection<Log> Logs { get; protected set; }

        public IEnumerable<LogGradeAuditRule> LogGradeAudits { get; protected set; }

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
            LogGradeAudits = LoadLogGradeAudits();
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

            foreach (var log in logs)
            {
                log.PropertyChanged += Log_PropertyChanged;
                ValidateLogGrade(log);
            }

            return logs;
        }

        IEnumerable<LogGradeAuditRule> LoadLogGradeAudits()
        {
            return DataStore.From<LogGradeAuditRule>()
                .Where("Species = ? OR Species = 'ANY'")
                .Query(Tree.TreeDefaultValue.Species).ToArray();
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

        public Log AddLogRec()
        {
            var newLog = MakeLogRec();
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

        public Log MakeLogRec()
        {
            var newLog = new Log();
            newLog.DAL = DataStore;
            newLog.Tree_CN = Tree.Tree_CN;
            newLog.LogNumber = GetNextLogNum();
            newLog.PropertyChanged += Log_PropertyChanged;

            return newLog;
        }

        private void Log_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var log = sender as Log;
            if (log == null) { return; }

            switch (e.PropertyName)
            {
                case "Grade":
                case "SeenDefect":
                    {
                        ValidateLogGrade(log);
                        break;
                    }
            }
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

        public void ValidateLogGrade(Log log)
        {
            ValidateLogGrade(log, LogGradeAudits);
        }

        public static void ValidateLogGrade(Log log, IEnumerable<LogGradeAuditRule> logGradAudits)
        {
            foreach (var lga in logGradAudits)
            {
                lga.ValidateLog(log);
            }
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