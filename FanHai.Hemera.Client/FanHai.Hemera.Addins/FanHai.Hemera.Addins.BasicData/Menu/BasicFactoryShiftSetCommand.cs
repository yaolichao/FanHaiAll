using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;
using SolarViewer.Hemera.Addins.BasicData;

namespace SolarViewer.Hemera.Addins.BasicData
{
    public class BasicFactoryShiftSetCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is BasicFactoryShiftSetViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            WorkbenchSingleton.Workbench.ShowView(new BasicFactoryShiftSetViewContent());
        }
    }
}
