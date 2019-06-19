//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  冯旭
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭               2012-02-16             新建 
// =================================================================================

using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui.OptionPanels;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 材料耗用菜单类
    /// </summary>
    class UseMaterialCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("材料耗用"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            UseMaterialViewContent view = new UseMaterialViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
     
        }
    }
}
