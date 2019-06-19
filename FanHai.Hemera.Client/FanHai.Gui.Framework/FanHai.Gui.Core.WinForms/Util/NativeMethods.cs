using System;
using System.Runtime.InteropServices;

namespace FanHai.Gui.Core.WinForms
{
    static class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr handle);
    }
}
