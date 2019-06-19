using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Addins.EMS;

namespace FanHai.Hemera.Addins.EMS
{
    public class OperationEquipmentsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                OperationEquipmentsViewContent openView = viewContent as OperationEquipmentsViewContent;

                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            OperationEquipmentsViewContent view = new OperationEquipmentsViewContent();

            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
