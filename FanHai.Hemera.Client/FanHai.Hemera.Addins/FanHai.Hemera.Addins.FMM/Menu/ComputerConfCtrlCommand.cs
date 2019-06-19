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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.Gui.OptionPanels;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 表示计算机配置的菜单命令类。通过该类显示计算机配置的窗口界面。
    /// </summary>
    public class ComputerConfCtrlCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                ComputerViewContext openView = viewContent as ComputerViewContext;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            ComputerViewContext view = new ComputerViewContext(new ComputerEntity());
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
