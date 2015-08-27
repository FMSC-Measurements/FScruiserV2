using System;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Forms;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using CruiseDAL.DataObjects;
using FMSC.Controls;

namespace FSCruiserV2.Logic
{
    

    public class ViewController : WinFormsViewControllerBase
    {
        #region static members
        public static FMSC.Controls.PlatformType PlatformType
        {
            get
            {
                return FMSC.Controls.DeviceInfo.DevicePlatform;
            }
        }
        #endregion

        private Dictionary<StratumDO, FormLogs> _logViews = new Dictionary<StratumDO, FormLogs>();


        public ViewController()
        {
            //this.PlatformType = //TODO implement ability to identify platform type
        }

        public FormLogs GetLogsView(StratumDO stratum)
        {
            if (_logViews.ContainsKey(stratum))
            {
                return _logViews[stratum];
            }
            FormLogs logView = new FormLogs(this.ApplicationController, stratum.Stratum_CN.Value);
            _logViews.Add(stratum, logView);

            return logView;
        }             



        public override void SignalInvalidAction()
        {
            Win32.MessageBeep(-1);
        }


        public override DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, TreeVM optTree, out string logMessage)
        {
            using (FormLimitingDistance view = new FormLimitingDistance(this.ApplicationController))
            {
                return view.ShowDialog(baf, isVariableRadius, optTree, out logMessage);
            }
        }

        public override void ShowLogsView(StratumDO stratum, TreeVM tree)
        {
            if (stratum == null)
            {
                MessageBox.Show("Invalid Action. Stratum not set.");
            }
            this.GetLogsView(stratum).ShowDialog(tree);
        }

        public override void ShowManageCruisers()
        {
            using (FormManageCruisers view = new FormManageCruisers(this.ApplicationController))
            {
                view.ShowDialog();
            }
        }


        public override TreeDefaultValueDO ShowAddPopulation()
        {
            if (this.ApplicationController._cDal == null)
            {
                MessageBox.Show("No File Selected");
                return null;
            }
            using (FormAddPopulation view = new FormAddPopulation(this.ApplicationController))
            {
                Cursor.Current = Cursors.WaitCursor;
                view.ShowDialog();

                return null;
            }
        }

        public override TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg)
        {
            if (this.ApplicationController._cDal == null)
            {
                MessageBox.Show("No File Selected");
                return null;
            }
            using (FormAddPopulation view = new FormAddPopulation(this.ApplicationController))
            {
                Cursor.Current = Cursors.WaitCursor;
                view.ShowDialog(sg);
                return null;
            }
        }

        public override void ShowBackupUtil()
        {
            using (FormBackupUtility view = new FormBackupUtility(this.ApplicationController))
            {
                view.ShowDialog();
            }
        }

        

        public override void ShowDataEntry(CuttingUnitDO unit)
        {
            lock (_dataEntrySyncLock)
            {
                if (_dataEntryView != null)
                {
                    _dataEntryView.Dispose();
                }

                _dataEntryView = new FormDataEntry(this.ApplicationController, unit);
            }
            _dataEntryView.ShowDialog();

        }

        public override DialogResult ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit)
        {
            using (FormEditSampleGroup view = new FormEditSampleGroup())
            {
                return view.ShowDialog(sg, allowEdit);
            }
        }

        public override DialogResult ShowEditTreeDefault(TreeDefaultValueDO tdv)
        {
            using (FormEditTreeDefault view = new FormEditTreeDefault(this.ApplicationController))
            {
                return view.ShowDialog(tdv);
            }
        }

        public override DialogResult ShowOpenCruiseFileDialog(out string fileName)
        {
            using (FMSC.Controls.OpenFileDialogRedux fileDialog = new FMSC.Controls.OpenFileDialogRedux())
            {
                fileDialog.Filter = "cruise files (*.cruise)|*.cruise";
                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fileName = fileDialog.FileName;
                }
                else
                {
                    fileName = null;
                }
                return result;
            }

        }

        public override void SignalMeasureTree(bool showMessage)
        {
            Win32.MessageBeep(Win32.MB_ICONQUESTION);
            if (showMessage)
            {
                MessageBox.Show("Measure Tree");
            }
        }

        public override void SignalInsuranceTree()
        {
            Win32.MessageBeep(Win32.MB_ICONASTERISK);
            MessageBox.Show("Insurance Tree");
        }


        #region IDisposable Members


        #endregion

    }
}
