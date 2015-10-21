namespace FSCruiserV2.Forms
{
    partial class FormDataEntry
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
            this.components = new System.ComponentModel.Container();
            this._BS_TDV = new System.Windows.Forms.BindingSource(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this._deleteRowButton = new System.Windows.Forms.MenuItem();
            this._showHideErrorColMI = new System.Windows.Forms.MenuItem();
            this._limitingDistanceMI = new System.Windows.Forms.MenuItem();
            this._editCruisersMI = new System.Windows.Forms.MenuItem();
            this._showHideLogColMI = new System.Windows.Forms.MenuItem();
            this._addTreeMI = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._BS_TDV)).BeginInit();
            this.SuspendLayout();
            // 
            // _BS_TDV
            // 
            this._BS_TDV.DataSource = typeof(CruiseDAL.DataObjects.TreeDefaultValueDO);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this._addTreeMI);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this._deleteRowButton);
            this.menuItem1.MenuItems.Add(this._limitingDistanceMI);
            this.menuItem1.MenuItems.Add(this._editCruisersMI);
            this.menuItem1.MenuItems.Add(this._showHideErrorColMI);
            this.menuItem1.MenuItems.Add(this._showHideLogColMI);
            this.menuItem1.Text = "Menu";
            this.menuItem1.Popup += new System.EventHandler(this.menuItem1_Popup);
            // 
            // _deleteRowButton
            // 
            this._deleteRowButton.Text = "&Delete Tree";
            this._deleteRowButton.Click += new System.EventHandler(this._deleteRowButton_Click);
            // 
            // _showHideErrorColMI
            // 
            this._showHideErrorColMI.Text = "&Show/Hide Error Message Col.";
            this._showHideErrorColMI.Click += new System.EventHandler(this.showHideErrorMessages_Click);
            // 
            // _limitingDistanceMI
            // 
            this._limitingDistanceMI.Text = "&Limiting Distance";
            this._limitingDistanceMI.Click += new System.EventHandler(this.LimitingDistance_Click);
            // 
            // _editCruisersMI
            // 
            this._editCruisersMI.Text = "&Edit Cruisers";
            this._editCruisersMI.Click += new System.EventHandler(this._editCruisersMI_Click);
            // 
            // _showHideLogColMI
            // 
            this._showHideLogColMI.Text = "S&how/Hide Log Column";
            this._showHideLogColMI.Click += new System.EventHandler(this._showHideLogColMI_Click);
            // 
            // _addTreeMI
            // 
            this._addTreeMI.Enabled = false;
            this._addTreeMI.Text = "Add Tree";
            this._addTreeMI.Click += new System.EventHandler(this._addTreeMI_Click);
            // 
            // FormDataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormDataEntry";
            this.Text = "FormDataEntry";
            ((System.ComponentModel.ISupportInitialize)(this._BS_TDV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.BindingSource _BS_TDV;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem _deleteRowButton;
        private System.Windows.Forms.MenuItem _showHideErrorColMI;
        private System.Windows.Forms.MenuItem _limitingDistanceMI;
        private System.Windows.Forms.MenuItem _editCruisersMI;
        private System.Windows.Forms.MenuItem _showHideLogColMI;
        private System.Windows.Forms.MenuItem _addTreeMI;
        
    }
}