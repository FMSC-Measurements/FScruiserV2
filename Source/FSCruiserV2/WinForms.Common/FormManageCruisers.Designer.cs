namespace FSCruiser.WinForms
{
    partial class FormManageCruisers
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._enableCruiserPopupCB = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._addBTN = new System.Windows.Forms.Button();
            this._initialsTB = new System.Windows.Forms.TextBox();
            this._cruiserListContainer = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._removeItemBTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.panel1.SuspendLayout();
            this._cruiserListContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._enableCruiserPopupCB);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._addBTN);
            this.panel1.Controls.Add(this._initialsTB);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 51);
            // 
            // _enableCruiserPopupCB
            // 
            this._enableCruiserPopupCB.Location = new System.Drawing.Point(3, 27);
            this._enableCruiserPopupCB.Name = "_enableCruiserPopupCB";
            this._enableCruiserPopupCB.Size = new System.Drawing.Size(224, 20);
            this._enableCruiserPopupCB.TabIndex = 2;
            this._enableCruiserPopupCB.Text = "Show Cruiser Selection Popup";
            this._enableCruiserPopupCB.CheckStateChanged += new System.EventHandler(this._enableCruiserPopupCB_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 20);
            this.label1.Text = "Enter Cruiser Initials:";
            // 
            // _addBTN
            // 
            this._addBTN.Location = new System.Drawing.Point(186, 3);
            this._addBTN.Name = "_addBTN";
            this._addBTN.Size = new System.Drawing.Size(41, 20);
            this._addBTN.TabIndex = 1;
            this._addBTN.Text = "Add";
            this._addBTN.Click += new System.EventHandler(this._addBTN_Click);
            // 
            // _initialsTB
            // 
            this._initialsTB.Location = new System.Drawing.Point(132, 3);
            this._initialsTB.MaxLength = 3;
            this._initialsTB.Name = "_initialsTB";
            this._initialsTB.Size = new System.Drawing.Size(48, 23);
            this._initialsTB.TabIndex = 0;
            this._initialsTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._initialsTB_KeyPress);
            // 
            // _cruiserListContainer
            // 
            this._cruiserListContainer.AutoScroll = true;
            this._cruiserListContainer.BackColor = System.Drawing.Color.LightGray;
            this._cruiserListContainer.Controls.Add(this.panel3);
            this._cruiserListContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cruiserListContainer.Location = new System.Drawing.Point(0, 51);
            this._cruiserListContainer.Name = "_cruiserListContainer";
            this._cruiserListContainer.Size = new System.Drawing.Size(240, 217);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Tan;
            this.panel3.Controls.Add(this._removeItemBTN);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(240, 25);
            // 
            // _removeItemBTN
            // 
            this._removeItemBTN.BackColor = System.Drawing.Color.Crimson;
            this._removeItemBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._removeItemBTN.Location = new System.Drawing.Point(213, 0);
            this._removeItemBTN.Name = "_removeItemBTN";
            this._removeItemBTN.Size = new System.Drawing.Size(27, 25);
            this._removeItemBTN.TabIndex = 1;
            this._removeItemBTN.Text = "X";
            this._removeItemBTN.Click += new System.EventHandler(this._removeItemBTN_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.Text = "<init>";
            // 
            // FormManageCruisers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._cruiserListContainer);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "FormManageCruisers";
            this.Text = "Manage Cruisers";
            this.panel1.ResumeLayout(false);
            this._cruiserListContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _addBTN;
        private System.Windows.Forms.TextBox _initialsTB;
        private System.Windows.Forms.CheckBox _enableCruiserPopupCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _cruiserListContainer;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button _removeItemBTN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MainMenu mainMenu1;
    }
}