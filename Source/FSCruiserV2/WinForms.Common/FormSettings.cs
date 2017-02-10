using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            var settings = ApplicationSettings.Instance;

            _enableTallySound.Checked = settings.EnableTallySound;
            _enablePageChangeSound.Checked = settings.EnablePageChangeSound;
            _askEnterTreeData.Checked = settings.EnableAskEnterTreeData;

            _untallyHotKeySelect.KeyInfo = settings.UntallyKeyStr;
            _jumpTreeTallyHotKeySelect.KeyInfo = settings.JumpTreeTallyKeyStr;
            _resequencePlotTreesHotKeySelectControl.KeyInfo = settings.ResequencePlotTreesKeyStr;

#if !NetCF
            _enableTallySound.Visible = false;
            _enablePageChangeSound.Visible = false;
#endif

            Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DialogResult == DialogResult.OK)
            {
                var settings = ApplicationSettings.Instance;

                settings.UntallyKeyStr = _untallyHotKeySelect.KeyInfo;
                settings.JumpTreeTallyKeyStr = _jumpTreeTallyHotKeySelect.KeyInfo;
                settings.ResequencePlotTreesKeyStr = _resequencePlotTreesHotKeySelectControl.KeyInfo;

                settings.EnablePageChangeSound = _enablePageChangeSound.Checked;
                settings.EnableTallySound = _enableTallySound.Checked;
                settings.EnableAskEnterTreeData = _askEnterTreeData.Checked;
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panel3;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Panel panel2;
        private Label label1;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this._addPlotHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.label7 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this._addTreeHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.label6 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this._resequencePlotTreesHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this._jumpTreeTallyHotKeySelect = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this._untallyHotKeySelect = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._askEnterTreeData = new System.Windows.Forms.CheckBox();
            this._enableTallySound = new System.Windows.Forms.CheckBox();
            this._enablePageChangeSound = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancel_button = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel3
            //
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 154);
            this.panel3.TabIndex = 3;
            //
            // panel8
            //
            this.panel8.BackColor = System.Drawing.SystemColors.Info;
            this.panel8.Controls.Add(this._addPlotHotKeySelectControl);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 112);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(266, 23);
            this.panel8.TabIndex = 0;
            //
            // _addPlotHotKeySelectControl
            //
            this._addPlotHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._addPlotHotKeySelectControl.KeyInfo = "";
            this._addPlotHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._addPlotHotKeySelectControl.Name = "_addPlotHotKeySelectControl";
            this._addPlotHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._addPlotHotKeySelectControl.TabIndex = 3;
            //
            // label7
            //
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Add Plot";
            //
            // panel7
            //
            this.panel7.BackColor = System.Drawing.SystemColors.Info;
            this.panel7.Controls.Add(this._addTreeHotKeySelectControl);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 89);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(266, 23);
            this.panel7.TabIndex = 1;
            //
            // _addTreeHotKeySelectControl
            //
            this._addTreeHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._addTreeHotKeySelectControl.KeyInfo = "";
            this._addTreeHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._addTreeHotKeySelectControl.Name = "_addTreeHotKeySelectControl";
            this._addTreeHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._addTreeHotKeySelectControl.TabIndex = 3;
            //
            // label6
            //
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "Add Tree";
            //
            // panel6
            //
            this.panel6.BackColor = System.Drawing.SystemColors.Info;
            this.panel6.Controls.Add(this._resequencePlotTreesHotKeySelectControl);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 66);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(266, 23);
            this.panel6.TabIndex = 2;
            //
            // _resequencePlotTreesHotKeySelectControl
            //
            this._resequencePlotTreesHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._resequencePlotTreesHotKeySelectControl.KeyInfo = "";
            this._resequencePlotTreesHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._resequencePlotTreesHotKeySelectControl.Name = "_resequencePlotTreesHotKeySelectControl";
            this._resequencePlotTreesHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._resequencePlotTreesHotKeySelectControl.TabIndex = 3;
            //
            // label5
            //
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Resequence Plot Trees";
            //
            // panel5
            //
            this.panel5.BackColor = System.Drawing.SystemColors.Info;
            this.panel5.Controls.Add(this._jumpTreeTallyHotKeySelect);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 43);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 23);
            this.panel5.TabIndex = 3;
            //
            // _jumpTreeTallyHotKeySelect
            //
            this._jumpTreeTallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._jumpTreeTallyHotKeySelect.KeyInfo = "";
            this._jumpTreeTallyHotKeySelect.Location = new System.Drawing.Point(147, 0);
            this._jumpTreeTallyHotKeySelect.Name = "_jumpTreeTallyHotKeySelect";
            this._jumpTreeTallyHotKeySelect.Size = new System.Drawing.Size(57, 20);
            this._jumpTreeTallyHotKeySelect.TabIndex = 3;
            //
            // label4
            //
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Jump Tree-Tally page";
            //
            // panel4
            //
            this.panel4.BackColor = System.Drawing.SystemColors.Info;
            this.panel4.Controls.Add(this._untallyHotKeySelect);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 23);
            this.panel4.TabIndex = 4;
            //
            // _untallyHotKeySelect
            //
            this._untallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._untallyHotKeySelect.KeyInfo = "";
            this._untallyHotKeySelect.Location = new System.Drawing.Point(147, 0);
            this._untallyHotKeySelect.Name = "_untallyHotKeySelect";
            this._untallyHotKeySelect.Size = new System.Drawing.Size(57, 20);
            this._untallyHotKeySelect.TabIndex = 3;
            //
            // label3
            //
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Untally";
            //
            // label2
            //
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hot Keys";
            //
            // panel2
            //
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.Controls.Add(this._askEnterTreeData);
            this.panel2.Controls.Add(this._enableTallySound);
            this.panel2.Controls.Add(this._enablePageChangeSound);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 96);
            this.panel2.TabIndex = 4;
            //
            // _askEnterTreeData
            //
            this._askEnterTreeData.Dock = System.Windows.Forms.DockStyle.Top;
            this._askEnterTreeData.Location = new System.Drawing.Point(0, 60);
            this._askEnterTreeData.Name = "_askEnterTreeData";
            this._askEnterTreeData.Size = new System.Drawing.Size(266, 20);
            this._askEnterTreeData.TabIndex = 6;
            this._askEnterTreeData.Text = "Ask Enter Tree Data";
            //
            // _enableTallySound
            //
            this._enableTallySound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enableTallySound.Location = new System.Drawing.Point(0, 40);
            this._enableTallySound.Name = "_enableTallySound";
            this._enableTallySound.Size = new System.Drawing.Size(266, 20);
            this._enableTallySound.TabIndex = 3;
            this._enableTallySound.Text = "Tally";
            //
            // _enablePageChangeSound
            //
            this._enablePageChangeSound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enablePageChangeSound.Location = new System.Drawing.Point(0, 20);
            this._enablePageChangeSound.Name = "_enablePageChangeSound";
            this._enablePageChangeSound.Size = new System.Drawing.Size(266, 20);
            this._enablePageChangeSound.TabIndex = 4;
            this._enablePageChangeSound.Text = "Page Changed";
            //
            // label1
            //
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Sounds & Notifications";
            //
            // panel1
            //
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this._cancel_button);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 368);
            this.panel1.TabIndex = 0;
            //
            // _cancel_button
            //
            this._cancel_button.BackColor = System.Drawing.SystemColors.Control;
            this._cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._cancel_button.Location = new System.Drawing.Point(0, 348);
            this._cancel_button.Name = "_cancel_button";
            this._cancel_button.Size = new System.Drawing.Size(266, 20);
            this._cancel_button.TabIndex = 2;
            this._cancel_button.Text = "Cancel";
            //
            // FormSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(266, 368);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.panel3.ResumeLayout(false);
            this.panel8.ResumeLayout(true);
            this.panel7.ResumeLayout(true);
            this.panel6.ResumeLayout(true);
            this.panel5.ResumeLayout(true);
            this.panel4.ResumeLayout(true);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox _enableTallySound;
        private System.Windows.Forms.CheckBox _enablePageChangeSound;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _jumpTreeTallyHotKeySelect;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _untallyHotKeySelect;
        private System.Windows.Forms.Button _cancel_button;
        private System.Windows.Forms.Panel panel6;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _resequencePlotTreesHotKeySelectControl;
        private System.Windows.Forms.Panel panel7;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _addTreeHotKeySelectControl;
        private System.Windows.Forms.Panel panel8;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _addPlotHotKeySelectControl;
        private System.Windows.Forms.CheckBox _askEnterTreeData;
    }
}