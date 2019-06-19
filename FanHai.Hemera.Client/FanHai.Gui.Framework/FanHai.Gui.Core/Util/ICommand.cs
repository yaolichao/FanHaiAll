using System;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 基本的命令执行接口，提供简单的命令执行方法<see cref="Run"/>。
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 返回当前命令对象。
        /// </summary>
        object Owner
        {
            get;
            set;
        }
        /// <summary>
        /// 执行命令。
        /// </summary>
        void Run();
        /// <summary>
        /// 属性改变事件，当属性发生改变时被调用。
        /// </summary>
        event EventHandler OwnerChanged;
    }
}
