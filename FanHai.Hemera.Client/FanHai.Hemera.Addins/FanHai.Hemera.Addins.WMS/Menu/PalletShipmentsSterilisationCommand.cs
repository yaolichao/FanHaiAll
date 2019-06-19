//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 创建人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang           2014-12-15            新增
// =================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 冲销作业。
    /// </summary>
    public class PalletShipmentsSterilisationCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent is PalletShipmentsSterilisationContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            PalletShipmentsSterilisationContent view = new PalletShipmentsSterilisationContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
