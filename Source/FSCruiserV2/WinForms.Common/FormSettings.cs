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

            _untallyHotKeySelect.KeyStr = settings.UntallyKeyStr;
            _jumpTreeTallyHotKeySelect.KeyStr = settings.JumpTreeTallyKeyStr;
            _resequencePlotTreesHotKeySelectControl.KeyStr = settings.ResequencePlotTreesKeyStr;
            _addTreeHotKeySelectControl.KeyStr = settings.AddTreeKeyStr;
            _addPlotHotKeySelectControl.KeyStr = settings.AddPlotKeyStr;

#if NetCF

#else
            soundsPanel.Visible = false;
            _enableTallySound.Visible = false;
            _enablePageChangeSound.Visible = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
#endif

            Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DialogResult == DialogResult.OK)
            {
                var settings = ApplicationSettings.Instance;

                settings.UntallyKeyStr = _untallyHotKeySelect.KeyStr;
                settings.JumpTreeTallyKeyStr = _jumpTreeTallyHotKeySelect.KeyStr;
                settings.ResequencePlotTreesKeyStr = _resequencePlotTreesHotKeySelectControl.KeyStr;
                settings.AddPlotKeyStr = _addPlotHotKeySelectControl.KeyStr;
                settings.AddTreeKeyStr = _addTreeHotKeySelectControl.KeyStr;

                settings.EnablePageChangeSound = _enablePageChangeSound.Checked;
                settings.EnableTallySound = _enableTallySound.Checked;
                settings.EnableAskEnterTreeData = _askEnterTreeData.Checked;
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel hotKeysPanel;
        private Panel soundsPanel;
        private Button _ok_button;

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
            System.Windows.Forms.Panel panel8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Panel panel7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Panel panel6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Panel panel5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Panel panel4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel dialogBtnPanel;
            System.Windows.Forms.Panel notificationsPanel;
            System.Windows.Forms.Label label8;
            this._addPlotHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this._addTreeHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this._resequencePlotTreesHotKeySelectControl = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this._jumpTreeTallyHotKeySelect = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this._untallyHotKeySelect = new FSCruiser.WinForms.Controls.HotKeySelectControl();
            this.soundsPanel = new System.Windows.Forms.Panel();
            this._enableTallySound = new System.Windows.Forms.CheckBox();
            this._enablePageChangeSound = new System.Windows.Forms.CheckBox();
            this._ok_button = new System.Windows.Forms.Button();
            this._cancel_button = new System.Windows.Forms.Button();
            this._askEnterTreeData = new System.Windows.Forms.CheckBox();
            this.hotKeysPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            panel8 = new System.Windows.Forms.Panel();
            label7 = new System.Windows.Forms.Label();
            panel7 = new System.Windows.Forms.Panel();
            label6 = new System.Windows.Forms.Label();
            panel6 = new System.Windows.Forms.Panel();
            label5 = new System.Windows.Forms.Label();
            panel5 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            panel4 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            dialogBtnPanel = new System.Windows.Forms.Panel();
            notificationsPanel = new System.Windows.Forms.Panel();
            label8 = new System.Windows.Forms.Label();
            panel8.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            this.soundsPanel.SuspendLayout();
            dialogBtnPanel.SuspendLayout();
            notificationsPanel.SuspendLayout();
            this.hotKeysPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // panel8
            //
            panel8.BackColor = System.Drawing.SystemColors.Info;
            panel8.Controls.Add(this._addPlotHotKeySelectControl);
            panel8.Controls.Add(label7);
            panel8.Dock = System.Windows.Forms.DockStyle.Top;
            panel8.Location = new System.Drawing.Point(0, 112);
            panel8.Name = "panel8";
            panel8.Size = new System.Drawing.Size(249, 23);
            panel8.TabIndex = 0;
            //
            // _addPlotHotKeySelectControl
            //
            this._addPlotHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._addPlotHotKeySelectControl.KeyStr = "";
            this._addPlotHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._addPlotHotKeySelectControl.Name = "_addPlotHotKeySelectControl";
            this._addPlotHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._addPlotHotKeySelectControl.TabIndex = 3;
            //
            // label7
            //
            label7.Dock = System.Windows.Forms.DockStyle.Left;
            label7.Location = new System.Drawing.Point(0, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(147, 23);
            label7.TabIndex = 4;
            label7.Text = "Add Plot";
            //
            // panel7
            //
            panel7.BackColor = System.Drawing.SystemColors.Info;
            panel7.Controls.Add(this._addTreeHotKeySelectControl);
            panel7.Controls.Add(label6);
            panel7.Dock = System.Windows.Forms.DockStyle.Top;
            panel7.Location = new System.Drawing.Point(0, 89);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(249, 23);
            panel7.TabIndex = 1;
            //
            // _addTreeHotKeySelectControl
            //
            this._addTreeHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._addTreeHotKeySelectControl.KeyStr = "";
            this._addTreeHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._addTreeHotKeySelectControl.Name = "_addTreeHotKeySelectControl";
            this._addTreeHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._addTreeHotKeySelectControl.TabIndex = 3;
            //
            // label6
            //
            label6.Dock = System.Windows.Forms.DockStyle.Left;
            label6.Location = new System.Drawing.Point(0, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(147, 23);
            label6.TabIndex = 4;
            label6.Text = "Add Tree";
            //
            // panel6
            //
            panel6.BackColor = System.Drawing.SystemColors.Info;
            panel6.Controls.Add(this._resequencePlotTreesHotKeySelectControl);
            panel6.Controls.Add(label5);
            panel6.Dock = System.Windows.Forms.DockStyle.Top;
            panel6.Location = new System.Drawing.Point(0, 66);
            panel6.Name = "panel6";
            panel6.Size = new System.Drawing.Size(249, 23);
            panel6.TabIndex = 2;
            //
            // _resequencePlotTreesHotKeySelectControl
            //
            this._resequencePlotTreesHotKeySelectControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._resequencePlotTreesHotKeySelectControl.KeyStr = "";
            this._resequencePlotTreesHotKeySelectControl.Location = new System.Drawing.Point(147, 0);
            this._resequencePlotTreesHotKeySelectControl.Name = "_resequencePlotTreesHotKeySelectControl";
            this._resequencePlotTreesHotKeySelectControl.Size = new System.Drawing.Size(57, 20);
            this._resequencePlotTreesHotKeySelectControl.TabIndex = 3;
            //
            // label5
            //
            label5.Dock = System.Windows.Forms.DockStyle.Left;
            label5.Location = new System.Drawing.Point(0, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(147, 23);
            label5.TabIndex = 4;
            label5.Text = "Resequence Plot Trees";
            //
            // panel5
            //
            panel5.BackColor = System.Drawing.SystemColors.Info;
            panel5.Controls.Add(this._jumpTreeTallyHotKeySelect);
            panel5.Controls.Add(label4);
            panel5.Dock = System.Windows.Forms.DockStyle.Top;
            panel5.Location = new System.Drawing.Point(0, 43);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(249, 23);
            panel5.TabIndex = 3;
            //
            // _jumpTreeTallyHotKeySelect
            //
            this._jumpTreeTallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._jumpTreeTallyHotKeySelect.KeyStr = "";
            this._jumpTreeTallyHotKeySelect.Location = new System.Drawing.Point(147, 0);
            this._jumpTreeTallyHotKeySelect.Name = "_jumpTreeTallyHotKeySelect";
            this._jumpTreeTallyHotKeySelect.Size = new System.Drawing.Size(57, 20);
            this._jumpTreeTallyHotKeySelect.TabIndex = 3;
            //
            // label4
            //
            label4.Dock = System.Windows.Forms.DockStyle.Left;
            label4.Location = new System.Drawing.Point(0, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(147, 23);
            label4.TabIndex = 4;
            label4.Text = "Jump Tree-Tally page";
            //
            // panel4
            //
            panel4.BackColor = System.Drawing.SystemColors.Info;
            panel4.Controls.Add(this._untallyHotKeySelect);
            panel4.Controls.Add(label3);
            panel4.Dock = System.Windows.Forms.DockStyle.Top;
            panel4.Location = new System.Drawing.Point(0, 20);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(249, 23);
            panel4.TabIndex = 4;
            //
            // _untallyHotKeySelect
            //
            this._untallyHotKeySelect.Dock = System.Windows.Forms.DockStyle.Left;
            this._untallyHotKeySelect.KeyStr = "";
            this._untallyHotKeySelect.Location = new System.Drawing.Point(147, 0);
            this._untallyHotKeySelect.Name = "_untallyHotKeySelect";
            this._untallyHotKeySelect.Size = new System.Drawing.Size(57, 20);
            this._untallyHotKeySelect.TabIndex = 3;
            //
            // label3
            //
            label3.Dock = System.Windows.Forms.DockStyle.Left;
            label3.Location = new System.Drawing.Point(0, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(147, 23);
            label3.TabIndex = 4;
            label3.Text = "Untally";
            //
            // label2
            //
            label2.BackColor = System.Drawing.Color.Silver;
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(249, 20);
            label2.TabIndex = 5;
            label2.Text = "Hot Keys";
            //
            // soundsPanel
            //
            this.soundsPanel.BackColor = System.Drawing.SystemColors.Info;
            this.soundsPanel.Controls.Add(this._enableTallySound);
            this.soundsPanel.Controls.Add(this._enablePageChangeSound);
            this.soundsPanel.Controls.Add(label1);
            this.soundsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.soundsPanel.Location = new System.Drawing.Point(0, 0);
            this.soundsPanel.Name = "soundsPanel";
            this.soundsPanel.Size = new System.Drawing.Size(249, 69);
            this.soundsPanel.TabIndex = 2;
            //
            // _enableTallySound
            //
            this._enableTallySound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enableTallySound.Location = new System.Drawing.Point(0, 40);
            this._enableTallySound.Name = "_enableTallySound";
            this._enableTallySound.Size = new System.Drawing.Size(249, 20);
            this._enableTallySound.TabIndex = 3;
            this._enableTallySound.Text = "Tally";
            //
            // _enablePageChangeSound
            //
            this._enablePageChangeSound.Dock = System.Windows.Forms.DockStyle.Top;
            this._enablePageChangeSound.Location = new System.Drawing.Point(0, 20);
            this._enablePageChangeSound.Name = "_enablePageChangeSound";
            this._enablePageChangeSound.Size = new System.Drawing.Size(249, 20);
            this._enablePageChangeSound.TabIndex = 4;
            this._enablePageChangeSound.Text = "Page Changed";
            //
            // label1
            //
            label1.BackColor = System.Drawing.Color.Silver;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(249, 20);
            label1.TabIndex = 5;
            label1.Text = "Sounds  ";
            //
            // dialogBtnPanel
            //
            dialogBtnPanel.Controls.Add(this._ok_button);
            dialogBtnPanel.Controls.Add(this._cancel_button);
            dialogBtnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            dialogBtnPanel.Location = new System.Drawing.Point(0, 248);
            dialogBtnPanel.Name = "dialogBtnPanel";
            dialogBtnPanel.Size = new System.Drawing.Size(266, 27);
            dialogBtnPanel.TabIndex = 1;
            //
            // _ok_button
            //
            this._ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok_button.Dock = System.Windows.Forms.DockStyle.Left;
            this._ok_button.Location = new System.Drawing.Point(0, 0);
            this._ok_button.Name = "_ok_button";
            this._ok_button.Size = new System.Drawing.Size(110, 27);
            this._ok_button.TabIndex = 3;
            this._ok_button.Text = "OK";
            //
            // _cancel_button
            //
            this._cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel_button.Dock = System.Windows.Forms.DockStyle.Right;
            this._cancel_button.Location = new System.Drawing.Point(156, 0);
            this._cancel_button.Name = "_cancel_button";
            this._cancel_button.Size = new System.Drawing.Size(110, 27);
            this._cancel_button.TabIndex = 2;
            this._cancel_button.Text = "Cancel";
            //
            // notificationsPanel
            //
            notificationsPanel.BackColor = System.Drawing.SystemColors.Info;
            notificationsPanel.Controls.Add(this._askEnterTreeData);
            notificationsPanel.Controls.Add(label8);
            notificationsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            notificationsPanel.Location = new System.Drawing.Point(0, 69);
            notificationsPanel.Name = "notificationsPanel";
            notificationsPanel.Size = new System.Drawing.Size(249, 49);
            notificationsPanel.TabIndex = 1;
            //
            // _askEnterTreeData
            //
            this._askEnterTreeData.Dock = System.Windows.Forms.DockStyle.Top;
            this._askEnterTreeData.Location = new System.Drawing.Point(0, 20);
            this._askEnterTreeData.Name = "_askEnterTreeData";
            this._askEnterTreeData.Size = new System.Drawing.Size(249, 20);
            this._askEnterTreeData.TabIndex = 6;
            this._askEnterTreeData.Text = "Ask Enter Tree Data";
            //
            // label8
            //
            label8.BackColor = System.Drawing.Color.Silver;
            label8.Dock = System.Windows.Forms.DockStyle.Top;
            label8.Location = new System.Drawing.Point(0, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(249, 20);
            label8.TabIndex = 7;
            label8.Text = "Notifications";
            //
            // hotKeysPanel
            //
            this.hotKeysPanel.BackColor = System.Drawing.SystemColors.Info;
            this.hotKeysPanel.Controls.Add(panel8);
            this.hotKeysPanel.Controls.Add(panel7);
            this.hotKeysPanel.Controls.Add(panel6);
            this.hotKeysPanel.Controls.Add(panel5);
            this.hotKeysPanel.Controls.Add(panel4);
            this.hotKeysPanel.Controls.Add(label2);
            this.hotKeysPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hotKeysPanel.Location = new System.Drawing.Point(0, 118);
            this.hotKeysPanel.Name = "hotKeysPanel";
            this.hotKeysPanel.Size = new System.Drawing.Size(249, 154);
            this.hotKeysPanel.TabIndex = 0;
            //
            // contentPanel
            //
            this.contentPanel.AutoScroll = true;
            this.contentPanel.BackColor = System.Drawing.SystemColors.Info;
            this.contentPanel.Controls.Add(this.hotKeysPanel);
            this.contentPanel.Controls.Add(notificationsPanel);
            this.contentPanel.Controls.Add(this.soundsPanel);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(266, 248);
            this.contentPanel.TabIndex = 0;
            //
            // FormSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(266, 275);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(dialogBtnPanel);
            this.Name = "FormSettings";
            this.Text = "Settings";
            panel8.ResumeLayout(false);
            panel7.ResumeLayout(false);;
            panel6.ResumeLayout(false);;
            panel5.ResumeLayout(false);
            panel4.ResumeLayout(false);
            this.soundsPanel.ResumeLayout(false);
            dialogBtnPanel.ResumeLayout(false);
            notificationsPanel.ResumeLayout(false);
            this.hotKeysPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.CheckBox _enableTallySound;
        private System.Windows.Forms.CheckBox _enablePageChangeSound;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _jumpTreeTallyHotKeySelect;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _untallyHotKeySelect;
        private System.Windows.Forms.Button _cancel_button;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _resequencePlotTreesHotKeySelectControl;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _addTreeHotKeySelectControl;
        private FSCruiser.WinForms.Controls.HotKeySelectControl _addPlotHotKeySelectControl;
        private System.Windows.Forms.CheckBox _askEnterTreeData;
    }
}