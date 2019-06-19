
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 表示资源管理的菜单命令类。通过该类显示资源管理的窗口界面。
    /// </summary>
    public class ResourceManageCommand : AbstractMenuCommand
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
                if (viewConten.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceViewContent.TitleName}"))
                {
                    viewConten.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            ResourceViewContent resourceViewContent = new ResourceViewContent();
            WorkbenchSingleton.Workbench.ShowView(resourceViewContent);
        }
    }
}
