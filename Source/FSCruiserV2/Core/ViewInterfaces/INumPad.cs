using System.Windows.Forms;

namespace FSCruiser.Core.ViewInterfaces
{
    interface INumPad
    {
        DialogResult ShowDialog(int? initialValue, bool canReturnNull);

        DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull);

        int? UserEnteredValue { get; set; }
    }
}