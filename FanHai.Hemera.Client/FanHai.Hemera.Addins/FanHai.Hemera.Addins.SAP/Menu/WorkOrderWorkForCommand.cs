//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  庞超
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 庞超               2012-04-11              新建 
// =================================================================================

using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui.OptionPanels;
namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 工单报工菜单项
    /// </summary>
    class WorkOrderWorkForCommand:AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("工单报工"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            WorkOrderWorkForContent view = new WorkOrderWorkForContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
