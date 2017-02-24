using System.Threading;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.Common;
using FSCruiser.WinForms.DataEntry;
using OpenNETCF.Media;
using System.IO;
using FSCruiser.Core;

namespace FSCruiser.WinForms
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

        #endregion static members

        public ViewController()
        {
        }

        public override bool ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            using (FormLimitingDistance view = new FormLimitingDistance(baf, isVariableRadius))
            {
                var result = view.ShowDialog();
                logMessage = view.Report;

                return result == DialogResult.OK;
            }
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
            if (this.ApplicationController.DataStore == null)
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

        public override void ShowBackupUtil()
        {
            using (FormBackupUtility view = new FormBackupUtility(this.ApplicationController))
            {
                view.ShowDialog();
            }
        }

        Thread _splashThread;

        public override void BeginShowSplash()
        {
            _splashThread = new Thread(ShowSplash);//TODO ensure thread gets killed when not needed
            _splashThread.Name = "Splash";
            _splashThread.Start();
        }

        private void ShowSplash()
        {
            using (FormAbout a = new FormAbout())
            {
                a.ShowDialog();
                //Application.Run(a);
            }
        }

        public override bool ShowOpenCruiseFileDialog(out string fileName)
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
                return result == DialogResult.OK;
            }
        }



        //#region IDisposable Members

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (disposing)
        //    {
        //    }
        //}

        //#endregion IDisposable Members
    }
}