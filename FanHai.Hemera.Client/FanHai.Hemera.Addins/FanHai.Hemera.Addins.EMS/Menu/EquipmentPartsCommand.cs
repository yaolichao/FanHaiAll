using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentPartsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is EquipmentPartsViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            WorkbenchSingleton.Workbench.ShowView(new EquipmentPartsViewContent());
        }
    }
}
