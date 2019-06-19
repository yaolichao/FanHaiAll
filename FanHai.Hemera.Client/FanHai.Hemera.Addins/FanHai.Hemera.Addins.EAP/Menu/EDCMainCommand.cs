// ----------------------------------------------------------------------------------
// Copyright (c) FanHai
// ----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-02-22            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 用于调用设备数据采集的菜单命令类，通过该类显示设备数据采集的界面。
    /// </summary>
    public class EDCMainCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 运行菜单命令
        /// </summary>
        public override void Run()
        {
            //遍历工作台已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果有数据采集视图，则选中该视图显示，返回以结束该方法的运行。
                EDCMainViewContent openView = viewContent as EDCMainViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }             
            }
            //创建新的视图对象，并显示该视图界面。
            EDCMainViewContent view = new EDCMainViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
