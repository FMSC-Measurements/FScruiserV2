﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl, ITallyButton
    {
        CountTree _count;

        private TallyRowButton _settingsBTN;
        private TallyRowButton _tallyBTN;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public TallyRow(CountTree count)
        {
            InitializeComponent();

            _tallyBTN.Click += new EventHandler(this.OnTallyButtonClicked);
            _settingsBTN.Click += new EventHandler(this.OnSettingsButtonClicked);
            this.Count = count;
            UpdateTallyButton();
        }

        public event EventHandler SettingsButtonClicked;

        public event EventHandler TallyButtonClicked;

        public CountTree Count
        {
            get { return _count; }
            set
            {
                UnWireCount();
                _count = value;
                WireUpCount();
            }
        }

        public void AdjustHeight()
        {
            var g = base.CreateGraphics();
            var fHeight = g.MeasureString("|", _tallyBTN.Font).Height;
            this.Height = (int)Math.Ceiling(fHeight) + 5;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                UnWireCount();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustHeight();
        }

        protected void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            if (SettingsButtonClicked != null)
            {
                this.SettingsButtonClicked(this, e);
                UpdateTallyButton();
            }
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {
            if (TallyButtonClicked != null)
            {
                this.TallyButtonClicked(this, e);
            }
        }

        void Count_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TreeCount")
            {
                UpdateTallyButton();
            }
        }

        void UnWireCount()
        {
            if (Count != null)
            {
                Count.PropertyChanged -= Count_PropertyChanged;
            }
        }

        void UpdateTallyButton()
        {
            var hotkey = (!String.IsNullOrEmpty(Count.Tally.Hotkey)) ?
                "[" + Count.Tally.Hotkey.Substring(0, 1) + "] "
                : String.Empty;

            this._tallyBTN.Text = string.Format("{0}{1}|{2}"
                    , hotkey
                    , Count.Tally.Description
                    , Count.TreeCount);
        }

        void WireUpCount()
        {
            if (Count != null)
            {
                this.Count.PropertyChanged += new PropertyChangedEventHandler(Count_PropertyChanged);
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tallyBTN = new FSCruiser.WinForms.DataEntry.TallyRow.TallyRowButton();
            this._settingsBTN = new FSCruiser.WinForms.DataEntry.TallyRow.TallyRowButton();
            this.SuspendLayout();
            //
            // _tallyBTN
            //
            this._tallyBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyBTN.Font = new System.Drawing.Font("Tahoma", 12.25F, System.Drawing.FontStyle.Bold);
            this._tallyBTN.Location = new System.Drawing.Point(0, 0);
            this._tallyBTN.Name = "_tallyBTN";
            this._tallyBTN.Size = new System.Drawing.Size(143, 35);
            this._tallyBTN.TabIndex = 3;
            this._tallyBTN.Text = "[_] <----> ###";
            //
            // _settingsBTN
            //
            this._settingsBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._settingsBTN.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold);
            this._settingsBTN.Location = new System.Drawing.Point(143, 0);
            this._settingsBTN.Name = "_settingsBTN";
            this._settingsBTN.Size = new System.Drawing.Size(17, 35);
            this._settingsBTN.TabIndex = 2;
            this._settingsBTN.Text = "i";
            //
            // TallyRow
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this._tallyBTN);
            this.Controls.Add(this._settingsBTN);
            this.Name = "TallyRow";
            this.Size = new System.Drawing.Size(160, 35);
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        protected class TallyRowButton
#if NetCF
 : FMSC.Controls.ButtonPanel
#else
 : Button
#endif
        {
            public TallyRowButton() : base() { }
        }
    }
}