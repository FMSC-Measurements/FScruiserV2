namespace FSCruiser.NetCF.WinForms
{
    partial class FormSettings
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._enableTallySound = new System.Windows.Forms.CheckBox();
            this._enablePageChangeSound = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._untallyHotKeySelect = new FSCruiser.NetCF.WinForms.Controls.HotKeySelectControl();
            this._jumpTreeTallyHotKeySelect = new FSCruiser.NetCF.WinForms.Controls.HotKeySelectControl();
            this._cancel_button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this._cancel_button);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 368);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 75);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Info;
            this.panel5.Controls.Add(this._jumpTreeTallyHotKeySelect);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 43);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 23);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 23);
            this.label4.Text = "Jump Tree-Tally page";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Info;
            this.panel4.Controls.Add(this._untallyHotKeySelect);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 23);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 23);
            this.label3.Text = "Untally";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 20);
            this.label2.Text = "Hot Keys";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.Controls.Add(this._enableTallySound);
            this.panel2.Controls.Add(this._enablePageChangeSound);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 82);
            // 
            // _enableTallySound
            // 
            this._enableTallySound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enableTallySound.Location = new System.Drawing.Point(0, 40);
            this._enableTallySound.Name = "_enableTallySound";
            this._enableTallySound.Size = new System.Drawing.Size(266, 20);
            this._enableTallySound.TabIndex = 3;
            this._enableTallySound.Text = "Tally";
            // 
            // _enablePageChangeSound
            // 
            this._enablePageChangeSound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enablePageChangeSound.Location = new System.Drawing.Point(0, 20);
            this._enablePageChangeSound.Name = "_enablePageChangeSound";
            this._enablePageChangeSound.Size = new System.Drawing.Size(266, 20);
            this._enablePageChangeSound.TabIndex = 4;
            this._enablePageChangeSound.Text = "Page Changed";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 20);
            this.label1.Text = "Sounds";
            // 
            // _untallyHotKeySelect
            // 
            this._untallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._untallyHotKeySelect.Location = new System.Drawing.Point(58, 0);
            this._untallyHotKeySelect.Name = "_untallyHotKeySelect";
            this._untallyHotKeySelect.Size = new System.Drawing.Size(57, 23);
            this._untallyHotKeySelect.TabIndex = 3;
            // 
            // _jumpTreeTallyHotKeySelect
            // 
            this._jumpTreeTallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._jumpTreeTallyHotKeySelect.Location = new System.Drawing.Point(177, 0);
            this._jumpTreeTallyHotKeySelect.Name = "_jumpTreeTallyHotKeySelect";
            this._jumpTreeTallyHotKeySelect.Size = new System.Drawing.Size(57, 23);
            this._jumpTreeTallyHotKeySelect.TabIndex = 3;
            // 
            // _cancel_button
            // 
            this._cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._cancel_button.Location = new System.Drawing.Point(0, 348);
            this._cancel_button.Name = "_cancel_button";
            this._cancel_button.Size = new System.Drawing.Size(266, 20);
            this._cancel_button.TabIndex = 2;
            this._cancel_button.Text = "Cancel";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(266, 368);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox _enableTallySound;
        private System.Windows.Forms.CheckBox _enablePageChangeSound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private FSCruiser.NetCF.WinForms.Controls.HotKeySelectControl _jumpTreeTallyHotKeySelect;
        private FSCruiser.NetCF.WinForms.Controls.HotKeySelectControl _untallyHotKeySelect;
        private System.Windows.Forms.Button _cancel_button;

    }
}