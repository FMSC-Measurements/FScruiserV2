using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using SGType = CruiseDAL.DataObjects.SampleGroupDO;
using StType = CruiseDAL.DataObjects.StratumDO;

namespace FSCruiser.WinForms
{
    public partial class FormAddPopulation : Form
    {
        private readonly SGType _newSGPlaceHolder = new SGType()
        {
            Code = "<new>"
        };

        private bool sampleGroupChangeing = false;

        private SGType _sampleGroup;
        private StType _stratum;

        //private AddPopulationController LogicController { get; set; }
        public IApplicationController Controller { get; protected set; }

        public SGType SampleGroup
        {
            get { return _sampleGroup; }
            protected set
            {
                if (sampleGroupChangeing || _sampleGroup == value) { return; }
                if (object.ReferenceEquals(value, _newSGPlaceHolder))
                {
                    SGType newSG = this.Controller.CreateNewSampleGroup(Stratum);
                    if (newSG == null)
                    {
                        return;
                    }
                    else
                    {
                        sampleGroupChangeing = true;
                        this._BS_SampleGroups.Add(newSG);
                        this._BS_SampleGroups.Position = this._BS_SampleGroups.IndexOf(newSG);
                        value = newSG;
                        sampleGroupChangeing = false;
                    }
                }

                this.OnSampleGroupChanging(value, _sampleGroup);
                _sampleGroup = value;
            }
        }

        public StType Stratum
        {
            get { return _stratum; }
            set
            {
                if (value == _stratum) { return; }
                _stratum = value;
                OnStratumChanged(value);
            }
        }

        public FormAddPopulation(IApplicationController controller)
        {
            //this.LogicController = new AddPopulationController(controller, this);
            this.Controller = controller;
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._ce_menuPanel.Visible = true;
            }
            else
            {
                this._ce_menuPanel.Visible = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.SampleGroup == null)
            {
                this._populationSelectPanel.Enabled = true;
                this._strataCB.DataSource = this.Controller._cDal.From<StType>().Read().ToList();
            }
            else
            {
                this._populationSelectPanel.Enabled = false;
            }
            //Cursor.Current = Cursors.Default;
        }

        protected void SaveTDVChanges()
        {
            bool hasNewTDVs = false;
            bool hasTDVRemovals = false;
            foreach (Control c in this._speciesSelectPanel.Controls)
            {
                CheckBox cb = c as CheckBox;
                if (cb == null) { continue; }
                TreeDefaultValueDO tdv = cb.Tag as TreeDefaultValueDO;
                if (tdv == null)
                {
                    System.Diagnostics.Debug.Assert(false);//condition should be unreachable
                    continue;
                }

                if (cb.Checked == true)
                {
                    if (!this.SampleGroup.TreeDefaultValues.Contains(tdv))
                    {
                        _sampleGroup.TreeDefaultValues.Add(tdv);
                        hasNewTDVs = true;
                    }
                }
                else
                {
                    if (this.SampleGroup.TreeDefaultValues.Contains(tdv))
                    {
                        _sampleGroup.TreeDefaultValues.Remove(tdv);
                        hasTDVRemovals = true;
                    }
                }
            }
            if (hasNewTDVs || hasTDVRemovals)
            {
                this.SampleGroup.TreeDefaultValues.Save();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            if (this.SampleGroup == null)
            {
                MessageBox.Show("Sample Group invalid");
                e.Cancel = true;
                return;
            }

            this.SaveTDVChanges();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.SampleGroup = null;
        }

        //public TreeDefaultValueDO CheckForExistingRecord(TreeDefaultValueDO tdv)
        //{
        //    CruiseDAL.DAL db = tdv.DAL;
        //    TreeDefaultValueDO tdv2 = db.ReadSingleRow<TreeDefaultValueDO>(CruiseDAL.Schema.TREEDEFAULTVALUE._NAME, "WHERE Species = ? and LiveDead = ? and Chargeable = ? and PrimaryProduct = ?", tdv.Species, tdv.LiveDead, tdv.Chargeable, tdv.PrimaryProduct);
        //    return (tdv2 != null) ? tdv2 : tdv;
        //}

        private void CreateCountRecord(SGType sg, TreeDefaultValueDO tdv)
        {
        }

        public DialogResult ShowDialog(SGType sg)
        {
            this.SampleGroup = sg;

            return base.ShowDialog();
        }

        private void _strataCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Stratum = this._strataCB.SelectedItem as StType;
        }

        private void _sampleGroupCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SampleGroup = this._sampleGroupCB.SelectedItem as SGType;
        }

        protected void OnStratumChanged(StType newStratum)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (newStratum != null)
            {
                List<SGType> list = this.Controller._cDal.From<SGType>()
                    .Where("Stratum_CN = ?")
                    .Read(newStratum.Stratum_CN).ToList();
                foreach (SGType sg in list)
                {
                    sg.TallyMethod = sg.GetSampleGroupTallyMode();
                }
                list.Add(_newSGPlaceHolder);
                this._BS_SampleGroups.DataSource = list;
            }
            else
            {
                this._BS_SampleGroups.DataSource = Constants.EMPTY_SG_LIST;
            }
            Cursor.Current = Cursors.Default;
        }

        protected void OnSampleGroupChanging(SGType newSG, SGType prevSG)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (prevSG != null)//check previous Sg
            {
                this.SaveTDVChanges();
            }
            this.BuildTDVList(newSG);
            this._addTDV_BTN.Enabled = newSG != null;
            Cursor.Current = Cursors.Default;
        }

        private void BuildTDVList(SampleGroupDO sg)
        {
            this._speciesSelectPanel.SuspendLayout();
            this._speciesSelectPanel.Controls.Clear();

            if (sg != null)
            {
                List<TreeDefaultValueDO> treeDefaults = this.Controller._cDal.From<TreeDefaultValueDO>()
                    .Where("PrimaryProduct = ?")
                    .Read(sg.PrimaryProduct).ToList();
                foreach (TreeDefaultValueDO tdv in treeDefaults)
                {
                    CheckBox cb = new CheckBox();
                    cb.Dock = DockStyle.Top;
                    cb.Text = String.Format("{0}-{1}",
                        tdv.Species,
                        tdv.LiveDead);

                    sg.TreeDefaultValues.Populate();
                    if (sg.TreeDefaultValues.Contains(tdv))
                    {
                        cb.Checked = true;
                    }
                    cb.Tag = tdv;
                    this._speciesSelectPanel.Controls.Add(cb);
                }
            }
            this._speciesSelectPanel.ResumeLayout();
        }

        private void _addTDV_BTN_Click(object sender, EventArgs e)
        {
            if (this.Controller.CreateNewTreeDefaultValue(this.SampleGroup.PrimaryProduct) != null)
            {
                this.BuildTDVList(this.SampleGroup);
            }
        }

        private void _cancel_MI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _editSG_BTN_Click(object sender, EventArgs e)
        {
            if (this.SampleGroup != null && this.SampleGroup != this._newSGPlaceHolder)
            {
                bool allowEdit = this.SampleGroup.CanEditSampleGroup();
                this.Controller.ViewController.ShowEditSampleGroup(this.SampleGroup, allowEdit);
            }
        }

        private void _editTallySetup_BTN_Click(object sender, EventArgs e)
        {
            if (this.SampleGroup != null && this.SampleGroup != this._newSGPlaceHolder)
            {
                this.SaveTDVChanges();
                FormTallySetup view = new FormTallySetup(this.Controller);

                if (view.ShowDialog(this.SampleGroup) == DialogResult.OK)
                {
                }
            }
        }
    }
}