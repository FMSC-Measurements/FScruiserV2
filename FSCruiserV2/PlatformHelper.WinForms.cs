using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FSCruiser
{
    public class PlatformHelper
    {
        public static string KeyToStr(Keys value)
        {
            var c = KeyToChar(value);

            if (c != char.MinValue)
            {
                return c.ToString();
            }
            else
            {
                return value.ToString();
            }
        }

        public static char KeyToChar(Keys value)
        {
            switch(value)
            {
                case Keys.A: case Keys.B: case Keys.C: case Keys.D: case Keys.E: case Keys.F:
                case Keys.G: case Keys.H: case Keys.I: case Keys.J: case Keys.K: case Keys.L:
                case Keys.M: case Keys.N: case Keys.O: case Keys.P: case Keys.Q: case Keys.R:
                case Keys.S: case Keys.T: case Keys.U: case Keys.V: case Keys.W: case Keys.X:
                case Keys.Y: case Keys.Z:
                    {
                        return (char)(value & Keys.KeyCode);
                    }
                case Keys.NumPad0:
                case Keys.D0: { return '0'; }
                case Keys.NumPad1:
                case Keys.D1: { return '1'; }
                case Keys.NumPad2:
                case Keys.D2: { return '2'; }
                case Keys.NumPad3:
                case Keys.D3: { return '3'; }
                case Keys.NumPad4:
                case Keys.D4: { return '4'; }
                case Keys.NumPad5:
                case Keys.D5: { return '5'; }
                case Keys.NumPad6:
                case Keys.D6: { return '6'; }
                case Keys.NumPad7:
                case Keys.D7: { return '7'; }
                case Keys.NumPad8:
                case Keys.D8: { return '8'; }
                case Keys.NumPad9:
                case Keys.D9: { return '9'; }

                case Keys.Add: { return '+'; }
                case Keys.Decimal: { return '.'; }
                case Keys.Divide: { return '/'; }
                case Keys.Multiply: { return '*'; }
                case Keys.Subtract: { return '-'; }
#if !NetCF
                case Keys.OemSemicolon: //oem1
                    return ';';
                case Keys.OemQuestion:  //oem2
                    return '?';
                case Keys.Oemtilde:     //oem3
                    return '~';
                case Keys.OemOpenBrackets:  //oem4
                    return '[';
                case Keys.OemPipe:  //oem5
                    return '|';
                case Keys.OemCloseBrackets:    //oem6
                    return ']';
                case Keys.OemQuotes:        //oem7
                    return ''';
                case Keys.OemBackslash: //oem102
                    return '/';
                case Keys.Oemplus:
                    return '+';
                case Keys.OemMinus:
                    return '-';
                case Keys.Oemcomma:
                    return ',';
                case Keys.OemPeriod:
                    return '.';
#endif
                default:
                    {
                        return Char.MinValue;
                    }            
            }
        }
    }
}
