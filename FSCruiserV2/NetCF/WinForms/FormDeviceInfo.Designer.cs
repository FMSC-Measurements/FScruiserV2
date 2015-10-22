namespace FSCruiser.WinForms
{
    partial class FormDeviceInfo
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
            this.textBoxDeviceInfo = new System.Windows.Forms.TextBox();
            this.progressBarMemoryLoad = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarMainBatteryCharge = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBarBackupBatteryCharge = new System.Windows.Forms.ProgressBar();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // textBoxDeviceInfo
            // 
            this.textBoxDeviceInfo.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBoxDeviceInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDeviceInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxDeviceInfo.Location = new System.Drawing.Point(0, 0);
            this.textBoxDeviceInfo.Multiline = true;
            this.textBoxDeviceInfo.Name = "textBoxDeviceInfo";
            this.textBoxDeviceInfo.ReadOnly = true;
            this.textBoxDeviceInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDeviceInfo.Size = new System.Drawing.Size(240, 117);
            this.textBoxDeviceInfo.TabIndex = 0;
            // 
            // progressBarMemoryLoad
            // 
            this.progressBarMemoryLoad.Location = new System.Drawing.Point(89, 120);
            this.progressBarMemoryLoad.Name = "progressBarMemoryLoad";
            this.progressBarMemoryLoad.Size = new System.Drawing.Size(147, 23);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.Text = "Memory Load";
            // 
            // progressBarMainBatteryCharge
            // 
            this.progressBarMainBatteryCharge.Location = new System.Drawing.Point(89, 149);
            this.progressBarMainBatteryCharge.Name = "progressBarMainBatteryCharge";
            this.progressBarMainBatteryCharge.Size = new System.Drawing.Size(147, 24);
            this.progressBarMainBatteryCharge.ParentChanged += new System.EventHandler(this.progressBarMainBatteryCharge_ParentChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 24);
            this.label2.Text = "Main Battery";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.Text = "Backup Battery";
            // 
            // progressBarBackupBatteryCharge
            // 
            this.progressBarBackupBatteryCharge.Location = new System.Drawing.Point(89, 179);
            this.progressBarBackupBatteryCharge.Name = "progressBarBackupBatteryCharge";
            this.progressBarBackupBatteryCharge.Size = new System.Drawing.Size(147, 25);
            // 
            // FormDeviceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.progressBarBackupBatteryCharge);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBarMainBatteryCharge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarMemoryLoad);
            this.Controls.Add(this.textBoxDeviceInfo);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormDeviceInfo";
            this.Text = "Device Information";
            this.Load += new System.EventHandler(this.FormDeviceInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDeviceInfo;
    //    private FMSC.Controls.
        private System.Windows.Forms.ProgressBar progressBarMemoryLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarMainBatteryCharge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBarBackupBatteryCharge;
        private System.Windows.Forms.MainMenu mainMenu1;


    }
}