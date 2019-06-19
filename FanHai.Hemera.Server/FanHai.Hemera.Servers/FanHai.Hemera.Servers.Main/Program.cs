/*
* 包含应用程序入口点类，启动远程服务器。
*/
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels ;
using System.Runtime.Remoting.Channels.Tcp ; 
using System.Configuration;
using System.Collections.Specialized ; 
using FanHai.Hemera.Modules;
using FanHai.Hemera.Modules.Common;
using System;

namespace FanHai.Hemera.Servers.Main
{
    /// <summary>
    /// 启动 Hemera 服务器平台应用程序类。
    /// </summary>
    class Program
    {
        /// <summary>
        /// Hemera 服务器平台应用程序入口点。
        /// </summary>
        /// <param name="args">应用程序参数</param>
        static void Main(string[] args)
        {
            //启动Remoting服务
            RemotingServer.Start();
            Console.WriteLine("Press Enter to Exit.");
            Console.ReadLine();
        }
       
    }
}
