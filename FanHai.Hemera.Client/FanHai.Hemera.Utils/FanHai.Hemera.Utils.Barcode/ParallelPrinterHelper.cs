// ----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
// ----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter Zhang          2012-03-23            新建 
// =================================================================================
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;


namespace FanHai.Hemera.Utils.Helper
{
    /// <summary>
    /// 将待打印数据发送到并口打印机的帮助类。
    /// </summary>
    public sealed class ParallelPrinterHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }

        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName,
                                             uint dwDesiredAccess,
                                             int dwShareMode,
                                             int lpSecurityAttributes,
                                             int dwCreationDisposition,
                                             int dwFlagsAndAttributes,
                                             int hTemplateFile);

        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile,
                                             byte[] lpBuffer,
                                             int nNumberOfBytesToWriter,
                                             out int lpNumberOfBytesWriten,
                                             out OVERLAPPED lpOverLapped);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);

        private static int iHandle;
        /// <summary>
        /// Open LPT port
        /// </summary>
        /// <returns>True or false</returns>
        /// <summary>
        /// Open LPT port
        /// </summary>
        /// <param name="printerPort">Printer port</param>
        /// <returns>Boolean value indication open port succeeded or failed</returns>
        private static bool LPTOpen(string printerPort)
        {
            iHandle = CreateFile(printerPort, 0x40000000, 0, 0, 3, 0, 0);
            if (iHandle != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Close LPT port
        /// </summary>
        /// <returns>True or false</returns>
        private static bool LPTClose()
        {
            return CloseHandle(iHandle);
        }

        /// <summary>
        /// 将字符串发送给指定的打印机进行打印。
        /// </summary>
        /// <param name="printerPort">打印机端口。</param>
        /// <param name="content">打印内容字符串。</param>
        /// <returns>是否打印成功，true：成功。false：失败。</returns>
        public static bool SendStringToPrinter(string printerPort, string content)
        {
            bool bSuccess = false;
            try
            {
                if (!LPTOpen(printerPort)) { return false; }
                if (iHandle != 1)
                {
                    int i;
                    OVERLAPPED x;
                    byte[] mybyte = Encoding.Default.GetBytes(content);
                    if (!WriteFile(iHandle, mybyte, mybyte.Length, out i, out x))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LPTClose();
            }
            return bSuccess;
        }
    }
}
