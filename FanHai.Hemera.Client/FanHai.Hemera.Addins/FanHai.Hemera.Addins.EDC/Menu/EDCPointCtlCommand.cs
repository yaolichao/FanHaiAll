//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释 
// =================================================================================
#region using
using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
#endregion 

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 表示抽检点设置的菜单命令类。通过该类显示抽检点设置的窗口界面。
    /// </summary>
    public class EDCPointCtlCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开抽检点设置视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent.TitleName == "抽检点设置")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            EDCPointCtlViewContent view = new EDCPointCtlViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
