namespace FSCruiser.WinForms
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._exeLBL = new System.Windows.Forms.Label();
            this._exeDOB_LBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(117, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 60);
            this.label1.Text = "Update in ApplicationController.cs";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 62);
            this.label2.Text = "U.S. Forest Service\r\nForest Management Service Center\r\nFort Collins, CO\r\nhttp://w" +
                "ww.fs.fed.us/fmsc/measure";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(117, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 27);
            this.label3.Text = "FScruiser";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(212, 52);
            this.label4.Text = "Matt Oberle - moberle@fs.fed.us       Benjamin Campbell - benjaminjcampbell@fs.fe" +
                "d.us";
            // 
            // _exeLBL
            // 
            this._exeLBL.Location = new System.Drawing.Point(11, 226);
            this._exeLBL.Name = "_exeLBL";
            this._exeLBL.Size = new System.Drawing.Size(211, 20);
            this._exeLBL.Text = "<exe name>";
            // 
            // _exeDOB_LBL
            // 
            this._exeDOB_LBL.Location = new System.Drawing.Point(11, 246);
            this._exeDOB_LBL.Name = "_exeDOB_LBL";
            this._exeDOB_LBL.Size = new System.Drawing.Size(211, 20);
            this._exeDOB_LBL.Text = "<exe dob>";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._exeDOB_LBL);
            this.Controls.Add(this._exeLBL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.Text = "About FScruiser";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label _exeLBL;
        private System.Windows.Forms.Label _exeDOB_LBL;
    }
}