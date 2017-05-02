using FSCruiser.Core.Models;
namespace FSCruiser.WinForms.DataEntry
{
    partial class LayoutTreeBased
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
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label1;
            this._tallyHistoryLB = new System.Windows.Forms.ListBox();
            this._untallyBTN = new System.Windows.Forms.Button();
            this._leftContentPanel = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(this._tallyHistoryLB);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(this._untallyBTN);
            panel1.Dock = System.Windows.Forms.DockStyle.Right;
            panel1.Location = new System.Drawing.Point(140, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(100, 245);
            // 
            // _tallyHistoryLB
            //             
            
            this._tallyHistoryLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyHistoryLB.Location = new System.Drawing.Point(0, 13);
            this._tallyHistoryLB.Name = "_tallyHistoryLB";
            this._tallyHistoryLB.Size = new System.Drawing.Size(100, 194);
            this._tallyHistoryLB.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 13);
            label1.Text = "(latest on bottom)";
            label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _untallyButton
            // 
            this._untallyBTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._untallyBTN.Location = new System.Drawing.Point(0, 212);
            this._untallyBTN.Name = "_untallyButton";
            this._untallyBTN.Size = new System.Drawing.Size(100, 33);
            this._untallyBTN.TabIndex = 1;
            this._untallyBTN.Text = "Untally";            
            // 
            // _leftContentPanel
            // 
            this._leftContentPanel.AutoScroll = true;
            this._leftContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftContentPanel.Location = new System.Drawing.Point(0, 0);
            this._leftContentPanel.Name = "_leftContentPanel";
            this._leftContentPanel.Size = new System.Drawing.Size(140, 245);
            // 
            // LayoutTreeBased
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._leftContentPanel);
            this.Controls.Add(panel1);
            this.Name = "LayoutTreeBased";
            this.Size = new System.Drawing.Size(240, 245);
            
            panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel _leftContentPanel;
        private System.Windows.Forms.ListBox _tallyHistoryLB;
        private System.Windows.Forms.Button _untallyBTN;
    }
}
