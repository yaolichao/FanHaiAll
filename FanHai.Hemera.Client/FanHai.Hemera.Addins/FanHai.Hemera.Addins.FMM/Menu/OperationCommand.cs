//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    ///  表示工序管理的菜单命令类。通过该类显示工序管理的窗口界面。
    /// </summary>
    public class OperationCommand:AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开该视图，则选中该视图显示，返回以结束该方法的运行。
                OperationViewContent openView = viewContent as OperationViewContent;
                if (openView != null && 
                    StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}") == openView.TitleName)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。 
            OperationViewContent view = new OperationViewContent(new OperationEntity());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
