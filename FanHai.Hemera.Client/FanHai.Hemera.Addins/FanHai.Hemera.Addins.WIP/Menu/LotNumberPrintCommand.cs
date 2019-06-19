using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次号打印的命令类。
    /// </summary>
    public class LotNumberPrintCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                LotNumberPrintViewContent openView = viewContent as LotNumberPrintViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            LotNumberPrintViewContent view = new LotNumberPrintViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
}
