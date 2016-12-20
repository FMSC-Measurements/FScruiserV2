using FSCruiser.Core.Models;
namespace FSCruiser.WinForms
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
            this._BS_CuttingUnits = new System.Windows.Forms.BindingSource(this.components);
            this._strataLB = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this._cuttingUnitCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._BS_CuttingUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // _BS_CuttingUnits
            // 
            this._BS_CuttingUnits.DataSource = typeof(FSCruiser.Core.Models.CuttingUnit);
            this._BS_CuttingUnits.CurrentChanged += new System.EventHandler(this._BS_CuttingUnits_CurrentChanged);
            // 
            // _strataLB
            // 
            this._strataLB.DisplayMember = "Code";
            this._strataLB.Dock = System.Windows.Forms.DockStyle.Top;
            this._strataLB.FormattingEnabled = true;
            this._strataLB.ItemHeight = 16;
            this._strataLB.Location = new System.Drawing.Point(0, 64);
            this._strataLB.Margin = new System.Windows.Forms.Padding(4);
            this._strataLB.Name = "_strataLB";
            this._strataLB.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this._strataLB.Size = new System.Drawing.Size(333, 116);
            this._strataLB.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Strata";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _cuttingUnitCB
            // 
            this._cuttingUnitCB.DataSource = this._BS_CuttingUnits;
            this._cuttingUnitCB.Dock = System.Windows.Forms.DockStyle.Top;
            this._cuttingUnitCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cuttingUnitCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cuttingUnitCB.FormatString = "[Code]";
            this._cuttingUnitCB.FormattingEnabled = true;
            this._cuttingUnitCB.Location = new System.Drawing.Point(0, 20);
            this._cuttingUnitCB.Margin = new System.Windows.Forms.Padding(4);
            this._cuttingUnitCB.Name = "_cuttingUnitCB";
            this._cuttingUnitCB.Size = new System.Drawing.Size(333, 24);
            this._cuttingUnitCB.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select Cutting Unit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CuttingUnitSelectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._strataLB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._cuttingUnitCB);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CuttingUnitSelectView";
            this.Size = new System.Drawing.Size(333, 471);
            ((System.ComponentModel.ISupportInitialize)(this._BS_CuttingUnits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource _BS_CuttingUnits;
        private System.Windows.Forms.ListBox _strataLB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _cuttingUnitCB;
        private System.Windows.Forms.Label label1;
    }
}
