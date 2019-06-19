using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.BasicData;

namespace FanHai.Hemera.Addins.BasicData
{
    public class SupplierCommand : AbstractMenuCommand
    {
        // <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                SupplierViewContent openView = viewContent as SupplierViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            SupplierViewContent view = new SupplierViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
