using System;
using System.Windows.Forms;

namespace FSCruiserV2.Forms
{
    interface INumPad
    {
        DialogResult ShowDialog(int? initialValue, bool canReturnNull);
        DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull);
        int? UserEnteredValue { get; set; } 
    }
}
