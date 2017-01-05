using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public partial class FormBackupUtility : Form
    {
        private IApplicationController _controller;
        //private string _backupPath;
        //private string _backupDir;

        public FormBackupUtility()
        {
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;

                this.Menu = null;
                this._ceControlPanel.Visible = true;
                this.mainMenu1.Dispose();
                this.mainMenu1 = null;
            }
        }

        public FormBackupUtility(IApplicationController controller)
            : this()
        {
            this._controller = controller;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this._RB_useCDOption.Checked = _controller.Settings.BackUpToCurrentDir;
            this._RB_useAlternate.Checked = !_controller.Settings.BackUpToCurrentDir;

            this._backupOnLeaveUnitCB.Checked = (_controller.Settings.BackUpMethod == BackUpMethod.LeaveUnit);

            _backupFileLBL.Text = _controller.Settings.BackupDir;

            OnRadioButtonsChanged();
        }

        void OnRadioButtonsChanged()
        {
            this._changeLocationBTN.Enabled = _RB_useAlternate.Checked;
            this._backupFileLBL.Enabled = _RB_useAlternate.Checked;
        }

        void PushSettings()
        {
            this._controller.Settings.BackUpToCurrentDir = _RB_useCDOption.Checked;
            this._controller.Settings.BackupDir = _backupFileLBL.Text;
            this._controller.Settings.BackUpMethod = (_backupOnLeaveUnitCB.Checked) ? BackUpMethod.LeaveUnit : BackUpMethod.None;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this.DialogResult == DialogResult.OK)
            {
                PushSettings();
            }
        }

        private void _cancelMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _okMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void _makeBackupBTN_Click(object sender, EventArgs e)
        {
            if (_controller.DataStore != null)
            {
                PushSettings();
                _controller.PerformBackup(true);
            }
            else
            {
                MessageBox.Show("No file open");
            }
        }

        private void _changeLocationBTN_Click(object sender, EventArgs e)
        {
            FMSC.Controls.FolderBrowserDialogCF view = new FMSC.Controls.FolderBrowserDialogCF("\\");
            if (view.ShowDialog() == DialogResult.OK)
            {
                _backupFileLBL.Text = view.Folder;
            }
        }

        private void _RB_useAlternate_CheckedChanged(object sender, EventArgs e)
        {
            OnRadioButtonsChanged();
        }
    }
}