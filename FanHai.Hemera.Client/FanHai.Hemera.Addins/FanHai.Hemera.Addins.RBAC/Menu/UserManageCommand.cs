using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 表示用户管理的菜单命令类。通过该类显示用户管理的窗口界面。
    /// </summary>
    public class UserManageCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewConten in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开该视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewConten.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}"))
                {
                    viewConten.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            UserManageViewContent userViewConten = new UserManageViewContent("",new User());
            WorkbenchSingleton.Workbench.ShowView(userViewConten);
        }
    }
}
