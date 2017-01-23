using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using System.Collections.Generic;
using FSCruiser.NetCF.WinForms;

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

        CuttingUnit SelectedUnit
        {
            get
            {
                CuttingUnit unitVM = _BS_cuttingUnits.Current as CuttingUnit;
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
                var fileLoaded = Controller.DataStore != null && Controller.DataStore.Exists;
                this._dataEntryMI.Enabled = fileLoaded;
                _cuttingUnitCB.Enabled = fileLoaded;

                _fileNameTB.Text = (fileLoaded) ?
                    Controller.DataStore.Path : String.Empty;

                Text = (fileLoaded) ?
                    ("FScruiser - " + System.IO.Path.GetFileName(Controller.DataStore.Path))
                    : FSCruiser.Core.Constants.APP_TITLE;

                UpdateCuttingUnits();
            }
        }

        void UpdateCuttingUnits()
        {
            _BS_cuttingUnits.DataSource = ReadCuttingUnits().ToArray();
        }

        public IEnumerable<CuttingUnit> ReadCuttingUnits()
        {
            if (Controller.DataStore != null)
            {
                yield return new CuttingUnit();
                foreach (var unit in Controller.DataStore.From<CuttingUnit>().Read())
                {
                    yield return unit;
                }
            }
            else
            {
                yield break;
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

        private void LoadCuttingUnitInfo(CuttingUnit unit)
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

            foreach (RecentProject rp in ApplicationSettings.Instance.RecentProjects)
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

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            using (FormSettings view = new FormSettings())
            {
                view.ShowDialog();
            }
        }
    }
}