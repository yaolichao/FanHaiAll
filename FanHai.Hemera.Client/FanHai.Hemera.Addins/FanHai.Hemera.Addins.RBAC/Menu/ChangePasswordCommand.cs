using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 表示修改密码的菜单命令类。通过该类显示修改密码的对话框。
    /// </summary>
    public class ChangePasswordCommand:AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            ChangePasswordDialog passwordDialog = new ChangePasswordDialog();
            passwordDialog.ShowDialog();
        }
    }
}
