namespace FSCruiser.WinForms.DataEntry
{
    partial class FormLimitingDistance
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this._distanceTB = new System.Windows.Forms.TextBox();
            this._measureToCB = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this._limitingDistanceLBL = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this._treeIsLBL = new System.Windows.Forms.Label();
            this._bafTB = new System.Windows.Forms.TextBox();
            this._bafOrfpsLBL = new System.Windows.Forms.Label();
            this._slopeTB = new System.Windows.Forms.TextBox();
            this._dbhTB = new System.Windows.Forms.TextBox();
            this._sipPlaceholder = new System.Windows.Forms.Panel();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            this._calculateMI = new System.Windows.Forms.MenuItem();
            this._okBTN = new System.Windows.Forms.Button();
            this._ceControlPanel = new System.Windows.Forms.Panel();
            this._cancelBTN = new System.Windows.Forms.Button();
            this._calculateBTN = new System.Windows.Forms.Button();
            this._BS_calculator = new System.Windows.Forms.BindingSource(this.components);
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this._ceControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_calculator)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            label6.Dock = System.Windows.Forms.DockStyle.Left;
            label6.Location = new System.Drawing.Point(0, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(115, 35);
            label6.Text = "Limiting Distance:";
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(3, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(74, 18);
            label7.Text = "The Tree is:";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(135, 3);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(46, 20);
            label4.Text = "feet to";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(0, 3);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(99, 20);
            label3.Text = "Slope Distance:";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(32, 33);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 20);
            label2.Text = "Slope %:";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(55, 4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(33, 20);
            label1.Text = "DBH:";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this._bafTB);
            this.panel1.Controls.Add(this._bafOrfpsLBL);
            this.panel1.Controls.Add(this._slopeTB);
            this.panel1.Controls.Add(label2);
            this.panel1.Controls.Add(this._dbhTB);
            this.panel1.Controls.Add(label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 218);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this._distanceTB);
            this.panel4.Controls.Add(this._measureToCB);
            this.panel4.Controls.Add(label3);
            this.panel4.Controls.Add(label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 118);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(240, 31);
            // 
            // _distanceTB
            // 
            this._distanceTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "SlopeDistance", true));
            this._distanceTB.Location = new System.Drawing.Point(94, 0);
            this._distanceTB.Name = "_distanceTB";
            this._distanceTB.Size = new System.Drawing.Size(39, 23);
            this._distanceTB.TabIndex = 0;
            this._distanceTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _measureToCB
            // 
            this._measureToCB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "MeasureTo", true));
            this._measureToCB.Location = new System.Drawing.Point(176, 0);
            this._measureToCB.Name = "_measureToCB";
            this._measureToCB.Size = new System.Drawing.Size(53, 23);
            this._measureToCB.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._limitingDistanceLBL);
            this.panel2.Controls.Add(label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 149);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 35);
            // 
            // _limitingDistanceLBL
            // 
            this._limitingDistanceLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._limitingDistanceLBL.Location = new System.Drawing.Point(115, 0);
            this._limitingDistanceLBL.Name = "_limitingDistanceLBL";
            this._limitingDistanceLBL.Size = new System.Drawing.Size(125, 35);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(label7);
            this.panel3.Controls.Add(this._treeIsLBL);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 184);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(240, 34);
            // 
            // _treeIsLBL
            // 
            this._treeIsLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "TreeStatus", true));
            this._treeIsLBL.Location = new System.Drawing.Point(83, 0);
            this._treeIsLBL.Name = "_treeIsLBL";
            this._treeIsLBL.Size = new System.Drawing.Size(64, 20);
            // 
            // _bafTB
            // 
            this._bafTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "BAForFPSize", true));
            this._bafTB.Location = new System.Drawing.Point(94, 59);
            this._bafTB.Name = "_bafTB";
            this._bafTB.Size = new System.Drawing.Size(39, 23);
            this._bafTB.TabIndex = 2;
            this._bafTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _bafOrfpsLBL
            // 
            this._bafOrfpsLBL.Location = new System.Drawing.Point(56, 61);
            this._bafOrfpsLBL.Name = "_bafOrfpsLBL";
            this._bafOrfpsLBL.Size = new System.Drawing.Size(32, 20);
            this._bafOrfpsLBL.Text = "BAF:";
            // 
            // _slopeTB
            // 
            this._slopeTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "SlopePCT", true));
            this._slopeTB.Location = new System.Drawing.Point(94, 30);
            this._slopeTB.Name = "_slopeTB";
            this._slopeTB.Size = new System.Drawing.Size(39, 23);
            this._slopeTB.TabIndex = 1;
            this._slopeTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _dbhTB
            // 
            this._dbhTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_calculator, "DBH", true));
            this._dbhTB.Location = new System.Drawing.Point(94, 3);
            this._dbhTB.Name = "_dbhTB";
            this._dbhTB.Size = new System.Drawing.Size(39, 23);
            this._dbhTB.TabIndex = 0;
            this._dbhTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _sipPlaceholder
            // 
            this._sipPlaceholder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._sipPlaceholder.Location = new System.Drawing.Point(0, 218);
            this._sipPlaceholder.Name = "_sipPlaceholder";
            this._sipPlaceholder.Size = new System.Drawing.Size(240, 0);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this._cancelMI);
            this.mainMenu1.MenuItems.Add(this._calculateMI);
            // 
            // _cancelMI
            // 
            this._cancelMI.Text = "Cancel";
            this._cancelMI.Click += new System.EventHandler(this._cancelMI_Click);
            // 
            // _calculateMI
            // 
            this._calculateMI.Text = "Calculate";
            this._calculateMI.Click += new System.EventHandler(this._calculateBTN_Click);
            // 
            // _okBTN
            // 
            this._okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._okBTN.Location = new System.Drawing.Point(80, 0);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(80, 34);
            this._okBTN.TabIndex = 1;
            this._okBTN.Text = "OK";
            // 
            // _ceControlPanel
            // 
            this._ceControlPanel.Controls.Add(this._okBTN);
            this._ceControlPanel.Controls.Add(this._cancelBTN);
            this._ceControlPanel.Controls.Add(this._calculateBTN);
            this._ceControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ceControlPanel.Location = new System.Drawing.Point(0, 218);
            this._ceControlPanel.Name = "_ceControlPanel";
            this._ceControlPanel.Size = new System.Drawing.Size(240, 34);
            this._ceControlPanel.Visible = false;
            // 
            // _cancelBTN
            // 
            this._cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._cancelBTN.Location = new System.Drawing.Point(0, 0);
            this._cancelBTN.Name = "_cancelBTN";
            this._cancelBTN.Size = new System.Drawing.Size(80, 34);
            this._cancelBTN.TabIndex = 2;
            this._cancelBTN.Text = "Cancel";
            // 
            // _calculateBTN
            // 
            this._calculateBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._calculateBTN.Location = new System.Drawing.Point(160, 0);
            this._calculateBTN.Name = "_calculateBTN";
            this._calculateBTN.Size = new System.Drawing.Size(80, 34);
            this._calculateBTN.TabIndex = 0;
            this._calculateBTN.Text = "Calculate";
            this._calculateBTN.Click += new System.EventHandler(this._calculateBTN_Click);
            // 
            // _BS_calculator
            // 
            this._BS_calculator.DataSource = typeof(FSCruiser.Core.DataEntry.LimitingDistanceCalculator);
            // 
            // FormLimitingDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 252);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._sipPlaceholder);
            this.Controls.Add(this._ceControlPanel);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormLimitingDistance";
            this.Text = "Limiting Distance";
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this._ceControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_calculator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

              
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _limitingDistanceLBL;
        private System.Windows.Forms.Label _treeIsLBL;
        private System.Windows.Forms.TextBox _bafTB;
        private System.Windows.Forms.Label _bafOrfpsLBL;
        private System.Windows.Forms.ComboBox _measureToCB;
        private System.Windows.Forms.TextBox _distanceTB;
        private System.Windows.Forms.TextBox _slopeTB;
        private System.Windows.Forms.TextBox _dbhTB;
        private System.Windows.Forms.Panel _sipPlaceholder;
        private System.Windows.Forms.BindingSource _BS_calculator;


        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _cancelMI;
        private System.Windows.Forms.MenuItem _calculateMI;

        private Microsoft.WindowsCE.Forms.InputPanel _sip;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.Panel _ceControlPanel;
        private System.Windows.Forms.Button _cancelBTN;
        private System.Windows.Forms.Button _calculateBTN;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}