namespace FSCruiserV2.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this._backupMI = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this._manageCruisersMI = new System.Windows.Forms.MenuItem();
            this._dataEntryMI = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
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
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem3);
            this.mainMenu1.MenuItems.Add(this._dataEntryMI);
            // 
            // menuItem3
            // 
            this.menuItem3.MenuItems.Add(this.menuItem4);
            this.menuItem3.MenuItems.Add(this.menuItem5);
            this.menuItem3.MenuItems.Add(this._backupMI);
            this.menuItem3.MenuItems.Add(this.menuItem6);
            this.menuItem3.MenuItems.Add(this.menuItem7);
            this.menuItem3.MenuItems.Add(this._manageCruisersMI);
            this.menuItem3.Text = "Menu";
            // 
            // menuItem4
            // 
            this.menuItem4.Enabled = false;
            this.menuItem4.Text = "&Cruise Info";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "&Device Info";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // _backupMI
            // 
            this._backupMI.Text = "&Backup";
            this._backupMI.Click += new System.EventHandler(this.backupMI_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.MenuItems.Add(this.menuItem9);
            this.menuItem6.MenuItems.Add(this.menuItem11);
            this.menuItem6.Text = "&Utilities";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Text = "&Add Population";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Enabled = false;
            this.menuItem11.Text = "&Supervisor Mode";
            // 
            // menuItem7
            // 
            this.menuItem7.Text = "&About";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // _manageCruisersMI
            // 
            this._manageCruisersMI.Text = "&Manage Cruisers";
            this._manageCruisersMI.Click += new System.EventHandler(this._manageCruisersMI_Click);
            // 
            // _dataEntryMI
            // 
            this._dataEntryMI.Text = "Data Entry";
            this._dataEntryMI.Click += new System.EventHandler(this.dataEntryButton_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Units";
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.Text = "View";
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
            this._BS_cuttingUnits.DataSource = typeof(FSCruiserV2.Logic.CuttingUnitVM);
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
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "FScruiser Main";
            ((System.ComponentModel.ISupportInitialize)(this._BS_cuttingUnits)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.BindingSource _BS_cuttingUnits;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem _backupMI;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _fileNameTB;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox _cuttingUnitCB;
        private System.Windows.Forms.Label _userInfoLBL;
        private System.Windows.Forms.MenuItem _manageCruisersMI;
        private System.Windows.Forms.MenuItem _dataEntryMI;
        private System.Windows.Forms.Panel _strataView;

    }
}