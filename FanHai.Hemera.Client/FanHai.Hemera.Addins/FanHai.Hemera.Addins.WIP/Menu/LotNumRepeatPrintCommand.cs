using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次号补打的命令类。
    /// </summary>
    public class LotNumRepeatPrintCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                LotNumRepeatPrintContent openView = viewContent as LotNumRepeatPrintContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            LotNumRepeatPrintContent view = new LotNumRepeatPrintContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
}
