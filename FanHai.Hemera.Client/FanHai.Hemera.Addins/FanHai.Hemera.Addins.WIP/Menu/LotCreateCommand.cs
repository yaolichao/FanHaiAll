using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次批量创建的菜单命令类。通过该类显示批次批量创建的窗口界面。
    /// </summary>
    public class LotBatchCreateCommand : AbstractMenuCommand
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
                LotCreateViewContent openView = viewContent as LotCreateViewContent;
                if (openView != null && openView.IsBatch == true)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotCreateViewContent view = new LotCreateViewContent(null,true);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
    /// <summary>
    /// 表示批次创建的菜单命令类。通过该类显示批次创建的窗口界面。
    /// </summary>
    public class LotCreateCommand : AbstractMenuCommand
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
                LotCreateViewContent openView = viewContent as LotCreateViewContent;
                if (openView != null && openView.IsBatch==false)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            LotCreateViewContent view = new LotCreateViewContent(null,false);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
