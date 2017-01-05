using System.Threading;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.Common;
using FSCruiser.WinForms.DataEntry;
using OpenNETCF.Media;
using System.IO;

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

        FormCruiserSelection _cruiserSelectionView;
        SoundPlayer _tallySoundPlayer;
        SoundPlayer _pageChangedSoundPlayer;

        public ViewController()
        {
            try
            {
                var soundsDir = System.IO.Path.Combine(GetExecutionDirectory(), "Sounds");

                _tallySoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\tally.wav", System.IO.FileMode.Open));
                _pageChangedSoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\pageChange.wav", FileMode.Open));
            }
            catch 
            {
            }
        }

        static string GetExecutionDirectory()
        {
            string name = System.Reflection.Assembly
                .GetCallingAssembly()
                .GetName().CodeBase;

            //clean up path, in FF name is a URI
            if (name.StartsWith(@"file:///"))
            {
                name = name.Replace("file:///", string.Empty);
            }
            string dir = System.IO.Path.GetDirectoryName(name);
            return dir;
        }

        public override void SignalInvalidAction()
        {
            FSCruiser.WinForms.Win32.MessageBeep(-1);
        }

        public override void ShowCruiserSelection(Tree tree)
        {
            if (this.ApplicationController.Settings.EnableCruiserPopup)
            {
                if (_cruiserSelectionView == null)
                {
                    _cruiserSelectionView = new FormCruiserSelection(ApplicationController);
                }
                _cruiserSelectionView.ShowDialog(tree);
            }
        }

        public override DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            using (FormLimitingDistance view = new FormLimitingDistance(baf, isVariableRadius))
            {
                var result = view.ShowDialog();
                logMessage = view.Report;

                return result;
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

        public override TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg)
        {
            if (this.ApplicationController.DataStore == null)
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

        public override DialogResult ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit)
        {
            using (FormEditSampleGroup view = new FormEditSampleGroup())
            {
                return view.ShowDialog(sg, allowEdit);
            }
        }

        public override DialogResult ShowEditTreeDefault(TreeDefaultValueDO tdv)
        {
            using (FormEditTreeDefault view = new FormEditTreeDefault(ApplicationController.DataStore))
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

        public override void SignalTally()
        {
            if (_tallySoundPlayer != null)
            {
                _tallySoundPlayer.Play();
            }
        }

        public override void SignalPageChanged()
        {
            if (_pageChangedSoundPlayer != null)
            {
                _pageChangedSoundPlayer.Play();
            }
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_cruiserSelectionView != null)
                {
                    this._cruiserSelectionView.Dispose();
                    this._cruiserSelectionView = null;
                }
                if (_tallySoundPlayer != null)
                {
                    _tallySoundPlayer.Dispose();
                    _tallySoundPlayer = null;
                }
            }
        }

        #endregion IDisposable Members
    }
}