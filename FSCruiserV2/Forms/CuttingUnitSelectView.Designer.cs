namespace FSCruiserV2.Forms
{
    partial class CuttingUnitSelectView
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
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this._strataLB = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this._cuttingUnitCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._BS_CuttingUnits = new System.Windows.Forms.BindingSource(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._BS_CuttingUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this._strataLB);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this._cuttingUnitCB);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 383);
            this.panel2.TabIndex = 1;
            // 
            // _strataLB
            // 
            this._strataLB.DisplayMember = "Code";
            this._strataLB.Dock = System.Windows.Forms.DockStyle.Top;
            this._strataLB.FormattingEnabled = true;
            this._strataLB.Location = new System.Drawing.Point(0, 47);
            this._strataLB.Name = "_strataLB";
            this._strataLB.Size = new System.Drawing.Size(200, 95);
            this._strataLB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Strata";
            // 
            // _cuttingUnitCB
            // 
            this._cuttingUnitCB.DisplayMember = "Code";
            this._cuttingUnitCB.Dock = System.Windows.Forms.DockStyle.Top;
            this._cuttingUnitCB.FormatString = "[Code]";
            this._cuttingUnitCB.Location = new System.Drawing.Point(0, 13);
            this._cuttingUnitCB.Name = "_cuttingUnitCB";
            this._cuttingUnitCB.Size = new System.Drawing.Size(200, 21);
            this._cuttingUnitCB.TabIndex = 0;
            this._cuttingUnitCB.SelectedValueChanged += new System.EventHandler(this._cuttingUnitCB_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Cutting Unit";
            // 
            // _BS_CuttingUnits
            // 
            this._BS_CuttingUnits.DataSource = typeof(FSCruiserV2.Logic.CuttingUnitVM);
            // 
            // CuttingUnitSelectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "CuttingUnitSelectView";
            this.Size = new System.Drawing.Size(250, 383);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._BS_CuttingUnits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox _strataLB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _cuttingUnitCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource _BS_CuttingUnits;
    }
}
