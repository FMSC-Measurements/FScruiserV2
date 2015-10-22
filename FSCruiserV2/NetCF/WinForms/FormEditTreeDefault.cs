using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public partial class FormEditTreeDefault : Form
    {
        private bool _viewLoading; 
        //private TreeDefaultValueDO _treeDefaultValue;
        public TreeDefaultValueDO TreeDefaultValue
        {
            get
            {
                return (TreeDefaultValueDO)this._BS_TreeDefaultValue.DataSource;
            }
            set
            {
                this._BS_TreeDefaultValue.DataSource = value;
            }
        }

        public IApplicationController Controller { get; protected set; }

        public FormEditTreeDefault(IApplicationController controller)
        {
            this.Controller = controller;
            InitializeComponent();
            this._pProd_CB.DataSource = Constants.PRODUCT_CODES;

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

        public DialogResult ShowDialog(TreeDefaultValueDO tdv)
        {
            this._viewLoading = true;
            this.TreeDefaultValue = tdv;

            this._primaryFieldsPanel.Enabled = !tdv.IsPersisted;

            return this.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this._viewLoading = false;
        }

        protected void EndEdit()
        {
            this._BS_TreeDefaultValue.EndEdit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.EndEdit();
            if (this.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            if (this.TreeDefaultValue.IsPersisted == false)
            {
                if (this.HasUniqueConflict(this.TreeDefaultValue))
                {
                    MessageBox.Show("Tree Default values entered conflict with existing Tree Default.\r\n" +
                        "Please change Species, Primary Product, Chargeable or Live Dead values");
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void OnUniqueChanged()
        {
            if (this._viewLoading) { return; }
            if (this.TreeDefaultValue.IsPersisted == false)
            {
                if (this.HasUniqueConflict(this.TreeDefaultValue))
                {
                    MessageBox.Show("Tree Default values entered conflict with existing Tree Default.\r\n" +
                        "Please change Species, Primary Product, Chargeable or Live Dead values");
                }
            }
        }

        private bool HasUniqueConflict(TreeDefaultValueDO tdv)
        {
            return (this.Controller._cDal.GetRowCount(CruiseDAL.Schema.TREEDEFAULTVALUE._NAME,
                "WHERE PrimaryProduct = ? AND Species = ? AND LiveDead = ?",
                    tdv.PrimaryProduct, tdv.Species, tdv.LiveDead) > 0);
        }


        private void FormEditTreeDefault_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }

        private void _cancel_MI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}