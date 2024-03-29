﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using SGType = CruiseDAL.DataObjects.SampleGroupDO;

namespace FSCruiser.WinForms
{
    public partial class FormTallySetup : Form
    {
        public IApplicationController Controller { get; protected set; }

        public FormTallySetup(IApplicationController controller)
        {
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

        private TallyDO _tally;

        public TallyDO Tally
        {
            get
            {
                return this._tally;
            }
            set
            {
                if (_tally == value) { return; }
                this._tally = value;
                this._BS_Tally.DataSource = value;
            }
        }

        public SGType SampleGroup
        { get; protected set; }

        public DialogResult ShowDialog(SGType sg)
        {
            this.SampleGroup = sg;

            this._sgInfo_LBL.Text = this.SampleGroup.Code + "-" + this.SampleGroup.TallyMethod.ToString();
            if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.BySampleGroup) == CruiseDAL.Enums.TallyMode.BySampleGroup)
            {
                this._species_LBL.Visible = false;
                this._speciesSelect_CB.Visible = false;
                TallyDO t = this.Controller.DataStore.From<TallyDO>()
                    .Join("CountTree", "USING (Tally_CN)")
                    .Where("SampleGroup_CN = ?")
                    .Read(sg.SampleGroup_CN).FirstOrDefault();
                if (t == null)
                {
                    t = new TallyDO(this.Controller.DataStore);
                    t.Description = sg.Code;
                }
                this.Tally = t;
            }
            else if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.BySpecies) == CruiseDAL.Enums.TallyMode.BySpecies)
            {
                this._species_LBL.Visible = true;
                this._speciesSelect_CB.Visible = true;
                this._speciesSelect_CB.DisplayMember = "Species";
                this._speciesSelect_CB.ValueMember = "Tag";

                foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                {
                    TallyDO t = this.Controller.DataStore.From<TallyDO>()
                        .Join("CountTree", "USING (Tally_CN)")
                        .Where("SampleGroup_CN = ? and TreeDefaultValue_CN = ?")
                        .Read(sg.SampleGroup_CN, tdv.TreeDefaultValue_CN).FirstOrDefault();
                    if (t == null)
                    {
                        t = new TallyDO(this.Controller.DataStore);
                        t.Description = tdv.Species;
                    }
                    tdv.Tag = t;
                }

                this._speciesSelect_CB.DataSource = sg.TreeDefaultValues;
            }
            else
            {
            }

            return this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this.DialogResult == DialogResult.Cancel)
            {
                return;
            }
            else if (this.DialogResult == DialogResult.OK)
            {
                this._BS_Tally.EndEdit();
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.Controller.DataStore.BeginTransaction();
                    if ((this.SampleGroup.TallyMethod & CruiseDAL.Enums.TallyMode.BySampleGroup) == CruiseDAL.Enums.TallyMode.BySampleGroup)
                    {
                        if (this.Tally.IsPersisted == false)
                        {
                            TallyDO t = this.Controller.DataStore.From<TallyDO>()
                                .Where("Description = ? and Hotkey = ?")
                                .Read(this.Tally.Description
                                , this.Tally.Hotkey).FirstOrDefault();
                            if (t != null) { this.Tally = t; }
                            else
                            {
                                this.Tally.Save();
                            }
                        }

                        string createCountsCommand = String.Format(@"INSERT OR IGNORE INTO CountTree (CuttingUnit_CN, SampleGroup_CN,  CreatedBy)
                        Select CuttingUnitStratum.CuttingUnit_CN, SampleGroup.SampleGroup_CN,  '{0}' AS CreatedBy
                        From SampleGroup
                        INNER JOIN CuttingUnitStratum
                        ON SampleGroup.Stratum_CN = CuttingUnitStratum.Stratum_CN
                        WHERE SampleGroup.SampleGroup_CN = {1};", this.Controller.DataStore.User, this.SampleGroup.SampleGroup_CN);
                        this.Controller.DataStore.Execute(createCountsCommand);
                        string setTallyRefCommand = String.Format("UPDATE CountTree SET Tally_CN = {0} WHERE SampleGroup_CN = {1} AND ifnull(TreeDefaultValue_CN, 0) = 0",
                            this.Tally.Tally_CN, this.SampleGroup.SampleGroup_CN);
                        this.Controller.DataStore.Execute(setTallyRefCommand);
                    }
                    else if ((this.SampleGroup.TallyMethod & CruiseDAL.Enums.TallyMode.BySpecies) == CruiseDAL.Enums.TallyMode.BySpecies)
                    {
                        string creatCountsBySpeciesCommand = String.Format(@"INSERT  OR IGNORE INTO CountTree (CuttingUnit_CN, SampleGroup_CN, TreeDefaultValue_CN, CreatedBy)
                        Select CuttingUnitStratum.CuttingUnit_CN, SampleGroup.SampleGroup_CN, SampleGroupTreeDefaultValue.TreeDefaultValue_CN, '{0}' AS CreatedBy
                        From SampleGroup
                        INNER JOIN CuttingUnitStratum
                        ON SampleGroup.Stratum_CN = CuttingUnitStratum.Stratum_CN
                        INNER JOIN SampleGroupTreeDefaultValue
                        ON SampleGroupTreeDefaultValue.SampleGroup_CN = SampleGroup.SampleGroup_CN
                        WHERE SampleGroup.SampleGroup_CN = {1};", this.Controller.DataStore.User, this.SampleGroup.SampleGroup_CN);
                        this.Controller.DataStore.Execute(creatCountsBySpeciesCommand);

                        foreach (TreeDefaultValueDO tdv in this.SampleGroup.TreeDefaultValues)
                        {
                            TallyDO tally = tdv.Tag as TallyDO;
                            if (tally == null)
                            {
                                e.Cancel = true;
                                return;
                            }

                            if (tally.IsPersisted == false)
                            {
                                TallyDO t = this.Controller.DataStore.From<TallyDO>()
                                    .Where("Description = ? and Hotkey = ?")
                                    .Read(tally.Description, tally.Hotkey).FirstOrDefault();
                                if (t != null) { tally = t; }
                                else
                                {
                                    tally.Save();
                                }
                            }

                            string setTallyRefCommand = String.Format("UPDATE CountTree SET Tally_CN = {0} WHERE SampleGroup_CN = {1} AND TreeDefaultValue_CN = {2}",
                                tally.Tally_CN, this.SampleGroup.SampleGroup_CN, tdv.TreeDefaultValue_CN);
                            this.Controller.DataStore.Execute(setTallyRefCommand);
                        }
                    }
                    this.Controller.DataStore.CommitTransaction();
                }
                catch (Exception ex)
                {
                    this.Controller.DataStore.RollbackTransaction();
                    this.Controller.HandleNonCriticalException(ex, "Some thing went wrong while saving");
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void _speciesSelect_CB_SelectedValueChanged(object sender, EventArgs e)
        {
            TallyDO tally = _speciesSelect_CB.SelectedValue as TallyDO;
            if (tally == null) { return; }
            this.Tally = tally;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}