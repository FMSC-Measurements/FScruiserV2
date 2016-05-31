using System;
using System.Drawing;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormMain : Form
    {
        class RecentFilesMenuItem : MenuItem
        {
            public string FilePath { get; set; }
        }

        private int _fontHeight = 0;

        public FormMain(IApplicationController controller)
        {
            this.Controller = controller;
            this.KeyPreview = true;
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            this._dataEntryMI.Enabled = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled) { return; }
            if (e.KeyData == Keys.O)
            {
                e.Handled = true;
                this.OpenButton_Click(null, null);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FMSC.Utility.MobileDeviceInfo di = new FMSC.Utility.MobileDeviceInfo();
            _userInfoLBL.Text = di.GetModelAndSerialNumber();

            //Thread.Sleep(1000);
        }

        IApplicationController Controller { get; set; }

        CuttingUnitVM SelectedUnit
        {
            get
            {
                CuttingUnitVM unitVM = _BS_cuttingUnits.Current as CuttingUnitVM;
                if (unitVM != null && unitVM.Code != null)
                {
                    return unitVM;
                }
                return null;
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            this.Controller.OpenFile();
        }

        public void HandleFileStateChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HandleFileStateChanged));
            }
            else
            {
                if (Controller._cDal != null && Controller._cDal.Exists)
                {
                    var fileName = System.IO.Path.GetFileName(Controller._cDal.Path);
                    this._dataEntryMI.Enabled = true;
                    Text = "FScruiser - " + fileName;
                    this._fileNameTB.Text = Controller._cDal.Path;
                }
                else
                {
                    this._dataEntryMI.Enabled = false;
                    Text = FSCruiser.Core.Constants.APP_TITLE;
                    this._fileNameTB.Text = string.Empty;
                }
                UpdateCuttingUnits();
            }
        }

        void UpdateCuttingUnits()
        {
            if (this.Controller.CuttingUnits != null)
            {
                var units = new CuttingUnitVM[Controller.CuttingUnits.Count + 1];
                Controller.CuttingUnits.CopyTo(units, 1);
                units[0] = new CuttingUnitVM();
                this._BS_cuttingUnits.DataSource = units;
                this._cuttingUnitCB.Enabled = true;
            }
            else
            {
                this._BS_cuttingUnits.DataSource = new CuttingUnitVM[0];
                this._cuttingUnitCB.Enabled = false;
            }
        }

        private void _cruiseInfo_MI_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Option not available yet. Check back later");
            //FormCruiseInfo ci = new FormCruiseInfo();
            //ci.ShowDialog();
        }

        private void _deviceInfo_MI_Click(object sender, EventArgs e)
        {
            using (FormDeviceInfo di = new FormDeviceInfo())
            {
                di.ShowDialog();
            }
        }

        private void _utilities_MI_Click(object sender, EventArgs e)
        {
            // do nothing. this only had sub-menus
        }

        private void _about_MI_Click(object sender, EventArgs e)
        {
            this.Controller.ViewController.ShowAbout();
        }

        private void backupMI_Click(object sender, EventArgs e)
        {
            // Backup
            this.Controller.ViewController.ShowBackupUtil();
        }

        private void _addPopulation_MI_Click(object sender, EventArgs e)
        {
            Controller.ViewController.ShowAddPopulation();
        }

        private void _BS_cuttingUnits_CurrentChanged(object sender, EventArgs e)
        {
            LoadCuttingUnitInfo(SelectedUnit);
        }

        private void LoadCuttingUnitInfo(CuttingUnitVM unit)
        {
            _strataView.SuspendLayout();
            _strataView.Controls.Clear();
            this._dataEntryMI.Enabled = (unit != null);
            if (unit != null)
            {
                var strata = unit.DAL.From<StratumDO>()
                    .Join("CuttingUnitStratum", "USING (Stratum_CN)", "CUST")
                    .Where("CUST.CuttingUnit_CN = ?")
                    .Query(unit.CuttingUnit_CN);

                foreach (StratumDO st in strata)
                {
                    Label stLBL = new Label();
                    stLBL.Text = st.GetDescriptionShort();
                    if (_fontHeight == 0)
                    {
                        using (Graphics g = base.CreateGraphics())
                        {
                            SizeF s = g.MeasureString(" ", stLBL.Font);
                            _fontHeight = (int)Math.Ceiling(s.Height);
                        }
                    }

                    stLBL.Dock = DockStyle.Top;
                    stLBL.Height = _fontHeight;
                    _strataView.Controls.Add(stLBL);
                }
            }
            _strataView.ResumeLayout();
        }

        private void dataEntryButton_Click(object sender, EventArgs e)
        {
            if (SelectedUnit != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();
                Controller.LoadCuttingUnit(SelectedUnit);
                Controller.ViewController.ShowDataEntry(SelectedUnit);
                // Cursor.Current = Cursors.Default; is at the end of FormDataEntry.OnLoad();
            }
        }

        // Kludgy and not sure I like this.
        private void _strataLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this._strataLB.SelectedIndex = -1; // unselect all items
        }

        private void _manageCruisersMI_Click(object sender, EventArgs e)
        {
            this.Controller.ViewController.ShowManageCruisers();
        }

        private void _menu_MI_Popup(object sender, EventArgs e)
        {
            _recentFiles_MI.MenuItems.Clear();

            foreach (RecentProject rp in Controller.Settings.RecentProjects)
            {
                var mi = new RecentFilesMenuItem()
                {
                    Text = rp.ProjectName,
                    FilePath = rp.FilePath
                };
                mi.Click += recentFileSelected;
                _recentFiles_MI.MenuItems.Add(mi);
            }
        }

        private void recentFileSelected(object sender, EventArgs e)
        {
            var tsmi = sender as RecentFilesMenuItem;
            if (tsmi == null) return;

            this.Controller.OpenFile(tsmi.FilePath);
        }
    }
}