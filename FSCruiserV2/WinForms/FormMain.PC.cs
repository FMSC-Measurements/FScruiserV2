using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormMain : Form
    {
        public Panel ViewContentPanel { get { return this._viewContentPanel; } }
        public Panel ViewNavPanel { get { return this._viewNavPanel; } }

        public IApplicationController Controller { get; protected set; }

        private Control _dataEntryButton;

        private CuttingUnitSelectView CuttingUnitSelectView
        {
            get
            {

                return this._cuttingUnitSelectView;
            }
        }


        public FormMain(IApplicationController controller)
        {
            this.Controller = controller;
            InitializeComponent();
            this.ClearNavPanel();
            
            this._dataEntryButton = this.AddNavButton("Data Entry", this.HandleDataEntryClick);
            this._dataEntryButton.Enabled = false;
            this.AddNavButton("Open Cruise File", this.HandleOpenCruiseFileClick);

            this._cuttingUnitSelectView.Controller = controller;
        }


        public Control AddNavButton(String text, EventHandler eventHandler)
        {

            Button newNavButton = new Button();
            newNavButton.AutoSize = true;
            newNavButton.BackColor = System.Drawing.Color.Yellow;
            newNavButton.Dock = System.Windows.Forms.DockStyle.Top;
            newNavButton.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            newNavButton.FlatAppearance.BorderSize = 2;
            newNavButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            //newNavButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            newNavButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            newNavButton.ForeColor = System.Drawing.SystemColors.ControlText;
            newNavButton.Location = new System.Drawing.Point(0, 0);
            newNavButton.Size = new System.Drawing.Size(200, 35);
            newNavButton.Text = text;
            newNavButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            newNavButton.UseVisualStyleBackColor = false;
            newNavButton.Click += eventHandler;

            newNavButton.Parent = this._viewNavPanel;
            return newNavButton;
        }

        public void ClearNavPanel()
        {
            this.ViewNavPanel.Controls.Clear();
        }

        public void HandleOpenCruiseFileClick(Object sender, EventArgs e)
        {
            this._dataEntryButton.Enabled = this.Controller.OpenFile();
            this.CuttingUnitSelectView.OnCuttingUnitsChanged();
        }

        public void HandleDataEntryClick(Object sender, EventArgs e)
        {
            CuttingUnitVM unit = this.CuttingUnitSelectView.SelectedUnit;
            if (unit != null)
            {
                this.Controller.LoadCuttingUnit(unit);
                this.Controller.ViewController.ShowDataEntry(this.CuttingUnitSelectView.SelectedUnit);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controller.ViewController.ShowAbout();
        }
    }
}
