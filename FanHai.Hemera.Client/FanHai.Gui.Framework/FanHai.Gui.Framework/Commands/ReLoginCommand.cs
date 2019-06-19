using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Gui.Framework.Commands
{
    public class ReLoginCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            LoginDialog loginForm = new LoginDialog();
            loginForm.TopMost = false;
            if (DialogResult.OK == loginForm.ShowDialog() && !LoginDialog.flag)
            {
                foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (viewContent.TitleName != StringParser.Parse("${res:FanHai.Hemera.Addins.StartPage.StartPageViewContent.TitleName}"))
                    {
                        viewContent.WorkbenchWindow.CloseWindow(true);
                    }
                }
                //hide pad ---pad can't be closed but only hided
                foreach (PadDescriptor padDescriptor in WorkbenchSingleton.Workbench.PadContentCollection)
                {
                    WorkbenchSingleton.Workbench.WorkbenchLayout.HidePad(padDescriptor);
                }
                //end
                WorkbenchSingleton.Workbench.RedrawMenu();
            }
        } 
    }
}
