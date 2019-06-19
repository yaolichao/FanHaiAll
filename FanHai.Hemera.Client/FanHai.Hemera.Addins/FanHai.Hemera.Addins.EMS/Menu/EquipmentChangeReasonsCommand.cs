using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentChangeReasonsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                EquipmentChangeReasonsViewContent openView = viewContent as EquipmentChangeReasonsViewContent;

                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            EquipmentChangeReasonsViewContent view = new EquipmentChangeReasonsViewContent();

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
