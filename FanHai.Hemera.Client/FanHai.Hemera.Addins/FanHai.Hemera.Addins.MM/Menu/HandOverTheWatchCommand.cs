//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  冯旭
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭                2012-02-16             新建 
// 庞超                2012-03-26             查看修改
// =================================================================================

using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 交接班记录菜单类
    /// </summary>
    class HandOverTheWatchCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("交接班记录"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            HandOverTheWatchViewContent view = new HandOverTheWatchViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
