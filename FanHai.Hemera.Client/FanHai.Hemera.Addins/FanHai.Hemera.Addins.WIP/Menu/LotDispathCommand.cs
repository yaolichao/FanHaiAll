using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示工作站作业(简单)的菜单命令类。通过该类显示工作站作业(简单)的窗口界面。
    /// </summary>
    class LotDispathCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开工作站作业(简单)视图，则选中该视图显示，返回以结束该方法的运行。
                LotDispathViewContent openView = viewContent as LotDispathViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            LotDispathViewContent view = new LotDispathViewContent(null);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    
}
