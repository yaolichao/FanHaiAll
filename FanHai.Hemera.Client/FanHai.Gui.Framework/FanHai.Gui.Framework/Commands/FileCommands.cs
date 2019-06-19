using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Gui.Framework.Commands
{
    public class ExitWorkbenchCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            try
            {
                User user = new User();
                user.LogUserLogoutInfo();
            }
            finally
            {
                WorkbenchSingleton.MainForm.Close();
            }
        }
    }
}
