#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
#endregion

namespace FanHai.Hemera.Share.Common
{
    /// <summary>
    /// 客户端订阅服务器端事件的代理类。
    /// </summary>
    public class NotifierProxy : MarshalByRefObject, INotifier
    {
        //Define INotifier object
        private INotifier n = null;
        //Define event
        public event Common.NotifyEventHandler Notify;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public NotifierProxy()
        {
            n = (INotifier)RemotingHelper.GetObject(typeof(INotifier));
            n.Notify += new NotifyEventHandler(MiddleBroadCast);
        }
        /// <summary>
        /// 广播消息时触发。
        /// </summary>
        /// <param name="info">消息字符串。</param>
        public void MiddleBroadCast(string info)
        {
            if (Notify != null)
            {
                Notify(info);
            }
        }
        /// <summary>
        /// 广播消息。
        /// </summary>
        /// <param name="info">信息字符串。</param>
        public void BroadCast(string info)
        {
            n.BroadCast(info);
        }
        /// <summary>
        /// 获取控制此实例生存期策略的生存期服务对象。
        /// </summary>
        /// <returns>控制此实例生存期策略的生存期服务对象。</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
