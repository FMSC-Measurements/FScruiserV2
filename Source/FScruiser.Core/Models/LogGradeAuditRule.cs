using CruiseDAL.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCruiser.Core.Models
{
    public class LogGradeAuditRule : LogGradeAuditRuleDO
    {
        IEnumerable<string> _grades;

        public IEnumerable<string> Grades
        {
            get
            {
                if (_grades == null)
                {
                    _grades = ValidGrades.Split(',').Select(x => x.Trim()).ToArray();
                }
                return _grades;
            }
        }

        public bool ValidateLog(Log log)
        {
            if ((Math.Abs(DefectMax) < .01 || log.SeenDefect <= DefectMax)//DefectMax == 0 || ...
                   && Grades.Contains(log.Grade))
            {
                return true;
            }
            else
            {
                var message = String.Empty;
                if (DefectMax > 0)
                {
                    message = String.Format("Species {2}, log grade(s) {0} max log defect is %{1}"
                        , String.Join(", ", Grades), DefectMax * 100, Species);
                }
                else
                {
                    message = String.Format("Species {1} can only have log grades {0}"
                        , String.Join(", ", Grades), Species);
                }
                log["Grade"] = message;
                return false;
            }
        }
    }
}