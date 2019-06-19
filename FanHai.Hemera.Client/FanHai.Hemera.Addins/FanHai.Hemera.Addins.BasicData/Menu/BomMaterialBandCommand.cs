using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.BasicData;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BomMaterialBandCommand : AbstractMenuCommand
    {
        // <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                BomMaterialBandViewContent openView = viewContent as BomMaterialBandViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。  
            BomMaterialBandViewContent view = new BomMaterialBandViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
