// ----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
// ----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter Zhang          2012-03-23            新建 
// =================================================================================
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Utils.Helper
{
    /// <summary>
    /// 将待打印数据发送到网络打印机的帮助类。
    /// </summary>
    public sealed class NetWorkPrinterHelper
    {
        /// <summary>
        /// 将字符串发送给指定的打印机进行打印。
        /// </summary>
        /// <param name="printerPort">打印机IP地址。</param>
        /// <param name="printerPort">打印机端口。</param>
        /// <param name="content">打印内容字符串。</param>
        /// <returns>是否打印成功，true：成功。false：失败。</returns>
        public static bool SendStringToPrinter(string printerIP, string printerPort, string content)
        {
            using (Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    IPAddress ipa = IPAddress.Parse(printerIP);
                    IPEndPoint ipe = new IPEndPoint(ipa, int.Parse(printerPort));
                    int count = 0;
                    do
                    {
                        try
                        {
                            soc.Connect(ipe);
                        }
                        catch (Exception ex)
                        {
                            count++;
                            if (count > 2 && soc.Connected == false)
                            {
                                throw ex;
                            }
                        }
                    } while (soc.Connected == false);

                    if (soc.Connected == true)
                    {
                        byte[] b = System.Text.Encoding.Default.GetBytes(content);
                        soc.Send(b);

                        soc.Close();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        //public static bool SendStringToPrinter( string printerIP, string printerPort, string content)
        //{
        //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    socket.Connect(printerIP, int.Parse(printerPort));
        //    int i=socket.Send((Encoding.Default.GetBytes(content)));
        //    socket.Disconnect(true);
        //    return i>0;
        //}


    }
}
