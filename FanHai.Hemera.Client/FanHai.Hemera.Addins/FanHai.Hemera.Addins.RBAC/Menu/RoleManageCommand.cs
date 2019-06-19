//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-30            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 表示角色管理的菜单命令类。通过该类显示角色管理的窗口界面。
    /// </summary>
    public class RoleManageCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewConten in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开该视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewConten.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageViewContent.TitleName}"))
                {
                    viewConten.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            RoleManageViewContent roleViewContent = new RoleManageViewContent();
            WorkbenchSingleton.Workbench.ShowView(roleViewContent);
        }
    }
}
