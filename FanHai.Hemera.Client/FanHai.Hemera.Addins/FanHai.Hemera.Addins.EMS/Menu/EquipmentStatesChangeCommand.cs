using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentStatesChangeCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                EquipmentStatesViewContent openView = viewContent as EquipmentStatesViewContent;

                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            EquipmentStatesChangeViewContent view = new EquipmentStatesChangeViewContent();

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
