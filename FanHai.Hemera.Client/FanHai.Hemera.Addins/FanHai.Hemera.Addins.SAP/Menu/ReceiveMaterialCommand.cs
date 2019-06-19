using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.SAP
{
    public class ReceiveMaterialCommand : AbstractMenuCommand
    {

        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is ReceiveMaterialContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            WorkbenchSingleton.Workbench.ShowView(new ReceiveMaterialContent());
        }
    }
}
