//----------------------------------------------------------------------------------
// Copyright (c) SolarViewer
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 处理远程调用的服务器类。
//----------------------------------------------------------------------------------
#region using
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Hemera.Share.Common;
using SolarViewer.Hemera.Modules;
using SolarViewer.Hemera.Modules.Common;
using System.Runtime.Remoting;
using SolarViewer.Hemera.Share.Interface;
using System.Runtime.Remoting.Channels;
#endregion


namespace SolarViewer.Hemera.Servers.Main
{
    /// <summary>
    /// 远程调用处理的服务类
    /// </summary>
    public class RemotingServer
    {
        //Define static Notifier object
        public static Notifier n = null;
        public static ServerObjFactory f = null;
        /// <summary>
        /// 启动远程调用服务监听器
        /// </summary>
        public static void Start()
        {
            if (RegisterChannel())
            {
                RegisterRemoteObject();
                Run();
            }
        }
        /// <summary>
        /// 注册远程调用通道。
        /// </summary>
        /// <returns>bool</returns>
        public static bool RegisterChannel()
        {
            try
            {
                RemotingHelper.EventModeServerRegister();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Register remoting server channel exception : " + ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// 注册远程对象——客户端可调用的对象
        /// </summary>
        /// <returns>bool</returns>
        public static bool RegisterRemoteObject()
        {
            try
            {
                if (RemotingConfiguration.CustomErrorsMode != CustomErrorsModes.Off)
                {
                    RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                }
                //RemotingHelper.RegisterRemoteObjectViaXmlConfig();
                f = new ServerObjFactory();
                //序列化f对象
                RemotingServices.Marshal(f, typeof(IServerObjFactory).FullName);
                n = new Notifier();
                //序列号n对象
                RemotingServices.Marshal(n, typeof(INotifier).FullName);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Register remoting server object exception : " + ex.ToString());
            }

            return false;
        }
        /// <summary>
        /// 通过远程对象唯一标识符（URL）取消注册的远程对象
        /// </summary>
        /// <param name="url">远程对象唯一标识符（URL）</param>
        /// <returns>bool</returns>
        public bool UnRegisterRemoteObject(string url)
        {
            Type type = RemotingServices.GetServerTypeForUri(url);
            if (type != null)
            {
                MarshalByRefObject obj = (MarshalByRefObject)Activator.CreateInstance(type);
                RemotingServices.Disconnect(obj);
            }
            return true;
        }
        /// <summary>
        /// 通过远程对象类型取消注册远程对象
        /// </summary>
        /// <param name="type">远程对象类型</param>
        /// <returns>bool</returns>
        public bool UnRegisterRemoteObject(Type type)
        {
            if (type != null)
            {
                MarshalByRefObject obj = (MarshalByRefObject)Activator.CreateInstance(type);
                RemotingServices.Disconnect(obj);
            }
            return true;
        }
        /// <summary>
        /// 通过远程信道名称关闭信道
        /// </summary>
        /// <param name="channelName">远程信道名称</param>
        /// <returns>bool</returns>
        public bool UnregisterChannelByName(string channelName)
        {
            IChannel[] channels = ChannelServices.RegisteredChannels;

            foreach (IChannel eachChannel in channels)
            {
                if (eachChannel.ChannelName == channelName)
                {
                    ChannelServices.UnregisterChannel(eachChannel);
                }
            }

            return true;

        }
        /// <summary>
        /// 运行远程调用服务器
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Remoting Server is Ready ...");
            string s;
            //如果输入的值不是空白
            while ((s = Console.ReadLine()) != "")
            {
                if (s == "kill")
                {
                    PrintPool.PrinterPoolClear();
                    Run();
                }
                n.BroadCast(s);
            }
            Console.WriteLine("Server shutdown ...");
        }
    }
}
