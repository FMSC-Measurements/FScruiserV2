using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using CruiseDAL.DataObjects;
using System.Threading;

namespace FSCruiserV2.Forms
{
    

    public partial class FormMain : Form
    {
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
                if (unitVM != null)
                {
                    return unitVM;
                }
                return null; 
            } 
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            bool success = this.Controller.OpenFile();
            this._dataEntryMI.Enabled = success;
            this._cuttingUnitCB.Enabled = success;
            this._fileNameTB.Text = (success) ? Controller._cDal.Path : String.Empty;
            if (success)
            {
                this._BS_cuttingUnits.DataSource = Controller.CuttingUnits;
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            //Controller.ShowTallies();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Option not available yet. Check back later");
            //FormCruiseInfo ci = new FormCruiseInfo();
            //ci.ShowDialog();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            using (FormDeviceInfo di = new FormDeviceInfo())
            {
                di.ShowDialog();
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            // do nothing. this only had sub-menus
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            this.Controller.ViewController.ShowAbout();
        }

        private void backupMI_Click(object sender, EventArgs e)
        {
            // Backup
            this.Controller.ViewController.ShowBackupUtil();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            Controller.ViewController.ShowAddPopulation();
        }

        private void _BS_cuttingUnits_CurrentChanged(object sender, EventArgs e)
        {
            
            if (SelectedUnit != null)
            {
                LoadCuttingUnitInfo(SelectedUnit);
            }
        }

        private void LoadCuttingUnitInfo(CuttingUnitVM unit)
        {
            //_cuttingUnitInfoLable.Text = String.Format("{0} Area: {1}", unit.Description, unit.Area);
            
            
            //_strataLB..Clear();
            //foreach (StratumDO st in unit.Strata)
            //{
            //    ListViewItem item = new ListViewItem(Controller.GetStratumInfoShort(st));
                
            //    _strataLB.Items.Add(item);
            //}



            unit.Strata.Populate();
            _strataView.Controls.Clear();
            _strataView.SuspendLayout();
            foreach (StratumDO st in unit.Strata)
            {
                Label stLBL = new Label();
                stLBL.Text = ApplicationController.GetStratumInfoShort(st);
                if( _fontHeight == 0 )
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
            _strataView.ResumeLayout();

            //string[] strataStr = new string[unit.Strata.Count];
            //for (int i = 0; i < unit.Strata.Count; i++)
            //{
            //    strataStr[i] = ApplicationController.GetStratumInfoShort(unit.Strata[i]);
            //}
            //_strataLB.DataSource = strataStr;
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
    }

}