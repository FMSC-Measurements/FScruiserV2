using System.Runtime.InteropServices;

namespace FSCruiser.WinForms
{
    public static class Win32
    {
        public static int MB_ICONEXCLAMATION = 0x00000030;// in C:\Program Files\Windows Mobile 6 SDK\PocketPC\Include\Armv4i\winuser.h
        public static int MB_ICONHAND = 0x00000010;
        public static int MB_ICONQUESTION = 0x00000020;
        public static int MB_ICONASTERISK = 0x00000040;

        [DllImport("CoreDll.dll")]
        public static extern void MessageBeep(int code);
    }
}