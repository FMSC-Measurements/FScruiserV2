﻿using System;
using System.Linq;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormMain : Form
    {
        public Panel ViewContentPanel { get { return this._viewContentPanel; } }

        public Panel ViewNavPanel { get { return this._viewNavPanel; } }

        #region Controller

        IApplicationController _controller;

        protected IApplicationController Controller
        {
            get { return _controller; }
            set
            {
                OnControllerChanging();
                _controller = value;
                OnControllerChanged();
            }
        }

        private void OnControllerChanged()
        {
            if (_controller != null)
            {
                _controller.FileStateChanged += HandleFileStateChanged;
            }
        }

        private void OnControllerChanging()
        {
            if (_controller != null)
            {
                _controller.FileStateChanged -= HandleFileStateChanged;
            }
        }

        #endregion Controller

        private Control _dataEntryButton;

        private CuttingUnitSelectView CuttingUnitSelectView
        {
            get
            {
                return this._cuttingUnitSelectView;
            }
        }

        protected FormMain()
        {
            InitializeComponent();

            this.Text = FSCruiser.Constants.APP_TITLE;

            this.ClearNavPanel();

            this._dataEntryButton = this.AddNavButton("Data Entry", this.HandleDataEntryClick);
            this._dataEntryButton.Enabled = false;

            this.AddNavButton("Open Cruise File", this.HandleOpenCruiseFileClick);
        }

        public FormMain(IApplicationController controller) : this()
        {
            this.Controller = controller;
            this._cuttingUnitSelectView.Controller = controller;
        }

        public Control AddNavButton(String text, EventHandler eventHandler)
        {
            Button newNavButton = new Button();
            newNavButton.AutoSize = true;
            newNavButton.BackColor = System.Drawing.Color.Yellow;
            newNavButton.Dock = System.Windows.Forms.DockStyle.Top;
            newNavButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            newNavButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            newNavButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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

        protected void HandleFileStateChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HandleFileStateChanged));
            }
            else
            {
                var controller = Controller;
                if (controller.DataStore != null && controller.DataStore.Exists)
                {
                    var fileName = System.IO.Path.GetFileName(Controller.DataStore.Path);
                    this._dataEntryButton.Enabled = true;
                    Text = "FScruiser - " + fileName;
                }
                else
                {
                    this._dataEntryButton.Enabled = false;
                    Text = FSCruiser.Constants.APP_TITLE;
                }
                CuttingUnitSelectView.HandleFileStateChanged();
            }
        }

        public void ClearNavPanel()
        {
            this.ViewNavPanel.Controls.Clear();
        }

        public void HandleOpenCruiseFileClick(Object sender, EventArgs e)
        {
            this.Controller.OpenFile();
        }

        public void HandleDataEntryClick(Object sender, EventArgs e)
        {
            var unit = this.CuttingUnitSelectView.SelectedUnit;
            if (unit != null)
            {
                Controller.ViewController.ShowDataEntry(this.CuttingUnitSelectView.SelectedUnit);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controller.ViewController.ShowAbout();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            recentToolStripMenuItem.DropDownItems.Clear();

            ToolStripMenuItem[] items =
                ApplicationSettings.Instance.RecentProjects.Select(
                r => new ToolStripMenuItem(r.ProjectName, null, recentFileSelected)
                {
                    ToolTipText = r.FilePath
                }
                ).ToArray();

            recentToolStripMenuItem.DropDownItems.AddRange(items);
        }

        private void recentFileSelected(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            if (tsmi == null) { return; }
            if (sender != null)
            {
                var path = tsmi.ToolTipText;

                this.Controller.OpenFile(tsmi.ToolTipText);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var view = new FormSettings())
            {
                view.ShowDialog(this);
            }
        }

        private void editCruisersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var view = new FormManageCruisers(ApplicationSettings.Instance))
            {
                view.ShowDialog(this);
            }
        }
    }
}