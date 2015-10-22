using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CruiseDAL;
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

        public FormBackupUtility(IApplicationController controller): this()
        {
            this._controller = controller;
        }

        


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this._backupOnLeaveUnitCB.Checked = (_controller.BackUpMethod == BackUpMethod.LeaveUnit);

            //string dbPath = this._controller._cDal.Path;
            //this._fileNameLBL.Text = System.IO.Path.GetFileName(dbPath);

            //_backupPath = GetBackupFileName(_backupDir);
            _backupFileLBL.Text = _controller.BackupDir;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(this.DialogResult == DialogResult.OK)
            {
                this._controller.BackUpMethod = (_backupOnLeaveUnitCB.Checked) ? BackUpMethod.LeaveUnit : BackUpMethod.None;
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
            if (_controller._cDal != null)
            {
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
                _controller.BackupDir = view.Folder;
                //this._backupPath = GetBackupFileName(_backupDir);
                _backupFileLBL.Text = _controller.BackupDir;
            }
        }

        


    }
}