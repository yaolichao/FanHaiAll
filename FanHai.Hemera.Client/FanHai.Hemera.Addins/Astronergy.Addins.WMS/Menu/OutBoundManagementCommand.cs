using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;
using Astronergy.Addins.WMS.Src;

namespace Astronergy.Addins.WMS
{
    class OutBoundManagementCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            //遍历工作台中已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果已打开视图，则选中该视图显示，返回以结束该方法的运行。
                if (viewContent is OutBoundManagementVeiwContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new OutBoundManagementVeiwContent());
        }
    }
}
