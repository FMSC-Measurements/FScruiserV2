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
            this.components = new System.ComponentModel.Container();
            this._topTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._leftContentPanel = new System.Windows.Forms.Panel();
            this._rightContentPanel = new System.Windows.Forms.Panel();
            this._tallyHistoryLB = new System.Windows.Forms.ListBox();
            this._untallyBTN = new System.Windows.Forms.Button();
            this._topTableLayout.SuspendLayout();
            this._rightContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _topTableLayout
            // 
            this._topTableLayout.ColumnCount = 2;
            this._topTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._topTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._topTableLayout.Controls.Add(this._leftContentPanel, 0, 0);
            this._topTableLayout.Controls.Add(this._rightContentPanel, 1, 0);
            this._topTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._topTableLayout.Location = new System.Drawing.Point(0, 0);
            this._topTableLayout.Name = "_topTableLayout";
            this._topTableLayout.RowCount = 1;
            this._topTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._topTableLayout.Size = new System.Drawing.Size(637, 368);
            this._topTableLayout.TabIndex = 0;
            // 
            // _leftContentPanel
            // 
            this._leftContentPanel.AutoScroll = true;
            this._leftContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftContentPanel.Location = new System.Drawing.Point(3, 3);
            this._leftContentPanel.Name = "_leftContentPanel";
            this._leftContentPanel.Size = new System.Drawing.Size(481, 362);
            this._leftContentPanel.TabIndex = 0;
            // 
            // _rightContentPanel
            // 
            this._rightContentPanel.Controls.Add(this._tallyHistoryLB);
            this._rightContentPanel.Controls.Add(this._untallyBTN);
            this._rightContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightContentPanel.Location = new System.Drawing.Point(490, 3);
            this._rightContentPanel.Name = "_rightContentPanel";
            this._rightContentPanel.Size = new System.Drawing.Size(144, 362);
            this._rightContentPanel.TabIndex = 1;
            // 
            // _tallyHistoryLB
            // 
            this._tallyHistoryLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyHistoryLB.FormattingEnabled = true;
            this._tallyHistoryLB.IntegralHeight = false;
            this._tallyHistoryLB.Location = new System.Drawing.Point(0, 0);
            this._tallyHistoryLB.Name = "_tallyHistoryLB";
            this._tallyHistoryLB.Size = new System.Drawing.Size(144, 329);
            this._tallyHistoryLB.TabIndex = 0;
            // 
            // _untallyBTN
            // 
            this._untallyBTN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._untallyBTN.Location = new System.Drawing.Point(0, 329);
            this._untallyBTN.Name = "_untallyBTN";
            this._untallyBTN.Size = new System.Drawing.Size(144, 33);
            this._untallyBTN.TabIndex = 1;
            this._untallyBTN.Text = "Untally";
            this._untallyBTN.UseVisualStyleBackColor = true;
            // 
            // LayoutTreeBased
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._topTableLayout);
            this.Name = "LayoutTreeBased";
            this.Size = new System.Drawing.Size(637, 368);
            this._topTableLayout.ResumeLayout(false);
            this._rightContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _topTableLayout;
        private System.Windows.Forms.Panel _leftContentPanel;
        private System.Windows.Forms.Panel _rightContentPanel;
        private System.Windows.Forms.Button _untallyBTN;
        private System.Windows.Forms.ListBox _tallyHistoryLB;
    }
}
