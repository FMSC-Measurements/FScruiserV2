namespace FSCruiser.WinForms
{
    partial class FormAddPopulation
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
            this._stratumLBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._cancel_MI = new System.Windows.Forms.MenuItem();
            this._populationSelectPanel = new System.Windows.Forms.Panel();
            this._editSG_BTN = new System.Windows.Forms.Button();
            this._BS_SampleGroups = new System.Windows.Forms.BindingSource(this.components);
            this._sampleGroupCB = new System.Windows.Forms.ComboBox();
            this._strataCB = new System.Windows.Forms.ComboBox();
            this._mainPanel = new System.Windows.Forms.Panel();
            this._speciesSelectPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this._ce_menuPanel = new System.Windows.Forms.Panel();
            this._ce_cancel_BTN = new System.Windows.Forms.Button();
            this._ce_ok_BTN = new System.Windows.Forms.Button();
            this._BS_population = new System.Windows.Forms.BindingSource(this.components);
            this._editTallySetup_BTN = new System.Windows.Forms.Button();
            this._addTDV_BTN = new System.Windows.Forms.Button();
            this._populationSelectPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_SampleGroups)).BeginInit();
            this._mainPanel.SuspendLayout();
            this._ce_menuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_population)).BeginInit();
            this.SuspendLayout();
            // 
            // _stratumLBL
            // 
            this._stratumLBL.Location = new System.Drawing.Point(3, 11);
            this._stratumLBL.Name = "_stratumLBL";
            this._stratumLBL.Size = new System.Drawing.Size(49, 20);
            this._stratumLBL.Text = "Stratum";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Sample Group";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this._cancel_MI);
            // 
            // _cancel_MI
            // 
            this._cancel_MI.Text = "Cancel";
            this._cancel_MI.Click += new System.EventHandler(this._cancel_MI_Click);
            // 
            // _populationSelectPanel
            // 
            this._populationSelectPanel.Controls.Add(this._editSG_BTN);
            this._populationSelectPanel.Controls.Add(this._sampleGroupCB);
            this._populationSelectPanel.Controls.Add(this.label1);
            this._populationSelectPanel.Controls.Add(this._strataCB);
            this._populationSelectPanel.Controls.Add(this._stratumLBL);
            this._populationSelectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._populationSelectPanel.Location = new System.Drawing.Point(0, 0);
            this._populationSelectPanel.Name = "_populationSelectPanel";
            this._populationSelectPanel.Size = new System.Drawing.Size(240, 64);
            // 
            // _editSG_BTN
            // 
            this._editSG_BTN.Location = new System.Drawing.Point(154, 34);
            this._editSG_BTN.Name = "_editSG_BTN";
            this._editSG_BTN.Size = new System.Drawing.Size(42, 20);
            this._editSG_BTN.TabIndex = 3;
            this._editSG_BTN.Text = "Edit";
            this._editSG_BTN.Click += new System.EventHandler(this._editSG_BTN_Click);
            // 
            // _BS_SampleGroups
            // 
            this._BS_SampleGroups.DataSource = typeof(CruiseDAL.DataObjects.SampleGroupDO);
            // 
            // _sampleGroupCB
            // 
            this._sampleGroupCB.DataSource = this._BS_SampleGroups;
            this._sampleGroupCB.DisplayMember = "Code";
            this._sampleGroupCB.Location = new System.Drawing.Point(99, 32);
            this._sampleGroupCB.Name = "_sampleGroupCB";
            this._sampleGroupCB.Size = new System.Drawing.Size(49, 23);
            this._sampleGroupCB.TabIndex = 2;
            this._sampleGroupCB.ValueMember = "Self";
            this._sampleGroupCB.SelectedIndexChanged += new System.EventHandler(this._sampleGroupCB_SelectedIndexChanged);
            // 
            // _strataCB
            // 
            this._strataCB.DisplayMember = "Code";
            this._strataCB.Location = new System.Drawing.Point(99, 9);
            this._strataCB.Name = "_strataCB";
            this._strataCB.Size = new System.Drawing.Size(49, 23);
            this._strataCB.TabIndex = 1;
            this._strataCB.ValueMember = "Self";
            this._strataCB.SelectedIndexChanged += new System.EventHandler(this._strataCB_SelectedIndexChanged);
            // 
            // _mainPanel
            // 
            this._mainPanel.AutoScroll = true;
            this._mainPanel.Controls.Add(this._speciesSelectPanel);
            this._mainPanel.Controls.Add(this.label2);
            this._mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainPanel.Location = new System.Drawing.Point(0, 64);
            this._mainPanel.Name = "_mainPanel";
            this._mainPanel.Size = new System.Drawing.Size(240, 139);
            // 
            // _speciesSelectPanel
            // 
            this._speciesSelectPanel.AutoScroll = true;
            this._speciesSelectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._speciesSelectPanel.Location = new System.Drawing.Point(0, 20);
            this._speciesSelectPanel.Name = "_speciesSelectPanel";
            this._speciesSelectPanel.Size = new System.Drawing.Size(240, 119);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 20);
            this.label2.Text = "Tree Defaults:";
            // 
            // _ce_menuPanel
            // 
            this._ce_menuPanel.Controls.Add(this._ce_cancel_BTN);
            this._ce_menuPanel.Controls.Add(this._ce_ok_BTN);
            this._ce_menuPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ce_menuPanel.Location = new System.Drawing.Point(0, 243);
            this._ce_menuPanel.Name = "_ce_menuPanel";
            this._ce_menuPanel.Size = new System.Drawing.Size(240, 25);
            // 
            // _ce_cancel_BTN
            // 
            this._ce_cancel_BTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._ce_cancel_BTN.Location = new System.Drawing.Point(0, 0);
            this._ce_cancel_BTN.Name = "_ce_cancel_BTN";
            this._ce_cancel_BTN.Size = new System.Drawing.Size(120, 25);
            this._ce_cancel_BTN.TabIndex = 7;
            this._ce_cancel_BTN.Text = "Cancel";
            // 
            // _ce_ok_BTN
            // 
            this._ce_ok_BTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._ce_ok_BTN.Location = new System.Drawing.Point(120, 0);
            this._ce_ok_BTN.Name = "_ce_ok_BTN";
            this._ce_ok_BTN.Size = new System.Drawing.Size(120, 25);
            this._ce_ok_BTN.TabIndex = 6;
            this._ce_ok_BTN.Text = "OK";
            // 
            // _BS_population
            // 
            this._BS_population.DataSource = typeof(CruiseDAL.DataObjects.TreeDefaultValueDO);
            // 
            // _editTallySetup_BTN
            // 
            this._editTallySetup_BTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._editTallySetup_BTN.Location = new System.Drawing.Point(0, 223);
            this._editTallySetup_BTN.Name = "_editTallySetup_BTN";
            this._editTallySetup_BTN.Size = new System.Drawing.Size(240, 20);
            this._editTallySetup_BTN.TabIndex = 5;
            this._editTallySetup_BTN.Text = "Edit Tally Setup";
            this._editTallySetup_BTN.Click += new System.EventHandler(this._editTallySetup_BTN_Click);
            // 
            // _addTDV_BTN
            // 
            this._addTDV_BTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._addTDV_BTN.Location = new System.Drawing.Point(0, 203);
            this._addTDV_BTN.Name = "_addTDV_BTN";
            this._addTDV_BTN.Size = new System.Drawing.Size(240, 20);
            this._addTDV_BTN.TabIndex = 4;
            this._addTDV_BTN.Text = "Add New Tree Default";
            this._addTDV_BTN.Click += new System.EventHandler(this._addTDV_BTN_Click);
            // 
            // FormAddPopulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._mainPanel);
            this.Controls.Add(this._addTDV_BTN);
            this.Controls.Add(this._editTallySetup_BTN);
            this.Controls.Add(this._ce_menuPanel);
            this.Controls.Add(this._populationSelectPanel);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormAddPopulation";
            this.Text = "Add a Population";
            this._populationSelectPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_SampleGroups)).EndInit();
            this._mainPanel.ResumeLayout(false);
            this._ce_menuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_population)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _populationSelectPanel;
        private System.Windows.Forms.ComboBox _sampleGroupCB;
        private System.Windows.Forms.ComboBox _strataCB;
        private System.Windows.Forms.BindingSource _BS_population;
        private System.Windows.Forms.Panel _mainPanel;
        private System.Windows.Forms.Panel _ce_menuPanel;
        private System.Windows.Forms.Button _ce_cancel_BTN;
        private System.Windows.Forms.Button _ce_ok_BTN;
        private System.Windows.Forms.MenuItem _cancel_MI;
        private System.Windows.Forms.Panel _speciesSelectPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _editSG_BTN;
        private System.Windows.Forms.BindingSource _BS_SampleGroups;
        private System.Windows.Forms.Button _editTallySetup_BTN;
        private System.Windows.Forms.Button _addTDV_BTN;
        private System.Windows.Forms.Label _stratumLBL;
        private System.Windows.Forms.Label label1;

    }
}