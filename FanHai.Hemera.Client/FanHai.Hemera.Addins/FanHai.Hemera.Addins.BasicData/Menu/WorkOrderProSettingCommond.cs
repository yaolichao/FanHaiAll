using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Commands;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class WorkOrderProSettingCommond : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is DecayCoeffiViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            WorkbenchSingleton.Workbench.ShowView(new WorkOrderProSettingViewContent());
        }
      
    }
}
