namespace FSCruiser.WinForms.DataEntry
{
    partial class Form3PPNTPlotInfo
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
            this._plotNumNUD = new System.Windows.Forms.NumericUpDown();
            this._aveHtBTN = new System.Windows.Forms.Button();
            this._treeCntBTN = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this._OKBtn = new System.Windows.Forms.Button();
            this._getGPSMI = new System.Windows.Forms.MenuItem();
            this._kpiLBL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._volFactorTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._kz_lbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._plotNumNUD)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _plotNumNUD
            // 
            this._plotNumNUD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._BS_Plot, "PlotNumber", true));
            this._plotNumNUD.Dock = System.Windows.Forms.DockStyle.Left;
            this._plotNumNUD.Location = new System.Drawing.Point(43, 0);
            this._plotNumNUD.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this._plotNumNUD.Name = "_plotNumNUD";
            this._plotNumNUD.Size = new System.Drawing.Size(70, 20);
            this._plotNumNUD.TabIndex = 12;
            this._plotNumNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _aveHtBTN
            // 
            this._aveHtBTN.Font = new System.Drawing.Font("Tahoma", 9F);
            this._aveHtBTN.Location = new System.Drawing.Point(122, 64);
            this._aveHtBTN.Name = "_aveHtBTN";
            this._aveHtBTN.Size = new System.Drawing.Size(40, 20);
            this._aveHtBTN.TabIndex = 14;
            // 
            // _treeCntBTN
            // 
            this._treeCntBTN.Font = new System.Drawing.Font("Tahoma", 9F);
            this._treeCntBTN.Location = new System.Drawing.Point(122, 38);
            this._treeCntBTN.Name = "_treeCntBTN";
            this._treeCntBTN.Size = new System.Drawing.Size(40, 20);
            this._treeCntBTN.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._OKBtn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 240);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(284, 22);
            this.panel3.TabIndex = 3;
            // 
            // _OKBtn
            // 
            this._OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._OKBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._OKBtn.Location = new System.Drawing.Point(0, 0);
            this._OKBtn.Name = "_OKBtn";
            this._OKBtn.Size = new System.Drawing.Size(284, 22);
            this._OKBtn.TabIndex = 7;
            this._OKBtn.Text = "OK";
            this._OKBtn.Visible = false;
            // 
            // _getGPSMI
            // 
            this._getGPSMI.Index = -1;
            this._getGPSMI.Text = "Get GPS";
            // 
            // _kpiLBL
            // 
            this._kpiLBL.Location = new System.Drawing.Point(122, 90);
            this._kpiLBL.Name = "_kpiLBL";
            this._kpiLBL.Size = new System.Drawing.Size(63, 20);
            this._kpiLBL.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(77, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "KPI:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "Plot #:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._aveHtBTN);
            this.panel2.Controls.Add(this._treeCntBTN);
            this.panel2.Controls.Add(this._kpiLBL);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this._volFactorTB);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 137);
            this.panel2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "Average Height:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(44, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "Tree Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _volFactorTB
            // 
            this._volFactorTB.Enabled = false;
            this._volFactorTB.Location = new System.Drawing.Point(122, 9);
            this._volFactorTB.Name = "_volFactorTB";
            this._volFactorTB.Size = new System.Drawing.Size(40, 20);
            this._volFactorTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "Volume Factor:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._kz_lbl);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this._plotNumNUD);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 24);
            this.panel1.TabIndex = 5;
            // 
            // _kz_lbl
            // 
            this._kz_lbl.AutoSize = true;
            this._kz_lbl.Location = new System.Drawing.Point(146, 2);
            this._kz_lbl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this._kz_lbl.Name = "_kz_lbl";
            this._kz_lbl.Size = new System.Drawing.Size(28, 13);
            this._kz_lbl.TabIndex = 15;
            this._kz_lbl.Text = "###";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(119, 2);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "KZ:";
            // 
            // _cancelMI
            // 
            this._cancelMI.Index = -1;
            this._cancelMI.Text = "Cancel";
            // 
            // Form3PPNTPlotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form3PPNTPlotInfo";
            this.Text = "FormPlotInfo3PPNT";
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._plotNumNUD)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _plotNumNUD;
        private System.Windows.Forms.Button _aveHtBTN;
        private System.Windows.Forms.Button _treeCntBTN;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button _OKBtn;
        private System.Windows.Forms.MenuItem _getGPSMI;
        private System.Windows.Forms.Label _kpiLBL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _volFactorTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuItem _cancelMI;
        private System.Windows.Forms.Label _kz_lbl;
        private System.Windows.Forms.Label label6;
    }
}