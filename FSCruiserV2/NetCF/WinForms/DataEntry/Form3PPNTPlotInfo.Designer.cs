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
            this._BS_plot = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this._aveHtBTN = new System.Windows.Forms.Button();
            this._treeCntBTN = new System.Windows.Forms.Button();
            this._kpiLBL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._volFactorTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this._OKBtn = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._getGPSMI = new System.Windows.Forms.MenuItem();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._BS_plot)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this._plotNumNUD.Size = new System.Drawing.Size(70, 22);
            this._plotNumNUD.TabIndex = 12;
            this._plotNumNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _BS_plot
            // 
            this._BS_plot.DataSource = typeof(CruiseDAL.DataObjects.PlotDO);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 24);
            this.label1.Text = "Plot #:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._plotNumNUD);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 24);
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
            this.panel2.Size = new System.Drawing.Size(240, 137);
            // 
            // _aveHtBTN
            // 
            this._aveHtBTN.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this._aveHtBTN.Location = new System.Drawing.Point(122, 64);
            this._aveHtBTN.Name = "_aveHtBTN";
            this._aveHtBTN.Size = new System.Drawing.Size(40, 20);
            this._aveHtBTN.TabIndex = 14;
            this._aveHtBTN.Click += new System.EventHandler(this._aveHtBTN_Click);
            // 
            // _treeCntBTN
            // 
            this._treeCntBTN.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this._treeCntBTN.Location = new System.Drawing.Point(122, 38);
            this._treeCntBTN.Name = "_treeCntBTN";
            this._treeCntBTN.Size = new System.Drawing.Size(40, 20);
            this._treeCntBTN.TabIndex = 8;
            this._treeCntBTN.Click += new System.EventHandler(this._treeCntBTN_Click);
            // 
            // _kpiLBL
            // 
            this._kpiLBL.Location = new System.Drawing.Point(122, 90);
            this._kpiLBL.Name = "_kpiLBL";
            this._kpiLBL.Size = new System.Drawing.Size(63, 20);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(77, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 20);
            this.label5.Text = "KPI:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.Text = "Average Height:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(44, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.Text = "Tree Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _volFactorTB
            // 
            this._volFactorTB.Enabled = false;
            this._volFactorTB.Location = new System.Drawing.Point(122, 9);
            this._volFactorTB.Name = "_volFactorTB";
            this._volFactorTB.Size = new System.Drawing.Size(40, 21);
            this._volFactorTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.Text = "Volume Factor:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._OKBtn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 246);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(240, 22);
            // 
            // _OKBtn
            // 
            this._OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._OKBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._OKBtn.Location = new System.Drawing.Point(0, 0);
            this._OKBtn.Name = "_OKBtn";
            this._OKBtn.Size = new System.Drawing.Size(240, 22);
            this._OKBtn.TabIndex = 7;
            this._OKBtn.Text = "OK";
            this._OKBtn.Visible = false;
            this._OKBtn.Click += new System.EventHandler(this._OKBtn_Click);
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
            // Form3PPNTPlotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form3PPNTPlotInfo";
            this.Text = "Plot";
            ((System.ComponentModel.ISupportInitialize)(this._BS_plot)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _plotNumNUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox _volFactorTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button _OKBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label _kpiLBL;
        private System.Windows.Forms.Button _treeCntBTN;
        private System.Windows.Forms.Button _aveHtBTN;
        private System.Windows.Forms.BindingSource _BS_plot;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _getGPSMI;
        private System.Windows.Forms.MenuItem _cancelMI;
    }
}