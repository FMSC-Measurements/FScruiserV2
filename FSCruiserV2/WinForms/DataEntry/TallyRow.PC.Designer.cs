namespace FSCruiser.WinForms.DataEntry
{
    partial class TallyRow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._settingsBTN = new System.Windows.Forms.Button();
            this._tallyBTN = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._descriptionLBL = new System.Windows.Forms.Label();
            this._hotKeyLBL = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this._settingsBTN, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._tallyBTN, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(275, 56);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _settingsBTN
            // 
            this._settingsBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._settingsBTN.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._settingsBTN.Location = new System.Drawing.Point(257, 3);
            this._settingsBTN.Name = "_settingsBTN";
            this._settingsBTN.Size = new System.Drawing.Size(15, 50);
            this._settingsBTN.TabIndex = 0;
            this._settingsBTN.Text = "i";
            this._settingsBTN.UseVisualStyleBackColor = true;
            // 
            // _tallyBTN
            // 
            this._tallyBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tallyBTN.Location = new System.Drawing.Point(151, 3);
            this._tallyBTN.Name = "_tallyBTN";
            this._tallyBTN.Size = new System.Drawing.Size(100, 50);
            this._tallyBTN.TabIndex = 1;
            this._tallyBTN.Text = "####";
            this._tallyBTN.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._hotKeyLBL);
            this.panel1.Controls.Add(this._descriptionLBL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 56);
            this.panel1.TabIndex = 2;
            // 
            // _descriptionLBL
            // 
            this._descriptionLBL.Dock = System.Windows.Forms.DockStyle.Left;
            this._descriptionLBL.Location = new System.Drawing.Point(0, 0);
            this._descriptionLBL.Name = "_descriptionLBL";
            this._descriptionLBL.Size = new System.Drawing.Size(109, 56);
            this._descriptionLBL.TabIndex = 0;
            this._descriptionLBL.Text = "__________";
            this._descriptionLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _hotKeyLBL
            // 
            this._hotKeyLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hotKeyLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._hotKeyLBL.Location = new System.Drawing.Point(109, 0);
            this._hotKeyLBL.Name = "_hotKeyLBL";
            this._hotKeyLBL.Size = new System.Drawing.Size(39, 56);
            this._hotKeyLBL.TabIndex = 1;
            this._hotKeyLBL.Text = "#";
            this._hotKeyLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TallyRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TallyRow";
            this.Size = new System.Drawing.Size(275, 56);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _settingsBTN;
        private System.Windows.Forms.Button _tallyBTN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _hotKeyLBL;
        private System.Windows.Forms.Label _descriptionLBL;
    }
}
