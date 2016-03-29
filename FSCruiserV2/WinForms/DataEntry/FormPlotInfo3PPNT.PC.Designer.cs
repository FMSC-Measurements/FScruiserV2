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
            this.components = new System.ComponentModel.Container();
            this._plotNumNUD = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this._OKBtn = new System.Windows.Forms.Button();
            this._getGPSMI = new System.Windows.Forms.MenuItem();
            this._kpiLBL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._avgHt_TB = new System.Windows.Forms.TextBox();
            this._treeCnt_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._volFactorTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._kz3ppnt_lbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            this._BS_plot = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._plotNumNUD)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_plot)).BeginInit();
            this.SuspendLayout();
            // 
            // _plotNumNUD
            // 
            this._plotNumNUD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._BS_plot, "PlotNumber", true));
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
            // 
            // _getGPSMI
            // 
            this._getGPSMI.Index = -1;
            this._getGPSMI.Text = "Get GPS";
            // 
            // _kpiLBL
            // 
            this._kpiLBL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_plot, "KPI", true));
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
            this.panel2.Controls.Add(this._avgHt_TB);
            this.panel2.Controls.Add(this._treeCnt_TB);
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
            // _avgHt_TB
            // 
            this._avgHt_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_plot, "AverageHeight", true));
            this._avgHt_TB.Location = new System.Drawing.Point(122, 63);
            this._avgHt_TB.MaxLength = 3;
            this._avgHt_TB.Name = "_avgHt_TB";
            this._avgHt_TB.Size = new System.Drawing.Size(40, 20);
            this._avgHt_TB.TabIndex = 21;
            // 
            // _treeCnt_TB
            // 
            this._treeCnt_TB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_plot, "TreeCount", true));
            this._treeCnt_TB.Location = new System.Drawing.Point(122, 38);
            this._treeCnt_TB.MaxLength = 3;
            this._treeCnt_TB.Name = "_treeCnt_TB";
            this._treeCnt_TB.Size = new System.Drawing.Size(40, 20);
            this._treeCnt_TB.TabIndex = 20;
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
            this._volFactorTB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BS_plot, "VolFactor", true));
            this._volFactorTB.Enabled = false;
            this._volFactorTB.Location = new System.Drawing.Point(122, 9);
            this._volFactorTB.Name = "_volFactorTB";
            this._volFactorTB.Size = new System.Drawing.Size(40, 20);
            this._volFactorTB.TabIndex = 1;
            this._volFactorTB.TextChanged += new System.EventHandler(this.TB_TextChanged);
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
            this.panel1.Controls.Add(this._kz3ppnt_lbl);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this._plotNumNUD);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 24);
            this.panel1.TabIndex = 5;
            // 
            // _kz3ppnt_lbl
            // 
            this._kz3ppnt_lbl.AutoSize = true;
            this._kz3ppnt_lbl.Location = new System.Drawing.Point(146, 2);
            this._kz3ppnt_lbl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this._kz3ppnt_lbl.Name = "_kz3ppnt_lbl";
            this._kz3ppnt_lbl.Size = new System.Drawing.Size(28, 13);
            this._kz3ppnt_lbl.TabIndex = 15;
            this._kz3ppnt_lbl.Text = "###";
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
            // _BS_plot
            // 
            this._BS_plot.DataSource = typeof(FSCruiser.Core.Models.Plot3PPNT);
            this._BS_plot.CurrentItemChanged += new System.EventHandler(this._BS_plot_CurrentItemChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this._plotNumNUD)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_plot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _plotNumNUD;
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
        private System.Windows.Forms.Label _kz3ppnt_lbl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _avgHt_TB;
        private System.Windows.Forms.TextBox _treeCnt_TB;
        private System.Windows.Forms.BindingSource _BS_plot;
    }
}