﻿using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormManageCruisers : Form
    {
        public ApplicationSettings Settings { get; set; }

        public FormManageCruisers(ApplicationSettings appSettings)
        {
            Settings = appSettings;
            InitializeComponent();

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
            this._enableCruiserPopupCB.Checked = Settings.EnableCruiserPopup;
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                Settings.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save settings");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Settings.NotifyCruisersChanged();
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

            foreach (Cruiser c in Settings.Cruisers)
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
            AddCruiser();
        }

        protected void AddCruiser()
        {
            if (!String.IsNullOrEmpty(this._initialsTB.Text))
            {
                Settings.AddCruiser(this._initialsTB.Text);
                this.UpdateCruiserList();
                this._initialsTB.Text = String.Empty;
            }
        }

        private void _removeItemBTN_Click(object sender, EventArgs e)
        {
            Panel p = ((Panel)((Button)sender).Parent);
            Cruiser cruiser = (Cruiser)p.Tag;
            Settings.RemoveCruiser(cruiser);
            this.UpdateCruiserList();
        }

        private void _enableCruiserPopupCB_CheckStateChanged(object sender, EventArgs e)
        {
            Settings.EnableCruiserPopup = this._enableCruiserPopupCB.Checked;
        }

        private void _initialsTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.AddCruiser();
            }
        }
    }
}