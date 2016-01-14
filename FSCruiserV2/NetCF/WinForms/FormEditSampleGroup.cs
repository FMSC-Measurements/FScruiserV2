using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public partial class FormEditSampleGroup : Form
    {
        private SampleGroupDO _sampleGroup;
        private readonly SampleGroupDO _initialState = new SampleGroupDO();
        private bool _allowEdit; 
        public SampleGroupDO SampleGroup
        {
            get
            {
                return _sampleGroup;
            }
            set
            {
                if (_sampleGroup != value)
                {
                    _sampleGroup = value;
                    this._BS_SampleGroup.DataSource = value;
                }
            }
        }

        public bool AllowEdit
        {
            get
            {
                return _allowEdit;
            }
            set
            {
                _allowEdit = value;
                this.OnAllowEditChanged();
            }
        }

        public FormEditSampleGroup()
        {

            InitializeComponent();
            this._pProdCB.DataSource = Constants.PRODUCT_CODES.Clone();
            this._sProdCB.DataSource = Constants.PRODUCT_CODES.Clone();
            this._uomCB.DataSource = Constants.UOM_CODES.Clone();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._ceDialogPanel.Visible = true;
            }
            else
            {
                this._ceDialogPanel.Visible = false;
            }
        }

        public DialogResult ShowDialog(SampleGroupDO sampleGroup, bool allowEdit)
        {
            bool isNew = !sampleGroup.IsPersisted;
            this.SampleGroup = sampleGroup;
            this.AllowEdit = allowEdit; 
            
            //TODO insted of reseting values from initial state could just reread from database
            this._initialState.SetValues(sampleGroup);

            if (isNew)
            {
                this.Text = "New Sample Group";
            }
            else if (allowEdit)
            {
                this.Text = "Edit Sample Group";
            }
            else
            {
                this.Text = "View Sample Group";
            }


            if ((this.SampleGroup.TallyMethod & CruiseDAL.Enums.TallyMode.BySampleGroup) == CruiseDAL.Enums.TallyMode.BySampleGroup)
            {
                this._tallyBySg_RB.Checked = true;
            }
            else if ((this.SampleGroup.TallyMethod & CruiseDAL.Enums.TallyMode.BySpecies) == CruiseDAL.Enums.TallyMode.BySpecies)
            {
                this._tallyBySpecies_RB.Checked = true;
            }
            this._systematicOpt_ChB.Checked = (sampleGroup.SampleSelectorType == CruiseDAL.Schema.CruiseMethods.SYSTEMATIC_SAMPLER_TYPE);
            this._systematicOpt_ChB.Enabled = this._tallyBySg_RB.Enabled = this._tallyBySpecies_RB.Enabled = CanEditTallyMode(sampleGroup);

            //if (isNew || allowEdit)
            //{
            //    StratumDO st = sampleGroup.Stratum;
            //    this._bigBaf_TB.Enabled = SampleGroupDO.CanEnableBigBAF(st);
            //    this._samplingFreq_TB = SampleGroupDO.CanEnableFrequency(st);
            //    this._insuranceFreq_TB.Enabled = SampleGroupDO.CanEnableIFreq(st);
            //    this._kz_TB.Enabled = SampleGroupDO.CanEnableKZ(st);
                
            //}

            return base.ShowDialog();

        }

        protected void OnAllowEditChanged()
        {
            
            this._codeTB.Enabled = _allowEdit;
            this._pProdCB.Enabled = _allowEdit;

            //this._cutLeaveCB.Enabled = _allowEdit;
            //this._defaultLD_CB.Enabled = _allowEdit;
            StratumDO st = this.SampleGroup.Stratum;
            this._bigBaf_TB.Enabled = _allowEdit && SampleGroupDO.CanEnableBigBAF(st);
            this._kz_TB.Enabled = _allowEdit && SampleGroupDO.CanEnableKZ(st);
            this._samplingFreq_TB.Enabled =  _allowEdit && SampleGroupDO.CanEnableFrequency(st);
            this._insuranceFreq_TB.Enabled = _allowEdit && SampleGroupDO.CanEnableIFreq(st);

            this._tallyBySg_RB.Enabled = this._tallyBySpecies_RB.Enabled = _allowEdit && FormEditSampleGroup.CanEditTallyMode(this.SampleGroup);
            this._systematicOpt_ChB.Enabled = _allowEdit && ((this.SampleGroup.Stratum.Method == CruiseDAL.Schema.CruiseMethods.STR)
                && SampleGroupDO.CanChangeSamplerType(this.SampleGroup));
        }

        public void EndEdit()
        {
            this._BS_SampleGroup.EndEdit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.DialogResult == DialogResult.Cancel)
            {
                this.SampleGroup.SetValues(this._initialState);
                return;
            }

            this.EndEdit();
            if(this.AllowEdit)
            {
                
                if(SampleGroupDO.CanChangeSamplerType(this.SampleGroup) && this._systematicOpt_ChB.Checked)// if systematic option checked, set SampleSelectorType to Systematic
                {
                    this.SampleGroup.SampleSelectorType = CruiseDAL.Schema.CruiseMethods.SYSTEMATIC_SAMPLER_TYPE;
                }
                if (FormEditSampleGroup.CanEditTallyMode(this.SampleGroup))
                {
                    if (this._tallyBySg_RB.Checked)
                    {
                        //TallyMethod = existing tallyMethod - TallyBySpecies + TallyBySampleGroup 
                        this.SampleGroup.TallyMethod = (this.SampleGroup.TallyMethod & ~CruiseDAL.Enums.TallyMode.BySpecies)
                            | CruiseDAL.Enums.TallyMode.BySampleGroup;
                    }
                    else if (this._tallyBySpecies_RB.Checked)
                    {
                        //TallyMethod = existing tallyMethod - TallyBySampleGroup + TallyBySpecies 
                        this.SampleGroup.TallyMethod = (this.SampleGroup.TallyMethod & ~CruiseDAL.Enums.TallyMode.BySampleGroup)
                            | CruiseDAL.Enums.TallyMode.BySpecies;
                    }
                }
            }

            

            this.SampleGroup.Validate();
            if (this.SampleGroup.HasErrors())
            {
                MessageBox.Show(this.SampleGroup.Error);
                e.Cancel = true;
                return;
            }

            try
            {
                this.SampleGroup.Save();
            }
            catch
            {
                MessageBox.Show("Unable to save sample group, make sture values are entered correctly");
                e.Cancel = true;
                return;
            }



        }

        private static bool CanEditTallyMode(SampleGroupDO sg)
        {
            if (!StratumDO.CanDefineTallys(sg.Stratum))
            {
                return false;
            }
            if (sg.DAL != null && sg.IsPersisted)
            {
                if (sg.DAL.GetRowCount("Tree", "WHERE SampleGroup_CN = ?", sg.SampleGroup_CN) > 0)
                {
                    return false;
                }
                else if (sg.DAL.GetRowCount("CountTree", "WHERE SampleGroup_CN = ? AND TreeCount > 0", sg.SampleGroup_CN) > 0)
                {
                    return false;
                }
            }
            return true;
        }




        private void _cancel_MI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}