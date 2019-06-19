using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Commands;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class BasicSettingsCommond:AbstractMenuCommand
    {
        //public override void Run()
        //{
        //    PadDescriptor padDescriptor = new PadDescriptor(typeof(BasicDataTreePad), "", "");
        //    WorkbenchSingleton.Workbench.ShowPad(padDescriptor);
        //}

        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is BasicDataSettingViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new BasicDataSettingViewContent());
        }

    }
}
