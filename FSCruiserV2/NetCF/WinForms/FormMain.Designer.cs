using FSCruiser.Core.Models;
namespace FSCruiser.WinForms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Splitter splitter1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this._userInfoLBL = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this._menu_MI = new System.Windows.Forms.MenuItem();
            this._cruiseInfo_MI = new System.Windows.Forms.MenuItem();
            this._deviceInfo_MI = new System.Windows.Forms.MenuItem();
            this._backup_MI = new System.Windows.Forms.MenuItem();
            this._utilities_MI = new System.Windows.Forms.MenuItem();
            this._addPopulation_MI = new System.Windows.Forms.MenuItem();
            this._about_MI = new System.Windows.Forms.MenuItem();
            this._manageCruisersMI = new System.Windows.Forms.MenuItem();
            this._recentFiles_MI = new System.Windows.Forms.MenuItem();
            this._dataEntryMI = new System.Windows.Forms.MenuItem();
            this.OpenButton = new System.Windows.Forms.Button();
            this._BS_cuttingUnits = new System.Windows.Forms.BindingSource(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this._strataView = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this._fileNameTB = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this._cuttingUnitCB = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this._BS_cuttingUnits)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Left;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 25);
            label1.Text = "Select Unit:";
            // 
            // splitter1
            // 
            splitter1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            splitter1.Location = new System.Drawing.Point(0, 17);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(240, 3);
            // 
            // _userInfoLBL
            // 
            this._userInfoLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this._userInfoLBL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._userInfoLBL.Location = new System.Drawing.Point(0, 0);
            this._userInfoLBL.Name = "_userInfoLBL";
            this._userInfoLBL.Size = new System.Drawing.Size(240, 19);
            this._userInfoLBL.Text = "Device Model and Serial Number";
            this._userInfoLBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this._menu_MI);
            this.mainMenu.MenuItems.Add(this._dataEntryMI);
            // 
            // _menu_MI
            // 
            this._menu_MI.MenuItems.Add(this._cruiseInfo_MI);
            this._menu_MI.MenuItems.Add(this._deviceInfo_MI);
            this._menu_MI.MenuItems.Add(this._backup_MI);
            this._menu_MI.MenuItems.Add(this._utilities_MI);
            this._menu_MI.MenuItems.Add(this._about_MI);
            this._menu_MI.MenuItems.Add(this._manageCruisersMI);
            this._menu_MI.MenuItems.Add(this._recentFiles_MI);
            this._menu_MI.Text = "Menu";
            this._menu_MI.Popup += new System.EventHandler(this._menu_MI_Popup);
            // 
            // _cruiseInfo_MI
            // 
            this._cruiseInfo_MI.Enabled = false;
            this._cruiseInfo_MI.Text = "&Cruise Info";
            this._cruiseInfo_MI.Click += new System.EventHandler(this._cruiseInfo_MI_Click);
            // 
            // _deviceInfo_MI
            // 
            this._deviceInfo_MI.Text = "&Device Info";
            this._deviceInfo_MI.Click += new System.EventHandler(this._deviceInfo_MI_Click);
            // 
            // _backup_MI
            // 
            this._backup_MI.Text = "&Backup";
            this._backup_MI.Click += new System.EventHandler(this.backupMI_Click);
            // 
            // _utilities_MI
            // 
            this._utilities_MI.MenuItems.Add(this._addPopulation_MI);
            this._utilities_MI.Text = "&Utilities";
            this._utilities_MI.Click += new System.EventHandler(this._utilities_MI_Click);
            // 
            // _addPopulation_MI
            // 
            this._addPopulation_MI.Text = "&Add Population";
            this._addPopulation_MI.Click += new System.EventHandler(this._addPopulation_MI_Click);
            // 
            // _about_MI
            // 
            this._about_MI.Text = "&About";
            this._about_MI.Click += new System.EventHandler(this._about_MI_Click);
            // 
            // _manageCruisersMI
            // 
            this._manageCruisersMI.Text = "&Manage Cruisers";
            this._manageCruisersMI.Click += new System.EventHandler(this._manageCruisersMI_Click);
            // 
            // _recentFiles_MI
            // 
            this._recentFiles_MI.Text = "Recent Files";
            // 
            // _dataEntryMI
            // 
            this._dataEntryMI.Text = "Data Entry";
            this._dataEntryMI.Click += new System.EventHandler(this.dataEntryButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OpenButton.Location = new System.Drawing.Point(0, 42);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(240, 20);
            this.OpenButton.TabIndex = 5;
            this.OpenButton.Text = "&Open Cruise File";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // _BS_cuttingUnits
            // 
            this._BS_cuttingUnits.DataSource = typeof(FSCruiser.Core.Models.CuttingUnitVM);
            this._BS_cuttingUnits.CurrentChanged += new System.EventHandler(this._BS_cuttingUnits_CurrentChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(splitter1);
            this.panel4.Controls.Add(this._strataView);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 87);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(240, 181);
            // 
            // _strataView
            // 
            this._strataView.AutoScroll = true;
            this._strataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._strataView.Location = new System.Drawing.Point(0, 17);
            this._strataView.Name = "_strataView";
            this._strataView.Size = new System.Drawing.Size(240, 164);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 17);
            this.label3.Text = "Strata";
            // 
            // _fileNameTB
            // 
            this._fileNameTB.Dock = System.Windows.Forms.DockStyle.Top;
            this._fileNameTB.Location = new System.Drawing.Point(0, 19);
            this._fileNameTB.Name = "_fileNameTB";
            this._fileNameTB.ReadOnly = true;
            this._fileNameTB.Size = new System.Drawing.Size(240, 23);
            this._fileNameTB.TabIndex = 10;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this._cuttingUnitCB);
            this.panel5.Controls.Add(label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 62);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(240, 25);
            // 
            // _cuttingUnitCB
            // 
            this._cuttingUnitCB.DataSource = this._BS_cuttingUnits;
            this._cuttingUnitCB.DisplayMember = "Code";
            this._cuttingUnitCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cuttingUnitCB.Enabled = false;
            this._cuttingUnitCB.Location = new System.Drawing.Point(75, 0);
            this._cuttingUnitCB.Name = "_cuttingUnitCB";
            this._cuttingUnitCB.Size = new System.Drawing.Size(165, 23);
            this._cuttingUnitCB.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this._fileNameTB);
            this.Controls.Add(this._userInfoLBL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "FScruiser Main";
            ((System.ComponentModel.ISupportInitialize)(this._BS_cuttingUnits)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.BindingSource _BS_cuttingUnits;
        private System.Windows.Forms.MenuItem _menu_MI;
        private System.Windows.Forms.MenuItem _cruiseInfo_MI;
        private System.Windows.Forms.MenuItem _deviceInfo_MI;
        private System.Windows.Forms.MenuItem _utilities_MI;
        private System.Windows.Forms.MenuItem _about_MI;
        private System.Windows.Forms.MenuItem _backup_MI;
        private System.Windows.Forms.MenuItem _addPopulation_MI;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _fileNameTB;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox _cuttingUnitCB;
        private System.Windows.Forms.Label _userInfoLBL;
        private System.Windows.Forms.MenuItem _manageCruisersMI;
        private System.Windows.Forms.MenuItem _dataEntryMI;
        private System.Windows.Forms.Panel _strataView;
        private System.Windows.Forms.MenuItem _recentFiles_MI;

    }
}