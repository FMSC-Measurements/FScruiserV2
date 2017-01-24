namespace FSCruiser.WinForms
{
    partial class FormTallySetup
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this._sgInfo_LBL = new System.Windows.Forms.Label();
            this._speciesSelect_CB = new System.Windows.Forms.ComboBox();
            this._species_LBL = new System.Windows.Forms.Label();
            this._mainContentPanel = new System.Windows.Forms.Panel();
            this._hotkey_TB = new System.Windows.Forms.TextBox();
            this._BS_Tally = new System.Windows.Forms.BindingSource(this.components);
            this.lbl1 = new System.Windows.Forms.Label();
            this._description_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._ce_menuPanel = new System.Windows.Forms.Panel();
            this._ce_cancel_BTN = new System.Windows.Forms.Button();
            this._ce_ok_BTN = new System.Windows.Forms.Button();
            this._mainContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Tally)).BeginInit();
            this._ce_menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Cancel";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.Text = "Sample Group:";
            // 
            // _sgInfo_LBL
            // 
            this._sgInfo_LBL.Dock = System.Windows.Forms.DockStyle.Top;
            this._sgInfo_LBL.Location = new System.Drawing.Point(0, 20);
            this._sgInfo_LBL.Name = "_sgInfo_LBL";
            this._sgInfo_LBL.Size = new System.Drawing.Size(240, 20);
            this._sgInfo_LBL.Text = "<sample group info>";
            // 
            // _speciesSelect_CB
            // 
            this._speciesSelect_CB.Dock = System.Windows.Forms.DockStyle.Top;
            this._speciesSelect_CB.Location = new System.Drawing.Point(0, 60);
            this._speciesSelect_CB.Name = "_speciesSelect_CB";
            this._speciesSelect_CB.Size = new System.Drawing.Size(240, 23);
            this._speciesSelect_CB.TabIndex = 1;
            this._speciesSelect_CB.SelectedValueChanged += new System.EventHandler(this._speciesSelect_CB_SelectedValueChanged);
            // 
            // _species_LBL
            // 
            this._species_LBL.Dock = System.Windows.Forms.DockStyle.Top;
            this._species_LBL.Location = new System.Drawing.Point(0, 40);
            this._species_LBL.Name = "_species_LBL";
            this._species_LBL.Size = new System.Drawing.Size(240, 20);
            this._species_LBL.Text = "Species:";
            // 
            // _mainContentPanel
            // 
            this._mainContentPanel.Controls.Add(this._hotkey_TB);
            this._mainContentPanel.Controls.Add(this.lbl1);
            this._mainContentPanel.Controls.Add(this._description_TB);
            this._mainContentPanel.Controls.Add(this.label2);
            this._mainContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContentPanel.Location = new System.Drawing.Point(0, 83);
            this._mainContentPanel.Name = "_mainContentPanel";
            this._mainContentPanel.Size = new System.Drawing.Size(240, 160);
            // 
            // _hotkey_TB
            // 
            this._hotkey_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Tally, "Hotkey", true));
            this._hotkey_TB.Location = new System.Drawing.Point(86, 27);
            this._hotkey_TB.MaxLength = 1;
            this._hotkey_TB.Name = "_hotkey_TB";
            this._hotkey_TB.Size = new System.Drawing.Size(100, 23);
            this._hotkey_TB.TabIndex = 3;
            // 
            // _BS_Tally
            // 
            this._BS_Tally.DataSource = typeof(CruiseDAL.DataObjects.TallyDO);
            // 
            // lbl1
            // 
            this.lbl1.Location = new System.Drawing.Point(4, 28);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(76, 20);
            this.lbl1.Text = "Hot Key";
            // 
            // _description_TB
            // 
            this._description_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Tally, "Description", true));
            this._description_TB.Location = new System.Drawing.Point(86, 3);
            this._description_TB.Name = "_description_TB";
            this._description_TB.Size = new System.Drawing.Size(100, 23);
            this._description_TB.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.Text = "Description";
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
            this._ce_cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ce_cancel_BTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._ce_cancel_BTN.Location = new System.Drawing.Point(0, 0);
            this._ce_cancel_BTN.Name = "_ce_cancel_BTN";
            this._ce_cancel_BTN.Size = new System.Drawing.Size(120, 25);
            this._ce_cancel_BTN.TabIndex = 5;
            this._ce_cancel_BTN.Text = "Cancel";
            // 
            // _ce_ok_BTN
            // 
            this._ce_ok_BTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ce_ok_BTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._ce_ok_BTN.Location = new System.Drawing.Point(120, 0);
            this._ce_ok_BTN.Name = "_ce_ok_BTN";
            this._ce_ok_BTN.Size = new System.Drawing.Size(120, 25);
            this._ce_ok_BTN.TabIndex = 4;
            this._ce_ok_BTN.Text = "OK";
            // 
            // FormTallySetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._mainContentPanel);
            this.Controls.Add(this._ce_menuPanel);
            this.Controls.Add(this._speciesSelect_CB);
            this.Controls.Add(this._species_LBL);
            this.Controls.Add(this._sgInfo_LBL);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "FormTallySetup";
            this.Text = "Tally Setup";
            this._mainContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_Tally)).EndInit();
            this._ce_menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _sgInfo_LBL;
        private System.Windows.Forms.ComboBox _speciesSelect_CB;
        private System.Windows.Forms.Label _species_LBL;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Panel _mainContentPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel _ce_menuPanel;
        private System.Windows.Forms.Button _ce_cancel_BTN;
        private System.Windows.Forms.Button _ce_ok_BTN;
        private System.Windows.Forms.TextBox _hotkey_TB;
        private System.Windows.Forms.BindingSource _BS_Tally;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.TextBox _description_TB;
    }
}