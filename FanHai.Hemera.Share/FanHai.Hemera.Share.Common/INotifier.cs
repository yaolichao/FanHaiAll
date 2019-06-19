using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Common
{
    //Define delegate
    /// <summary>
    /// 表示通知器广播消息的事件方法。
    /// </summary>
    /// <param name="info">通知内容信息</param>
    public delegate void NotifyEventHandler(string info);
    /// <summary>
    /// 表示用于进行广播消息的通知器接口。
    /// </summary>
    public interface INotifier
    {
        //Define broadcast event
        /// <summary>
        /// 定义广播事件。
        /// </summary>
        event NotifyEventHandler Notify;

        /// <summary>
        /// 广播消息的方法。
        /// </summary>
        /// <param name="info">广播的消息信息</param>
        void BroadCast(string info);
    }
}
