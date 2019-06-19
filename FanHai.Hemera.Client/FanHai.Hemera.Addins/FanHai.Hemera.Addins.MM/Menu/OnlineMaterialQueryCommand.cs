//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  乔永明
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 乔永明               2012-02-16             新建 
// =================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.MM
{
    class OnlineMaterialQueryCommand : AbstractMenuCommand
    {
        public override void Run() //抽象类需要重写
        {
            // Switch to previously opened view.
            //已经打开就直接显示  add by qym
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("在线库存查询"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            //没有打开就新建 add by qym
            OnlineMaterialQueryViewContent view = new OnlineMaterialQueryViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
