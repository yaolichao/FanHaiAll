using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class ByProductPartCtrlCommand : AbstractMenuCommand
    {
        // <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                ByProductPartCtrlContent openView = viewContent as ByProductPartCtrlContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            ByProductPartCtrlContent view = new ByProductPartCtrlContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
