using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace FanHai.Gui.Framework
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }
    /// <summary>
    /// Contains P/Invoke methods for functions in the Windows API.
    /// </summary>
    static class NativeMethods
    {
        static readonly IntPtr FALSE = new IntPtr(0);
        static readonly IntPtr TRUE = new IntPtr(1);

        public const int WM_SETREDRAW = 0x00B;
        public const int WM_USER = 0x400;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 获取上一次输入的时间。
        /// </summary>
        /// <param name="plii"></param>
        /// <returns>true：获取成功。false：获取失败。</returns>
        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref　LASTINPUTINFO plii);
        /// <summary>
        /// 设置占用常驻物理内存的大小，把多余部分交换/释放出去。
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll",EntryPoint="SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process,int minSize,int maxSize);
        /// <summary>
        /// 获取上一次操作后的闲置时间。
        /// </summary>
        /// <returns>闲置时间的毫秒数。</returns>
        public static long GetIdleTick()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref　vLastInputInfo)) return 0;
            return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }

        public static void SetWindowRedraw(IntPtr hWnd, bool allowRedraw)
        {
            SendMessage(hWnd, WM_SETREDRAW, allowRedraw ? TRUE : FALSE, IntPtr.Zero);
        }

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern short GetKeyState(int vKey);

        public static bool IsKeyPressed(Keys key)
        {
            return GetKeyState((int)key) < 0;
        }

        #region SHFileOperation
        enum FO_FUNC : uint
        {
            FO_MOVE = 0x0001,
            FO_COPY = 0x0002,
            FO_DELETE = 0x0003,
            FO_RENAME = 0x0004,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FO_FUNC wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public FILEOP_FLAGS fFlags;
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;
        }

        [Flags]
        private enum FILEOP_FLAGS : ushort
        {
            None = 0,
            FOF_MULTIDESTFILES = 0x0001,
            FOF_CONFIRMMOUSE = 0x0002,
            FOF_SILENT = 0x0004,  // don't create progress/report
            FOF_RENAMEONCOLLISION = 0x0008,
            FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
            FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
            // Must be freed using SHFreeNameMappings
            FOF_ALLOWUNDO = 0x0040,
            FOF_FILESONLY = 0x0080,  // on *.*, do only files
            FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
            FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
            FOF_NOERRORUI = 0x0400,  // don't put up error UI
            FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
            FOF_NORECURSION = 0x1000,  // don't recurse into directories.
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected elements.
            FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
            FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

        public static void DeleteToRecycleBin(string fileName)
        {
            if (!File.Exists(fileName) && !Directory.Exists(fileName))
                throw new FileNotFoundException("File not found.", fileName);
            SHFILEOPSTRUCT info = new SHFILEOPSTRUCT();
            info.hwnd = Gui.WorkbenchSingleton.MainForm.Handle;
            info.wFunc = FO_FUNC.FO_DELETE;
            info.fFlags = FILEOP_FLAGS.FOF_ALLOWUNDO | FILEOP_FLAGS.FOF_NOCONFIRMATION;
            info.lpszProgressTitle = "Delete " + Path.GetFileName(fileName);
            info.pFrom = fileName + "\0"; // pFrom is double-null-terminated
            int result = SHFileOperation(ref info);
            if (result != 0)
                throw new IOException("Could not delete file " + fileName + ". Error " + result);
        }
        #endregion

        [DllImport("kernel32.dll")]
        static extern int GetOEMCP();

        public static Encoding OemEncoding
        {
            get
            {
                try
                {
                    return Encoding.GetEncoding(GetOEMCP());
                }
                catch (ArgumentException)
                {
                    return Encoding.Default;
                }
                catch (NotSupportedException)
                {
                    return Encoding.Default;
                }
            }
        }
    }
}
