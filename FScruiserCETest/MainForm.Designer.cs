namespace FSCruiserV2.Test
{
    partial class MainForm
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.panel1 = new System.Windows.Forms.Panel();
            this._plotInfo_BTN = new System.Windows.Forms.Button();
            this._editTDV_BTN = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.STRSampplingTest_BTN = new System.Windows.Forms.Button();
            this.LimitingDistance = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LimitingDistance);
            this.panel1.Controls.Add(this._plotInfo_BTN);
            this.panel1.Controls.Add(this._editTDV_BTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 100);
            // 
            // _plotInfo_BTN
            // 
            this._plotInfo_BTN.Location = new System.Drawing.Point(82, 4);
            this._plotInfo_BTN.Name = "_plotInfo_BTN";
            this._plotInfo_BTN.Size = new System.Drawing.Size(72, 20);
            this._plotInfo_BTN.TabIndex = 1;
            this._plotInfo_BTN.Text = "Plot Info";
            this._plotInfo_BTN.Click += new System.EventHandler(this._plotInfo_BTN_Click);
            // 
            // _editTDV_BTN
            // 
            this._editTDV_BTN.Location = new System.Drawing.Point(4, 4);
            this._editTDV_BTN.Name = "_editTDV_BTN";
            this._editTDV_BTN.Size = new System.Drawing.Size(72, 20);
            this._editTDV_BTN.TabIndex = 0;
            this._editTDV_BTN.Text = "Edit TDV";
            this._editTDV_BTN.Click += new System.EventHandler(this._editTDV_BTN_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.Text = "Test UI";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 20);
            this.label2.Text = "Logic Tests";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.STRSampplingTest_BTN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 140);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 100);
            // 
            // STRSampplingTest_BTN
            // 
            this.STRSampplingTest_BTN.Location = new System.Drawing.Point(4, 4);
            this.STRSampplingTest_BTN.Name = "STRSampplingTest_BTN";
            this.STRSampplingTest_BTN.Size = new System.Drawing.Size(102, 20);
            this.STRSampplingTest_BTN.TabIndex = 0;
            this.STRSampplingTest_BTN.Text = "STR Sampling";
            this.STRSampplingTest_BTN.Click += new System.EventHandler(this.STRSampplingTest_BTN_Click);
            // 
            // LimitingDistance
            // 
            this.LimitingDistance.Location = new System.Drawing.Point(4, 30);
            this.LimitingDistance.Name = "LimitingDistance";
            this.LimitingDistance.Size = new System.Drawing.Size(110, 20);
            this.LimitingDistance.TabIndex = 2;
            this.LimitingDistance.Text = "LimitingDistance";
            this.LimitingDistance.Click += new System.EventHandler(this.LimitingDistance_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _editTDV_BTN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button STRSampplingTest_BTN;
        private System.Windows.Forms.Button _plotInfo_BTN;
        private System.Windows.Forms.Button LimitingDistance;
    }
}