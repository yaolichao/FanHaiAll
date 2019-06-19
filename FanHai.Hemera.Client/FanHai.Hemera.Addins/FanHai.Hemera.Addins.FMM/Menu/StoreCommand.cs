
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    ///  表示线上仓管理的菜单命令类。通过该类显示线上仓管理的窗口界面。
    /// </summary>
    public class StoreCommand:AbstractMenuCommand
    {
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewConten in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewConten.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreManagement.Name}"))
                {
                    viewConten.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            StoreViewContent storeViewContent = new StoreViewContent();
            WorkbenchSingleton.Workbench.ShowView(storeViewContent);
        }
    }
}
