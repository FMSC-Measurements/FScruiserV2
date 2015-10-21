namespace FSCruiser.WinForms.DataEntry
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
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.Windows.Forms.BindingSource _BS_Plot;
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
    }
}