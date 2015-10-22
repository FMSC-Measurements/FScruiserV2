namespace FSCruiser.WinForms
{
    partial class FormBackupUtility
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
            System.Windows.Forms.Label label2;
            this._backupFileLBL = new System.Windows.Forms.Label();
            this._changeLocationBTN = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._cancelMI = new System.Windows.Forms.MenuItem();
            this._okMI = new System.Windows.Forms.MenuItem();
            this._ceControlPanel = new System.Windows.Forms.Panel();
            this._cancelBTN = new System.Windows.Forms.Button();
            this._okBTN = new System.Windows.Forms.Button();
            this._backupOnLeaveUnitCB = new System.Windows.Forms.CheckBox();
            this._makeBackupBTN = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            this._ceControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(240, 20);
            label2.Text = "Backup directory:";
            // 
            // _backupFileLBL
            // 
            this._backupFileLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this._backupFileLBL.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this._backupFileLBL.Location = new System.Drawing.Point(0, 20);
            this._backupFileLBL.Name = "_backupFileLBL";
            this._backupFileLBL.Size = new System.Drawing.Size(240, 60);
            this._backupFileLBL.Text = "<backup dir>";
            // 
            // _changeLocationBTN
            // 
            this._changeLocationBTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._changeLocationBTN.Location = new System.Drawing.Point(0, 228);
            this._changeLocationBTN.Name = "_changeLocationBTN";
            this._changeLocationBTN.Size = new System.Drawing.Size(240, 20);
            this._changeLocationBTN.TabIndex = 5;
            this._changeLocationBTN.Text = "Change &Location";
            this._changeLocationBTN.Click += new System.EventHandler(this._changeLocationBTN_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this._cancelMI);
            this.mainMenu1.MenuItems.Add(this._okMI);
            // 
            // _cancelMI
            // 
            this._cancelMI.Text = "Cancel";
            this._cancelMI.Click += new System.EventHandler(this._cancelMI_Click);
            // 
            // _okMI
            // 
            this._okMI.Text = "OK";
            this._okMI.Click += new System.EventHandler(this._okMI_Click);
            // 
            // _ceControlPanel
            // 
            this._ceControlPanel.Controls.Add(this._cancelBTN);
            this._ceControlPanel.Controls.Add(this._okBTN);
            this._ceControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ceControlPanel.Location = new System.Drawing.Point(0, 248);
            this._ceControlPanel.Name = "_ceControlPanel";
            this._ceControlPanel.Size = new System.Drawing.Size(240, 20);
            this._ceControlPanel.Visible = false;
            // 
            // _cancelBTN
            // 
            this._cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._cancelBTN.Location = new System.Drawing.Point(0, 0);
            this._cancelBTN.Name = "_cancelBTN";
            this._cancelBTN.Size = new System.Drawing.Size(120, 20);
            this._cancelBTN.TabIndex = 1;
            this._cancelBTN.Text = "Cancel";
            // 
            // _okBTN
            // 
            this._okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._okBTN.Location = new System.Drawing.Point(120, 0);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(120, 20);
            this._okBTN.TabIndex = 0;
            this._okBTN.Text = "OK";
            // 
            // _backupOnLeaveUnitCB
            // 
            this._backupOnLeaveUnitCB.Dock = System.Windows.Forms.DockStyle.Top;
            this._backupOnLeaveUnitCB.Location = new System.Drawing.Point(0, 80);
            this._backupOnLeaveUnitCB.Name = "_backupOnLeaveUnitCB";
            this._backupOnLeaveUnitCB.Size = new System.Drawing.Size(240, 20);
            this._backupOnLeaveUnitCB.TabIndex = 11;
            this._backupOnLeaveUnitCB.Text = "Create Back Up When Leaving Unit";
            // 
            // _makeBackupBTN
            // 
            this._makeBackupBTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._makeBackupBTN.Location = new System.Drawing.Point(0, 208);
            this._makeBackupBTN.Name = "_makeBackupBTN";
            this._makeBackupBTN.Size = new System.Drawing.Size(240, 20);
            this._makeBackupBTN.TabIndex = 12;
            this._makeBackupBTN.Text = "&Make Backup";
            this._makeBackupBTN.Click += new System.EventHandler(this._makeBackupBTN_Click);
            // 
            // FormBackupUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._makeBackupBTN);
            this.Controls.Add(this._backupOnLeaveUnitCB);
            this.Controls.Add(this._changeLocationBTN);
            this.Controls.Add(this._backupFileLBL);
            this.Controls.Add(label2);
            this.Controls.Add(this._ceControlPanel);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormBackupUtility";
            this.Text = "Backup File";
            this._ceControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _backupFileLBL;
        private System.Windows.Forms.Button _changeLocationBTN;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _cancelMI;
        private System.Windows.Forms.MenuItem _okMI;
        private System.Windows.Forms.Panel _ceControlPanel;
        private System.Windows.Forms.Button _cancelBTN;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.CheckBox _backupOnLeaveUnitCB;
        private System.Windows.Forms.Button _makeBackupBTN;
    }
}