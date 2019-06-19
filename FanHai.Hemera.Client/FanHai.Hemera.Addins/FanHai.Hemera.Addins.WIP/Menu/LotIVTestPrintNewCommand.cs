using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Addins.WIP
{
    public class LotIVTestPrintNewCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            // Switch to previously opened view.
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent is LotIVTestPrintViewNewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotIVTestPrintViewNewContent view = new LotIVTestPrintViewNewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
