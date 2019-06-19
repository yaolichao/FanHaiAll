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
    public class ExchangeEquipmentCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            // Switch to previously opened view.
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                ExchangeEquipmentContent openView = viewContent as ExchangeEquipmentContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }

            // Create new view.
            ExchangeEquipmentContent view = new ExchangeEquipmentContent();
            WorkbenchSingleton.Workbench.ShowView(view);

        }
    }
}
