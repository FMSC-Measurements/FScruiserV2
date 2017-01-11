using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.NetCF.WinForms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            KeyPreview = true;

            var settings = ApplicationSettings.Instance;

            _enableTallySound.Checked = settings.EnableTallySound;
            _enablePageChangeSound.Checked = settings.EnablePageChangeSound;

            _untallyHotKeySelect.KeyInfo = settings.UntallyKeyStr;

            _jumpTreeTallyHotKeySelect.KeyInfo = settings.JumpTreeTallyKeyStr;


            Refresh();
        }


        void EndEdits()
        {

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
           base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DialogResult == DialogResult.OK)
            {
                var settings = ApplicationSettings.Instance;

                settings.UntallyKeyStr = _untallyHotKeySelect.KeyInfo;

                settings.JumpTreeTallyKeyStr = _jumpTreeTallyHotKeySelect.KeyInfo;

                settings.EnablePageChangeSound = _enablePageChangeSound.Checked;
                settings.EnableTallySound = _enableTallySound.Checked;
            }
        }
    }
}