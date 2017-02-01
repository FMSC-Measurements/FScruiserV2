using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FSCruiser.WinForms.Controls
{
    public partial class HotKeySelectControl : TextBox
    {
        Keys[] INVALID_KEY_VALUES = new Keys[] { Keys.Back, Keys.Delete };
        Keys[] INVALID_MODIFIER_VALUES = new Keys[] { Keys.Shift, Keys.ShiftKey };

        KeysConverter _keysConverter = new KeysConverter();
        string _keyInfo;
        public string KeyInfo 
        {
            get { return _keyInfo; }
            set
            {
                _keyInfo = value;
                Text = value;
            }
        }

        public HotKeySelectControl()
        {
            KeyInfo = String.Empty;
        }

        public override bool Multiline
        {
            get
            {
                return base.Multiline;
            }
            set
            {
                base.Multiline = false;// ignore user entered value
            }
        }

        

        void ResetKeys()
        {
            KeyInfo = String.Empty;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            SetHotKey(e);
        }

        

        void SetHotKey(KeyEventArgs ea)
        {
            if (INVALID_KEY_VALUES.Contains(ea.KeyCode)
                || INVALID_MODIFIER_VALUES.Contains(ea.Modifiers))
            {
                ResetKeys();
                return;
            }

            KeyInfo = _keysConverter.ConvertToString(ea.KeyData);
        }




    }
}
