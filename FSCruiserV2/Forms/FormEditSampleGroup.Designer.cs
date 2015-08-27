namespace FSCruiserV2.Forms
{
    partial class FormEditSampleGroup
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
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label lbl3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._cancel_MI = new System.Windows.Forms.MenuItem();
            this._BS_SampleGroup = new System.Windows.Forms.BindingSource(this.components);
            this._ceDialogPanel = new System.Windows.Forms.Panel();
            this._cancel_BTN = new System.Windows.Forms.Button();
            this._ok_BTN = new System.Windows.Forms.Button();
            this._mainContentPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._bigBaf_TB = new System.Windows.Forms.TextBox();
            this._kz_TB = new System.Windows.Forms.TextBox();
            this._insuranceFreq_TB = new System.Windows.Forms.TextBox();
            this._samplingFreq_TB = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this._systematicOpt_ChB = new System.Windows.Forms.CheckBox();
            this._manualTall_ChB = new System.Windows.Forms.CheckBox();
            this._tallyBySpecies_RB = new System.Windows.Forms.RadioButton();
            this._tallyBySg_RB = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this._uomCB = new System.Windows.Forms.ComboBox();
            this._defaultLD_CB = new System.Windows.Forms.ComboBox();
            this._sProdCB = new System.Windows.Forms.ComboBox();
            this._pProdCB = new System.Windows.Forms.ComboBox();
            this._cutLeaveCB = new System.Windows.Forms.ComboBox();
            this._codeTB = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lbl3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._BS_SampleGroup)).BeginInit();
            this._ceDialogPanel.SuspendLayout();
            this._mainContentPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(4, 79);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(100, 20);
            label8.Text = "BigBAF";
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(4, 55);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(100, 20);
            label7.Text = "KZ";
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(4, 28);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(100, 20);
            label6.Text = "Insurance Freq.";
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(4, 4);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(100, 20);
            label5.Text = "Sampling Freq.";
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(4, 128);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(83, 20);
            label9.Text = "UOM";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(4, 52);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(83, 20);
            label4.Text = "Default L/D";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(4, 104);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(83, 20);
            label3.Text = "2nd Prod";
            // 
            // lbl3
            // 
            lbl3.Location = new System.Drawing.Point(4, 80);
            lbl3.Name = "lbl3";
            lbl3.Size = new System.Drawing.Size(83, 20);
            lbl3.Text = "Primary Prod";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(4, 28);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 20);
            label2.Text = "Cut Leave";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(4, 4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 20);
            label1.Text = "SG Code";
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
            // _BS_SampleGroup
            // 
            this._BS_SampleGroup.DataSource = typeof(CruiseDAL.DataObjects.SampleGroupDO);
            // 
            // _ceDialogPanel
            // 
            this._ceDialogPanel.Controls.Add(this._cancel_BTN);
            this._ceDialogPanel.Controls.Add(this._ok_BTN);
            this._ceDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ceDialogPanel.Location = new System.Drawing.Point(0, 424);
            this._ceDialogPanel.Name = "_ceDialogPanel";
            this._ceDialogPanel.Size = new System.Drawing.Size(240, 25);
            // 
            // _cancel_BTN
            // 
            this._cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_BTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._cancel_BTN.Location = new System.Drawing.Point(0, 0);
            this._cancel_BTN.Name = "_cancel_BTN";
            this._cancel_BTN.Size = new System.Drawing.Size(120, 25);
            this._cancel_BTN.TabIndex = 16;
            this._cancel_BTN.Text = "Cancel";
            // 
            // _ok_BTN
            // 
            this._ok_BTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok_BTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._ok_BTN.Location = new System.Drawing.Point(120, 0);
            this._ok_BTN.Name = "_ok_BTN";
            this._ok_BTN.Size = new System.Drawing.Size(120, 25);
            this._ok_BTN.TabIndex = 15;
            this._ok_BTN.Text = "OK";
            // 
            // _mainContentPanel
            // 
            this._mainContentPanel.AutoScroll = true;
            this._mainContentPanel.Controls.Add(this.panel3);
            this._mainContentPanel.Controls.Add(this.panel4);
            this._mainContentPanel.Controls.Add(this.panel5);
            this._mainContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContentPanel.Location = new System.Drawing.Point(0, 0);
            this._mainContentPanel.Name = "_mainContentPanel";
            this._mainContentPanel.Size = new System.Drawing.Size(240, 424);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._bigBaf_TB);
            this.panel3.Controls.Add(label8);
            this.panel3.Controls.Add(this._kz_TB);
            this.panel3.Controls.Add(label7);
            this.panel3.Controls.Add(this._insuranceFreq_TB);
            this.panel3.Controls.Add(label6);
            this.panel3.Controls.Add(this._samplingFreq_TB);
            this.panel3.Controls.Add(label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 255);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(240, 113);
            // 
            // _bigBaf_TB
            // 
            this._bigBaf_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "BigBAF", true));
            this._bigBaf_TB.Location = new System.Drawing.Point(111, 77);
            this._bigBaf_TB.Name = "_bigBaf_TB";
            this._bigBaf_TB.Size = new System.Drawing.Size(83, 23);
            this._bigBaf_TB.TabIndex = 14;
            // 
            // _kz_TB
            // 
            this._kz_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "KZ", true));
            this._kz_TB.Location = new System.Drawing.Point(111, 55);
            this._kz_TB.Name = "_kz_TB";
            this._kz_TB.Size = new System.Drawing.Size(83, 23);
            this._kz_TB.TabIndex = 13;
            // 
            // _insuranceFreq_TB
            // 
            this._insuranceFreq_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "InsuranceFrequency", true));
            this._insuranceFreq_TB.Location = new System.Drawing.Point(111, 27);
            this._insuranceFreq_TB.Name = "_insuranceFreq_TB";
            this._insuranceFreq_TB.Size = new System.Drawing.Size(83, 23);
            this._insuranceFreq_TB.TabIndex = 12;
            // 
            // _samplingFreq_TB
            // 
            this._samplingFreq_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "SamplingFrequency", true));
            this._samplingFreq_TB.Location = new System.Drawing.Point(111, 3);
            this._samplingFreq_TB.Name = "_samplingFreq_TB";
            this._samplingFreq_TB.Size = new System.Drawing.Size(82, 23);
            this._samplingFreq_TB.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this._systematicOpt_ChB);
            this.panel4.Controls.Add(this._manualTall_ChB);
            this.panel4.Controls.Add(this._tallyBySpecies_RB);
            this.panel4.Controls.Add(this._tallyBySg_RB);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 166);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(240, 89);
            // 
            // _systematicOpt_ChB
            // 
            this._systematicOpt_ChB.Dock = System.Windows.Forms.DockStyle.Top;
            this._systematicOpt_ChB.Location = new System.Drawing.Point(0, 60);
            this._systematicOpt_ChB.Name = "_systematicOpt_ChB";
            this._systematicOpt_ChB.Size = new System.Drawing.Size(240, 20);
            this._systematicOpt_ChB.TabIndex = 10;
            this._systematicOpt_ChB.Text = "Use Systematic Sampling";
            // 
            // _manualTall_ChB
            // 
            this._manualTall_ChB.Dock = System.Windows.Forms.DockStyle.Top;
            this._manualTall_ChB.Location = new System.Drawing.Point(0, 40);
            this._manualTall_ChB.Name = "_manualTall_ChB";
            this._manualTall_ChB.Size = new System.Drawing.Size(240, 20);
            this._manualTall_ChB.TabIndex = 9;
            this._manualTall_ChB.Text = "Manual Tally";
            // 
            // _tallyBySpecies_RB
            // 
            this._tallyBySpecies_RB.Dock = System.Windows.Forms.DockStyle.Top;
            this._tallyBySpecies_RB.Location = new System.Drawing.Point(0, 20);
            this._tallyBySpecies_RB.Name = "_tallyBySpecies_RB";
            this._tallyBySpecies_RB.Size = new System.Drawing.Size(240, 20);
            this._tallyBySpecies_RB.TabIndex = 8;
            this._tallyBySpecies_RB.Text = "Tally by Species";
            // 
            // _tallyBySg_RB
            // 
            this._tallyBySg_RB.Dock = System.Windows.Forms.DockStyle.Top;
            this._tallyBySg_RB.Location = new System.Drawing.Point(0, 0);
            this._tallyBySg_RB.Name = "_tallyBySg_RB";
            this._tallyBySg_RB.Size = new System.Drawing.Size(240, 20);
            this._tallyBySg_RB.TabIndex = 7;
            this._tallyBySg_RB.Text = "Tally by Sample Group";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this._uomCB);
            this.panel5.Controls.Add(label9);
            this.panel5.Controls.Add(this._defaultLD_CB);
            this.panel5.Controls.Add(label4);
            this.panel5.Controls.Add(this._sProdCB);
            this.panel5.Controls.Add(label3);
            this.panel5.Controls.Add(this._pProdCB);
            this.panel5.Controls.Add(lbl3);
            this.panel5.Controls.Add(this._cutLeaveCB);
            this.panel5.Controls.Add(label2);
            this.panel5.Controls.Add(this._codeTB);
            this.panel5.Controls.Add(label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(240, 166);
            // 
            // _uomCB
            // 
            this._uomCB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "UOM", true));
            this._uomCB.Location = new System.Drawing.Point(93, 128);
            this._uomCB.Name = "_uomCB";
            this._uomCB.Size = new System.Drawing.Size(100, 23);
            this._uomCB.TabIndex = 6;
            // 
            // _defaultLD_CB
            // 
            this._defaultLD_CB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "DefaultLiveDead", true));
            this._defaultLD_CB.Items.Add("L");
            this._defaultLD_CB.Items.Add("D");
            this._defaultLD_CB.Location = new System.Drawing.Point(93, 50);
            this._defaultLD_CB.Name = "_defaultLD_CB";
            this._defaultLD_CB.Size = new System.Drawing.Size(100, 23);
            this._defaultLD_CB.TabIndex = 3;
            // 
            // _sProdCB
            // 
            this._sProdCB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "SecondaryProduct", true));
            this._sProdCB.Location = new System.Drawing.Point(93, 102);
            this._sProdCB.Name = "_sProdCB";
            this._sProdCB.Size = new System.Drawing.Size(100, 23);
            this._sProdCB.TabIndex = 5;
            // 
            // _pProdCB
            // 
            this._pProdCB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "PrimaryProduct", true));
            this._pProdCB.Location = new System.Drawing.Point(93, 78);
            this._pProdCB.Name = "_pProdCB";
            this._pProdCB.Size = new System.Drawing.Size(100, 23);
            this._pProdCB.TabIndex = 4;
            // 
            // _cutLeaveCB
            // 
            this._cutLeaveCB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "CutLeave", true));
            this._cutLeaveCB.Items.Add("C");
            this._cutLeaveCB.Items.Add("L");
            this._cutLeaveCB.Location = new System.Drawing.Point(93, 26);
            this._cutLeaveCB.Name = "_cutLeaveCB";
            this._cutLeaveCB.Size = new System.Drawing.Size(100, 23);
            this._cutLeaveCB.TabIndex = 2;
            // 
            // _codeTB
            // 
            this._codeTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_SampleGroup, "Code", true));
            this._codeTB.Location = new System.Drawing.Point(93, 3);
            this._codeTB.Name = "_codeTB";
            this._codeTB.Size = new System.Drawing.Size(100, 23);
            this._codeTB.TabIndex = 1;
            // 
            // FormEditSampleGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 449);
            this.Controls.Add(this._mainContentPanel);
            this.Controls.Add(this._ceDialogPanel);
            this.Menu = this.mainMenu1;
            this.Name = "FormEditSampleGroup";
            this.Text = "Sample Group";
            ((System.ComponentModel.ISupportInitialize)(this._BS_SampleGroup)).EndInit();
            this._ceDialogPanel.ResumeLayout(false);
            this._mainContentPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource _BS_SampleGroup;
        private System.Windows.Forms.MenuItem _cancel_MI;
        private System.Windows.Forms.Panel _ceDialogPanel;
        private System.Windows.Forms.Button _ok_BTN;
        private System.Windows.Forms.Panel _mainContentPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox _bigBaf_TB;
        private System.Windows.Forms.TextBox _kz_TB;
        private System.Windows.Forms.TextBox _insuranceFreq_TB;
        private System.Windows.Forms.TextBox _samplingFreq_TB;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox _systematicOpt_ChB;
        private System.Windows.Forms.CheckBox _manualTall_ChB;
        private System.Windows.Forms.RadioButton _tallyBySpecies_RB;
        private System.Windows.Forms.RadioButton _tallyBySg_RB;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox _defaultLD_CB;
        private System.Windows.Forms.ComboBox _sProdCB;
        private System.Windows.Forms.ComboBox _pProdCB;
        private System.Windows.Forms.ComboBox _cutLeaveCB;
        private System.Windows.Forms.TextBox _codeTB;
        private System.Windows.Forms.Button _cancel_BTN;
        private System.Windows.Forms.ComboBox _uomCB;
    }
}