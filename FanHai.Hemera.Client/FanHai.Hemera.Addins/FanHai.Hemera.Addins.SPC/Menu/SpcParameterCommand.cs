/*
<FileInfo>
  <Author>Rayna Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;

namespace SolarViewer.Hemera.Addins.SPC
{
    public class SpcParameterCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is SpcParameterViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }

            WorkbenchSingleton.Workbench.ShowView(new SpcParameterViewContent());
        }
    }
}
