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
        string _keyStr;
        public string KeyStr 
        {
            get { return _keyStr; }
            set
            {
                _keyStr = value;
                Text = value;
            }
        }

        public HotKeySelectControl()
        {
            KeyStr = String.Empty;
        }

        public override bool Multiline
        {
            get
            {
                return false;
            }
            set
            {}
        }

        

        void ResetKeys()
        {
            KeyStr = String.Empty;
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

            KeyStr = _keysConverter.ConvertToString(ea.KeyData);
        }




    }
}
