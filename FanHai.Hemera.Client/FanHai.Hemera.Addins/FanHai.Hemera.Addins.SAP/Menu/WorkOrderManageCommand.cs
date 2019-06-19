using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Commands;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 表示工单管理（创建，修改，删除）的菜单命令类。
    /// </summary>
    public class WorkOrderManageCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                WorkOrderManageViewContent openView = viewContent as WorkOrderManageViewContent;
                if (openView != null)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            WorkOrderManageViewContent view = new WorkOrderManageViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
