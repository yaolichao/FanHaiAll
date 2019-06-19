using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace QCCheck
{
    ///   <summary>   
    ///   POSPrinter的摘要说明。  
    ///   此类处理网络打印,使用了IP端口.  
    ///   </summary>   
    public class NetPOSPrinter
    {
        string ipPort = string.Empty;
        Socket soc;

        public NetPOSPrinter()
        {
        }

        public NetPOSPrinter(string IpPort)
        {
            this.ipPort = IpPort;//打印机端口   
        }

        /// <summary>
        /// 打开打印机端口
        /// </summary>
        /// <returns></returns>
        public bool IPPortOpen()
        {
            try
            {
                soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load("Config.xml");
                ipPort = xmldoc.SelectSingleNode("//UI/IPADRESS").InnerText.Trim();
                //建立连接  
                IPAddress ipa = IPAddress.Parse(ipPort);
                IPEndPoint ipe = new IPEndPoint(ipa, 9100);
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
                        if (count > 5 && soc.Connected == false)
                        {
                            throw ex;
                        }
                    }
                } while (soc.Connected == false);

                if (soc.Connected == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印输出的内容
        /// </summary>
        /// <param name="lst">组件序列号的集合</param>
        public void PrintLine(IList<string> lst)
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\" + "ZebraCode.txt";
                string zebraCodeTemplate = string.Empty;

                using (StreamReader myReader = new StreamReader(filePath))
                {
                    zebraCodeTemplate = myReader.ReadToEnd();
                    myReader.Close();
                }

                foreach (string sn in lst)
                {
                    byte[] b = System.Text.Encoding.Default.GetBytes(string.Format(zebraCodeTemplate, sn.Substring(0, 4), sn.Substring(4, 10), sn));
                    soc.Send(b);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 关闭打印机对应的端口
        /// </summary>
        /// <returns>true：关闭，false：未关闭</returns>
        public bool IPPortClose()
        {
            try
            {
                if (soc.Connected == true)
                {
                    soc.Close();
                }

                if (soc.Connected == false)
                {
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
}