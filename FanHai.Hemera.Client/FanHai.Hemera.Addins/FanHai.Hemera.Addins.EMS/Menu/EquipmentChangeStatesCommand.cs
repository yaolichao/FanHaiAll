using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class EquipmentChangeStatesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                EquipmentChangeStatesViewContent openView = viewContent as EquipmentChangeStatesViewContent;

                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            EquipmentChangeStatesViewContent view = new EquipmentChangeStatesViewContent();

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
