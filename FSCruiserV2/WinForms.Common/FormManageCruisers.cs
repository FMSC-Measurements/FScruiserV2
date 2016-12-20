using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormManageCruisers : Form
    {
        public IApplicationController Controller { get; private set; }

        public FormManageCruisers(IApplicationController controller)
        {
            InitializeComponent();
            this.Controller = controller;

#if NetCF
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Menu = null;
                this.mainMenu1.Dispose();
                this.mainMenu1 = null;
            }
#endif
        }

        protected override void OnLoad(EventArgs e)
        {
            this.UpdateCruiserList();
            this._enableCruiserPopupCB.Checked = this.Controller.Settings.EnableCruiserPopup;
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Controller.ViewController.HandleCruisersChanged();
        }

        private void UpdateCruiserList()
        {
            if (this._cruiserListContainer.Controls.Count != 0)
            {
                foreach (Control c in this._cruiserListContainer.Controls)
                {
                    c.Dispose();
                }
                this._cruiserListContainer.Controls.Clear();
            }

            foreach (Cruiser c in this.Controller.Settings.Cruisers)
            {
                MakeCruiserListItem(c, this._cruiserListContainer);
            }
        }

        private void MakeCruiserListItem(Cruiser cruiser, Control parent)
        {
            Panel panel = new Panel();
            panel.SuspendLayout();

            Label lbl = new Label();

            Button btn = new Button();

            panel.BackColor = System.Drawing.Color.Tan;
            panel.Controls.Add(lbl);
            panel.Controls.Add(btn);
            panel.Size = new System.Drawing.Size(240, 25);
            panel.Dock = System.Windows.Forms.DockStyle.Top;
            //panel.Location = new System.Drawing.Point(0, 0);
            //panel.Name = "panel3";

            btn.BackColor = System.Drawing.Color.Crimson;
            btn.Dock = System.Windows.Forms.DockStyle.Right;
            //this.button1.Location = new System.Drawing.Point(213, 0);
            //this.button1.Name = "button1";
            btn.Size = new System.Drawing.Size(27, 25);
            btn.TabIndex = 1;
            btn.Text = "X";
            btn.Click += new System.EventHandler(this._removeItemBTN_Click);

            lbl.Location = new System.Drawing.Point(12, 3);
            lbl.Name = "label2";
            lbl.Size = new System.Drawing.Size(48, 20);
            lbl.Text = cruiser.Initials;

            panel.Tag = cruiser;
            panel.Parent = parent;

#if NetCF
            FMSC.Controls.DpiHelper.AdjustControl(panel);
            FMSC.Controls.DpiHelper.AdjustControl(lbl);
            FMSC.Controls.DpiHelper.AdjustControl(btn);
#endif

            panel.ResumeLayout(false);
        }

        private void _addBTN_Click(object sender, EventArgs e)
        {
            OnAddCruiser();
        }

        protected void OnAddCruiser()
        {
            if (!String.IsNullOrEmpty(this._initialsTB.Text))
            {
                this.Controller.Settings.AddCruiser(this._initialsTB.Text);
                this.UpdateCruiserList();
                this._initialsTB.Text = String.Empty;
            }
        }

        private void _removeItemBTN_Click(object sender, EventArgs e)
        {
            Panel p = ((Panel)((Button)sender).Parent);
            Cruiser cruiser = (Cruiser)p.Tag;
            this.Controller.Settings.RemoveCruiser(cruiser);
            this.UpdateCruiserList();
        }

        private void _enableCruiserPopupCB_CheckStateChanged(object sender, EventArgs e)
        {
            this.Controller.Settings.EnableCruiserPopup = this._enableCruiserPopupCB.Checked;
        }

        private void _initialsTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.OnAddCruiser();
            }
        }
    }
}