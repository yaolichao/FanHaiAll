using System;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 菜单命令接口
    /// </summary>
    public interface IMenuCommand : ICommand
    {
        /// <summary>
        /// 是否启用菜单
        /// </summary>
        bool IsEnabled
        {
            get;
            set;
        }
    }
}
