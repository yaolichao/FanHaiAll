//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 张振东               2012-05-11            新建，用于手工输入来料数据。
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 手工输入来料数据的菜单命令。
    /// </summary>
    public class MaterialRequisitionCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is ManualReceiveMaterialViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new MaterialRequisitionViewContent());
        }
    }
}
