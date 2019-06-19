using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// 表示设备管理的菜单命令类。通过该类显示代码管理的窗口界面。
    /// </summary>
    public class EquipmentsCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                EquipmentsViewContent openView = viewContent as EquipmentsViewContent;

                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();

                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。 
            EquipmentsViewContent view = new EquipmentsViewContent();

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
