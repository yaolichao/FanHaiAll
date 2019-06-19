using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;

namespace SolarViewer.Hemera.Addins.WARK
{
    public class HaveArkedInfQueryCtrlCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent is HaveArkedInfQueryCtrlContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            HaveArkedInfQueryCtrlContent view = new HaveArkedInfQueryCtrlContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
