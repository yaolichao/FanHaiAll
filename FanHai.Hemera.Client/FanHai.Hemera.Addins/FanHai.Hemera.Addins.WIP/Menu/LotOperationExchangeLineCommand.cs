using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    public class LotOperationExchangeLineCommand : AbstractMenuCommand
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
                //如果已打开批次创建的视图，则选中该视图显示，返回以结束该方法的运行。
                LotOperationExchangeLineViewContent openView = viewContent as LotOperationExchangeLineViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotOperationExchangeLineViewContent view = new LotOperationExchangeLineViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
