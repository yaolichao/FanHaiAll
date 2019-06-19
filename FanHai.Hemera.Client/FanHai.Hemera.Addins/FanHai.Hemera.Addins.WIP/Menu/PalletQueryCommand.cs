using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 托盘信息查询菜单。
    /// </summary>
    public class PalletQueryCommand : AbstractMenuCommand
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
                PalletQueryViewContent openView = viewContent as PalletQueryViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            PalletQueryViewContent view = new PalletQueryViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
