﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 
    /// </summary>
    public class GetWorkOrderCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开工单获取视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent.TitleName == "SAP获取工单")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            GetWorkOrderViewContent view = new GetWorkOrderViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
