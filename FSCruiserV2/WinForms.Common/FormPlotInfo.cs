using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;
using FSCruiser.Core;


namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormPlotInfo : Form, INotifyPropertyChanged
    {
        PlotStratum _stratum;

        uint? _editPlotNumber = null;
        bool? _editIsNull = null;
        float? _editSlope = null;
        float? _editAspect = null;
        string _editRemarks = null;

        public uint PlotNumber
        {
            get
            {
                return (_editPlotNumber.HasValue) ? _editPlotNumber.Value
                    : (uint)Plot.PlotNumber;
            }
            set
            {
                _editPlotNumber = (value == Plot.PlotNumber) ? (uint?)null : value;
                NotifyPropertyChanged("PlotNumber");
            }
        }

        public bool IsNull
        {
            get
            {
                return (_editIsNull.HasValue) ? _editIsNull.Value
                    : Plot.IsNull;
            }
            set
            {
                _editIsNull = (value == Plot.IsNull) ? (bool?)null : value;
                NotifyPropertyChanged("IsNull");
            }
        }

        public float Slope
        {
            get
            {
                return (_editSlope.HasValue) ? _editSlope.Value
                    : Plot.Slope;
            }
            set
            {
                _editSlope = (value.EqualsEx(Plot.Slope)) ? (float?)null : value;
                NotifyPropertyChanged("Slope");
            }
        }

        public float Aspect
        {
            get
            {
                return (_editAspect.HasValue) ? _editAspect.Value
                    : Plot.Aspect;
            }
            set
            {
                _editAspect = (value.EqualsEx(Plot.Aspect)) ? (float?)null : value;
                NotifyPropertyChanged("Aspect");
            }
        }

        public string Remarks
        {
            get
            {
                return (_editRemarks != null) ? _editRemarks : Plot.Remarks;
            }
            set
            {
                _editRemarks = (value == Plot.Remarks) ? null : value;
                NotifyPropertyChanged("Remarks");
            }
        }

        PlotVM Plot { get; set; }


        public FormPlotInfo(): base()
        {
            InitializeComponent();
            this._BS_Plot.DataSource = this;
        }

        public virtual DialogResult ShowDialog(PlotVM plot, PlotStratum stratum, bool isNewPlot)
        {
            if (plot == null) { throw new ArgumentNullException("plot"); }
            if (stratum == null) { throw new ArgumentNullException("stratum"); }

            _stratum = stratum;
            Plot = plot;
           
            this._BS_Plot.ResetBindings(false);

            this.DialogResult = DialogResult.OK;
            return this.ShowDialog();
        }

        protected override void  OnLoad(EventArgs e)
        {
            _editAspect = null;
            _editIsNull = null;
            _editPlotNumber = null;
            _editRemarks = null;
            _editSlope = null;

 	         base.OnLoad(e);
        }

        void PushEditValues()
        {
            if (_editPlotNumber.HasValue)
            {
                this.Plot.PlotNumber = _editPlotNumber.Value;
            }
            if (_editIsNull.HasValue)
            {
                this.Plot.IsNull = _editIsNull.Value;
            }
            if (_editAspect.HasValue)
            {
                this.Plot.Aspect = _editAspect.Value;
            }
            if (_editRemarks != null)
            {
                this.Plot.Remarks = _editRemarks;
            }
            if (_editSlope.HasValue)
            {
                this.Plot.Slope = _editSlope.Value;
            }            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._BS_Plot.EndEdit();

            if (DialogResult == DialogResult.OK)
            {
                if (_editPlotNumber.HasValue
                    && _editPlotNumber.Value == 0)
                {
                    MessageBox.Show("Plot Number can't be 0");
                    e.Cancel = true;
                }
                if (_editPlotNumber.HasValue
                    && !_stratum.IsPlotNumberAvailable(_editPlotNumber.Value))
                {
                    MessageBox.Show("Plot Number Already Exists");
                    e.Cancel = true;
                }
                else if (IsNull && this.Plot.Trees.Count > 0)
                {
                    MessageBox.Show("Null plot can not contain trees");
                    e.Cancel = true;
                }
                else
                {
                    PushEditValues();
                }
            }
            else if (this.DialogResult == DialogResult.Cancel)
            {
                return;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled) { return; }
            if (!this._remarksTB.Focused && e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.OK;//setting dialogResult causes form to close
                //this.Close();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

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

#if NetCF

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _gpsButton_Click(object sender, EventArgs e)
        {

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
            this._BS_Plot.DataSource = typeof(FSCruiser.Core.Models.PlotVM);
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
#else

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._remarksTB = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this._plotNumTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._isNullCB = new System.Windows.Forms.CheckBox();
            this._aspect = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._slope = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._CancelBTN = new System.Windows.Forms.Button();
            this._okBTN = new System.Windows.Forms.Button();
            this._BS_Plot = new System.Windows.Forms.BindingSource(this.components);
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).BeginInit();
            this.SuspendLayout();
            // 
            // _remarksTB
            // 
            this._remarksTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Remarks", true));
            this._remarksTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._remarksTB.Location = new System.Drawing.Point(0, 96);
            this._remarksTB.Multiline = true;
            this._remarksTB.Name = "_remarksTB";
            this._remarksTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._remarksTB.Size = new System.Drawing.Size(251, 135);
            this._remarksTB.TabIndex = 11;
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
            this.panel2.Size = new System.Drawing.Size(251, 78);
            this.panel2.TabIndex = 12;
            // 
            // _plotNumTB
            // 
            this._plotNumTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "PlotNumber", true));
            this._plotNumTB.Location = new System.Drawing.Point(53, 3);
            this._plotNumTB.Name = "_plotNumTB";
            this._plotNumTB.Size = new System.Drawing.Size(30, 20);
            this._plotNumTB.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(85, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "%";
            // 
            // _isNullCB
            // 
            this._isNullCB.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._BS_Plot, "IsNull", true));
            this._isNullCB.Location = new System.Drawing.Point(154, 3);
            this._isNullCB.Name = "_isNullCB";
            this._isNullCB.Size = new System.Drawing.Size(73, 20);
            this._isNullCB.TabIndex = 13;
            this._isNullCB.Text = "Null Plot";
            // 
            // _aspect
            // 
            this._aspect.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Aspect", true));
            this._aspect.Location = new System.Drawing.Point(165, 29);
            this._aspect.Name = "_aspect";
            this._aspect.Size = new System.Drawing.Size(70, 20);
            this._aspect.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(119, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "Aspect:";
            // 
            // _slope
            // 
            this._slope.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_Plot, "Slope", true));
            this._slope.Location = new System.Drawing.Point(41, 29);
            this._slope.Name = "_slope";
            this._slope.Size = new System.Drawing.Size(42, 20);
            this._slope.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "Slope:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Plot #:";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(251, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Remarks";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._CancelBTN);
            this.panel1.Controls.Add(this._okBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 231);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 31);
            this.panel1.TabIndex = 14;
            // 
            // _CancelBTN
            // 
            this._CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._CancelBTN.Location = new System.Drawing.Point(173, 5);
            this._CancelBTN.Name = "_CancelBTN";
            this._CancelBTN.Size = new System.Drawing.Size(75, 23);
            this._CancelBTN.TabIndex = 1;
            this._CancelBTN.Text = "Cancel";
            this._CancelBTN.UseVisualStyleBackColor = true;
            // 
            // _okBTN
            // 
            this._okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBTN.Location = new System.Drawing.Point(92, 5);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(75, 23);
            this._okBTN.TabIndex = 0;
            this._okBTN.Text = "OK";
            this._okBTN.UseVisualStyleBackColor = true;
            // 
            // _BS_Plots
            // 
            this._BS_Plot.DataSource = typeof(FSCruiser.WinForms.DataEntry.FormPlotInfo);
            // 
            // FormPlotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 262);
            this.Controls.Add(this._remarksTB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPlotInfo";
            this.Text = "Plot Information";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _remarksTB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox _plotNumTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox _isNullCB;
        private System.Windows.Forms.TextBox _aspect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _slope;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _CancelBTN;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.BindingSource _BS_Plot;
#endif
    }
}
