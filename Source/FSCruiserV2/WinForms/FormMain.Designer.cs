﻿namespace FSCruiser.WinForms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button ExampleButton;
            System.Windows.Forms.Panel panel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this._viewContentPanel = new System.Windows.Forms.Panel();
            this._viewNavPanel = new System.Windows.Forms.Panel();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this._cuttingUnitSelectView = new FSCruiser.WinForms.CuttingUnitSelectView();
            this.editCruisersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExampleButton = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this._viewContentPanel.SuspendLayout();
            this._viewNavPanel.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExampleButton
            // 
            ExampleButton.AutoSize = true;
            ExampleButton.BackColor = System.Drawing.Color.Yellow;
            ExampleButton.Dock = System.Windows.Forms.DockStyle.Top;
            ExampleButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            ExampleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            ExampleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ExampleButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ExampleButton.ForeColor = System.Drawing.SystemColors.ControlText;
            ExampleButton.Location = new System.Drawing.Point(0, 0);
            ExampleButton.Name = "ExampleButton";
            ExampleButton.Size = new System.Drawing.Size(147, 34);
            ExampleButton.TabIndex = 1;
            ExampleButton.Text = "<Example Button>";
            ExampleButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ExampleButton.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(this._viewContentPanel);
            panel1.Controls.Add(this._viewNavPanel);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 24);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(3);
            panel1.Size = new System.Drawing.Size(752, 442);
            panel1.TabIndex = 3;
            // 
            // _viewContentPanel
            // 
            this._viewContentPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_viewContentPanel.BackgroundImage")));
            this._viewContentPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._viewContentPanel.Controls.Add(this._cuttingUnitSelectView);
            this._viewContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewContentPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._viewContentPanel.Location = new System.Drawing.Point(154, 3);
            this._viewContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this._viewContentPanel.Name = "_viewContentPanel";
            this._viewContentPanel.Size = new System.Drawing.Size(595, 436);
            this._viewContentPanel.TabIndex = 2;
            // 
            // _viewNavPanel
            // 
            this._viewNavPanel.BackColor = System.Drawing.Color.Transparent;
            this._viewNavPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._viewNavPanel.Controls.Add(ExampleButton);
            this._viewNavPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._viewNavPanel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._viewNavPanel.Location = new System.Drawing.Point(3, 3);
            this._viewNavPanel.Name = "_viewNavPanel";
            this._viewNavPanel.Size = new System.Drawing.Size(151, 436);
            this._viewNavPanel.TabIndex = 1;
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.HandleOpenCruiseFileClick);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.editCruisersToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.DropDownOpened += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpened);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(752, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // _cuttingUnitSelectView
            // 
            this._cuttingUnitSelectView.Controller = null;
            this._cuttingUnitSelectView.Dock = System.Windows.Forms.DockStyle.Left;
            this._cuttingUnitSelectView.Location = new System.Drawing.Point(0, 0);
            this._cuttingUnitSelectView.Margin = new System.Windows.Forms.Padding(4);
            this._cuttingUnitSelectView.Name = "_cuttingUnitSelectView";
            this._cuttingUnitSelectView.Size = new System.Drawing.Size(200, 436);
            this._cuttingUnitSelectView.TabIndex = 0;
            // 
            // editCruisersToolStripMenuItem
            // 
            this.editCruisersToolStripMenuItem.Name = "editCruisersToolStripMenuItem";
            this.editCruisersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editCruisersToolStripMenuItem.Text = "Edit Cruisers";
            this.editCruisersToolStripMenuItem.Click += new System.EventHandler(this.editCruisersToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 466);
            this.Controls.Add(panel1);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FormMain";
            this.Text = "FScruiser - 2016.02.09";
            panel1.ResumeLayout(false);
            this._viewContentPanel.ResumeLayout(false);
            this._viewNavPanel.ResumeLayout(false);
            this._viewNavPanel.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Panel _viewNavPanel;
        private System.Windows.Forms.Panel _viewContentPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private CuttingUnitSelectView _cuttingUnitSelectView;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCruisersToolStripMenuItem;
    }
}