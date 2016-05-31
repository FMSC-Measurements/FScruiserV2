using System;
using System.Windows.Forms;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IView
    {
    }

    public static class ViewExtentions
    {
        public static void ShowMessage(this IView view, string message)
        {
#if NetCF
            MessageBox.Show(message);
#else
            var window = view as IWin32Window;
            MessageBox.Show(window, message);
#endif
        }

        public static void ShowMessage(this IView view, string message, string caption)
        {
#if NetCF
            MessageBox.Show(message, caption);
#else
            var window = view as IWin32Window;
            MessageBox.Show(window, message, caption);
#endif
        }

        public static bool AskYesNo(this IView view, String message, String caption, MessageBoxIcon icon)
        {
#if NetCF
            return DialogResult.Yes == MessageBox.Show(message, caption, MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button2);
#else
            var window = view as IWin32Window;
            return DialogResult.Yes == MessageBox.Show(window, message, caption, MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button2);
#endif
        }

        public static bool AskYesNo(this IView view, string message, String caption, MessageBoxIcon icon, bool defaultNo)
        {
#if NetCF
            return DialogResult.Yes == MessageBox.Show(message,
                caption,
                MessageBoxButtons.YesNo,
                icon,
                (defaultNo) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1);
#else

            var window = view as IWin32Window;
            return DialogResult.Yes == MessageBox.Show(window,
                message,
                caption,
                MessageBoxButtons.YesNo,
                icon,
                (defaultNo) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1);
#endif
        }
    }
}