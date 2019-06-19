/*
* 处理远程调用的服务器类。
*/
#region using
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using FanHai.Hemera.Utils;
using System.Timers;
using System.Configuration;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;
#endregion


namespace FanHai.Hemera.Modules
{
    /// <summary>
    /// 远程调用处理的服务类
    /// </summary>
    public class RemotingServer
    {
        //Define static Notifier object
        private static Notifier n = null;
        private static ServerObjFactory f = null;
        /// <summary>
        /// 获取消息通知类。
        /// </summary>
        public static Notifier Notifier
        {
            get
            {
                if (n == null)
                {
                    Start();
                }
                return n;
            }
        }
        /// <summary>
        /// 获取服务器对象工厂类。
        /// </summary>
        public static ServerObjFactory ServerObjFactory
        {
            get
            {
                if (f == null)
                {
                    Start();
                }
                return f;
            }
        }
        /// <summary>
        /// 启动远程调用服务监听器
        /// </summary>
        public static void Start()
        {
            try
            {
                if (RegisterChannel())
                {
                    RegisterRemoteObject();
                    SetTimer();
                    Run();
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
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
                LogService.LogInfo("Register remoting server channel exception : " + ex.ToString());
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
                LogService.LogError("Register remoting server object exception : " + ex.ToString());
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
            LogService.LogInfo("Remoting Server is Ready ...");
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
            LogService.LogInfo("Server shutdown ...");
        }


        public static void SetTimer()
        {
            string loadSapDataRunTime = ConfigurationManager.AppSettings["LoadSapDataRunTime"];
            int sapRunTime = 10;
            try
            {
                if (!string.IsNullOrWhiteSpace(loadSapDataRunTime)) sapRunTime = Convert.ToInt32(loadSapDataRunTime);
            }
            catch (Exception)
            {
            }


            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(GetSapInfo);
            // 设置引发时间的时间间隔 此处设置为10分钟检测一次
            aTimer.Interval = 1000 * 60 * sapRunTime;
            aTimer.Enabled = true;
        }
        public static void GetSapInfo(object source, ElapsedEventArgs e)
        {
            LogService.LogInfo("开始获取SAP信息 ...");
            ServerObjFactory serverFactory = new ServerObjFactory();
            ISAPEngine engine = serverFactory.CreateISAPEngine();
            engine.RefreshSAPWorkOrderInfo();
            LogService.LogInfo("结束SAP信息的操作 ...");
        }
    }
}
