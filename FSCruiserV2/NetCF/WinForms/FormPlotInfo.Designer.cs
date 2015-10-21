namespace FSCruiserV2.Forms
{
    partial class FormPlotInfo
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
            this._BS_Plot = new System.Windows.Forms.BindingSource(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._getGPSMI = new System.Windows.Forms.MenuItem();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this._plotStatsTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._remarksTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._plotNumTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._isNullCB = new System.Windows.Forms.CheckBox();
            this._aspect = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._slope = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._ceControlPanel = new System.Windows.Forms.Panel();
            this._okBTN = new System.Windows.Forms.Button();
            this._getGPSBTN = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this._ceControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _BS_Plot
            // 
            this._BS_Plot.DataSource = typeof(FSCruiserV2.Logic.PlotVM);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this._getGPSMI);
            this.mainMenu1.MenuItems.Add(this._cancelMI);
            // 
            // _getGPSMI
            // 
            this._getGPSMI.Text = "Get GPS";
            this._getGPSMI.Click += new System.EventHandler(this._gpsButton_Click);
            // 
            // _cancelMI
            // 
            this._cancelMI.Text = "Cancel";
            this._cancelMI.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this._plotStatsTB);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this._remarksTB);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(238, 266);
            // 
            // _plotStatsTB
            // 
            this._plotStatsTB.Dock = System.Windows.Forms.DockStyle.Top;
            this._plotStatsTB.Location = new System.Drawing.Point(0, 152);
            this._plotStatsTB.Multiline = true;
            this._plotStatsTB.Name = "_plotStatsTB";
            this._plotStatsTB.ReadOnly = true;
            this._plotStatsTB.Size = new System.Drawing.Size(238, 76);
            this._plotStatsTB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(238, 17);
            this.label5.Text = "Plot Stats";
            // 
            // _remarksTB
            // 
            this._remarksTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Remarks", true));
            this._remarksTB.Dock = System.Windows.Forms.DockStyle.Top;
            this._remarksTB.Location = new System.Drawing.Point(0, 70);
            this._remarksTB.Multiline = true;
            this._remarksTB.Name = "_remarksTB";
            this._remarksTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._remarksTB.Size = new System.Drawing.Size(238, 65);
            this._remarksTB.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 18);
            this.label4.Text = "Remarks";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._plotNumTB);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this._isNullCB);
            this.panel2.Controls.Add(this._aspect);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this._slope);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(238, 52);
            // 
            // _plotNumTB
            // 
            this._plotNumTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "PlotNumber", true));
            this._plotNumTB.Location = new System.Drawing.Point(53, 3);
            this._plotNumTB.Name = "_plotNumTB";
            this._plotNumTB.Size = new System.Drawing.Size(30, 23);
            this._plotNumTB.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(85, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 20);
            this.label6.Text = "%";
            // 
            // _isNullCB
            // 
            this._isNullCB.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._BS_Plot, "IsNull", true));
            this._isNullCB.Location = new System.Drawing.Point(154, 3);
            this._isNullCB.Name = "_isNullCB";
            this._isNullCB.Size = new System.Drawing.Size(73, 20);
            this._isNullCB.TabIndex = 2;
            this._isNullCB.Text = "Null Plot";
            // 
            // _aspect
            // 
            this._aspect.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Aspect", true));
            this._aspect.Location = new System.Drawing.Point(165, 29);
            this._aspect.Name = "_aspect";
            this._aspect.Size = new System.Drawing.Size(70, 23);
            this._aspect.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(119, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.Text = "Aspect:";
            // 
            // _slope
            // 
            this._slope.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Slope", true));
            this._slope.Location = new System.Drawing.Point(41, 29);
            this._slope.Name = "_slope";
            this._slope.Size = new System.Drawing.Size(42, 23);
            this._slope.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "Slope:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.Text = "Plot #:";
            // 
            // _ceControlPanel
            // 
            this._ceControlPanel.Controls.Add(this._okBTN);
            this._ceControlPanel.Controls.Add(this._getGPSBTN);
            this._ceControlPanel.Controls.Add(this.button1);
            this._ceControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ceControlPanel.Location = new System.Drawing.Point(0, 266);
            this._ceControlPanel.Name = "_ceControlPanel";
            this._ceControlPanel.Size = new System.Drawing.Size(238, 22);
            this._ceControlPanel.Visible = false;
            // 
            // _okBTN
            // 
            this._okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._okBTN.Location = new System.Drawing.Point(80, 0);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(78, 22);
            this._okBTN.TabIndex = 7;
            this._okBTN.Text = "OK";
            // 
            // _getGPSBTN
            // 
            this._getGPSBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._getGPSBTN.Location = new System.Drawing.Point(158, 0);
            this._getGPSBTN.Name = "_getGPSBTN";
            this._getGPSBTN.Size = new System.Drawing.Size(80, 22);
            this._getGPSBTN.TabIndex = 6;
            this._getGPSBTN.Text = "Get GPS";
            this._getGPSBTN.Click += new System.EventHandler(this._gpsButton_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 22);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // FormPlotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(238, 288);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._ceControlPanel);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormPlotInfo";
            this.Text = "Plot Information";
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this._ceControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource _BS_Plot;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _getGPSMI;
        private System.Windows.Forms.MenuItem _cancelMI;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _plotStatsTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _remarksTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox _plotNumTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox _isNullCB;
        private System.Windows.Forms.TextBox _aspect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _slope;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _ceControlPanel;
        private System.Windows.Forms.Button _getGPSBTN;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button _okBTN;

    }
}