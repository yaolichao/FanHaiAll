using System;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 抽象命令类，实现<see cref="ICommand"/> 接口。
    /// </summary>
    public abstract class AbstractCommand : ICommand
    {
        object owner = null;
        /// <summary>
        /// 返回当前命令对象的所有者。
        /// </summary>
        public virtual object Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
                OnOwnerChanged(EventArgs.Empty);
            }
        }
        /// <summary>
        /// 执行命令。
        /// </summary>
        public abstract void Run();
        /// <summary>
        /// 属性改变事件，当属性发生改变时被调用。
        /// </summary>
        public event EventHandler OwnerChanged;
        /// <summary>
        /// 属性发生改变时调用的方法。
        /// </summary>
        /// <param name="e">包含事件数据的对象</param>
        protected virtual void OnOwnerChanged(EventArgs e)
        {
            if (OwnerChanged != null)
            {
                OwnerChanged(this, e);
            }
        }
    }
}
