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
using System.IO.Ports;

namespace FanHai.Hemera.Utils.Helper
{
    /// <summary>
    /// 将待打印数据发送到串口打印机的帮助类。
    /// </summary>
    public sealed class SerialPrinterHelper
    {
        /// <summary>
        /// 将字符串发送给指定的打印机进行打印。
        /// </summary>
        /// <param name="printerPort">打印机端口。</param>
        /// <param name="content">打印内容字符串。</param>
        /// <returns>是否打印成功，true：成功。false：失败。</returns>
        public static bool SendStringToPrinter(string printerPort, string content)
        {
            bool bSuccess = true;
            SerialPort comPort = new SerialPort(printerPort);
            if (!comPort.IsOpen)
            {
                comPort.Open();
            }
            try
            {
                comPort.WriteLine(content);
            }
            finally
            {
                comPort.Close();
            }
            return bSuccess;
        }
    }
}
