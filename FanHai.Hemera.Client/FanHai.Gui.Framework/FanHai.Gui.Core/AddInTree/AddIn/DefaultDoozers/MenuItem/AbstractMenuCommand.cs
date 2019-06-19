using System;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 菜单命令抽象类，继承自<see cref="AbstractCommand"/>并实现<see cref="IMenuCommand"/>接口。用于显示指定的视图界面。
    /// </summary>
	public abstract class AbstractMenuCommand : AbstractCommand, IMenuCommand
	{
		bool isEnabled = true;

        /// <summary>
        /// 是否启用菜单，默认启用。
        /// </summary>
		public virtual bool IsEnabled {
			get {
				return isEnabled;
			}
			set {
				isEnabled = value;
			}
		}
	}
}
