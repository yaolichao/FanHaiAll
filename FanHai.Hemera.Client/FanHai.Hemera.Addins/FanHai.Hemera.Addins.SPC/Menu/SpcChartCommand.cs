/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.SPC
{
    public class SpcChartCommand:AbstractMenuCommand
    {
        public override void Run()
        {
            //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //{
            //    if (viewContent is SpcChartViewContent)
            //    {
            //        viewContent.WorkbenchWindow.SelectWindow();

            //        return;
            //    }
            //}

            WorkbenchSingleton.Workbench.ShowView(new SpcChartViewContent());
        }
    }
}
