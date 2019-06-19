using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Commands;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.BasicData
{
    public class ErrorMessageCommand:AbstractMenuCommand
    {
        public override void Run()
        {
            PadDescriptor padDescriptor = new PadDescriptor(typeof(ErrorMessagePad), "", "");
            WorkbenchSingleton.Workbench.ShowPad(padDescriptor);
        }
    }
}