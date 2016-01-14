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
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            this._calculateMI = new System.Windows.Forms.MenuItem();
            this._okBTN = new System.Windows.Forms.Button();
            this._ceControlPanel = new System.Windows.Forms.Panel();
            this._cancelBTN = new System.Windows.Forms.Button();
            this._calculateBTN = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._limitingDistanceLBL = new System.Windows.Forms.Label();
            this._treeIsLBL = new System.Windows.Forms.Label();
            this._bafTB = new System.Windows.Forms.TextBox();
            this._bafOrfpsLBL = new System.Windows.Forms.Label();
            this._measureToCB = new System.Windows.Forms.ComboBox();
            this._distanceTB = new System.Windows.Forms.TextBox();
            this._slopeTB = new System.Windows.Forms.TextBox();
            this._dbhTB = new System.Windows.Forms.TextBox();
            this._sipPlaceholder = new System.Windows.Forms.Panel();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this._ceControlPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(6, 108);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(127, 19);
            label6.Text = "Limiting Distance:";
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(41, 127);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(74, 18);
            label7.Text = "The Tree is:";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(139, 58);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(46, 20);
            label4.Text = "feet to";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(1, 58);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 20);
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
            this._okBTN.TabIndex = 15;
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
            this._cancelBTN.TabIndex = 17;
            this._cancelBTN.Text = "Cancel";
            // 
            // _calculateBTN
            // 
            this._calculateBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._calculateBTN.Location = new System.Drawing.Point(160, 0);
            this._calculateBTN.Name = "_calculateBTN";
            this._calculateBTN.Size = new System.Drawing.Size(80, 34);
            this._calculateBTN.TabIndex = 16;
            this._calculateBTN.Text = "Calculate";
            this._calculateBTN.Click += new System.EventHandler(this._calculateBTN_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this._limitingDistanceLBL);
            this.panel1.Controls.Add(label6);
            this.panel1.Controls.Add(this._treeIsLBL);
            this.panel1.Controls.Add(label7);
            this.panel1.Controls.Add(this._bafTB);
            this.panel1.Controls.Add(this._bafOrfpsLBL);
            this.panel1.Controls.Add(this._measureToCB);
            this.panel1.Controls.Add(label4);
            this.panel1.Controls.Add(this._distanceTB);
            this.panel1.Controls.Add(label3);
            this.panel1.Controls.Add(this._slopeTB);
            this.panel1.Controls.Add(label2);
            this.panel1.Controls.Add(this._dbhTB);
            this.panel1.Controls.Add(label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 218);
            // 
            // _limitingDistanceLBL
            // 
            this._limitingDistanceLBL.Location = new System.Drawing.Point(121, 108);
            this._limitingDistanceLBL.Name = "_limitingDistanceLBL";
            this._limitingDistanceLBL.Size = new System.Drawing.Size(117, 19);
            // 
            // _treeIsLBL
            // 
            this._treeIsLBL.Location = new System.Drawing.Point(121, 127);
            this._treeIsLBL.Name = "_treeIsLBL";
            this._treeIsLBL.Size = new System.Drawing.Size(64, 20);
            // 
            // _bafTB
            // 
            this._bafTB.Location = new System.Drawing.Point(94, 84);
            this._bafTB.Name = "_bafTB";
            this._bafTB.Size = new System.Drawing.Size(39, 23);
            this._bafTB.TabIndex = 23;
            this._bafTB.TextChanged += new System.EventHandler(this.input_TextChanged);
            this._bafTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _bafOrfpsLBL
            // 
            this._bafOrfpsLBL.Location = new System.Drawing.Point(56, 86);
            this._bafOrfpsLBL.Name = "_bafOrfpsLBL";
            this._bafOrfpsLBL.Size = new System.Drawing.Size(32, 20);
            this._bafOrfpsLBL.Text = "BAF:";
            // 
            // _measureToCB
            // 
            this._measureToCB.Items.Add("face");
            this._measureToCB.Items.Add("center");
            this._measureToCB.Location = new System.Drawing.Point(185, 55);
            this._measureToCB.Name = "_measureToCB";
            this._measureToCB.Size = new System.Drawing.Size(53, 23);
            this._measureToCB.TabIndex = 22;
            this._measureToCB.TextChanged += new System.EventHandler(this.input_TextChanged);
            // 
            // _distanceTB
            // 
            this._distanceTB.Location = new System.Drawing.Point(94, 57);
            this._distanceTB.Name = "_distanceTB";
            this._distanceTB.Size = new System.Drawing.Size(39, 23);
            this._distanceTB.TabIndex = 21;
            this._distanceTB.TextChanged += new System.EventHandler(this.input_TextChanged);
            this._distanceTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _slopeTB
            // 
            this._slopeTB.Location = new System.Drawing.Point(94, 30);
            this._slopeTB.Name = "_slopeTB";
            this._slopeTB.Size = new System.Drawing.Size(39, 23);
            this._slopeTB.TabIndex = 19;
            this._slopeTB.TextChanged += new System.EventHandler(this.input_TextChanged);
            this._slopeTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _dbhTB
            // 
            this._dbhTB.Location = new System.Drawing.Point(94, 3);
            this._dbhTB.Name = "_dbhTB";
            this._dbhTB.Size = new System.Drawing.Size(39, 23);
            this._dbhTB.TabIndex = 16;
            this._dbhTB.TextChanged += new System.EventHandler(this.input_TextChanged);
            this._dbhTB.GotFocus += new System.EventHandler(this._TB_GotFocus);
            // 
            // _sipPlaceholder
            // 
            this._sipPlaceholder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._sipPlaceholder.Location = new System.Drawing.Point(0, 252);
            this._sipPlaceholder.Name = "_sipPlaceholder";
            this._sipPlaceholder.Size = new System.Drawing.Size(240, 0);
            // 
            // FormLimitingDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 252);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._ceControlPanel);
            this.Controls.Add(this._sipPlaceholder);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormLimitingDistance";
            this.Text = "Limiting Distance";
            this._ceControlPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _cancelMI;
        private System.Windows.Forms.MenuItem _calculateMI;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.Panel _ceControlPanel;
        private System.Windows.Forms.Button _cancelBTN;
        private System.Windows.Forms.Button _calculateBTN;
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
    }
}