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

        public string LogLevelDiscription
        {
            get
            {
                var tree = Tree;
                var tdv = tree.TreeDefaultValue;
                var expectedNumberOfLogs = GetDefaultLogCount();

                return String.Format("Tree:{0} Sp:{1} DBH:{2} Ht:{3} Log Length:{4}{5}",
                tree.TreeNumber,
                tree.Species,
                tree.DBH,
                tree.TotalHeight,
                (tdv != null) ? tdv.MerchHeightLogLength.ToString() : string.Empty,
                (expectedNumberOfLogs > 0) ? " Calculated Logs:" + expectedNumberOfLogs.ToString() : string.Empty);
            }
        }

        public double NumCaldulatedLogs
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
                var defaultLogCnt = Math.Ceiling(NumCaldulatedLogs);
                for (int i = 0; i < NumCaldulatedLogs; i++)
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
            if (Tree.TreeDefaultValue != null)
            {
                var species = Tree.TreeDefaultValue.Species;

                return DataStore.From<LogGradeAuditRule>()
                .Where("Species = ? OR Species = 'ANY'")
                .Query(Tree.TreeDefaultValue.Species).ToArray();
            }
            else
            {
                return Enumerable.Empty<LogGradeAuditRule>();
            }
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

        public bool ValidateLogGrades()
        {
            bool success = true;
            foreach (var log in Logs)
            {
                success = ValidateLogGrade(log) && success;
            }
            return success;
        }

        public bool ValidateLogGrade(Log log)
        {
            return ValidateLogGrade(log, LogGradeAudits);
        }

        public static bool ValidateLogGrade(Log log, IEnumerable<LogGradeAuditRule> logGradAudits)
        {
            if (log == null) { throw new ArgumentNullException("log"); }
            if (logGradAudits == null || logGradAudits.Count() == 0)
            {
                log["Grade"] = null;
                return true;
            }

            var logGrade = (log.Grade ?? string.Empty).Trim();

            foreach (var lga in logGradAudits)
            {
                if (lga.Grades.Contains(logGrade))
                {
                    if (Math.Round(log.SeenDefect, 2) > Math.Round(lga.DefectMax, 2))
                    {
                        log["Grade"] = String.Format("Species {0}, log grade(s) {1} max log defect is {2}%"
                        , lga.Species, String.Join(", ", lga.Grades.ToArray()), lga.DefectMax);
                        return false;
                    }
                    else
                    {
                        log["Grade"] = null;
                        return true;
                    }
                }
            }

            //after going through all audits if no valid grade match found then validation fails
            string[] allValidGrades = logGradAudits.SelectMany(x => x.Grades)
                .Distinct().ToArray();

            if (allValidGrades.Count() == 0)
            {
                log["Grade"] = null;
                return true;
            }
            else
            {
                string[] species = logGradAudits.Select(x => x.Species).Distinct().ToArray();
                Array.Sort(allValidGrades);
                log["Grade"] = String.Format("Species {0} can only have log grades {1}"
                            , String.Join(", ", species)
                            , String.Join(", ", allValidGrades));

                return false;
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