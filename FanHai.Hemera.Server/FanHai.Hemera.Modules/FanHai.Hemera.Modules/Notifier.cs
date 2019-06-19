using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils;
using System.Data;


namespace FanHai.Hemera.Modules
{
    /// <summary>
    /// 表示远程调用的通知器对象类。
    /// </summary>
    public class Notifier : MarshalByRefObject, INotifier
    {
        /// <summary>
        /// 定义广播事件。
        /// </summary>
        public event NotifyEventHandler Notify;
        /// <summary>
        /// 将消息广播出去。
        /// </summary>
        /// <param name="info">广播的消息信息</param>
        public void BroadCast(string info)
        {
          
            Console.WriteLine("Sending Notify...");
            if (Notify != null)
            {
                foreach (NotifyEventHandler doEvent in Notify.GetInvocationList())
                {
                    try
                    {
                        doEvent(info);
                        Console.WriteLine("Notify Sent succeeded");
                    }
                    catch
                    {
                        Console.WriteLine("One of client untouchable, disconnect it.");
                        Notify -= doEvent;
                    }
                }
            }
        }
        /// <summary>
        /// 初始化远程对象的生命期为永久有效。
        /// </summary>
        /// <returns>生存期服务对象。</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
