namespace FSCruiser.WinForms.DataEntry
{
    partial class FormNumPad
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._outputView = new System.Windows.Forms.TextBox();
            this._okBTN = new System.Windows.Forms.Button();
            this._clearBTN = new System.Windows.Forms.Button();
            this._cancelBTN = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33133F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33433F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33433F));
            this.tableLayoutPanel1.Controls.Add(this._outputView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._okBTN, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._clearBTN, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._cancelBTN, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(261, 49);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _outputView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._outputView, 3);
            this._outputView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outputView.Location = new System.Drawing.Point(3, 3);
            this._outputView.Name = "_outputView";
            this._outputView.Size = new System.Drawing.Size(255, 20);
            this._outputView.TabIndex = 0;
            // 
            // _okBTN
            // 
            this._okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._okBTN.Location = new System.Drawing.Point(3, 27);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(80, 19);
            this._okBTN.TabIndex = 1;
            this._okBTN.Text = "OK";
            this._okBTN.UseVisualStyleBackColor = true;
            // 
            // _clearBTN
            // 
            this._clearBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._clearBTN.Location = new System.Drawing.Point(89, 27);
            this._clearBTN.Name = "_clearBTN";
            this._clearBTN.Size = new System.Drawing.Size(81, 19);
            this._clearBTN.TabIndex = 2;
            this._clearBTN.Text = "Clear";
            this._clearBTN.UseVisualStyleBackColor = true;
            // 
            // _cancelBTN
            // 
            this._cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cancelBTN.Location = new System.Drawing.Point(176, 27);
            this._cancelBTN.Name = "_cancelBTN";
            this._cancelBTN.Size = new System.Drawing.Size(82, 19);
            this._cancelBTN.TabIndex = 3;
            this._cancelBTN.Text = "Cancel";
            this._cancelBTN.UseVisualStyleBackColor = true;
            // 
            // FormNumPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 49);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormNumPad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Number";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _outputView;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.Button _clearBTN;
        private System.Windows.Forms.Button _cancelBTN;
    }
}